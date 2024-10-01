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
        private static readonly Type T_PropertyEditor = typeof(Editor).Assembly.GetType("UnityEditor.PropertyEditor");
        private static readonly MethodInfo MI_SetObjectsLocked = T_InspectorWindow.GetMethod("SetObjectsLocked", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly MethodInfo MI_SetNormal = T_PropertyEditor.GetMethod("SetNormal", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly MethodInfo MI_SetDebug = T_PropertyEditor.GetMethod("SetDebug", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly MethodInfo MI_SetDebugInternal = T_PropertyEditor.GetMethod("SetDebugInternal", BindingFlags.NonPublic | BindingFlags.Instance);
        public List<Object> targets;
        public EditorWindow inspector;

        //[MenuItem("Assets/Open in new Inspector", false, 15)]
        internal static void Init()
        {
            if(Selection.activeObject)
            {
                var window = CreateWindow<TabInspector>(Selection.activeObject.name, new[]{T_InspectorWindow});
                window.titleContent.image = AssetPreview.GetMiniThumbnail(Selection.activeObject);
            }
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

            // Mode Button
            var buttonNormal = new Button{text = L10n.L("Normal")};
            buttonNormal.clicked += () => MI_SetNormal.Invoke(inspector, null);
            header.Add(buttonNormal);
            var buttonDebug = new Button{text = L10n.L("Debug")};
            buttonDebug.clicked += () => MI_SetDebug.Invoke(inspector, null);
            header.Add(buttonDebug);
            var buttonInternal = new Button{text = L10n.L("Developer")};
            buttonInternal.clicked += () => MI_SetDebugInternal.Invoke(inspector, null);
            header.Add(buttonInternal);

            // Close Button
            var button = new Button{text = "☓", tooltip = "Close this tab."};
            button.clicked += () => Close();
            header.Add(button);

            // Inspector
            inspector = CreateInstance(T_InspectorWindow) as EditorWindow;
            MI_SetObjectsLocked.Invoke(inspector, new object[]{targets});
            inspector.rootVisualElement.style.flexGrow = 1;

            rootVisualElement.Add(header);
            rootVisualElement.Add(inspector.rootVisualElement);
        }
    }
}
