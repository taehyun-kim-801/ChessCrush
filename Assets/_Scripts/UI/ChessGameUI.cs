using ChessCrush.Game;
using System.Collections.Generic;
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
        public Text turnText;
        public ChessActionScroll chessActionScroll;
        [SerializeField]
        private Button inputButton;
        [SerializeField]
        private InputTimeCircle inputTimeCircle;

        private ChessGameDirector chessGameDirector;

        private void Start()
        {
            chessGameDirector = Director.instance.GetSubDirector<ChessGameDirector>();
            chessGameDirector.turnCount.Subscribe(value =>
            {
                turnText.text = $"Turn {value.ToString()}";
                inputTimeCircle.gameObject.SetActive(true);
            });
            inputButton.OnClickAsObservable().Subscribe(_ => chessGameDirector.inputCompleted = true).AddTo(gameObject);
        }

        public void SetSelectButtons(List<ChessAction> actions)
        {
            objectPool.DestroyAll();
            foreach(var action in actions)
                PieceSelectButton.UseWithComponent(action.pieceId, action.chessBoardVector, action.pieceType);
        }
    }
}