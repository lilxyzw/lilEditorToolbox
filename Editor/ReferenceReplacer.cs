using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace jp.lilxyzw.editortoolbox
{
    internal class ReferenceReplacer : EditorWindow
    {
        public Vector2 scrollPos;
        public Object target;
        public Object from;
        public Object to;
        public HashSet<Object> modified = new();
        [MenuItem(Common.MENU_HEAD + "Reference Replacer")]
        static void Init() => GetWindow(typeof(ReferenceReplacer)).Show();

        void OnGUI()
        {
            target = EditorGUILayout.ObjectField("Edit target", target, typeof(Object), true);
            EditorGUILayout.Space();
            from = EditorGUILayout.ObjectField("From", from, typeof(Object), true);
            to = EditorGUILayout.ObjectField("To", to, typeof(Object), true);
            EditorGUILayout.Space();
            if(GUILayout.Button("Run"))
            {
                modified.Clear();
                var scaned = new HashSet<Object>();
                if(target is GameObject gameObject)
                {
                    Replace(scaned, gameObject.GetComponentsInChildren<Component>(true));
                }
                else if(target is SceneAsset)
                {
                    if(AssetDatabase.GetAssetPath(target) != SceneManager.GetActiveScene().path)
                    {
                        EditorUtility.DisplayDialog("Reference Replacer", "実行前にシーンを開いてください。", "OK");
                        return;
                    }
                    else
                    {
                        Replace(scaned, SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(o => o.GetComponentsInChildren<Component>(true)).ToArray());
                    }
                }
                else
                {
                    var assetPath = AssetDatabase.GetAssetPath(target);
                    if(!string.IsNullOrEmpty(assetPath)) Replace(scaned, AssetDatabase.LoadAllAssetsAtPath(assetPath));
                    else Replace(scaned, target);
                }
                EditorUtility.DisplayDialog("Reference Replacer", "Complete!", "OK");
            }
            if(modified.Count > 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Modified Objects", EditorStyles.boldLabel);
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                EditorGUI.BeginDisabledGroup(true);
                foreach(var m in modified) EditorGUILayout.ObjectField(m, typeof(Object), true);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
            }
        }

        private void Replace(HashSet<Object> scaned, Object[] objs)
        {
            foreach(var obj in objs) Replace(scaned, obj);
        }

        private Object Replace(HashSet<Object> scaned, Object obj)
        {
            if(obj == from) obj = to;

            if(!obj || scaned.Contains(obj)) return obj;
            scaned.Add(obj);
            if(Common.SkipScan(obj)) return obj;

            Debug.Log(obj, obj);

            using var so = new SerializedObject(obj);
            using var iter = so.GetIterator();
            var enterChildren = true;
            var isDirty = false;
            while(iter.Next(enterChildren))
            {
                enterChildren = iter.propertyType != SerializedPropertyType.String;
                if(iter.propertyType == SerializedPropertyType.ObjectReference && iter.name != "m_CorrespondingSourceObject")
                {
                    var replaced = Replace(scaned, iter.objectReferenceValue);
                    if(iter.objectReferenceValue != replaced)
                    {
                        iter.objectReferenceValue = replaced;
                        isDirty = true;
                    }
                }
            }
            if(isDirty)
            {
                so.ApplyModifiedProperties();
                modified.Add(obj);
            }
            return obj;
        }
    }
}
