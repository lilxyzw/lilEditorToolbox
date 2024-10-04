using System;
using System.Reflection;
using UnityEditor;

namespace jp.lilxyzw.editortoolbox
{
    public static class MenuWrap
    {
        private static readonly Type T_Menu = typeof(Menu);
        private static readonly MethodInfo MI_AddMenuItem = T_Menu.GetMethod("AddMenuItem", BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly MethodInfo MI_RemoveMenuItem = T_Menu.GetMethod("RemoveMenuItem", BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly MethodInfo MI_RebuildAllMenus = T_Menu.GetMethod("RebuildAllMenus", BindingFlags.NonPublic | BindingFlags.Static);

        internal static void AddMenuItem(string name, string shortcut, bool @checked, int priority, Action execute, Func<bool> validate)
            => MI_AddMenuItem.Invoke(null, new object[]{name, shortcut, @checked, priority, execute, validate});

        internal static void RemoveMenuItem(string name)
            => MI_RemoveMenuItem.Invoke(null, new object[]{name});

        internal static void RebuildAllMenus()
            => MI_RebuildAllMenus.Invoke(null, null);
    }
}
