using ChessCrush.UI;
using UniRx;

namespace ChessCrush.Game
{
    public class StartSceneDirector: SubDirector
    {
        private StartUI startUI;
        public ReactiveProperty<bool> signedIn = new ReactiveProperty<bool>();

        private void OnEnable()
        {
            var ui = MainCanvas.instance.objectPool.Use(nameof(StartUI));
            startUI = ui.GetComponent<StartUI>();
        }
    }
}