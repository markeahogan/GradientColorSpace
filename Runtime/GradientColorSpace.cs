using System;
using UnityEngine;

namespace PopupAsylum.GradientColorSpace
{
    /// <summary>
    /// Provides methods for converting a Gradient into a piecewise gradient interpolated in a different color space
    /// </summary>
    public static class GradientColorSpace
    {
        private const int MAX_KEYS_MINUS_ONE = 7;

        /// <summary>
        /// Determines if data/keys would be lost by converting the gradient
        /// </summary>
        /// <param name="gradient">The gradient to be converted</param>
        /// <returns>Returns false if converting would cause data/keys to be lost</returns>
        public static bool IsSafeToConvert(Gradient gradient)
        {
            var keys = gradient.colorKeys;

            if (keys.Length == 2) { return true; }
            if (keys.Length != 8) { return false; }

            float expectedSpacing = (keys[7].time - keys[0].time)/7;
            for (int i = 1; i < 8; i++)
            {
                float diff = Mathf.Abs((keys[i].time - keys[i - 1].time) - expectedSpacing);
                if (diff > 0.001f) { return false; }
            }

            return true;
        }

        /// <summary>
        /// Converts a simple gradient into a piecewise gradient interpolated in a different color space
        /// </summary>
        /// <param name="gradient">Gradient to be converted, must have at least 2 keys, keys between the first and last will be discarded</param>
        /// <param name="space">The color space to convert to</param>
        public static void ConvertGradient(Gradient gradient, ColorSpace space)
        {
            var originalKeys = gradient.colorKeys;

            if (originalKeys.Length < 2)
            {
                throw new ArgumentException($"Gradient must have at least 2 color keys, found {originalKeys.Length}");
            }

            var startKey = originalKeys[0];
            var endKey = originalKeys[originalKeys.Length - 1];
            if (space == ColorSpace.sRGB)
            {
                gradient.colorKeys = new GradientColorKey[] { startKey, endKey };
                return;
            }

            var keys = originalKeys.Length == 8 ? originalKeys : new GradientColorKey[8];
            keys[0] = startKey;
            keys[MAX_KEYS_MINUS_ONE] = endKey;

            switch (space)
            {
                case ColorSpace.Oklab:
                    OklabGradient(startKey, endKey, keys);
                    break;
                case ColorSpace.SRLAB2:
                    SRLAB2Gradient(startKey, endKey, keys);
                    break;
                default:
                    throw new Exception($"Can't handle {space}");
            }

            gradient.colorKeys = keys;
        }

        private static void OklabGradient(GradientColorKey startKey, GradientColorKey endKey, GradientColorKey[] keys)
        {
            Lab start = Oklab.FromColor(startKey.color);
            Lab end = Oklab.FromColor(endKey.color);

            for (int i = 1; i < MAX_KEYS_MINUS_ONE; i++)
            {
                float t = i / (float)MAX_KEYS_MINUS_ONE;
                var time = Mathf.Lerp(startKey.time, endKey.time, t);
                var col = Oklab.ToColor(Lab.Lerp(start, end, t));
                keys[i] = new GradientColorKey(col, time);
            }
        }

        private static void SRLAB2Gradient(GradientColorKey startKey, GradientColorKey endKey, GradientColorKey[] keys)
        {
            Lab start = SRLAB2.FromColor(startKey.color);
            Lab end = SRLAB2.FromColor(endKey.color);

            for (int i = 1; i < MAX_KEYS_MINUS_ONE; i++)
            {
                float t = i / (float)MAX_KEYS_MINUS_ONE;
                var time = Mathf.Lerp(startKey.time, endKey.time, t);
                var col = SRLAB2.ToColor(Lab.Lerp(start, end, t));
                keys[i] = new GradientColorKey(col, time);
            }
        }
    }

    public enum ColorSpace
    {
        sRGB,
        Oklab,
        SRLAB2
    }
}
