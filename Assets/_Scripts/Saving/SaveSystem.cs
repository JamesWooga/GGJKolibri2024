using System;
using _Scripts.Prefs;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace _Scripts.Saving
{
    public static class SaveSystem
    {

        public static SaveFile GetSaveFile()
        {
            var save = PlayerPrefs.GetString(PlayerPrefsService.SaveFileKey, string.Empty);
            if (save == string.Empty)
            {
                return new SaveFile();
            }

            try
            {
                var deserialized = JsonUtility.FromJson<SaveFile>(save);
                return deserialized;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to deserialize: {e.Message}");
                return new SaveFile();
            }
        }

        public static void Save(SaveFile saveFile)
        {
            var str = JsonUtility.ToJson(saveFile);
            PlayerPrefs.SetString(PlayerPrefsService.SaveFileKey, str);
            PlayerPrefs.Save();
        }

        #if UNITY_EDITOR
        [MenuItem("Frog/Clear Save")]
        public static void Clear()
        {
            PlayerPrefs.DeleteKey(PlayerPrefsService.SaveFileKey);
        }
        #endif
    }
}