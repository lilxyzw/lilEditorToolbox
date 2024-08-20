using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    public class MissingFinder : EditorWindow
    {
        [MenuItem("Tools/lilEditorToolbox/Missing Finder")]
        static void Init() => GetWindow(typeof(MissingFinder)).Show();

        HashSet<Object> objects = new HashSet<Object>();
        HashSet<Object> scaneds = new HashSet<Object>();
        public Object target;
        public Vector2 scrollPos;

        void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            target = EditorGUILayout.ObjectField(target, typeof(Object), true);
            if(EditorGUI.EndChangeCheck() && target)
            {
                objects.Clear();
                scaneds.Clear();
                ScanRecursive(target);
            }

            if(!target)
            {
                EditorGUILayout.LabelField("オブジェクトが選択されていません");
                return;
            }

            if(objects.Count > 0)
            {
                EditorGUILayout.Space();
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                GUI.enabled = false;
                foreach(var obj in objects) EditorGUILayout.ObjectField(obj, typeof(Object), true);
                GUI.enabled = true;
                EditorGUILayout.EndScrollView();
            }
            else
            {
                EditorGUILayout.LabelField("Missingはたぶんありません");
            }
        }

        private void ScanRecursive(Object obj)
        {
            if(!obj || scaneds.Contains(obj)) return;
            scaneds.Add(obj);
            var serializedObject = new SerializedObject(obj);
            SerializedProperty iter = serializedObject.GetIterator();
            bool enterChildren = true;
            while(iter.Next(enterChildren))
            {
                enterChildren = true;
                switch(iter.propertyType)
                {
                    case SerializedPropertyType.ObjectReference:
                        if(iter.objectReferenceValue)
                        {
                            ScanRecursive(iter.objectReferenceValue);
                            break;
                        }
                        if(iter.objectReferenceInstanceIDValue == 0) break;
                        var guid = GlobalObjectId.GetGlobalObjectIdSlow(iter.objectReferenceInstanceIDValue).assetGUID.ToString();
                        if(!string.IsNullOrEmpty(AssetDatabase.GUIDToAssetPath(guid))) break;
                        objects.Add(obj);
                        break;
                    case SerializedPropertyType.String:
                        enterChildren = false;
                        break;
                }
            }

            if(obj is GameObject g)
            {
                foreach(var component in g.GetComponents<Component>())
                {
                    if(!component) objects.Add(g);
                    else ScanRecursive(component);
                }
                foreach(var transform in g.GetComponentsInChildren<Transform>(true)) ScanRecursive(transform.gameObject);
            }
        }
    }
}
