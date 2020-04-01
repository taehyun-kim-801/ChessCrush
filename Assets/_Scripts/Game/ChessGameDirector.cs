using ChessCrush.UI;
using UnityEngine;

namespace ChessCrush.Game
{
    public class ChessGameDirector: SubDirector
    {
        //TODO: Player, Enemy Player field
        public ChessGameObjects chessGameObjects;
        public ChessGameUI chessGameUI;

        private void OnEnable()
        {
            var cgo = Director.instance.nonUiObjectPool.Use(nameof(ChessGameObjects));
            chessGameObjects = cgo.GetComponent<ChessGameObjects>();
            var cgUI = MainCanvas.instance.Use(nameof(ChessGameUI));
            chessGameUI = cgUI.GetComponent<ChessGameUI>();
        }
    }
}