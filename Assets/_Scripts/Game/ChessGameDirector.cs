using ChessCrush.UI;
using System;
using System.Collections;
using UnityEngine;

namespace ChessCrush.Game
{
    public class ChessGameDirector : SubDirector
    {
        public Player player;
        public Player enemyPlayer;
        [NonSerialized]
        public ChessGameObjects chessGameObjects;
        [NonSerialized]
        public ChessGameUI chessGameUI;
        [SerializeField]
        private float InputTime;

        private void OnEnable()
        {
            player = new Player(Director.instance.playerName, true);
            enemyPlayer = new Player();
            var cgo = Director.instance.nonUiObjectPool.Use(nameof(ChessGameObjects));
            chessGameObjects = cgo.GetComponent<ChessGameObjects>();
            var cgUI = MainCanvas.instance.Use(nameof(ChessGameUI));
            chessGameUI = cgUI.GetComponent<ChessGameUI>();
            StartCoroutine(CoGamePlay());
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
        }

        private IEnumerator CoInput()
        {
            yield return new WaitForSeconds(InputTime);
        }

        private IEnumerator CoSimulate()
        {
            yield return null;
        }
    }
}