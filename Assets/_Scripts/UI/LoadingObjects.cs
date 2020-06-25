using ChessCrush.Game;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class LoadingObjects: MonoBehaviour
    {
        [SerializeField]
        private Image loadingBar;

        private readonly int AllLoadingCount = 2;
        private ReactiveProperty<int> LoadingCount = new ReactiveProperty<int>();

        private StartSceneDirector startSceneDirector;
        private BackendDirector backendDirector;

        private void Awake()
        {
            LoadingCount.Subscribe(value => loadingBar.fillAmount = (float)value / AllLoadingCount).AddTo(gameObject);
            LoadingCount.Where(value => value == AllLoadingCount).Subscribe(_ => startSceneDirector.startUI.AfterLoading()).AddTo(gameObject);
        }

        private void Start()
        {
            startSceneDirector = Director.instance.GetSubDirector<StartSceneDirector>();
            backendDirector = Director.instance.GetSubDirector<BackendDirector>();
        }

        private void OnEnable()
        {
            loadingBar.fillAmount = 0f;
            LoadingCount.Value = 0;
            StartCoroutine(CoEnable());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator CoEnable()
        {
            Director.instance.GetUserInfo();
            yield return new WaitUntil(() => !Director.instance.userInfo.Value.Equals(default));
            LoadingCount.Value++;

            yield return new WaitUntil(() => backendDirector.MatchMakingServerJoined);
            LoadingCount.Value++;
        }
    }
}