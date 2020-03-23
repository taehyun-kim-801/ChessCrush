using UnityEngine;

namespace ChessCrush
{
    public static class Vector3Extensions
    {
        private static RectTransform canvasRectTransform;
        public static Vector3 ToCanvasPosition(this Vector3 worldPosition)
        {
            if (canvasRectTransform is null)
                canvasRectTransform = MainCanvas.instance.GetComponent<RectTransform>();
            Vector2 viewportPosition = Camera.main.WorldToViewportPoint(worldPosition);
            Vector2 canvasPosition = new Vector2(
                ((viewportPosition.x * canvasRectTransform.sizeDelta.x) - (canvasRectTransform.sizeDelta.x * 0.5f)),
                ((viewportPosition.y * canvasRectTransform.sizeDelta.y) - (canvasRectTransform.sizeDelta.y * 0.5f)));

            return canvasPosition;
        }
    }
}