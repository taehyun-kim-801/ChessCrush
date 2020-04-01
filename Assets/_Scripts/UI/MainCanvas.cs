using UnityEngine;

namespace ChessCrush.UI
{
    public class MainCanvas:MonoBehaviour
    {
        public static MainCanvas instance;
        public Canvas canvas;
        public ObjectPool objectPool;

        private void Awake()
        {
            if (instance)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            instance = this;
            canvas = GetComponent<Canvas>();
            objectPool = GetComponent<ObjectPool>();
        }

        public GameObject Use(string uiName)
        {
            return objectPool.Use(uiName);
        }
    }
}
