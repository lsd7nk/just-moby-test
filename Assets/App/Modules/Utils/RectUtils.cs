using UnityEngine;

namespace App.Utils
{
    public static class RectUtils
    {
        public static bool CheckRectInOtherRect(RectTransform rect1, RectTransform rect2)
        {
            var parentRect = GetWorldRect(rect2);
            var childRect = GetWorldRect(rect1);

            return !CheckObjectOutOfOtherObject(parentRect, childRect);
        }

        public static bool CheckRectsTouching(RectTransform rect1, RectTransform rect2, float offset = 0)
        {
            var rect1World = GetWorldRect(rect1);
            var rect2World = GetWorldRect(rect2);

            return CheckBottomTouchesTop(rect1World, rect2World, offset);
        }

        private static bool CheckBottomTouchesTop(Rect rect1, Rect rect2, float offset = 0)
        {
            return Mathf.Abs(rect1.yMin - rect2.yMax) <= offset;
        }

        private static bool CheckObjectOutOfOtherObject(Rect parentRect, Rect childRect)
        {
            if (childRect.xMin < parentRect.xMin || childRect.xMax > parentRect.xMax ||
                childRect.yMin < parentRect.yMin || childRect.yMax > parentRect.yMax)
            {
                return true;
            }

            return false;
        }

        private static Rect GetWorldRect(RectTransform rectTransform)
        {
            var corners = new Vector3[4];

            rectTransform.GetWorldCorners(corners);

            var size = new Vector2(corners[2].x - corners[0].x, corners[2].y - corners[0].y);

            return new Rect(corners[0], size);
        }
    }
}