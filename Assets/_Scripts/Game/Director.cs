using ChessCrush.UI;
using System.Collections;
using UnityEngine;
using BackEnd;
using System;

namespace ChessCrush.Game
{
    public class Director: MonoBehaviour
    {
        static public Director instance;
        [NonSerialized]
        public ObjectPool nonUiObjectPool;
        public string playerName;
        public NetworkHelper networkHelper;

        private void Awake()
        {
            if(instance)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
            instance = this;

            networkHelper = new NetworkHelper();

            Backend.Initialize(() =>
            {
                if (Backend.IsInitialized)
                    networkHelper.connected = true;
                else
                    networkHelper.connected = false;
            });

            nonUiObjectPool = Instantiate(Resources.Load("Prefabs/NonUIObjectPool") as GameObject).GetComponent<ObjectPool>();
            Instantiate(Resources.Load("Prefabs/MainCanvas") as GameObject);
        }


        private IEnumerator Start()
        {
            yield return new WaitUntil(() => nonUiObjectPool.isCreated && MainCanvas.instance.objectPool.isCreated && Backend.IsInitialized);
            GetSubDirector<StartSceneDirector>();
        }

        public T GetSubDirector<T>() where T:SubDirector
        {
            var director = SubDirectorsSet.Find<T>();

            var parameterType = typeof(T);
            var resultObj = SubDirectorsSet.directorDictionary[parameterType];

            if(resultObj is null)
            {
                resultObj = Instantiate(director.gameObject) as GameObject;
                SubDirectorsSet.directorDictionary[parameterType] = resultObj;
            }

            return resultObj.GetComponent<T>();
        }
    }
}