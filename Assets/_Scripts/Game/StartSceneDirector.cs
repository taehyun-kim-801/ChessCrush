using ChessCrush.UI;
using UniRx;

namespace ChessCrush.Game
{
    public class StartSceneDirector: SubDirector
    {
        public StartUI startUI;
        public ReactiveProperty<bool> signedIn = new ReactiveProperty<bool>();

        private void OnEnable()
        {
            var ui = MainCanvas.instance.objectPool.Use(nameof(StartUI));
            if (ReferenceEquals(startUI, null))
                startUI = ui.GetComponent<StartUI>();
        }

        private void OnDisable()
        {
            startUI.gameObject.SetActive(false);
        }
    }
}