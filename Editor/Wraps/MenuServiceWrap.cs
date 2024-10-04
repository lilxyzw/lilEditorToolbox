using System;
using System.Reflection;
using UnityEditor;

namespace jp.lilxyzw.editortoolbox
{
    public static class MenuServiceWrap
    {
        private static readonly Type T_MenuService = typeof(Editor).Assembly.GetType("UnityEditor.MenuService");
        private static readonly MethodInfo MI_ValidateMethodForMenuCommand = T_MenuService.GetMethod("ValidateMethodForMenuCommand", BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly MethodInfo MI_SanitizeMenuItemName = T_MenuService.GetMethod("SanitizeMenuItemName", BindingFlags.NonPublic | BindingFlags.Static);

        internal static bool ValidateMethodForMenuCommand(MethodInfo methodInfo)
            => (bool)MI_ValidateMethodForMenuCommand.Invoke(null, new object[]{methodInfo});

        internal static string SanitizeMenuItemName(string name)
            => (string)MI_SanitizeMenuItemName.Invoke(null, new object[]{name});
    }
}
