using ChessCrush.Game;
using System.Collections.Generic;

namespace ChessCrush
{
    public static class MemoryStreamExtensions
    {
        public static void Write(this OutputMemoryStream oms, ChessAction data) => data.Write(oms);

        public static void Read(this InputMemoryStream ims, out ChessAction res)
        {
            res = new ChessAction();
            res.Read(ims);
        }

        public static void Write(this OutputMemoryStream oms, List<ChessAction> data)
        {
            oms.Write(data.Count);
            foreach(var d in data)
                oms.Write(d);
        }

        public static void Read(this InputMemoryStream ims, out List<ChessAction> res)
        {
            ims.Read(out int count);
            res = new List<ChessAction>();

            for(int i=0;i<count;i++)
            {
                ims.Read(out ChessAction action);
                res.Add(action);
            }
        }

        public static void Write(this OutputMemoryStream oms, ChessPiece data) => data.Write(oms);

        public static void Read(this InputMemoryStream ims, out ChessPiece res)
        {
            ims.Read(out int pieceId);
            ims.Read(out int pieceType);
            ims.Read(out int x);
            ims.Read(out int y);
            ims.Read(out bool isMine);

            res = ChessPiece.UseWithComponent(pieceId, x, y, (PieceType)pieceType, false, isMine);
        }

        public static void Write(this OutputMemoryStream oms,List<ChessPiece> data)
        {
            oms.Write(data.Count);
            data.ForEach(piece => oms.Write(piece));
        }

        public static void Read(this InputMemoryStream ims,out List<ChessPiece> res)
        {
            ims.Read(out int count);
            res = new List<ChessPiece>();

            for(int i=0;i<count;i++)
            {
                ims.Read(out ChessPiece piece);
                res.Add(piece);
            }
        }
    }
}