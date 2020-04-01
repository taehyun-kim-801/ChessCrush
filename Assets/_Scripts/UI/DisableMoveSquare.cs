using ChessCrush.Game;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class DisableMoveSquare: ChessBoardObject
    {
        private Button button;
        public static ReactiveProperty<bool> haveToAppearProperty = new ReactiveProperty<bool>();
        private IDisposable haveToAppearPropertyCallback;

        private void Awake()
        {
            base.Awake();
            button = gameObject.GetComponent<Button>();
            button.OnClickAsObservable().Subscribe(_ => SubscribeAbleMoveSquare());
        }

        private void SubscribeAbleMoveSquare()
        {
            haveToAppearProperty.Value = false;
        }

        protected override void Initialize(int x, int y)
        {
            base.Initialize(x, y);
            haveToAppearProperty.Value = true;
            haveToAppearPropertyCallback = haveToAppearProperty.Subscribe(value =>
            {
                if (!value)
                    Destroy();
            });
        }

        private void Destroy()
        {
            AbleMoveSquare.haveToAppearProperty.Value = false;
            gameObject.SetActive(false);
            haveToAppearPropertyCallback.Dispose();
        }

        public static DisableMoveSquare UseWithComponent(ChessBoardVector boardVector)
        {
            var result = Director.instance.GetSubDirector<ChessGameDirector>().chessGameUI.objectPool.Use(nameof(DisableMoveSquare)).GetComponent<DisableMoveSquare>();
            result.Initialize(boardVector.x, boardVector.y);
            return result;
        }
    }
}