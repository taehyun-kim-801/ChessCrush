using UnityEngine;

namespace ChessCrush.UI
{
    public class Game: MonoBehaviour
    {
        static public Game instance;
        public ObjectPool objectPool;
        public ChessBoard chessBoard;
        public Transform chessBoardOrigin;

        private void Awake()
        {
            if(instance)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }
}