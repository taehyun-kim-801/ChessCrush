using ChessCrush.Game;
using System.Collections.Generic;
using System.Data.Common;

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
    }
}