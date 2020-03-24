using ChessCrush.Game;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class AbleMoveSquare: ChessBoardObject
    {
        private PieceType spawnType;
        private Button button;
        private ChessBoard chessBoard;

        private void Awake()
        {
            button = gameObject.GetComponent<Button>();
            chessBoard = Game.instance.chessBoard;
            button.OnClickAsObservable().Subscribe(_ => SubscribeAbleMoveSquare());
        }

        private void SubscribeAbleMoveSquare()
        {
            if(spawnType == default)
            {
                ChessPiece piece = Game.instance.chessBoard.GetChessPiece(chessBoardPosition.x, chessBoardPosition.y);
                piece.MoveTo(chessBoardPosition.x, chessBoardPosition.y);
            }
            else
            {
                ChessPiece piece = new ChessPiece(chessBoardPosition.x, chessBoardPosition.y, spawnType);
                chessBoard.AddChessPiece(piece);
            }
        }

        private void Initialize(int x, int y, PieceType spawnType)
        {
            base.Initialize(x, y);
            this.spawnType = spawnType;
        }

        public static AbleMoveSquare UseWithComponent(ChessBoardVector boardVector, PieceType spawnType = default)
        {
            var result = Game.instance.objectPool.Use(nameof(AbleMoveSquare)).GetComponent<AbleMoveSquare>();
            result.Initialize(boardVector.x, boardVector.y, spawnType);
            return result;
        }
    }
}