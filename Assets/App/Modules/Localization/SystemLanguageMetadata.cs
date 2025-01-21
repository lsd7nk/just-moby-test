using UnityEngine.Localization.Metadata;
using System.ComponentModel;
using UnityEngine;
using System;

namespace App.Localization
{
    [Metadata(AllowedTypes = MetadataType.Locale)]
    [DisplayName("System language")]
    [Serializable]
    public sealed class SystemLanguageMetadata : IMetadata
    {
        public SystemLanguage Language;
    }
}