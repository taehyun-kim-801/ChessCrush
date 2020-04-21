using System.Collections.Generic;
using UnityEngine;

namespace ChessCrush.Game
{
    public class ChessGameObjects : MonoBehaviour 
    {
        public ChessBoard chessBoard;
        public Transform chessBoardOrigin;

        public void SetExpectedAction(List<ChessAction> actions)
        {
            chessBoard.ClearExpectedChessPieces();
            foreach(var action in actions)
                chessBoard.AddExpectedChessPiece(ChessPiece.UseWithComponent(action.pieceId, action.chessBoardVector.x, action.chessBoardVector.y, action.pieceType, true));
        }
    }
}