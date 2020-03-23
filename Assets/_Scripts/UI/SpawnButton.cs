using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class SpawnButton: MonoBehaviour
    {
        public Button button;
        public SpawnType spawnType;

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
                    var square = Game.instance.objectPool.Use("DisableMoveSquare");
                    square.GetComponent<ChessBoardObject>().Initialize(i, 0);
                }
                else
                {
                    var square = Game.instance.objectPool.Use("AbleMoveSquare");
                    square.GetComponent<AbleMoveSquare>().Initialize(i, 0);
                }
            }
        }
    }
}