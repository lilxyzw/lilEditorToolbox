using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal class JsonObjectViewer : EditorWindow
    {
        public Vector2 scrollPos;
        public Object target;
        public string json;

        [MenuItem(Common.MENU_HEAD + "Json Object Viewer")]
        static void Init() => GetWindow(typeof(JsonObjectViewer)).Show();

        void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            target = EditorGUILayout.ObjectField(target, typeof(Object), true);
            if(EditorGUI.EndChangeCheck()) json = target ? EditorJsonUtility.ToJson(target, true) : "";
            if(!target) return;

            if(GUILayout.Button("Refresh")) json = EditorJsonUtility.ToJson(target, true);
            if(GUILayout.Button("Apply Modification"))
            {
                var clone = Instantiate(target);
                EditorJsonUtility.FromJsonOverwrite(json, clone);
                using var so = new SerializedObject(target);
                using var soClone = new SerializedObject(clone);
                using var iter = soClone.GetIterator();
                iter.Next(true);
                so.CopyFromSerializedProperty(iter);
                while(iter.Next(false))
                    so.CopyFromSerializedProperty(iter);
                so.ApplyModifiedProperties();
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            json = EditorGUILayout.TextArea(json);
            EditorGUILayout.EndScrollView();
        }
    }
}
