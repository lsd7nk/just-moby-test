using Lofelt.NiceVibrations;
using UnityEngine;
using App.Events;
using System;

namespace App.Vibrations
{
    public sealed class VibrationsService : IDisposable
    {
        public bool IsEnabled
        {
            get => HapticController.hapticsEnabled;
            set
            {
                HapticController.hapticsEnabled = value;
                PlayerPrefs.SetInt(/*Prefs.VIBRO_ENABLED*/"v", value ? 1 : 0); // to do

                if (value)
                {
                    Initialize();
                }
                else
                {
                    RemoveListeners();
                }
            }
        }
        public bool Initialized { get; private set; }

        public VibrationsService()
        {
            IsEnabled = PlayerPrefs.GetInt(/*Prefs.VIBRO_ENABLED*/"v", 1) == 1;

            if (!IsEnabled)
            {
                return;
            }

            Initialize();
        }

        public void Dispose()
        {
            RemoveListeners();
        }

        public void PlaySelectionVibration()
        {
            PlayVibration(HapticPatterns.PresetType.Selection);
        }

        public void PlaySuccessVibration()
        {
            PlayVibration(HapticPatterns.PresetType.Success);
        }

        public void PlayFailureVibration()
        {
            PlayVibration(HapticPatterns.PresetType.Failure);
        }

        public void PlayLightImpactVibration()
        {
            PlayVibration(HapticPatterns.PresetType.LightImpact);
        }

        public void PlayMediumImpactVibration()
        {
            PlayVibration(HapticPatterns.PresetType.MediumImpact);
        }

        public void PlayHeavyImpactVibration()
        {
            PlayVibration(HapticPatterns.PresetType.LightImpact);
        }

        public void PlayRigidImpactVibration()
        {
            PlayVibration(HapticPatterns.PresetType.RigidImpact);
        }

        public void PlaySoftImpactVibration()
        {
            PlayVibration(HapticPatterns.PresetType.SoftImpact);
        }

        private void PlayVibration(PlayVibrationEvent e)
        {
            PlayVibration(e.VibrationType);
        }

        private void PlayVibration(HapticPatterns.PresetType vibrationType)
        {
            if (!IsEnabled)
            {
                return;
            }

            HapticPatterns.PlayPreset(vibrationType);
        }

        private void Initialize()
        {
            if (Initialized)
            {
                return;
            }

            HapticController.Init();
            EventSystem.Subscribe<PlayVibrationEvent>(PlayVibration);

            Initialized = true;
        }

        private void RemoveListeners()
        {
            EventSystem.Unsubscribe<PlayVibrationEvent>(PlayVibration);

            Initialized = false;
        }
    }
}
