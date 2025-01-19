using Cysharp.Threading.Tasks;
using Firebase.RemoteConfig;
using UnityEngine;
using System;

namespace App
{
    public abstract class FirebaseConfig
    {
        public bool Initialized { get; private set; }
        public bool Fetched { get; private set; }

        private readonly IFirebaseDependenciesService _firebase;
        private readonly TimeSpan _timeout;

        public FirebaseConfig(IFirebaseDependenciesService firebase, TimeSpan timeout)
        {
            _firebase = firebase;
            _timeout = timeout;
        }

        public void Initialize()
        {
            if (Initialized)
            {
                return;
            }

            InternalInitialize();
        }

        protected int GetIntValue(string key)
        {
            return (int)GetConfigValue(key).LongValue;
        }

        protected bool GetBoolValue(string key)
        {
            return GetConfigValue(key).BooleanValue;
        }

        protected string GetStringValue(string key)
        {
            return GetConfigValue(key).StringValue;
        }

        protected virtual void OnSuccessfulCloudLoad()
        {
            Debug.Log($"[{nameof(FirebaseConfig)}] Successful config download from the cloud");
        }

        protected virtual void LocalLoad()
        {
            Debug.Log($"[{nameof(FirebaseConfig)}] Loading config from local");

            Initialized = true;
        }

        private async UniTaskVoid CloudLoad()
        {
            Debug.Log($"[{nameof(FirebaseConfig)}] Loading config from cloud");

            await FirebaseRemoteConfig.DefaultInstance.FetchAsync(_timeout)
                .AsUniTask();

            var result = await FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                .AsUniTask();

            if (result)
            {
                Fetched = true;

                OnSuccessfulCloudLoad();
            }
            else
            {
                Debug.Log($"[{nameof(FirebaseConfig)}] Failed configuration download from the cloud");
                LocalLoad();
            }

            Initialized = true;
        }

        private ConfigValue GetConfigValue(string key)
        {
            return FirebaseRemoteConfig.DefaultInstance.GetValue(key);
        }

        private void InternalInitialize()
        {
            if (!_firebase.FirebaseIsAvailable)
            {
                LocalLoad();
                return;
            }

            if (InternetReachabilityVerifier.Instance.status == InternetReachabilityVerifier.Status.Offline)
            {
                LocalLoad();
                return;
            }

            CloudLoad().Forget();
        }
    }
}