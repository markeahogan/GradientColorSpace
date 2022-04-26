using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace PopupAsylum.GradientColorSpace.Editor
{
    /// <summary>
    /// Adds a 'Color Space' context menu to Gradient properties that will create a piecewise gradient in a different color space
    /// </summary>
    [InitializeOnLoad]
    public class GradientColorSpaceContextMenu
    {
        static GradientColorSpaceContextMenu()
        {
            EditorApplication.contextualPropertyMenu += OnPropertyContextMenu;
        }

        private static void OnPropertyContextMenu(GenericMenu menu, SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.Gradient)
                return;

            var propertyCopy = property.Copy();
            menu.AddItem(new GUIContent("Color Space/sRGB"), false, () =>
            {
                UpdateGradient(propertyCopy, ColorSpace.sRGB);
            });
            menu.AddItem(new GUIContent("Color Space/Oklab"), false, () =>
            {
                UpdateGradient(propertyCopy, ColorSpace.Oklab);
            });
            menu.AddItem(new GUIContent("Color Space/SRLAB2"), false, () =>
            {
                UpdateGradient(propertyCopy, ColorSpace.SRLAB2);
            });
        }

        private static void UpdateGradient(SerializedProperty property, ColorSpace colorSpace)
        {
            Gradient g = GetGradient(property);

            if (!GradientColorSpace.IsSafeToConvert(g) && 
                !EditorUtility.DisplayDialog("Gradient Color Space", 
                "Gradient Color Space interpolates between the first and last keys, inbetween keys will be lost", 
                "OK", "Cancel"))
            {
                return;
            }

            GradientColorSpace.ConvertGradient(g, colorSpace);
            SetGradient(property, g);
            property.serializedObject.ApplyModifiedProperties();
        }

        static Gradient GetGradient(SerializedProperty property)
        {
            PropertyInfo propertyInfo = typeof(SerializedProperty).GetProperty("gradientValue", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return propertyInfo.GetValue(property, null) as Gradient;
        }

        static void SetGradient(SerializedProperty property, Gradient gradient)
        {
            PropertyInfo propertyInfo = typeof(SerializedProperty).GetProperty("gradientValue", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            propertyInfo.SetValue(property, gradient);
        }
    }
}
