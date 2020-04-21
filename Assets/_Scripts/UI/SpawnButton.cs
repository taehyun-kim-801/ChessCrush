using ChessCrush.Game;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class SpawnButton: MonoBehaviour
    {
        private ChessGameDirector gameDirector;
        private Button button;
        public PieceType pieceSpawnType;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.OnClickAsObservable().Subscribe(_ => SubscribeButton());
        }

        private void Start()
        {
            gameDirector = Director.instance.GetSubDirector<ChessGameDirector>();
        }

        private void SubscribeButton()
        {
            for (int i = 0; i < 8; i++)
            {
                if (gameDirector.chessGameObjects.chessBoard.AnybodyIn(i, 0, true))
                {
                    var square = DisableMoveSquare.UseWithComponent(new ChessBoardVector(i, 0));
                }
                else
                {
                    var square = AbleMoveSquare.UseWithComponent(0, new ChessBoardVector(i, 0), pieceSpawnType);
                }
            }
        }
    }
}