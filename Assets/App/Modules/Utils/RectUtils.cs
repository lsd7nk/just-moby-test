using UnityEngine;

namespace App.Utils
{
    public static class RectUtils
    {
        public static bool CheckRectsTouching(RectTransform rect1, RectTransform rect2, float offset = 0)
        {
            var rect1World = GetWorldRect(rect1);
            var rect2World = GetWorldRect(rect2);

            return CheckBottomTouchesTop(rect1World, rect2World, offset);
        }

        public static bool CheckBottomTouchesTop(Rect rect1, Rect rect2, float offset = 0)
        {
            return Mathf.Abs(rect1.yMin - rect2.yMax) <= offset;
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