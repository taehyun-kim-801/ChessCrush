using ChessCrush.UI;
using UnityEngine;

namespace ChessCrush.Game
{
    public class ChessPiece: MonoBehaviour
    {
        private static int pieceIdCount = 0;

        public int PieceId { get; private set; }
        public ChessBoardVector chessBoardVector { get; private set; }
        private SpriteRenderer spriteRenderer;
        private bool isExpected;

        private void Awake()
        {
            chessBoardVector = new ChessBoardVector();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void MoveTo(int x, int y)
        {
            chessBoardVector.x = x;
            chessBoardVector.y = y;
        }

        private void Initialize(int pieceId, int x, int y, PieceType pieceType, bool isExpected)
        {
            if (pieceId == 0 && !isExpected)
                PieceId = ++pieceIdCount;
            else
                PieceId = pieceId;
            MoveTo(x, y);
            this.isExpected = isExpected;
            spriteRenderer.sprite = GetSprite(pieceType);
            
            if (isExpected)
                spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            else
                spriteRenderer.color = new Color(1, 1, 1, 1);

            transform.position = chessBoardVector.ToWorldVector();
            if(!isExpected)
                PieceSelectButton.UseWithComponent(pieceId, chessBoardVector, pieceType);
        }

        public static ChessPiece UseWithComponent(int pieceId, int x, int y, PieceType pieceType, bool isExpected)
        {
            var result = Director.instance.nonUiObjectPool.Use(nameof(ChessPiece)).GetComponent<ChessPiece>();
            result.Initialize(pieceId, x, y, pieceType, isExpected);
            return result;
        }

        private static Sprite GetSprite(PieceType pieceType)
        {
            string path = "Textures/Chess/ChessPiece/{0}";
            switch(pieceType)
            {
                case PieceType.Pawn:
                    return Resources.Load<Sprite>(string.Format(path, "Black_Pawn"));
                case PieceType.Bishop:
                    return Resources.Load<Sprite>(string.Format(path, "Black_Bishop"));
                case PieceType.Knight:
                    return Resources.Load<Sprite>(string.Format(path, "Black_Knight"));
                case PieceType.Rook:
                    return Resources.Load<Sprite>(string.Format(path, "Black_Rook"));
                case PieceType.Queen:
                    return Resources.Load<Sprite>(string.Format(path, "Black_Queen"));
                case PieceType.King:
                    return Resources.Load<Sprite>(string.Format(path, "Black_King"));
            }
            return null;
        }
    }
}