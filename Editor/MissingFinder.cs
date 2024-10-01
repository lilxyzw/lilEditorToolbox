using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal class MissingFinder : EditorWindow
    {
        [MenuItem(Common.MENU_HEAD + "Missing Finder")]
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
                EditorGUILayout.LabelField(L10n.L("No object selected."));
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
                EditorGUILayout.LabelField(L10n.L("There are probably no missing references."));
            }
        }

        private void ScanRecursive(Object obj)
        {
            if(!obj || scaneds.Contains(obj)) return;
            scaneds.Add(obj);

            if(obj is GameObject g)
            {
                foreach(var c in g.GetComponentsInChildren<Component>(true))
                {
                    if(!c) objects.Add(c);
                    ScanRecursive(c);
                }
            }

            if(Common.SkipScan(obj)) return;

            using var serializedObject = new SerializedObject(obj);
            using var iter = serializedObject.GetIterator();
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
        }
    }
}
