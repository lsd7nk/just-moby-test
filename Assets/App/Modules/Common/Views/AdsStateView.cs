using UnityEngine;
using App.UI;

namespace App.Common.Views
{
    public abstract class AdsView : StateView
    {
        [field: SerializeField] public BannerUIPositionAdjuster BannerAdjuster { get; private set; }
    }
}
