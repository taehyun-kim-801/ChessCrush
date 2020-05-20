using ChessCrush.UI;
using System;
using System.Collections;
using UnityEngine;

namespace ChessCrush.Game
{
    public class ChessGameDirector : SubDirector
    {
        [NonSerialized]
        public bool inputCompleted;
        [NonSerialized]
        public ChessGameObjects chessGameObjects;
        [NonSerialized]
        public ChessGameUI chessGameUI;
        [SerializeField]
        private float InputTime;

        public Player player;
        public Player enemyPlayer;

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
            yield return StartCoroutine(CoSetAttackPlayer());
            while (true)
            {
                yield return StartCoroutine(CoInput());
                yield return StartCoroutine(CoSimulate());
            }
        }

        private IEnumerator CoSetAttackPlayer()
        {
            yield return null;
            player = new Player(Director.instance.playerName, true, true);
            enemyPlayer = new Player();
        }

        private IEnumerator CoInput()
        {
            float temp = Time.time;
            yield return new WaitUntil(() => inputCompleted || Time.time - temp > InputTime);
        }

        private IEnumerator CoSimulate()
        {
            yield return null;
        }
    }
}