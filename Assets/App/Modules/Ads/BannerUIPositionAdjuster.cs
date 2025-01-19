using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using System;

namespace App.UI
{
    public class BannerUIPositionAdjuster : BannerAdjuster<RectTransform>
    {
        [Serializable]
        private sealed class PlatformSpecificValues
        {
            [field: SerializeField] public RuntimePlatform Platform { get; private set; }
            [field: SerializeField] public Vector3 bannerHiddenPosition { get; private set; }
            [field: SerializeField] public Vector3 bannerVisiblePosition { get; private set; }
        }


        [SerializeField] private Vector3 _bannerHiddenPosition;
        [SerializeField] private Vector3 _bannerVisiblePosition;
        [SerializeField] private List<PlatformSpecificValues> _platformSpecifics;

        [ContextMenu("OnBannerShown")]
        protected override void OnBannerShown()
        {
            if (adjustableViews == null)
            {
                return;
            }

            var bannerPosition = _bannerVisiblePosition;

            foreach (var spec in _platformSpecifics)
            {
                if (spec.Platform == Application.platform)
                {
                    bannerPosition = spec.bannerVisiblePosition;
                    break;
                }
            }

            foreach (var view in adjustableViews)
            {
                if (view == null)
                {
                    continue;
                }

                view.anchoredPosition = bannerPosition;

                var uiView = view.GetComponent<UIView>();

                if (uiView != null)
                {
                    uiView.CustomStartAnchoredPosition = bannerPosition;
                    uiView.UseCustomStartAnchoredPosition = true;
                }
            }
        }

        [ContextMenu("OnBannerHidden")]
        protected override void OnBannerHidden()
        {
            if (adjustableViews == null)
            {
                return;
            }

            var bannerPosition = _bannerHiddenPosition;

            foreach (var spec in _platformSpecifics)
            {
                if (spec.Platform == Application.platform)
                {
                    bannerPosition = spec.bannerHiddenPosition;
                    break;
                }
            }

            foreach (var view in adjustableViews)
            {
                if (view == null)
                {
                    continue;
                }

                view.anchoredPosition = bannerPosition;

                var uiView = view.GetComponent<UIView>();

                if (uiView != null)
                {
                    uiView.CustomStartAnchoredPosition = bannerPosition;
                    uiView.UseCustomStartAnchoredPosition = true;
                }
            }
        }
    }
}
