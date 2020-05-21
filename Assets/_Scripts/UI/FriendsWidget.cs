using ChessCrush.Game;
using LitJson;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class FriendsWidget: MonoBehaviour
    {
        [SerializeField]
        private GameObject requestView;
        [SerializeField]
        private Text requestText;
        [SerializeField]
        private Button acceptButton;
        [SerializeField]
        private Button rejectButton;
        [SerializeField]
        private GameObject myFriendsScrollContent;
        [SerializeField]
        private Button goToSearchButton;
        [SerializeField]
        private GameObject requestWidget;
        [SerializeField]
        private Button exitButton;

        private ObjectPool contentObjectPool;
        private ReactiveProperty<List<UserInfo>> friendList = new ReactiveProperty<List<UserInfo>>();
        private List<UserInfo> requestList = new List<UserInfo>();

        private BackendDirector backendDirector;

        private void Awake()
        {
            contentObjectPool = myFriendsScrollContent.GetComponent<ObjectPool>();
            friendList.Value = new List<UserInfo>();

            acceptButton.OnClickAsObservable().Subscribe(_ => backendDirector.AcceptFriend(requestList[0].inDate, SetAfterAcceptFriend, str => MessageBoxUI.UseWithComponent(str))).AddTo(gameObject);
            rejectButton.OnClickAsObservable().Subscribe(_ => backendDirector.RejectFriend(requestList[0].inDate, SetAfterRejectFriend, str => MessageBoxUI.UseWithComponent(str))).AddTo(gameObject);
            goToSearchButton.OnClickAsObservable().Subscribe(_ => requestWidget.SetActive(true)).AddTo(gameObject);
            exitButton.OnClickAsObservable().Subscribe(_ => gameObject.SetActive(false)).AddTo(gameObject);
            friendList.Subscribe(_ => SubscribeFriendList()).AddTo(gameObject);
        }

        private void Start()
        {
            backendDirector = Director.instance.GetSubDirector<BackendDirector>();
        }

        private void OnEnable()
        {
            backendDirector.GetFriendList(SetAfterGetFriendList);
            backendDirector.GetReceivedRequestList(SetAfterGetReceivedRequestList);
        }

        private void SetAfterGetFriendList(JsonData jsonData)
        {
            var newList = new List<UserInfo>();
            if (jsonData["rows"].Count != 0)
            {
                for (int i = 0; i < jsonData["rows"].Count; i++)
                {
                    var userInfo = new UserInfo();
                    userInfo.nickname = (string)jsonData["rows"][i]["nickname"]["S"];
                    userInfo.inDate = (string)jsonData["rows"][i]["inDate"]["S"];

                    newList.Add(userInfo);
                }
            }

            friendList.Value = newList;
        }

        private void SetAfterGetReceivedRequestList(JsonData jsonData)
        {
            var newList = new List<UserInfo>();
            if (jsonData["rows"].Count != 0)
            {
                for (int i = 0; i < jsonData["rows"].Count; i++)
                {
                    var userInfo = new UserInfo();
                    userInfo.nickname = (string)jsonData["rows"][i]["nickname"]["S"];
                    userInfo.inDate = (string)jsonData["rows"][i]["inDate"]["S"];

                    newList.Add(userInfo);
                }
            }

            requestList = newList;
            AppearReceivedRequest();
        }

        private void SubscribeFriendList()
        {
            contentObjectPool.DestroyAll();

            foreach(var info in friendList.Value)
            {
                var view = contentObjectPool.Use(nameof(FriendsView)).GetComponent<FriendsView>();
                view.Set(info.nickname, info.inDate);
            }
        }

        private void AppearReceivedRequest()
        {
            if (requestList.Count > 0)
            {
                requestView.SetActive(true);
                requestText.text = requestList[0].nickname;
            }
            else
                requestView.SetActive(false);
        }

        private void SetAfterAcceptFriend()
        {
            backendDirector.GetFriendList(SetAfterGetFriendList);
            requestList.RemoveAt(0);
            AppearReceivedRequest();
        }

        private void SetAfterRejectFriend()
        {
            requestList.RemoveAt(0);
            AppearReceivedRequest();
        }
    }
}