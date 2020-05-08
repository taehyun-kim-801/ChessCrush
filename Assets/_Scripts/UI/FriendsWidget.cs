using BackEnd;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class FriendsWidget: MonoBehaviour
    {
        [SerializeField]
        private GameObject myFriendsScrollContent;
        [SerializeField]
        private Button goToSearchButton;
        [SerializeField]
        private GameObject requestWidget;
        [SerializeField]
        private Button exitButton;

        private void Awake()
        {
            goToSearchButton.OnClickAsObservable().Subscribe(_ => requestWidget.SetActive(true));
            exitButton.OnClickAsObservable().Subscribe(_ => gameObject.SetActive(false));
        }
    }
}