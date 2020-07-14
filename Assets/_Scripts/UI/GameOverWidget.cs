using ChessCrush.Game;
using System.Collections;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class GameOverWidget: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI winText;
        [SerializeField] private TextMeshProUGUI loseText;
        [SerializeField] private TextMeshProUGUI rateText;
        [SerializeField] private TextMeshProUGUI rateNumText;
        [SerializeField] private Button continueButton;

        private ChessGameDirector chessGameDirector;

        private void Awake()
        {
            continueButton.OnClickAsObservable().Subscribe(_ => chessGameDirector.GameEnd()).AddTo(gameObject);
        }

        private void Start()
        {
            chessGameDirector = Director.instance.GetSubDirector<ChessGameDirector>();
        }

        private void OnEnable()
        {
            if(!(chessGameDirector is null))
                StartCoroutine(CoEnable(chessGameDirector.isPlayerWin));
        }

        private void OnDisable()
        {
            winText.gameObject.SetActive(false);
            loseText.gameObject.SetActive(false);
            rateText.gameObject.SetActive(false);
            rateNumText.gameObject.SetActive(false);
            continueButton.gameObject.SetActive(false);
        }

        private IEnumerator CoEnable(bool isPlayerWin)
        {
            winText.gameObject.SetActive(isPlayerWin);
            loseText.gameObject.SetActive(!isPlayerWin);

            yield return new WaitForSeconds(0.7f);

            rateText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);

            rateNumText.gameObject.SetActive(true);

            //rateNumText 수 올라가는 애니메이션

            continueButton.gameObject.SetActive(true);
        }
    }
}