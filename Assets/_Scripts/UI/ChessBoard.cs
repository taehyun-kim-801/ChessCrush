using ChessCrush.Game;
using System.Collections.Generic;
using UnityEngine;

namespace ChessCrush.UI
{
    public class ChessBoard:MonoBehaviour
    {
        private List<ChessPiece> pieces = new List<ChessPiece>();
        public bool AnybodyIn(int x, int y) { return pieces.FindIndex(piece => piece.x == x && piece.y == y) != -1; }
    }
}