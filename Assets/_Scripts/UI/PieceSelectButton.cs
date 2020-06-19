using ChessCrush.Game;
using UniRx;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class PieceSelectButton: ChessBoardObject
    {
        public int PieceId { get; private set; }
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
                    break;
                case PieceType.Bishop:
                    UseBishopMoveSquare();
                    break;
                case PieceType.Knight:
                    UseKnightMoveSquare();
                    break;
                case PieceType.Rook:
                    UseRookMoveSquare();
                    break;
                case PieceType.Queen:
                    UseQueenMoveSquare();
                    break;
                case PieceType.King:
                    UseKingMoveSquare();
                    break;
                default:
                    return;
            }

            gameObject.SetActive(false);
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

            //내가 수비 상황일 때 자살을 막음
            if (((chessGameDirector.player.IsWhite && chessGameDirector.turnCount.Value % 2 == 0) || (!chessGameDirector.player.IsWhite && chessGameDirector.turnCount.Value % 2 != 0))
                && chessGameDirector.chessGameObjects.chessBoard.AnybodyIn(position.x, position.y, true))
                DisableMoveSquare.UseWithComponent(position);
            else if (chessGameDirector.chessGameObjects.chessBoard.MyPieceIn(position.x, position.y, true))
                DisableMoveSquare.UseWithComponent(position);
            else
                AbleMoveSquare.UseWithComponent(PieceId, position, pieceType);
        }
        #endregion

        private void Initialize(int pieceId, ChessBoardVector position, PieceType pieceType)
        {
            this.PieceId = pieceId;
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