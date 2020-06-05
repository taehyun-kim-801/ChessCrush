using UnityEngine;

namespace ChessCrush.Game
{
    public class ResourceDirector: SubDirector
    {
        #region Chess Sprite
        private Sprite blackPawnSprite;
        private Sprite blackBishopSprite;
        private Sprite blackKnightSprite;
        private Sprite blackRookSprite;
        private Sprite blackQueenSprite;
        private Sprite blackKingSprite;

        private Sprite whitePawnSprite;
        private Sprite whiteBishopSprite;
        private Sprite whiteKnightSprite;
        private Sprite whiteRookSprite;
        private Sprite whiteQueenSprite;
        private Sprite whiteKingSprite;
        #endregion
        private void Awake()
        {
            LoadChessSprite();
        }

        private void LoadChessSprite()
        {
            string path = "Textures/Chess/ChessPiece/{0}";
            blackPawnSprite = Resources.Load<Sprite>(string.Format(path, "Black_Pawn"));
            blackBishopSprite = Resources.Load<Sprite>(string.Format(path, "Black_Bishop"));
            blackKnightSprite = Resources.Load<Sprite>(string.Format(path, "Black_Knight"));
            blackRookSprite = Resources.Load<Sprite>(string.Format(path, "Black_Rook"));
            blackQueenSprite = Resources.Load<Sprite>(string.Format(path, "Black_Queen"));
            blackKingSprite = Resources.Load<Sprite>(string.Format(path, "Black_King"));
            whitePawnSprite = Resources.Load<Sprite>(string.Format(path, "White_Pawn"));
            whiteBishopSprite = Resources.Load<Sprite>(string.Format(path, "White_Bishop"));
            whiteKnightSprite = Resources.Load<Sprite>(string.Format(path, "White_Knight"));
            whiteRookSprite = Resources.Load<Sprite>(string.Format(path, "White_Rook"));
            whiteQueenSprite = Resources.Load<Sprite>(string.Format(path, "White_Queen"));
            whiteKingSprite = Resources.Load<Sprite>(string.Format(path, "White_King"));
        }

        public Sprite GetChessSprite(PieceType pieceType, bool isWhite)
        {
            switch(pieceType)
            {
                case PieceType.Pawn:
                    if (isWhite) return whitePawnSprite;
                    else return blackPawnSprite;
                case PieceType.Bishop:
                    if (isWhite) return whiteBishopSprite;
                    else return blackBishopSprite;
                case PieceType.Knight:
                    if (isWhite) return whiteKnightSprite;
                    else return blackKnightSprite;
                case PieceType.Rook:
                    if (isWhite) return whiteRookSprite;
                    else return blackRookSprite;
                case PieceType.Queen:
                    if (isWhite) return whiteQueenSprite;
                    else return blackQueenSprite;
                case PieceType.King:
                    if (isWhite) return whiteKingSprite;
                    else return blackKingSprite;
                default: return null;
            }
        }
    }
}