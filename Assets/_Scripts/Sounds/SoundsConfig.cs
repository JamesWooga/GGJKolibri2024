using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Objects;
using UnityEngine;
using Utility.Extensions;

namespace _Scripts.Sounds
{
    [CreateAssetMenu(fileName = nameof(SoundsConfig), menuName = "Sounds/" + nameof(SoundsConfig), order = 0)]
    public class SoundsConfig : ScriptableObject
    {
        public enum SoundType
        {
            Appear = 0,
            Catch = 1,
            Crash = 2,
        }
        
        [Serializable]
        private class Config
        {
            [SerializeField] private DroppedObjectType _type;
            [SerializeField] private AudioClip _appearSound;
            [SerializeField] private AudioClip _catchSound;
            [SerializeField] private AudioClip _crashSound;

            public DroppedObjectType Type => _type;
            public AudioClip AppearSound => _appearSound;
            public AudioClip CatchSound => _catchSound;
            public AudioClip CrashSound => _crashSound;
        }

        [SerializeField] private Config[] _settings;
        [SerializeField] private AudioClip[] _defaultAppearSounds;
        [SerializeField] private AudioClip[] _defaultCatchSounds;
        [SerializeField] private AudioClip[] _defaultCrashSounds;

        private Dictionary<DroppedObjectType, Config> _configs;
        private Dictionary<DroppedObjectType, Config> Configs => _configs ??= _settings.ToDictionary(e => e.Type);

        public AudioClip GetClip(DroppedObjectType objectType, SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.Appear:
                    break;
                case SoundType.Catch:
                    break;
                case SoundType.Crash:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(soundType), soundType, null);
            }
            
            if (Configs.ContainsKey(objectType))
            {
                return soundType switch
                {
                    SoundType.Appear when Configs[objectType].AppearSound != null => Configs[objectType].AppearSound,
                    SoundType.Catch when Configs[objectType].CatchSound != null => Configs[objectType].CatchSound,
                    SoundType.Crash when Configs[objectType].CrashSound != null => Configs[objectType].CrashSound,
                    _ => ReturnDefault(soundType)
                };
            }

            return ReturnDefault(soundType);
        }

        private AudioClip ReturnDefault(SoundType soundType)
        {
            return soundType switch
            {
                SoundType.Appear => _defaultAppearSounds.RandomElement(),
                SoundType.Catch => _defaultCatchSounds.RandomElement(),
                SoundType.Crash => _defaultCrashSounds.RandomElement(),
                _ => throw new ArgumentOutOfRangeException(nameof(soundType), soundType, null)
            };
        }
    }
}