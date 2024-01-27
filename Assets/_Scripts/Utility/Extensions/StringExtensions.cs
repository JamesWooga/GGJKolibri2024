using System;
using UnityEngine;

namespace Utility.Extensions
{
    public static class StringExtensions
    {
        public static string GetColorTagOpen(string hexColor)
        {
            return $"<color=#{hexColor}>";
        }

        public static string GetColorTagClose()
        {
            return "</color>";
        }
        
        public static string Colorize(this string stringToColorize, string hexColor)
        {
            return $"<color={hexColor}>{stringToColorize}</color>";
        }
        
        public static string Italic(this string stringToSetItalic)
        {
            return $"<i>{stringToSetItalic}</i>";
        }

        public static string Colorize(this string stringToColorize, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{stringToColorize}</color>";
        }

        public static string NonBreaking(this string nonBreakingString)
        {
            return $"<nobr>{nonBreakingString}</nobr>";
        }

        public static string Resize(this string resizedString, float size)
        {
            return $"<size={size}>{resizedString}</size>";
        }

        public static string ResizeProportional(this string resizedString, int percentage)
        {
            return $"<size={percentage}%>{resizedString}</size>";
        }

        public static string VerticalOffset(this string originalString, float verticalOffset)
        {
            return $"<voffset={verticalOffset}>{originalString}</voffset>";
        }

        public static bool TryParseGuid(this string input, out Guid guid)
        {
            try {
                guid = new Guid(input);
                return true;
            } catch {
                guid = Guid.Empty;
                return false;
            }
        }

        public static bool IsUrl(this string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }

        public static string FirstCharToLower(this string input)
        {
            var newString = input;
            if (!string.IsNullOrEmpty(newString) && char.IsUpper(newString[0]))
                newString = char.ToLower(newString[0]) + newString[1..];
            return newString;
        }

        public static string FirstCharToUpper(this string input)
        {
            var newString = input;
            if (!string.IsNullOrEmpty(newString) && char.IsUpper(newString[0]))
                newString = char.ToUpper(newString[0]) + newString[1..];
            return newString;
        }
        
        /// <summary>
        /// Puts the string into the Clipboard.
        /// </summary>
        public static void CopyToClipboard(this string str)
        {
            GUIUtility.systemCopyBuffer = str;
        }
    }
}