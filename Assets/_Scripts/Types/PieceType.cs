namespace ChessCrush
{
    public enum PieceType { Pawn = 1, Bishop, Knight, Rook, Queen, King }

    public static class PieceTypeExtensions
    {
        public static int GetNeedEnergy(this PieceType pieceType)
        {
            switch(pieceType)
            {
                case PieceType.Pawn: return 1;
                case PieceType.Bishop: return 3;
                case PieceType.Knight: return 3;
                case PieceType.Rook: return 4;
                case PieceType.Queen: return 6;
                case PieceType.King: return 9;
                default: return 0;
            }
        }

        public static int GetPower(this PieceType pieceType)
        {
            switch (pieceType)
            {
                case PieceType.Pawn: return 1;
                case PieceType.Bishop: return 2;
                case PieceType.Knight: return 2;
                case PieceType.Rook: return 3;
                case PieceType.Queen: return 4;
                case PieceType.King: return 7;
                default: return 0;
            }
        }
    }
}
