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
            blackQueenSprite = Resources.Load<Sprite>(string.Format(path, "Black_Queen"));
            blackKingSprite = Resources.Load<Sprite>(string.Format(path, "Black_King"));
            whitePawnSprite = Resources.Load<Sprite>(string.Format(path, "White_Pawn"));
            whiteBishopSprite = Resources.Load<Sprite>(string.Format(path, "White_Bishop"));
            whiteKnightSprite = Resources.Load<Sprite>(string.Format(path, "White_Knight"));
            whiteQueenSprite = Resources.Load<Sprite>(string.Format(path, "White_Queen"));
            whiteKingSprite = Resources.Load<Sprite>(string.Format(path, "White_King"));
        }

        public Sprite GetChessSprite(PieceType pieceType, bool isWhite) => pieceType switch
        {
            PieceType.Pawn => isWhite switch
            {
                true => whitePawnSprite,
                false => blackPawnSprite
            },
            PieceType.Bishop => isWhite switch
            {
                true => whiteBishopSprite,
                false => blackBishopSprite
            },
            PieceType.Knight => isWhite switch
            {
                true => whiteKnightSprite,
                false => blackKnightSprite
            },
            PieceType.Rook => isWhite switch
            {
                true => whiteRookSprite,
                false => blackRookSprite
            },
            PieceType.Queen => isWhite switch
            {
                true => whiteQueenSprite,
                false => blackQueenSprite
            },
            PieceType.King => isWhite switch
            {
                true => whiteKingSprite,
                false => blackKingSprite
            },
            _ => default
        };
    }
}