using System.Collections.Generic;
using UnityEngine;

namespace ChessCrush.Game
{
    public class ChessBoard:MonoBehaviour
    {
        private List<ChessPiece> pieces = new List<ChessPiece>();
        private List<ChessPiece> expectedPieces = new List<ChessPiece>();

        public bool AnybodyIn(int x, int y, bool includeExpectedPieces = false) 
        {
            int piecesIdx = pieces.FindIndex(piece => piece.chessBoardVector.x == x && piece.chessBoardVector.y == y);

            if (piecesIdx != -1)
                return true;
            else if (includeExpectedPieces)
                return expectedPieces.FindIndex(piece => piece.chessBoardVector.x == x && piece.chessBoardVector.y == y) != -1;
            else
                return false;    
        }

        public ChessPiece GetChessPiece(int x, int y, bool includeExpectedPieces = false)
        {
            if (!AnybodyIn(x, y, includeExpectedPieces))
                return null;

            var res = pieces.Find(piece => piece.chessBoardVector.x == x && piece.chessBoardVector.y == y);
            if (res is null)
                res = expectedPieces.Find(piece => piece.chessBoardVector.x == x && piece.chessBoardVector.y == y);

            return res;
        }

        public ChessPiece GetChessPieceById(int id) => pieces.Find(piece => piece.PieceId == id);

        public void AddChessPiece(ChessPiece piece)
        {
            pieces.Add(piece);
        }

        public void AddExpectedChessPiece(ChessPiece piece)
        {
            expectedPieces.Add(piece);
        }

        public void ClearChessPieces()
        {
            foreach (var piece in pieces)
                piece.gameObject.SetActive(false);

            pieces.Clear();
        }

        public void ClearExpectedChessPieces()
        {
            foreach (var piece in expectedPieces)
                piece.gameObject.SetActive(false);

            expectedPieces.Clear();
        }
    }
}