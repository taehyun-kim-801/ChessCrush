using UnityEngine;

namespace ChessCrush.UI
{
    public class MainCanvas:MonoBehaviour
    {
        public static MainCanvas instance;
        public Canvas canvas;

        private void Awake()
        {
            if (instance)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            instance = this;
            canvas = GetComponent<Canvas>();
        }
    }
}
