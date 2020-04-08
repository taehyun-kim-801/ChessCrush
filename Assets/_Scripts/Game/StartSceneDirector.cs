using ChessCrush.UI;
using ChessGame.UI;

namespace ChessCrush.Game
{
    public class StartSceneDirector: SubDirector
    {
        private StartUI startUI;

        private void OnEnable()
        {
            var ui = MainCanvas.instance.objectPool.Use(nameof(StartUI));
            startUI = ui.GetComponent<StartUI>();
        }
    }
}