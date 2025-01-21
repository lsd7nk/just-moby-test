using Cysharp.Threading.Tasks;
using Doozy.Engine.UI;
using UnityEngine;
using App.Utils;
using System;

namespace App.Popups
{
    public abstract class PopupView : MonoBehaviour, IPopupView
    {
        public abstract Type ServiceType { get; }

        [field: SerializeField] public UIPopup UIPopup { get; private set; }

        public async UniTask HideAsync()
        {
            await UIPopupUtils.HideAsync(UIPopup);
        }

        private void Start()
        {
            UIPopup.Show();
        }
    }
}
