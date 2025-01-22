using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using MiniContainer;
using UnityEngine;
using System;

namespace App.AppStates
{
    public sealed class AppStateAssets : MonoBehaviour, IRegistrable
    {
        [Serializable]
        private sealed class StateAsset
        {
            [field: SerializeField] public AppStateType State { get; set; }
            [field: SerializeField] public AssetReferenceGameObject[] Assets { get; set; }
        }


        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private StateAsset[] _stateAssets;

        public async UniTask<GameObject[]> InstantiateAssetsAsync(AppStateType state)
        {
            if (!TryGetAssets(state, out var assets))
            {
                return null;
            }

            var instances = new GameObject[assets.Length];

            for (int i = 0; i < assets.Length; ++i)
            {
                instances[i] = await assets[i].InstantiateAsync(_mainCanvas.transform);
            }

            return instances;
        }

        private bool TryGetAssets(AppStateType state, out AssetReferenceGameObject[] assets)
        {
            assets = null;

            foreach (var stateAsset in _stateAssets)
            {
                if (stateAsset.State == state)
                {
                    assets = stateAsset.Assets;
                    return true;
                }
            }

            return false;
        }
    }
}