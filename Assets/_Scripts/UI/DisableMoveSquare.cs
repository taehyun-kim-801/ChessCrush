using ChessCrush.Game;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class DisableMoveSquare: ChessBoardObject
    {
        private Button button;

        private void Awake()
        {
            base.Awake();
            button = gameObject.GetComponent<Button>();
            //button.OnClickAsObservable().Subscribe(_ => SubscribeAbleMoveSquare());
        }

        private void SubscribeAbleMoveSquare()
        {

        }

        public static DisableMoveSquare UseWithComponent(ChessBoardVector boardVector)
        {
            var result = Director.instance.uiObjectPool.Use(nameof(DisableMoveSquare)).GetComponent<DisableMoveSquare>();
            result.Initialize(boardVector.x, boardVector.y);
            return result;
        }
    }
}