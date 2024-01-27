using UnityEngine;

namespace _Scripts.Prefs
{
    public static class PlayerPrefsService
    {
        public const string SaveFileKey = "frogrope.highscore";
        public const string MusicKey = "frogrope.music";
        public const string SoundKey = "frogrope.sound";

        public static bool Music
        {
            get => PlayerPrefs.GetInt(MusicKey, 1) == 1;
            set
            {
                PlayerPrefs.SetInt(MusicKey, value ? 1 : 0);
                GameEvents.GameEvents.MusicToggled(value);
                PlayerPrefs.Save();
            }
        }

        public static bool Sound
        {
            get => PlayerPrefs.GetInt(SoundKey, 1) == 1;
            set
            {
                PlayerPrefs.SetInt(SoundKey, value ? 1 : 0);
                GameEvents.GameEvents.SoundToggled(value);
                PlayerPrefs.Save();
            }
        }
    }
}