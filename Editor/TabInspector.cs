using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace jp.lilxyzw.editortoolbox
{
    internal class TabInspector : EditorWindow
    {
        private static readonly Type T_InspectorWindow = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
        private static readonly MethodInfo MI_SetObjectsLocked = T_InspectorWindow.GetMethod("SetObjectsLocked", BindingFlags.NonPublic | BindingFlags.Instance);
        public List<Object> targets;

        //[MenuItem("Assets/Open in new Inspector", false, 15)]
        internal static void Init()
        {
            if(Selection.activeObject)
                CreateWindow<TabInspector>($"@{Selection.activeObject.name}", new[]{T_InspectorWindow});
        }

        public void CreateGUI()
        {
            rootVisualElement.Clear();
            targets ??= Selection.objects.ToList();

            var header = new VisualElement();
            header.style.flexDirection = FlexDirection.Row;

            // Target
            var objfield = new ObjectField{value = targets[0]};
            objfield.style.flexGrow = 1;
            header.Add(objfield);

            // Close Button
            var button = new Button{text = "☓", tooltip = "Close this tab."};
            button.clicked += () => Close();
            header.Add(button);

            // Inspector
            var inspector = CreateInstance(T_InspectorWindow) as EditorWindow;
            MI_SetObjectsLocked.Invoke(inspector, new object[]{targets});
            inspector.rootVisualElement.style.flexGrow = 1;

            rootVisualElement.Add(header);
            rootVisualElement.Add(inspector.rootVisualElement);
        }
    }
}
