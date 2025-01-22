using Cysharp.Threading.Tasks;
using System.Threading;
using App.Common.Views;
using UnityEngine;
using App.Popups;
using App.Core;
using App.Ads;
using System;

namespace App.Common
{
    public sealed class GameViewService : ViewService<GameView>, IDisposable
    {
        private readonly IPopupFactoryService _popupFactory;
        private readonly AppConfig _appConfig;

        private readonly IFigureModelsFactory _figureModelsFactory;
        private readonly IFiguresBuilder _figuresBuilder;

        private readonly LevelModel _levelModel;

        public GameViewService(IAdService adService, IPopupFactoryService popupFactory,
            AppConfig appConfig, AppStateService appStates)
        {
            _popupFactory = popupFactory;
            _appConfig = appConfig;

            _levelModel = new LevelModel(_appConfig.FiguresCount);
            _figureModelsFactory = new FigureModelsHSVFactory();
            _figuresBuilder = new FiguresBuilder();
        }

        public override void Initialize()
        {
            _figuresBuilder.SetView(_view.FiguresBuilderView);
            _figuresBuilder.Initialize();

            _figuresBuilder.OnFigurePlacedUncorrectlyEvent += DestroyFigure;
            _figuresBuilder.OnFigurePlacedCorrectlyEvent += MoveFigure;
            _figuresBuilder.OnFigureTakeFromScrollEvent += CopyFigure;

            CreateFigures();
        }

        public void Dispose()
        {
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
            _view.Initialize(_appConfig.FiguresCount);
            _view.AddSettingsButtonOnCickHandler(OnSettingsButtonClick);
        }

        private void CreateFigures()
        {
            int figuresCount = _levelModel.FiguresCount;
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
            var figureView = _view.CopyFigureView(copiedFigure.Color, copiedFigure.Index);

            copiedFigure.SetView(figureView);

            _figuresBuilder.AddFigure(copiedFigure);
        }

        private void DestroyFigure(FigureModel figure)
        {
            figure.GetDraggable().Interactable = false;

            _view.PlayDestroyAnimation(figure.GetRectTransform(), () =>
            {
                figure.Dispose();
            });
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
        }

        private void OnSettingsButtonClick()
        {
            _popupFactory.ShowPopup<SettingsPopup>(true);
        }
    }
}
