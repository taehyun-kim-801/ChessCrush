using BackEnd;
using ChessCrush.UI;
using UniRx;
using UniRx.Triggers;

namespace ChessCrush.Game
{
    public class BackendDirector: SubDirector
    {
        private string roomToken;

        private void Awake()
        {
            Backend.Initialize(() =>
            {
                if (Backend.IsInitialized)
                    gameObject.UpdateAsObservable().Subscribe(_ => Backend.Match.poll()).AddTo(gameObject);
                else
                    MessageBoxUI.UseWithComponent("Failed to connect to server");
            });
        }
    }
}