using ChessCrush.Game;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace ChessCrush.UI
{
    public class WaitingText: MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;

        private ChessGameDirector chessGameDirector;

        private void Start()
        {
            chessGameDirector = Director.instance.GetSubDirector<ChessGameDirector>();
            chessGameDirector.ReceivedData.Subscribe(value => text.gameObject.SetActive(!value)).AddTo(chessGameDirector);
        }
    }
}