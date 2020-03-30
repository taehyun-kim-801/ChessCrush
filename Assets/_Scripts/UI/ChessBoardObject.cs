using UnityEngine;

namespace ChessCrush.UI
{
    public class ChessBoardObject: MonoBehaviour
    {
        private RectTransform rectTransform;
        protected ChessBoardVector chessBoardPosition = new ChessBoardVector();

        protected void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        protected virtual void Initialize(int x, int y)
        {
            chessBoardPosition.x = x;
            chessBoardPosition.y = y;
            if (transform.parent.GetComponent<RectTransform>().TryGetAnchorPreset(out AnchorPreset anchorPreset))
                rectTransform.anchoredPosition = chessBoardPosition.ToWorldVector().ToAnchoredPosition(anchorPreset);
        }
    }
}