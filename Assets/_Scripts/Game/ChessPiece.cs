using ChessCrush.UI;
using UnityEngine;

namespace ChessCrush.Game
{
    public class ChessPiece: MonoBehaviour
    {
        private static int pieceIdCount = 0;

        public int PieceId { get; private set; }
        public ChessBoardVector chessBoardVector { get; private set; }
        public PieceType PieceType { get; private set; }

        private SpriteRenderer spriteRenderer;

        public bool IsMine { get; private set; }
        private bool isExpected;

        private ChessGameDirector chessGameDirector;
        private ResourceDirector resourceDirector;

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

        private void Initialize(int pieceId, int x, int y, PieceType pieceType, bool isExpected, bool isMine)
        {
            chessGameDirector = Director.instance.GetSubDirector<ChessGameDirector>();
            resourceDirector = Director.instance.GetSubDirector<ResourceDirector>();

            if (pieceId == 0 && !isExpected)
                PieceId = ++pieceIdCount;
            else
                PieceId = pieceId;
            MoveTo(x, y);
            this.PieceType = pieceType;
            this.isExpected = isExpected;
            this.IsMine = isMine;

            if (isExpected)
                spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            else
                spriteRenderer.color = new Color(1, 1, 1, 1);

            if (isMine)
                spriteRenderer.sprite = resourceDirector.GetChessSprite(pieceType, chessGameDirector.player.IsWhite);
            else
                spriteRenderer.sprite = resourceDirector.GetChessSprite(pieceType, chessGameDirector.enemyPlayer.IsWhite);

            transform.position = chessBoardVector.ToWorldVector();

            if(!isExpected)
                PieceSelectButton.UseWithComponent(pieceId, chessBoardVector, pieceType);
        }

        public static ChessPiece UseWithComponent(int pieceId, int x, int y, PieceType pieceType, bool isExpected, bool isMine)
        {
            var result = Director.instance.nonUiObjectPool.Use(nameof(ChessPiece)).GetComponent<ChessPiece>();
            result.Initialize(pieceId, x, y, pieceType, isExpected, isMine);
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