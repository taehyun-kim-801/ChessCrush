using ChessCrush.UI;
using UnityEngine;

namespace ChessCrush.Game
{
    public class ChessPiece: MonoBehaviour
    {
        public static int PieceIdCount { get; private set; }

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
            chessBoardVector = new ChessBoardVector(x, y);
        }

        private void Initialize(int pieceId, int x, int y, PieceType pieceType, bool isExpected, bool isMine)
        {
            chessGameDirector = Director.instance.GetSubDirector<ChessGameDirector>();
            resourceDirector = Director.instance.GetSubDirector<ResourceDirector>();

            if (pieceId == 0 && !isExpected)
                PieceId = ++PieceIdCount;
            else
            {
                PieceId = pieceId;
                if (PieceIdCount < pieceId) PieceIdCount = pieceId;
            }
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
        }

        public static ChessPiece UseWithComponent(int pieceId, int x, int y, PieceType pieceType, bool isExpected, bool isMine)
        {
            var result = Director.instance.nonUiObjectPool.Use(nameof(ChessPiece)).GetComponent<ChessPiece>();
            result.Initialize(pieceId, x, y, pieceType, isExpected, isMine);
            return result;
        }

        public void SetMovingState(bool isMoving)
        {
            if (isMoving)
                spriteRenderer.sortingOrder = 2;
            else
                spriteRenderer.sortingOrder = 1;
        }

        public void Write(OutputMemoryStream oms)
        {
            oms.Write(PieceId);
            oms.Write((int)PieceType);
            oms.Write(7 - chessBoardVector.x);
            oms.Write(7 - chessBoardVector.y);
            oms.Write(!IsMine);
        }
    }
}