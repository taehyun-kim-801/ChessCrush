using UnityEngine;

namespace ChessCrush.UI
{
    public class ChessBoardObject: MonoBehaviour
    {
        RectTransform rectTransform;
        ChessBoardVector chessBoardPosition = new ChessBoardVector();

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void Initialize(int x, int y)
        {
            chessBoardPosition.x = x;
            chessBoardPosition.y = y;
            if (rectTransform.TryGetAnchorPreset(out AnchorPreset anchorPreset))
                rectTransform.anchoredPosition = chessBoardPosition.ToWorldVector().ToAnchoredPosition(anchorPreset);
        }
    }
}