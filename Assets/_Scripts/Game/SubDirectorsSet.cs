using System.Collections.Generic;
using UnityEngine;

namespace ChessCrush.Game
{
    public class SubDirectorsSet
    {
        private static List<GameObject> subDirectors;
        static SubDirectorsSet()
        {
            subDirectors = new List<GameObject>();
            Initialize();
        }

        static void Initialize()
        {
            subDirectors.Add(Resources.Load("Prefabs/ChessGameDirector") as GameObject);
        }
        public static T Find<T>() where T: SubDirector
        {
            var result = subDirectors.Find(go => !(go.GetComponent<T>() is null));
            if (result is null)
                return null;
            return result.GetComponent<T>();
        } 
    }
}