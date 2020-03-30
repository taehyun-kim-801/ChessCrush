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
            base.Awake();
            button = gameObject.GetComponent<Button>();
            chessBoard = Director.instance.chessBoard;
            button.OnClickAsObservable().Subscribe(_ => SubscribeAbleMoveSquare());
        }

        private void SubscribeAbleMoveSquare()
        {
            var piece = ChessPiece.UseWithComponent(chessBoardPosition.x, chessBoardPosition.y, spawnType);
            
            if(spawnType == default)
            {
                piece.MoveTo(chessBoardPosition.x, chessBoardPosition.y);
            }
            else
            {
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
            var result = Director.instance.uiObjectPool.Use(nameof(AbleMoveSquare)).GetComponent<AbleMoveSquare>();
            result.Initialize(boardVector.x, boardVector.y, spawnType);
            return result;
        }
    }
}