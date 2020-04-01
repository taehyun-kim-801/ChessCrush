using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChessCrush.Game
{
    public class SubDirectorsSet
    {
        public static Dictionary<Type, GameObject> directorDictionary;
        private static List<GameObject> subDirectors;
        static SubDirectorsSet()
        {
            directorDictionary = new Dictionary<Type, GameObject>();
            subDirectors = new List<GameObject>();
            Initialize();
        }

        static void Initialize()
        {
            subDirectors.Add(Resources.Load("Prefabs/ChessGameDirector") as GameObject);
            directorDictionary.Add(typeof(ChessGameDirector), null);
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