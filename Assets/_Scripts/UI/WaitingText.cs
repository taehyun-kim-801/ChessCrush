using ChessCrush.Game;
using System;
using System.Collections;
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
        [SerializeField]
        private float textAnimationTime;

        private ChessGameDirector chessGameDirector;

        private readonly string[] textPool = { "Loading", "Loading.", "Loading..", "Loading..." };

        private void Start()
        {
            chessGameDirector = Director.instance.GetSubDirector<ChessGameDirector>();
            chessGameDirector.ReceivedData.Subscribe(value => text.gameObject.SetActive(!value)).AddTo(chessGameDirector);
        }

        private void OnEnable()
        {
            StartCoroutine(TextAnimation());
        }

        private IEnumerator TextAnimation()
        {
            int index = 0;
            while(text.gameObject.activeSelf)
            {
                text.text = textPool[index = (index + 1) % 4];
                yield return new WaitForSeconds(textAnimationTime);
            }
        }
    }
}