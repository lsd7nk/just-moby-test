using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using DG.Tweening;
using App.Utils;

namespace App.Core
{
    public sealed class FiguresBuilderView : MonoBehaviour
    {
        private int UNI_TASK_DELAY_MULTIPLIER = 1000;
        private float HIDE_ANIMATION_JUMP_FORCE = 1000f;
        private float HIDE_ANIMATION_DURATION = 1f;
        private int HIDE_ANIMATION_JUMP_COUNT = 1;

        private float ROTATE_ANIMATION_VALUE = 720f;

        [SerializeField] private RectTransform _placeFieldRect;
        [SerializeField] private RectTransform _hideTargetRect;

        public bool CheckFigureInField(RectTransform figureRect)
        {
            return RectUtils.CheckRectInOtherRect(figureRect, _placeFieldRect);
        }

        public async UniTask HideAsync(RectTransform[] rects, CancellationToken cancellationToken)
        {
            for (int i = 0; i < rects.Length; ++i)
            {
                var rect = rects[i];
                var position = _hideTargetRect.position;

                position.x += HIDE_ANIMATION_JUMP_FORCE;

                if (i % 2 == 0)
                {
                    position.x *= -1;
                }

                PlayScaleAnimation(rect);
                PlayRotationAnimation(rect);

                rect.DOJump(position,
                    HIDE_ANIMATION_JUMP_FORCE,
                    HIDE_ANIMATION_JUMP_COUNT,
                    HIDE_ANIMATION_DURATION)
                    .SetLink(rect.gameObject);
            }

            await UniTask.Delay((int)(HIDE_ANIMATION_DURATION * UNI_TASK_DELAY_MULTIPLIER));
        }

        private void PlayRotationAnimation(RectTransform rect)
        {
            rect.DORotate(new Vector3(0f, 0f, ROTATE_ANIMATION_VALUE), HIDE_ANIMATION_DURATION, RotateMode.FastBeyond360)
                .SetLink(rect.gameObject);
        }

        private void PlayScaleAnimation(RectTransform rect)
        {
            rect.DOScale(Vector3.zero, HIDE_ANIMATION_DURATION * 0.3f)
                .SetLink(rect.gameObject);
        }
    }
}