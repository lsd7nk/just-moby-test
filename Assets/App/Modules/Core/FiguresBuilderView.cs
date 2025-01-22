using UnityEngine;
using App.Utils;

namespace App.Core
{
    public sealed class FiguresBuilderView : MonoBehaviour
    {
        [SerializeField] private RectTransform _fieldRect;

        public bool CheckFigureInField(RectTransform figureRect)
        {
            return RectUtils.CheckRectInOtherRect(figureRect, _fieldRect);
        }
    }
}