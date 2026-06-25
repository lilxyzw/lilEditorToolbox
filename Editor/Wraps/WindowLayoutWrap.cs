using System;
using System.Linq;
using UnityEditor;

namespace jp.lilxyzw.editortoolbox
{
    internal class WindowLayoutWrap : WrapBase
    {
        private static readonly Type type = typeof(Editor).Assembly.GetType("UnityEditor.WindowLayout");
        internal static Action UpdateWindowLayoutMenu = GetAction(type, "UpdateWindowLayoutMenu");

        /*
        [InitializeOnLoadMethod]
        private static void Test()
        {
            foreach (var field in typeof(WrapBase).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(WrapBase))).SelectMany(t => t.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic)).ToArray())
            {
                var obj = field.GetValue(null);
                try
                {
                    if (obj == null) UnityEngine.Debug.LogError($"{field.DeclaringType}.{field.Name}");
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError(e);
                }
            }
        }
        */
    }
}
