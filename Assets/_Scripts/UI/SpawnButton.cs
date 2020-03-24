using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class SpawnButton: MonoBehaviour
    {
        public Button button;
        public PieceType pieceSpawnType;

        private void Awake()
        {
            button.OnClickAsObservable().Subscribe(_ => SubscribeButton());
        }

        private void SubscribeButton()
        {
            for (int i = 0; i < 8; i++)
            {
                if (Game.instance.chessBoard.AnybodyIn(i, 0))
                {
                    var square = DisableMoveSquare.UseWithComponent(new ChessBoardVector(i, 0));
                }
                else
                {
                    var square = AbleMoveSquare.UseWithComponent(new ChessBoardVector(i, 0), pieceSpawnType);
                }
            }
        }
    }
}