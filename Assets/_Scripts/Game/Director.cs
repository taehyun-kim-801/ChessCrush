using System.Collections;
using UnityEngine;

namespace ChessCrush.Game
{
    public class Director: MonoBehaviour
    {
        static public Director instance;
        public ObjectPool uiObjectPool;
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

        public void StartChessGame()
        {

        }

        public T GetSubDirector<T>() where T:SubDirector
        {
            var director = SubDirectorsSet.Find<T>();
            var result = Instantiate(director.gameObject).GetComponent<T>();
            return result;
        }
    }
}