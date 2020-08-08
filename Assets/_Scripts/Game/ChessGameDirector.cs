using ChessCrush.UI;
using DG.Tweening;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace ChessCrush.Game
{
    public class ChessGameDirector : SubDirector
    {
        [NonSerialized]
        public bool inputCompleted;
        [NonSerialized]
        public ReactiveProperty<bool> ReceivedData = new ReactiveProperty<bool>();
        [NonSerialized]
        public ChessGameObjects chessGameObjects;
        [NonSerialized]
        public ChessGameUI chessGameUI;
        [SerializeField]
        private float InputTime;

        public Player player;
        public Player enemyPlayer;
        public bool isPlayerWin;

        public event Action gameReadyEvents;
        public event Action gameEndEvents;

        public ReactiveProperty<int?> myLocalEnergy = new ReactiveProperty<int?>();

        public ReactiveProperty<int> turnCount = new ReactiveProperty<int>();

        public Subject<int> expectedActionDeleteSubject = new Subject<int>();

        private Sequence actionAnimation;

        private bool initializedInReconnect;

        private BackendDirector backendDirector;

        private void Awake()
        {
            myLocalEnergy.Where(value => !(value is null)).Subscribe(value =>
              {
                  chessGameUI.myStatus.energyText.text = $"{value} / 10";
                  chessGameUI.myStatus.energyBar.fillAmount = (float)value / 10;
              }).AddTo(gameObject);

            gameEndEvents += () =>
            {
                turnCount.Value = 0;
                chessGameObjects.chessBoard.ClearChessPieces();
                chessGameObjects.chessBoard.ClearExpectedChessPieces();

                player.Dispose();
                player = null;
                enemyPlayer.Dispose();
                enemyPlayer = null;

                backendDirector.LeaveGameServer();
            };
        }

        private void Start()
        {
            backendDirector = Director.instance.GetSubDirector<BackendDirector>();
        }

        private void OnEnable()
        {
            var cgo = Director.instance.nonUiObjectPool.Use(nameof(ChessGameObjects));
            if (ReferenceEquals(chessGameObjects, null))
                chessGameObjects = cgo.GetComponent<ChessGameObjects>();

            var cgUI = MainCanvas.instance.Use(nameof(ChessGameUI));
            if (ReferenceEquals(chessGameUI, null))
                chessGameUI = cgUI.GetComponent<ChessGameUI>();

            StartCoroutine(CoGamePlay());
        }

        private void OnDisable()
        {
            chessGameObjects.gameObject.SetActive(false);
            chessGameUI.gameObject.SetActive(false);
            StopAllCoroutines();
        }

        private IEnumerator CoGamePlay()
        {
            yield return new WaitWhile(() => player is null || enemyPlayer is null);
            player.Hp.Where(value => value <= 0).Subscribe(_ =>
            {
                actionAnimation?.Kill(true);
                actionAnimation = null;
                StopAllCoroutines();

                backendDirector.MatchEnd(isPlayerWin = false);
            });

            enemyPlayer.Hp.Where(value => value <= 0).Subscribe(_ =>
            {
                actionAnimation?.Kill(true);
                actionAnimation = null;
                StopAllCoroutines();
                backendDirector.MatchEnd(isPlayerWin = true);
            });

            gameReadyEvents();

            while (true)
            {
                if (backendDirector.IsReconnect && !initializedInReconnect)
                    initializedInReconnect = true;
                else
                    turnCount.Value++;

                chessGameUI.SetSelectButtons(chessGameObjects.chessBoard.Pieces);
                myLocalEnergy.Value = player.EnergyPoint.Value;
                yield return StartCoroutine(CoInput());
                yield return StartCoroutine(CoSimulate());
            }
        }

        private IEnumerator CoInput()
        {
            float temp = Time.time;

            chessGameUI.SetInputAreaActive(true);
            yield return new WaitUntil(() => inputCompleted || Time.time - temp > InputTime);

            chessGameUI.SetInputAreaActive(false);

            var oms = new OutputMemoryStream();
            oms.Write(player.chessActions);
            backendDirector.SendDataToInGameRoom(oms.buffer);
        }

        private IEnumerator CoSimulate()
        {
            yield return new WaitUntil(() => ReceivedData.Value);

            myLocalEnergy.Value = null;
            chessGameUI.myStatus.energyText.text = $"{player.EnergyPoint.Value} / 10";
            chessGameUI.myStatus.energyBar.fillAmount = (float)player.EnergyPoint.Value / 10;

            chessGameObjects.DestroyExpectedAction();

            actionAnimation = DOTween.Sequence();
            if ((player.IsWhite && turnCount.Value % 2 != 0) || (!player.IsWhite && turnCount.Value % 2 == 0))
                actionAnimation = chessGameObjects.MakeActionAnimation(player, enemyPlayer);
            else
                actionAnimation = chessGameObjects.MakeActionAnimation(enemyPlayer, player);

            actionAnimation.Play();
            yield return new WaitUntil(() => !actionAnimation.IsPlaying());

            inputCompleted = false;
            ReceivedData.Value = false;

            player.chessActions.Clear();
            player.actionsSubject.OnNext(player.chessActions);

            enemyPlayer.chessActions.Clear();
            enemyPlayer.actionsSubject.OnNext(enemyPlayer.chessActions);

            actionAnimation.Kill(true);
        }

        public void SetPlayer(bool isWhite, string enemyName)
        {
            player = new Player(Director.instance.userInfo.Value.nickname, isWhite, true);
            enemyPlayer = new Player(enemyName, !isWhite, false);
        }

        public void GameEnd()
        {
            gameEndEvents();
            Director.instance.GetSubDirector<StartSceneDirector>();
            chessGameUI.gameOverWidget.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
