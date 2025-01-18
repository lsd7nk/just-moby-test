﻿using App.AppStates;
using App.Bootstrap.LoadingSegments;
using App.Common;
using Cysharp.Threading.Tasks;
using MiniContainer;
using UnityEngine;

namespace App.Bootstrap
{
    public sealed class AppBootstrap : Bootstrap
    {
        private AppStateService _stateService;
        private LoadingViewService _loadingService;

        protected override void Resolve()
        {
            base.Resolve();

            _stateService = DIContainer.Resolve<AppStateService>();
            _loadingService = DIContainer.Resolve<LoadingViewService>();
        }

        protected override void RegisterLoadingFlow()
        {
            RegisterLoadingSegment<EmptyLoadingSegment>();
        }

        protected override void UpdateProgressor(float value)
        {
            _loadingService?.SetLoadingProgress(value);
        }

        protected override void OnReady()
        {
            OnReadyAsync().Forget();
        }

        protected override void Awake()
        {
            base.Awake();

            Application.targetFrameRate = 60;
            Input.multiTouchEnabled = false;
        }

        private async UniTaskVoid OnReadyAsync()
        {
            await _stateService.GoToState(AppStateType.Lobby, new AppStatePayload());
        }
    }
}
