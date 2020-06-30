using ChessCrush.Game;
using System;
using System.Runtime.CompilerServices;
using UniRx;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class AbleMoveSquare: ChessBoardObject
    {
        private int pieceId;
        private PieceType spawnType;
        private Button button;
        private ChessBoard chessBoard;
        private PieceSelectButton selectButton;
        public static ReactiveProperty<bool> haveToAppearProperty = new ReactiveProperty<bool>();
        private IDisposable haveToAppearPropertyCallback;

        private ChessGameDirector chessGameDirector;

        private void Awake()
        {
            base.Awake();
            button = gameObject.GetComponent<Button>();
            button.OnClickAsObservable().Subscribe(_ => SubscribeAbleMoveSquare()).AddTo(gameObject);
        }

        private void Start()
        {
            chessGameDirector = Director.instance.GetSubDirector<ChessGameDirector>();
            chessBoard = chessGameDirector.chessGameObjects.chessBoard;
        }

        private void SubscribeAbleMoveSquare()
        {
            var gameDirector = Director.instance.GetSubDirector<ChessGameDirector>();
            gameDirector.player.chessActions.Add(new ChessAction(pieceId, spawnType, new ChessBoardVector(chessBoardPosition.x,chessBoardPosition.y)));
            gameDirector.player.actionsSubject.OnNext(gameDirector.player.chessActions);
            if (selectButton == null)
                chessGameDirector.myLocalEnergy.Value -= spawnType.GetNeedEnergy();
            selectButton?.gameObject.SetActive(false);

            haveToAppearProperty.Value = false;
        }

        private void Initialize(int pieceId, int x, int y, PieceType spawnType, PieceSelectButton selectButton)
        {
            this.pieceId = pieceId;
            base.Initialize(x, y);
            this.spawnType = spawnType;
            this.selectButton = selectButton;
            haveToAppearProperty.Value = true;
            haveToAppearPropertyCallback = haveToAppearProperty.Subscribe(value =>
            {
                if(!value)
                    Destroy();
            });
        }

        private void Destroy()
        {
            DisableMoveSquare.haveToAppearProperty.Value = false;
            gameObject.SetActive(false);
            haveToAppearPropertyCallback.Dispose();
        }

        public static AbleMoveSquare UseWithComponent(int pieceId, ChessBoardVector boardVector, PieceType spawnType, PieceSelectButton selectButton = null)
        {
            var result = Director.instance.GetSubDirector<ChessGameDirector>().chessGameUI.objectPool.Use(nameof(AbleMoveSquare)).GetComponent<AbleMoveSquare>();
            result.Initialize(pieceId, boardVector.x, boardVector.y, spawnType, selectButton);
            return result;
        }
    }
}