using ChessCrush.UI;
using System.Collections;
using UnityEngine;

namespace ChessCrush.Game
{
    public class Director: MonoBehaviour
    {
        static public Director instance;
        public ObjectPool nonUiObjectPool;

        private void Awake()
        {
            if(instance)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
            instance = this;
        }


        private void Start()
        {
            StartCoroutine(StartChessGame());
        }
        public IEnumerator StartChessGame()
        {
            yield return new WaitUntil(() => nonUiObjectPool.isCreated && MainCanvas.instance.objectPool.isCreated);
            GetSubDirector<ChessGameDirector>();
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