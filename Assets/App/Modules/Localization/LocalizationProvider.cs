using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using System.Collections.Generic;
using UnityEngine.Localization;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using App.Settings;
using System.Linq;
using UnityEngine;
using System;

namespace App.Localization
{
    public static class LocalizationProvider
    {
        private const char SEPARATOR = '/';

        public static event Action OnLocalizeEvent;

        public static LanguageType CurrentLanguage { get; private set; }

        private static Dictionary<string, LocalizationFileData> _localizationFiles;
        private static LocalizedText _defaultLocalizedText;

        static LocalizationProvider()
        {
            var localizationSettings = SettingsProvider.Get<LocalizationDefaultSettings>();

            _localizationFiles = new Dictionary<string, LocalizationFileData>();
            _defaultLocalizedText = localizationSettings.DefaultLocalizationFile;
        }

        public static string GetText(LocalizedText asset, string key)
        {
            string text = string.Empty;

            string entryName = asset.TableReference.TableCollectionName;
            long entryId = asset.TableEntryReference.KeyId;

            if (_localizationFiles.TryGetValue(GetFileDataKey(entryName, entryId), out var fileData))
            {
                fileData.TryGetValue(key, out text);
            }

            return text;
        }

        public static string GetText(string key)
        {
            string text = string.Empty;

            string entryName = _defaultLocalizedText.TableReference.TableCollectionName;
            long entryId = _defaultLocalizedText.TableEntryReference.KeyId;

            if (_localizationFiles.TryGetValue(GetFileDataKey(entryName, entryId), out var fileData))
            {
                fileData.TryGetValue(key, out text);
            }

            return text;
        }

        public static string GetLocaleCode(LanguageType type)
        {
            return type switch
            {
                LanguageType.English => "en",
                LanguageType.Russia => "ru",
                _ => string.Empty
            };
        }

        public static string GetCurrentLocaleCode()
        {
            return GetLocaleCode(CurrentLanguage);
        }

        public static Locale GetLocale(string localeCode)
        {
            if (!string.IsNullOrEmpty(localeCode))
            {
                int availableLocalesCount = LocalizationSettings.AvailableLocales.Locales.Count;

                for (int i = 0; i < availableLocalesCount; ++i)
                {
                    var availableLocale = LocalizationSettings.AvailableLocales.Locales[i];

                    if (availableLocale.Identifier.Code == localeCode)
                    {
                        return availableLocale;
                    }
                }
            }

            return LocalizationSettings.ProjectLocale;
        }

        public static LanguageType GetLanguageType(string localeCode)
        {
            return localeCode switch
            {
                "en" => LanguageType.English,
                "ru" => LanguageType.Russia,
                _ => LanguageType.English,
            };
        }

        public static async UniTask Initialize(Locale locale)
        {
            await Setup(locale);
            OnLocalizeEvent?.Invoke();
        }

        private static async UniTask Setup(Locale locale)
        {
            if (_localizationFiles.Any())
            {
                _localizationFiles.Clear();
            }

            CurrentLanguage = GetLanguageType(locale.Identifier.Code);

            await LoadLocalization(locale);
        }

        private static async UniTask LoadLocalization(Locale locale)
        {
            var tables = await LocalizationSettings.AssetDatabase.GetAllTables(locale).Task;
            var tableEntries = tables.SelectMany(t => t.Values).ToArray();
            var tasks = new List<UniTask>(tableEntries.Length);

            foreach (var entry in tableEntries)
            {
                tasks.Add(LoadLocalizationFile(entry, locale));
            }

            await UniTask.WhenAll(tasks);
        }

        private static async UniTask LoadLocalizationFile(AssetTableEntry entry, Locale locale)
        {
            string entryName = entry.Table.TableCollectionName;
            long entryId = entry.KeyId;
            var asset = await LocalizationSettings.AssetDatabase
                .GetLocalizedAssetAsync<TextAsset>(entryName, entryId, locale, FallbackBehavior.UseFallback)
                .Task.AsUniTask();

            var fileData = GetFileData(asset);
            string fileKey = GetFileDataKey(entryName, entryId);

            if (_localizationFiles.ContainsKey(fileKey))
            {
                return;
            }

            _localizationFiles.Add(fileKey, fileData);
        }

        private static LocalizationFileData GetFileData(TextAsset asset)
        {
            var data = JsonConvert.DeserializeObject<IEnumerable<LocalizationItem>>(asset.text);
            var loadedData = new LocalizationFileData();

            foreach (var item in data)
            {
                string key = string.Join(SEPARATOR, item.Tags);

                if (loadedData.ContainsKey(key))
                {
                    continue;
                }

                loadedData.Add(key, item.Topic);
            }

            return loadedData;
        }

        private static string GetFileDataKey(string entryName, long entryId)
        {
            return string.Join(SEPARATOR, new string[]
            {
                entryName,
                entryId.ToString()
            });
        }
    }
}