using ChessCrush.UI;
using System;
using UnityEngine;

namespace ChessCrush
{
    public static class Vector3Extensions
    {
        private static RectTransform canvasRectTransform;
        public static Vector3 ToAnchoredPosition(this Vector3 worldPosition, AnchorPreset anchorPreset)
        {
            if (canvasRectTransform is null)
                canvasRectTransform = MainCanvas.instance.GetComponent<RectTransform>();
            var viewportPosition = Camera.main.WorldToViewportPoint(worldPosition);
            var canvasMiddleCenterPosition = new Vector2(
                (viewportPosition.x - 0.5f) * canvasRectTransform.sizeDelta.x,
                (viewportPosition.y - 0.5f) * canvasRectTransform.sizeDelta.y);

            switch(anchorPreset)
            {
                case AnchorPreset.TopLeft:
                    return new Vector2(canvasMiddleCenterPosition.x + canvasRectTransform.sizeDelta.x * 0.5f, canvasMiddleCenterPosition.y + canvasRectTransform.sizeDelta.y * 0.5f);
                case AnchorPreset.TopCenter:
                case AnchorPreset.TopStretch:
                    return new Vector2(canvasMiddleCenterPosition.x, canvasMiddleCenterPosition.y + canvasRectTransform.sizeDelta.y * 0.5f);
                case AnchorPreset.TopRight:
                    return new Vector2(canvasMiddleCenterPosition.x - canvasRectTransform.sizeDelta.x * 0.5f, canvasMiddleCenterPosition.y + canvasRectTransform.sizeDelta.y * 0.5f);
                case AnchorPreset.MiddleLeft:
                case AnchorPreset.StretchLeft:
                    return new Vector2(canvasMiddleCenterPosition.x + canvasRectTransform.sizeDelta.x * 0.5f, canvasMiddleCenterPosition.y);
                case AnchorPreset.MiddleCenter:
                case AnchorPreset.MiddleStretch:
                case AnchorPreset.StretchMiddle:
                case AnchorPreset.StretchStretch:
                    return canvasMiddleCenterPosition;
                case AnchorPreset.MiddleRight:
                case AnchorPreset.StretchRight:
                    return new Vector2(canvasMiddleCenterPosition.x - canvasRectTransform.sizeDelta.x * 0.5f, canvasMiddleCenterPosition.y);
                case AnchorPreset.BottomLeft:
                    return new Vector2(canvasMiddleCenterPosition.x + canvasRectTransform.sizeDelta.x * 0.5f, canvasMiddleCenterPosition.y - canvasRectTransform.sizeDelta.y * 0.5f);
                case AnchorPreset.BottomCenter:
                case AnchorPreset.BottomStretch:
                    return new Vector2(canvasMiddleCenterPosition.x, canvasMiddleCenterPosition.y - canvasRectTransform.sizeDelta.y * 0.5f);
                case AnchorPreset.BottomRight:
                    return new Vector2(canvasMiddleCenterPosition.x - canvasRectTransform.sizeDelta.x * 0.5f, canvasMiddleCenterPosition.y - canvasRectTransform.sizeDelta.y * 0.5f);
                default:
                    throw new ArgumentException("No custom anchor");
            }
        }
    }
}