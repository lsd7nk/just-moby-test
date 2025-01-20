using System.Collections.Generic;
using UnityEngine;
using System;

namespace App.Settings
{
    public static class SettingsProvider
    {
        private readonly static Dictionary<Type, ScriptableObject> _settings;
        private const string PATH_TEMPLATE = "Settings/{0}";

        static SettingsProvider()
        {
            _settings = new Dictionary<Type, ScriptableObject>();
        }

        public static T Get<T>() where T : ScriptableObject
        {
            var type = typeof(T);

            if (_settings.ContainsKey(type))
            {
                return (T)_settings[type];
            }

            var path = string.Format(PATH_TEMPLATE, type.Name);
            var settings = Resources.Load<T>(path);

            _settings.Add(type, settings);

            return settings;
        }
    }
}
