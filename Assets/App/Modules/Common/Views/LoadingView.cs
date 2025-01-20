using Doozy.Engine.Progress;
using UnityEngine;

namespace App.Common.Views
{
    public sealed class LoadingView : StateView
    {
        [Header("Loading")]
        [SerializeField] private Progressor _progressor;

        public void SetProgress(float value)
        {
            _progressor.SetValue(value);
        }
    }
}