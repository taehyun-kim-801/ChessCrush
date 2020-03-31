using ChessCrush.UI;
using UnityEngine;

namespace ChessCrush.Game
{
    public class ChessGameDirector: SubDirector
    {
        //TODO: Player, Enemy Player field
        public ChessGameObjects chessGameObjects;

        private void OnEnable()
        {
            Director.instance.nonUiObjectPool.Use(nameof(ChessGameObjects));
            Director.instance.uiObjectPool.Use(nameof(ChessGameUI));
        }
    }
}