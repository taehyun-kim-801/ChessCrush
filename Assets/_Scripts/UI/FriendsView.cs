using BackEnd;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class FriendsView:MonoBehaviour
    {
        [SerializeField]
        private Text nicknameText;
        [SerializeField]
        private Button chatButton;
        [SerializeField]
        private Button breakButton;

        private string friendInDate;

        private void Awake()
        {
            chatButton.OnClickAsObservable().Subscribe(_ => SubscribeChatButton()).AddTo(gameObject);
            breakButton.OnClickAsObservable().Subscribe(_ => SubscribeBreakButton()).AddTo(gameObject);
        }

        public void Set(string nickname, string inDate)
        {
            nicknameText.text = nickname;
            friendInDate = inDate;
        }

        private void SubscribeChatButton()
        {
            
        }

        private void SubscribeBreakButton()
        {
            Backend.Social.Friend.BreakFriend(friendInDate);
        }
    }
}