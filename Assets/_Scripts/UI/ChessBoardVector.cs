using UnityEngine;

namespace ChessCrush.UI
{
    public class ChessBoardVector
    {
        public int x;
        public int y;

        static Vector2 originPosition;
        private static readonly float ChessBoardBlockSize = 0.73f;

        public Vector3 ToWorldVector()
        {
            if (originPosition == default)
                originPosition = Game.instance.chessBoardOrigin.position;
            return new Vector3(x * ChessBoardBlockSize + originPosition.x, y * ChessBoardBlockSize + originPosition.y);
        }
    }
}