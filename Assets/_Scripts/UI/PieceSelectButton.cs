using ChessCrush.Game;
using UniRx;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class PieceSelectButton: ChessBoardObject
    {
        private int pieceId;
        private Button button;
        private PieceType pieceType;
        private static ChessGameDirector chessGameDirector;

        private void Awake()
        {
            base.Awake();
            button = GetComponent<Button>();
            button.OnClickAsObservable().Subscribe(_ => SubscribeButton()).AddTo(gameObject);
        }

        private void SubscribeButton()
        {
            AbleMoveSquare.haveToAppearProperty.Value = false;
            DisableMoveSquare.haveToAppearProperty.Value = false;
            AbleMoveSquare.haveToAppearProperty.Value = true;
            AbleMoveSquare.haveToAppearProperty.Value = true;

            switch(pieceType)
            {
                case PieceType.Pawn:
                    UsePawnMoveSquare();
                    return;
                case PieceType.Bishop:
                    UseBishopMoveSquare();
                    return;
                case PieceType.Knight:
                    UseKnightMoveSquare();
                    return;
                case PieceType.Rook:
                    UseRookMoveSquare();
                    return;
                case PieceType.Queen:
                    UseQueenMoveSquare();
                    return;
                case PieceType.King:
                    UseKingMoveSquare();
                    return;
                default:
                    return;
            }
        }

        #region Use MoveSquare
        private void UsePawnMoveSquare()
        {
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x,chessBoardPosition.y + 1));
            if (chessGameDirector.chessGameObjects.chessBoard.AnybodyIn(chessBoardPosition.x - 1, chessBoardPosition.y + 1))
                UseMoveSquare(new ChessBoardVector(chessBoardPosition.x - 1, chessBoardPosition.y + 1));
            if (chessGameDirector.chessGameObjects.chessBoard.AnybodyIn(chessBoardPosition.x + 1, chessBoardPosition.y + 1))
                UseMoveSquare(new ChessBoardVector(chessBoardPosition.x + 1, chessBoardPosition.y + 1));
        }

        private void UseBishopMoveSquare()
        {
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x + 2, chessBoardPosition.y + 2));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x + 1, chessBoardPosition.y + 1));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x - 1, chessBoardPosition.y + 1));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x - 2, chessBoardPosition.y + 2));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x - 1, chessBoardPosition.y - 1));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x - 2, chessBoardPosition.y - 2));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x + 1, chessBoardPosition.y - 1));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x + 2, chessBoardPosition.y - 2));
        }

        private void UseKnightMoveSquare()
        {
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x + 2, chessBoardPosition.y + 1));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x + 1, chessBoardPosition.y + 2));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x - 2, chessBoardPosition.y + 1));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x - 1, chessBoardPosition.y + 2));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x - 2, chessBoardPosition.y - 1));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x - 1, chessBoardPosition.y - 2));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x + 2, chessBoardPosition.y - 1));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x + 1, chessBoardPosition.y - 2));
        }

        private void UseRookMoveSquare()
        {
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x + 2, chessBoardPosition.y));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x + 1, chessBoardPosition.y));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x - 2, chessBoardPosition.y));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x - 1, chessBoardPosition.y));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x, chessBoardPosition.y + 2));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x, chessBoardPosition.y + 1));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x, chessBoardPosition.y - 2));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x, chessBoardPosition.y - 1));
        }

        private void UseQueenMoveSquare()
        {
            UseRookMoveSquare();
            UseBishopMoveSquare();
        }

        private void UseKingMoveSquare()
        {
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x + 1, chessBoardPosition.y));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x - 1, chessBoardPosition.y));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x, chessBoardPosition.y + 1));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x, chessBoardPosition.y - 1));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x + 1, chessBoardPosition.y + 1));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x - 1, chessBoardPosition.y + 1));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x - 1, chessBoardPosition.y - 1));
            UseMoveSquare(new ChessBoardVector(chessBoardPosition.x + 1, chessBoardPosition.y - 1));
        }

        private void UseMoveSquare(ChessBoardVector position)
        {
            if (position.x < 0 || position.y < 0 || position.x > 7 || position.y > 7)
                return;

            if (chessGameDirector.chessGameObjects.chessBoard.AnybodyIn(position.x, position.y))
                DisableMoveSquare.UseWithComponent(position);
            else
                AbleMoveSquare.UseWithComponent(pieceId, position, pieceType);
        }
        #endregion

        private void Initialize(int pieceId, ChessBoardVector position, PieceType pieceType)
        {
            this.pieceId = pieceId;
            base.Initialize(position.x, position.y);
            chessBoardPosition = position;
            this.pieceType = pieceType;
        }

        public static PieceSelectButton UseWithComponent(int pieceId, ChessBoardVector position,PieceType pieceType)
        {
            if(chessGameDirector is null)
                chessGameDirector = Director.instance.GetSubDirector<ChessGameDirector>();

            var result = chessGameDirector.chessGameUI.objectPool.Use("PieceSelectButton").GetComponent<PieceSelectButton>();
            result.Initialize(pieceId, position, pieceType);
            return result;
        }
    }
}