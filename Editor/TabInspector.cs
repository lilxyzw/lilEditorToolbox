using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace jp.lilxyzw.editortoolbox
{
    internal class TabInspector : EditorWindow
    {
        public List<Object> targets;
        public InspectorWindowWrap inspector;

        //[MenuItem("Assets/Open in new Inspector", false, 15)]
        internal static void Init()
        {
            if(Selection.activeObject)
            {
                var window = CreateWindow<TabInspector>(Selection.activeObject.name, new[]{InspectorWindowWrap.type});
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
            buttonNormal.clicked += () => inspector.SetNormal();
            header.Add(buttonNormal);
            var buttonDebug = new Button{text = L10n.L("Debug")};
            buttonDebug.clicked += () => inspector.SetDebug();
            header.Add(buttonDebug);
            var buttonInternal = new Button{text = L10n.L("Developer")};
            buttonInternal.clicked += () => inspector.SetDebugInternal();
            header.Add(buttonInternal);

            // Close Button
            var button = new Button{text = "☓", tooltip = "Close this tab."};
            button.clicked += () => Close();
            header.Add(button);

            // Inspector
            inspector = new InspectorWindowWrap(CreateInstance(InspectorWindowWrap.type));
            inspector.SetObjectsLocked(targets);
            inspector.w.rootVisualElement.style.flexGrow = 1;

            rootVisualElement.Add(header);
            rootVisualElement.Add(inspector.w.rootVisualElement);
        }
    }
}
