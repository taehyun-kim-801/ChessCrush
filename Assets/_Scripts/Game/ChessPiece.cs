namespace ChessCrush.Game
{
    public class ChessPiece
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public PieceType pieceType { get; private set; }

        public ChessPiece() { }
        public ChessPiece(int x,int y,PieceType pieceType)
        {
            this.x = x;
            this.y = y;
            this.pieceType = pieceType;
        }
        public ChessPiece(ChessPiece other)
        {
            x = other.x;
            y = other.y;
            pieceType = other.pieceType;
        }

        public void MoveTo(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}