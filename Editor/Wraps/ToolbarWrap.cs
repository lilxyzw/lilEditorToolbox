using System;
using System.Reflection;
using UnityEditor;
using UnityEngine.UIElements;

namespace jp.lilxyzw.editortoolbox
{
    internal class ToolbarWrap
    {
        private static readonly Type T_Toolbar = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
        private static readonly FieldInfo FI_get = T_Toolbar.GetField("get", BindingFlags.Public | BindingFlags.Static);
        private static readonly FieldInfo FI_m_Root = T_Toolbar.GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);

        public object instance;
        public ToolbarWrap(object instance) => this.instance = instance;

        public static ToolbarWrap get => new(FI_get.GetValue(null));
        public VisualElement m_Root => FI_m_Root.GetValue(instance) as VisualElement;
    }
}
