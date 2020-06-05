using ChessCrush.Game;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class SpawnButton: MonoBehaviour
    {
        private Button button;
        private Image buttonImage;
        private Image chessImage;
        public PieceType pieceSpawnType;
        private int needEnergy;

        private ChessGameDirector chessGameDirector;

        private void Awake()
        {
            button = GetComponent<Button>();
            buttonImage = gameObject.GetComponent<Image>();
            chessImage = gameObject.GetComponentInChildren<Image>();
            button.OnClickAsObservable().Subscribe(_ => SubscribeButton());
            needEnergy = SetNeedEnergy();
        }

        private void Start()
        {
            chessGameDirector = Director.instance.GetSubDirector<ChessGameDirector>();
            if(chessGameDirector.player.IsWhite)
            {
                buttonImage.color = Color.black;
            }
            else
            {
                buttonImage.color = Color.white;
            }
        }

        private void SubscribeButton()
        {
            for (int i = 0; i < 8; i++)
            {
                if (chessGameDirector.chessGameObjects.chessBoard.AnybodyIn(i, 0, true))
                {
                    var square = DisableMoveSquare.UseWithComponent(new ChessBoardVector(i, 0));
                }
                else
                {
                    var square = AbleMoveSquare.UseWithComponent(0, new ChessBoardVector(i, 0), pieceSpawnType);
                }
            }
        }

        private int SetNeedEnergy()
        {
            switch(pieceSpawnType)
            {
                case PieceType.Pawn: return 1;
                case PieceType.Bishop: return 3;
                case PieceType.Knight: return 3;
                case PieceType.Rook: return 5;
                case PieceType.Queen: return 7;
                case PieceType.King: return 8;
                default: return 0;
            }
        }
    }
}