namespace ChessCrush.Game
{
    public abstract class ChessPiece
    {
        public int x { get; private set; }
        public int y { get; private set; }

        public abstract void MoveTo(int x, int y);
    }
}