using BackEnd;
using System.Collections.Generic;
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

        private ObjectPool contentObjectPool;
        private ReactiveProperty<List<UserInfo>> friendList = new ReactiveProperty<List<UserInfo>>();

        private void Awake()
        {
            contentObjectPool = myFriendsScrollContent.GetComponent<ObjectPool>();
            friendList.Value = new List<UserInfo>();

            goToSearchButton.OnClickAsObservable().Subscribe(_ => requestWidget.SetActive(true)).AddTo(gameObject);
            exitButton.OnClickAsObservable().Subscribe(_ => gameObject.SetActive(false)).AddTo(gameObject);
            friendList.Subscribe(_ => SubscribeFriendList()).AddTo(gameObject);
        }

        private void OnEnable()
        {
            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.Social.Friend.GetFriendList(c =>
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
                        LitJson.JsonData jsonData = bro.GetReturnValuetoJSON();

                        var newList = new List<UserInfo>();
                        if (jsonData["rows"].Count != 0)
                        {
                            for (int i = 0; i < jsonData["rows"].Count; i++)
                            {
                                var userInfo = new UserInfo();
                                userInfo.nickname = (string)jsonData["rows"][i]["nickname"];
                                userInfo.inDate = (string)jsonData["rows"][i]["inDate"];

                                newList.Add(userInfo);
                            }
                        }

                        friendList.Value = newList;
                    }

                    bro.Clear();
                    success.Dispose();
                }
            });
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
    }
}