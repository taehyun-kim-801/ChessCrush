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
        public bool receivedData;
        [NonSerialized]
        public ChessGameObjects chessGameObjects;
        [NonSerialized]
        public ChessGameUI chessGameUI;
        [SerializeField]
        private float InputTime;

        public Player player;
        public Player enemyPlayer;

        public ReactiveProperty<int> turnCount = new ReactiveProperty<int>();

        private BackendDirector backendDirector;

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
            while (true)
            {
                turnCount.Value++;
                yield return StartCoroutine(CoInput());
                yield return StartCoroutine(CoSimulate());
            }
        }

        private IEnumerator CoInput()
        {
            float temp = Time.time;
            yield return new WaitUntil(() => inputCompleted || Time.time - temp > InputTime);
            var oms = new OutputMemoryStream();
            oms.Write(player.chessActions);
            backendDirector.SendDataToInGameRoom(oms.buffer);
        }

        private IEnumerator CoSimulate()
        {
            yield return new WaitUntil(() => receivedData);
            chessGameObjects.DestroyExpectedAction();

            Sequence seq = DOTween.Sequence();
            if ((player.IsWhite && turnCount.Value % 2 != 0) || (!player.IsWhite && turnCount.Value % 2 == 0))
                seq = chessGameObjects.MakeActionAnimation(player, enemyPlayer);
            else
                seq = chessGameObjects.MakeActionAnimation(enemyPlayer, player);

            seq.Play();
            yield return new WaitUntil(() => seq.IsPlaying());

            inputCompleted = false;
            receivedData = false;

            seq.Kill(true);
        }

        public void SetPlayer(bool isWhite, string enemyName)
        {
            player = new Player(Director.instance.userInfo.Value.nickname, isWhite, true);
            enemyPlayer = new Player(enemyName, !isWhite, false);
        }
    }
}