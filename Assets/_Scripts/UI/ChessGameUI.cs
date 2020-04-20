using ChessCrush.Game;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class ChessGameUI: MonoBehaviour
    {
        public ObjectPool objectPool;
        public PlayerStatus myStatus;
        public PlayerStatus enemyStatus;
        public ChessActionScroll chessActionScroll;
        [SerializeField]
        private Button inputButton;

        private void Awake()
        {
            inputButton.OnClickAsObservable().Subscribe(_ => Director.instance.GetSubDirector<ChessGameDirector>().inputCompleted = true).AddTo(gameObject);
        }
    }
}