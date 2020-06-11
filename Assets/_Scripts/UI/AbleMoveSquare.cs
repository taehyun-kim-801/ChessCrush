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
        private bool isCreate;
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
            if (isCreate)
                chessGameDirector.myLocalEnergy.Value -= spawnType.GetNeedEnergy();

            haveToAppearProperty.Value = false;
        }

        private void Initialize(int pieceId, int x, int y, PieceType spawnType, bool isCreate)
        {
            this.pieceId = pieceId;
            base.Initialize(x, y);
            this.spawnType = spawnType;
            this.isCreate = isCreate;
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

        public static AbleMoveSquare UseWithComponent(int pieceId, ChessBoardVector boardVector, PieceType spawnType, bool isCreate = false)
        {
            var result = Director.instance.GetSubDirector<ChessGameDirector>().chessGameUI.objectPool.Use(nameof(AbleMoveSquare)).GetComponent<AbleMoveSquare>();
            result.Initialize(pieceId, boardVector.x, boardVector.y, spawnType, isCreate);
            return result;
        }
    }
}