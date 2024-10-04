using System;
using System.Reflection;
using UnityEditor;

namespace jp.lilxyzw.editortoolbox
{
    internal static class EditorUtilityWrap
    {
        private static readonly Type T_EditorUtility = typeof(EditorUtility);
        private static readonly MethodInfo MI_Internal_UpdateAllMenus = T_EditorUtility.GetMethod("Internal_UpdateAllMenus", BindingFlags.NonPublic | BindingFlags.Static);

        internal static void Internal_UpdateAllMenus()
            => MI_Internal_UpdateAllMenus.Invoke(null, null);
    }
}
