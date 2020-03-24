using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class AbleMoveSquare: ChessBoardObject
    {
        private Action<SpawnType> clickCallback;
        private Button button;

        private void Awake()
        {
            button = gameObject.GetComponent<Button>();
            button.OnClickAsObservable().Subscribe(_ => SubscribeAbleMoveSquare());
        }

        private void SubscribeAbleMoveSquare()
        {
            
        }

        private void Initialize(int x, int y, Action<SpawnType> callback)
        {
            base.Initialize(x, y);
            clickCallback = callback;
        }

        public static AbleMoveSquare UseWithComponent(ChessBoardVector boardVector, SpawnType spawnType)
        {
            var result = Game.instance.objectPool.Use(nameof(AbleMoveSquare)).GetComponent<AbleMoveSquare>();
            result.Initialize(boardVector.x, boardVector.y);
            return result;
        }
    }
}