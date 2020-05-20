using BackEnd;
using ChessCrush.Game;
using LitJson;
using System.Collections.Generic;
using UniRx;
using UnityEditor.PackageManager.Requests;
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

            acceptButton.OnClickAsObservable().Subscribe(_ => SubscribeAcceptButton()).AddTo(gameObject);
            rejectButton.OnClickAsObservable().Subscribe(_ => SubscribeRejectButton()).AddTo(gameObject);
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

        private void GetFriendList() => backendDirector.GetFriendList(SetAfterGetFriendList);

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

        private void SubscribeAcceptButton()
        {
            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.Social.Friend.AcceptFriend(requestList[0].inDate, c =>
            {
                bro = c;
                success.Value = true;
            });

            success.ObserveOnMainThread().Subscribe(value =>
            {
                if (value)
                {
                    if (bro.IsSuccess())
                    {
                        GetFriendList();
                        requestList.RemoveAt(0);
                        AppearReceivedRequest();
                    }
                    else
                        MessageBoxUI.UseWithComponent("Failed to accept friend");

                    bro.Clear();
                    success.Dispose();
                }
            });
        }

        private void SubscribeRejectButton()
        {
            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.Social.Friend.RejectFriend(requestList[0].inDate, c =>
            {
                bro = c;
                success.Value = true;
            });

            success.ObserveOnMainThread().Subscribe(value =>
            {
                if (value)
                {
                    if (bro.IsSuccess())
                    {
                        requestList.RemoveAt(0);
                        AppearReceivedRequest();
                    }
                    else
                        MessageBoxUI.UseWithComponent("Failed to reject friend");

                    bro.Clear();
                    success.Dispose();
                }
            });
        }
    }
}