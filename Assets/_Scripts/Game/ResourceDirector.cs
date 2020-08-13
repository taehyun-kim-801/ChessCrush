using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.U2D;

namespace ChessCrush.Game
{
    public class ResourceDirector: SubDirector
    {
        private SpriteAtlas chessSpriteAtlas;
        private Queue<Object> inGameResources = new Queue<Object>();
        private readonly string ChessPiecePath = "Textures/Chess/chessPiece/{0}";

        private ChessGameDirector chessGameDirector;

        private void Start()
        {
            chessGameDirector = Director.instance.PeekSubDirector<ChessGameDirector>();
            chessGameDirector.gameReadyEvents += LoadChessSprite;
            chessGameDirector.gameEndEvents += UnloadChessGameAssets;
        }

        private void LoadChessSprite()
        {
            chessSpriteAtlas = Resources.Load<SpriteAtlas>(string.Format(ChessPiecePath, "ChessSpriteAtlas"));
            inGameResources.Enqueue(chessSpriteAtlas);
        }

        public void UnloadChessGameAssets()
        {
            while (inGameResources.Count != 0)
                Resources.UnloadAsset(inGameResources.Dequeue());
        }

        public Sprite GetChessSprite(PieceType pieceType, bool isWhite)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(isWhite ? "White_" : "Black_");
            stringBuilder.Append(pieceType.ToString());
            return chessSpriteAtlas.GetSprite(stringBuilder.ToString());
        }
    }
}
