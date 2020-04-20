using ChessCrush.Game;
using System.Collections.Generic;
using UnityEngine;

namespace ChessCrush.UI
{
    public class ChessActionScroll: MonoBehaviour
    {
        [SerializeField]
        private Transform content;
        private ObjectPool viewPool;

        private void Awake()
        {
            viewPool = gameObject.GetComponent<ObjectPool>();
        }

        public void SetView(List<ChessAction> chessActions)
        {
            viewPool.DestroyAll();
            foreach(var action in chessActions)
            {
                var view = viewPool.Use(nameof(ChessActionView)).GetComponent<ChessActionView>();
                view.transform.parent = content;
                view.SetText(action);
            }
        }
    }
}