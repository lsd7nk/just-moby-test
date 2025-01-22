using UnityEngine;

namespace App.Core
{
    public interface IFigureModelsFactory
    {
        FigureModel[] GetFigureModels(int count);
    }


    public sealed class FigureModelsHSVFactory : IFigureModelsFactory
    {
        public FigureModel[] GetFigureModels(int count)
        {
            var models = new FigureModel[count];

            for (int i = 0; i < count; ++i)
            {
                models[i] = new FigureModel(Color.HSVToRGB((float)i / count, 1f, 0.9f));
            }

            return models;
        }
    }
}