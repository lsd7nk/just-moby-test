using Cysharp.Threading.Tasks;
using System.Threading;
using App.Common.Views;
using App.Vibrations;
using UnityEngine;
using App.Events;
using App.Popups;
using App.Core;
using System;

namespace App.Common
{
    public sealed class GameViewService : ViewService<GameView>, IDisposable
    {
        private readonly IPopupFactoryService _popupFactory;
        private readonly VibrationsService _vibrations;

        private readonly IFigureModelsFactory _figureModelsFactory;
        private readonly IFiguresBuilder _figuresBuilder;

        private readonly LevelModel _levelModel;

        public GameViewService(IPopupFactoryService popupFactory,
            AppConfig appConfig, VibrationsService vibrations)
        {
            _popupFactory = popupFactory;
            _vibrations = vibrations;

            _levelModel = new LevelModel(appConfig.FiguresCount);
            _figureModelsFactory = new FigureModelsHSVFactory();
            _figuresBuilder = new FiguresBuilder();
        }

        public override void Initialize()
        {
            EventSystem.Subscribe<RestartLevelEvent>(OnLevelRestart);

            _figuresBuilder.SetView(_view.FiguresBuilderView);
            _figuresBuilder.Initialize();

            _figuresBuilder.OnFigurePlacedUncorrectlyEvent += DestroyFigure;
            _figuresBuilder.OnFigurePlacedCorrectlyEvent += MoveFigure;
            _figuresBuilder.OnFigureTakeFromScrollEvent += CopyFigure;

            CreateFigures();
        }

        public void Dispose()
        {
            EventSystem.Unsubscribe<RestartLevelEvent>(OnLevelRestart);

            _figuresBuilder.OnFigurePlacedUncorrectlyEvent -= DestroyFigure;
            _figuresBuilder.OnFigurePlacedCorrectlyEvent -= MoveFigure;
            _figuresBuilder.OnFigureTakeFromScrollEvent -= CopyFigure;

            _figuresBuilder.Dispose();
        }

        public override async UniTask HideViewAsync(CancellationToken cancellationToken)
        {
            _figuresBuilder.HideViewAsync(cancellationToken).Forget();

            await base.HideViewAsync(cancellationToken);
        }

        protected override void OnViewSet()
        {
            _view.Initialize(_levelModel.FiguresCount);
            _view.AddSettingsButtonOnCickHandler(OnSettingsButtonClick);
        }

        private void CreateFigures()
        {
            int slotsCount = _view.LastSlotIndex + 1;
            int figuresCount = _levelModel.FiguresCount > slotsCount
                ? slotsCount
                : _levelModel.FiguresCount;

            var figures = _figureModelsFactory.GetFigureModels(figuresCount);

            for (int i = 0; i < figuresCount; ++i)
            {
                var figure = figures[i];
                var figureView = _view.CreateFigureView(figure.Color, i);

                figure.SetView(figureView);

                _figuresBuilder.AddFigure(figure);
            }
        }

        private void CopyFigure(FigureModel figure)
        {
            var copiedFigure = _figureModelsFactory.GetFigureModel(figure.Color, figure.Index);
            var figureView = _view.CreateFigureView(copiedFigure.Color, copiedFigure.Index);

            copiedFigure.SetView(figureView);

            _figuresBuilder.AddFigure(copiedFigure);
            _vibrations.PlayLightImpactVibration();
        }

        private void DestroyFigure(FigureModel figure)
        {
            figure.GetDraggable().Interactable = false;

            _view.PlayDestroyAnimation(figure.GetRectTransform(), () =>
            {
                figure.Dispose();
            });

            _vibrations.PlayFailureVibration();
        }

        private void MoveFigure(FigureModel figure, Vector3 position)
        {
            var draggable = figure.GetDraggable();

            draggable.Interactable = false;
            draggable.CancelDrag();

            _view.PlayJumpAnimation(figure.GetRectTransform(), position, () =>
            {
                draggable.Interactable = true;
            });

            _vibrations.PlaySuccessVibration();
        }

        private void OnLevelRestart(RestartLevelEvent e)
        {
            var placedFigures = _figuresBuilder.GetPlacedFigures();

            for (int i = 0; i < placedFigures.Length; ++i)
            {
                placedFigures[i].Dispose();
            }

            _figuresBuilder.OnLevelRestart();
        }

        private void OnSettingsButtonClick()
        {
            _popupFactory.ShowPopup<SettingsPopup>(true);
        }
    }
}
