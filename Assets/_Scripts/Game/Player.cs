using System.Collections.Generic;

namespace ChessCrush.Game
{
    public class Player
    {
        private string name;
        private bool isWhite;
        private int hp;
        private int energyPoint;
        public List<ChessAction> chessActions;

        public Player()
        {
            chessActions = new List<ChessAction>();
        }

        public Player(string name, bool isWhite)
        {
            this.name = name;
            this.isWhite = isWhite;
            hp = 20;
            energyPoint = 0;
            chessActions = new List<ChessAction>();
        }
    }
}