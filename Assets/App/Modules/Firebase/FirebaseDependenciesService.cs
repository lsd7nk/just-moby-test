using Cysharp.Threading.Tasks;
using UnityEngine;
using Firebase;
using System;

namespace App
{
    public interface IFirebaseDependenciesService : IDisposable
    {
        DependencyStatus DependencyStatus { get; }
        bool FirebaseIsAvailable { get; }

        UniTask FixDependenciesAsync();
    }


    public sealed class FirebaseDependenciesService : IFirebaseDependenciesService
    {
        public DependencyStatus DependencyStatus { get; private set; }
        public bool FirebaseIsAvailable
        {
            get
            {
                return DependencyStatus == DependencyStatus.Available;
            }
        }

        public async UniTask FixDependenciesAsync()
        {
            DependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();

            if (!FirebaseIsAvailable)
            {
                Debug.LogError($"[{nameof(FirebaseDependenciesService)}] Firebase not resolve all dependencies: {DependencyStatus}");
                return;
            }

            Debug.Log($"[{nameof(FirebaseDependenciesService)}] Firebase successfully resolve all dependencies");
        }

        public void Dispose() { }
    }
}
