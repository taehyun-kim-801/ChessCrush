using ChessCrush.Game;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class AbleMoveSquare: ChessBoardObject
    {
        private int pieceId;
        private PieceType spawnType;
        private Button button;
        private ChessBoard chessBoard;
        public static ReactiveProperty<bool> haveToAppearProperty = new ReactiveProperty<bool>();
        private IDisposable haveToAppearPropertyCallback;

        private void Awake()
        {
            base.Awake();
            button = gameObject.GetComponent<Button>();
            button.OnClickAsObservable().Subscribe(_ => SubscribeAbleMoveSquare()).AddTo(gameObject);
        }

        private void Start()
        {
            chessBoard = Director.instance.GetSubDirector<ChessGameDirector>().chessGameObjects.chessBoard;
        }

        private void SubscribeAbleMoveSquare()
        {
            var piece = ChessPiece.UseWithComponent(pieceId, chessBoardPosition.x, chessBoardPosition.y, spawnType);
            
            if(spawnType == default)
            {
                piece.MoveTo(chessBoardPosition.x, chessBoardPosition.y);
            }
            else
            {
                chessBoard.AddChessPiece(piece);
            }

            haveToAppearProperty.Value = false;
        }

        private void Initialize(int pieceId, int x, int y, PieceType spawnType)
        {
            this.pieceId = pieceId;
            base.Initialize(x, y);
            this.spawnType = spawnType;
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

        public static AbleMoveSquare UseWithComponent(int pieceId, ChessBoardVector boardVector, PieceType spawnType = default)
        {
            var result = Director.instance.GetSubDirector<ChessGameDirector>().chessGameUI.objectPool.Use(nameof(AbleMoveSquare)).GetComponent<AbleMoveSquare>();
            result.Initialize(pieceId, boardVector.x, boardVector.y, spawnType);
            return result;
        }
    }
}