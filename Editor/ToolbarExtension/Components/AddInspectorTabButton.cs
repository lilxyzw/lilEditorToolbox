using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace jp.lilxyzw.editortoolbox
{
    internal class AddInspectorTabButton : IToolbarExtensionComponent
    {
        public int Priority => 0;
        public bool InLeftSide => false;

        private static Texture tex = EditorGUIUtility.IconContent("Toolbar Plus").image;
        public VisualElement GetRootElement()
        {
            var root = new ToolbarButton();
            var icon = new Image{image = tex};
            root.Add(icon);
            root.clicked += () => TabInspector.Init();
            return root;
        }
    }
}
