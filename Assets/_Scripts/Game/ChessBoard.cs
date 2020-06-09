using System.Collections.Generic;
using UnityEngine;

namespace ChessCrush.Game
{
    public class ChessBoard:MonoBehaviour
    {
        public List<ChessPiece> Pieces { get; private set; } = new List<ChessPiece>();
        private List<ChessPiece> expectedPieces = new List<ChessPiece>();

        public bool AnybodyIn(int x, int y, bool includeExpectedPieces = false) 
        {
            int piecesIdx = Pieces.FindIndex(piece => piece.chessBoardVector.x == x && piece.chessBoardVector.y == y);

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

            var res = Pieces.Find(piece => piece.chessBoardVector.x == x && piece.chessBoardVector.y == y);
            if (res is null)
                res = expectedPieces.Find(piece => piece.chessBoardVector.x == x && piece.chessBoardVector.y == y);

            return res;
        }

        public ChessPiece GetChessPieceById(int id) => Pieces.Find(piece => piece.PieceId == id);

        public void AddChessPiece(ChessPiece piece)
        {
            Pieces.Add(piece);
        }

        public void AddExpectedChessPiece(ChessPiece piece)
        {
            expectedPieces.Add(piece);
        }

        public void ClearChessPieces()
        {
            foreach (var piece in Pieces)
                piece.gameObject.SetActive(false);

            Pieces.Clear();
        }

        public void ClearExpectedChessPieces()
        {
            foreach (var piece in expectedPieces)
                piece.gameObject.SetActive(false);

            expectedPieces.Clear();
        }

        public void RemoveChessPiece(ChessPiece piece) => Pieces.Remove(piece);
    }
}