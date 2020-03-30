using System.Collections.Generic;
using UnityEngine;

namespace ChessCrush.Game
{
    public class ChessBoard:MonoBehaviour
    {
        private List<ChessPiece> pieces = new List<ChessPiece>();

        public bool AnybodyIn(int x, int y) { return pieces.FindIndex(piece => piece.chessBoardVector.x == x && piece.chessBoardVector.y == y) != -1; }
        public ChessPiece GetChessPiece(int x,int y)
        {
            if (!AnybodyIn(x, y))
                return null;

            return pieces.Find(piece => piece.chessBoardVector.x == x && piece.chessBoardVector.y == y);
        }

        public void AddChessPiece(ChessPiece piece)
        {
            pieces.Add(piece);
        }
    }
}