using UnityEngine;

namespace ChessCrush
{
    public enum AnchorPreset
    {
        TopLeft,
        TopCenter,
        TopRight,
        TopStretch,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        MiddleStretch,
        BottomLeft,
        BottomCenter,
        BottomRight,
        BottomStretch,
        StretchLeft,
        StretchMiddle,
        StretchRight,
        StretchStretch,
    }

    public enum PivotPreset
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    public static class RectTransformExtensions
    {
        public static bool TryGetAnchorPreset(this RectTransform rectTransform, out AnchorPreset anchorPreset)
        {
            if (rectTransform.anchorMin.x == rectTransform.anchorMax.x && rectTransform.anchorMin.y == rectTransform.anchorMax.y)
            {
                if (rectTransform.anchorMin.x == 0)
                {
                    if (rectTransform.anchorMin.y == 1)
                        anchorPreset = AnchorPreset.TopLeft;
                    else if (rectTransform.anchorMin.y == 0.5f)
                        anchorPreset = AnchorPreset.MiddleLeft;
                    else if (rectTransform.anchorMin.y == 0)
                        anchorPreset = AnchorPreset.BottomLeft;
                    else
                    {
                        anchorPreset = default;
                        return false;
                    }
                }
                else if (rectTransform.anchorMin.x == 0.5f)
                {
                    if (rectTransform.anchorMin.y == 1)
                        anchorPreset = AnchorPreset.TopCenter;
                    else if (rectTransform.anchorMin.y == 0.5f)
                        anchorPreset = AnchorPreset.MiddleCenter;
                    else if (rectTransform.anchorMin.y == 0)
                        anchorPreset = AnchorPreset.BottomCenter;
                    else
                    {
                        anchorPreset = default;
                        return false;
                    }
                }
                else if (rectTransform.anchorMin.x == 1f)
                {
                    if (rectTransform.anchorMin.y == 1)
                        anchorPreset = AnchorPreset.TopRight;
                    else if (rectTransform.anchorMin.y == 0.5f)
                        anchorPreset = AnchorPreset.MiddleRight;
                    else if (rectTransform.anchorMin.y == 0)
                        anchorPreset = AnchorPreset.BottomRight;
                    else
                    {
                        anchorPreset = default;
                        return false;
                    }
                }
                else
                {
                    anchorPreset = default;
                    return false;
                }
                return true;
            }
            else if (rectTransform.anchorMax.x - rectTransform.anchorMin.x == 1 && rectTransform.anchorMin.y == rectTransform.anchorMax.y)
            {
                if (rectTransform.anchorMin.y == 1)
                    anchorPreset = AnchorPreset.TopStretch;
                else if (rectTransform.anchorMin.y == 0.5f)
                    anchorPreset = AnchorPreset.MiddleStretch;
                else if (rectTransform.anchorMin.y == 0)
                    anchorPreset = AnchorPreset.BottomStretch;
                else
                {
                    anchorPreset = default;
                    return false;
                }
                return true;
            }
            else if(rectTransform.anchorMin.x == rectTransform.anchorMax.x && rectTransform.anchorMax.y-rectTransform.anchorMin.y==1)
            {
                if (rectTransform.anchorMin.x == 0)
                    anchorPreset = AnchorPreset.StretchLeft;
                else if (rectTransform.anchorMin.x == 0.5f)
                    anchorPreset = AnchorPreset.StretchMiddle;
                else if (rectTransform.anchorMin.x == 1f)
                    anchorPreset = AnchorPreset.StretchRight;
                else
                {
                    anchorPreset = default;
                    return false;
                }
                return true;
            }
            else if(rectTransform.anchorMax.x - rectTransform.anchorMin.x == 1 && rectTransform.anchorMax.y - rectTransform.anchorMin.y == 1)
            {
                anchorPreset = AnchorPreset.StretchStretch;
                return true;
            }
            else
            {
                anchorPreset = default;
                return false;
            }
        }

        public static bool TryGetPivotPreset(this RectTransform rectTransform,out PivotPreset pivotPreset)
        {
            int pivotPresetIndex = 0;
            switch(rectTransform.pivot.y)
            {
                case 0:
                    pivotPresetIndex += 6;
                    break;
                case 0.5f:
                    pivotPresetIndex += 3;
                    break;
                case 1:
                    break;
                default:
                    pivotPreset = default;
                    return false;
            }

            switch(rectTransform.pivot.x)
            {
                case 0:
                    break;
                case 0.5f:
                    pivotPresetIndex += 1;
                    break;
                case 1:
                    pivotPresetIndex += 2;
                    break;
                default:
                    pivotPreset = default;
                    return false;
            }

            pivotPreset = (PivotPreset)pivotPresetIndex;
            return true;
        }
    }
}