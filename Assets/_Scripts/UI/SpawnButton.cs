using ChessCrush.Game;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class SpawnButton: MonoBehaviour
    {
        private Button button;
        public PieceType pieceSpawnType;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.OnClickAsObservable().Subscribe(_ => SubscribeButton());
        }

        private void SubscribeButton()
        {
            for (int i = 0; i < 8; i++)
            {
                if (Director.instance.chessBoard.AnybodyIn(i, 0))
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