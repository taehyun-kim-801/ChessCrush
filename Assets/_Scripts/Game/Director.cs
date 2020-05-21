using ChessCrush.UI;
using System.Collections;
using UnityEngine;
using BackEnd;
using System;
using UniRx;

namespace ChessCrush.Game
{
    public class Director: MonoBehaviour
    {
        static public Director instance;
        [NonSerialized]
        public ObjectPool nonUiObjectPool;
        private DirectorsPool directorsPool;

        public ReactiveProperty<UserInfo> userInfo = new ReactiveProperty<UserInfo>();
        public string playerName;
        public NetworkHelper networkHelper;

        private void Awake()
        {
            if(instance)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
            instance = this;

            networkHelper = new NetworkHelper();

            Instantiate(Resources.Load("Prefabs/MainCanvas") as GameObject);
            nonUiObjectPool = Instantiate(Resources.Load("Prefabs/NonUIObjectPool") as GameObject).GetComponent<ObjectPool>();
            directorsPool = Instantiate(Resources.Load("Prefabs/DirectorsPool") as GameObject).GetComponent<DirectorsPool>();
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => nonUiObjectPool.isCreated && MainCanvas.instance.objectPool.isCreated && Backend.IsInitialized);
            GetSubDirector<BackendDirector>();
            GetSubDirector<StartSceneDirector>();
        }
        
        public T GetSubDirector<T>() where T : SubDirector => directorsPool.UseDirector<T>();

        public void DestroySubDirector<T>(T subDirector) where T: SubDirector
        {
            directorsPool.Destroy(subDirector.gameObject);
        }

        public void GetUserInfo()
        {
            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.BMember.GetUserInfo(c =>
            {
                bro = c;
                success.Value = true;
            });

            success.Subscribe(value =>
            {
                if (value)
                {
                    if (bro.IsSuccess())
                    {
                        var infoJson = bro.GetReturnValuetoJSON()["row"];
                        SetUserInfoUsingJson(infoJson);
                        Backend.Match.JoinMatchMakingServer(out var errorInfo);
                    }
                   
                    bro.Clear();
                    success.Dispose();
                }
            });
        }

        private void SetUserInfoUsingJson(LitJson.JsonData jsonData)
        {
            var newUserInfo = new UserInfo();
            newUserInfo.nickname = ReferenceEquals(jsonData["nickname"], null) ? null : (string)jsonData["nickname"];
            newUserInfo.inDate = ReferenceEquals(jsonData["inDate"], null) ? null : (string)jsonData["inDate"];
            newUserInfo.subscriptionType = ReferenceEquals(jsonData["subscriptionType"], null) ? null : (string)jsonData["subscriptionType"];
            newUserInfo.emailForFindPassword = ReferenceEquals(jsonData["emailForFindPassword"], null) ? null : (string)jsonData["emailForFindPassword"];

            userInfo.Value = newUserInfo;
        }
    }
}