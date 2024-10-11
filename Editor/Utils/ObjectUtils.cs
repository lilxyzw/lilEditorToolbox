using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal static class ObjectUtils
    {
        internal static void FromJsonOverwrite(string json, Object target)
        {
            var clone = Object.Instantiate(target);
            EditorJsonUtility.FromJsonOverwrite(json, clone);
            using var so = new SerializedObject(target);
            using var soClone = new SerializedObject(clone);
            using var iter = soClone.GetIterator();
            iter.Next(true);
            CopyFromSerializedProperty(so, iter);
            while(iter.Next(false))
                CopyFromSerializedProperty(so, iter);
            so.ApplyModifiedProperties();
            if(clone is Component c) Object.DestroyImmediate(c.gameObject);
            else Object.DestroyImmediate(clone);
        }

        private static void CopyFromSerializedProperty(SerializedObject so, SerializedProperty prop)
        {
            if(
                prop.propertyPath == "m_ObjectHideFlags" ||
                prop.propertyPath == "m_CorrespondingSourceObject" ||
                prop.propertyPath == "m_PrefabInstance" ||
                prop.propertyPath == "m_PrefabAsset" ||
                prop.propertyPath == "m_GameObject" ||
                prop.propertyPath == "m_EditorHideFlags"
            ) return;

            so.CopyFromSerializedProperty(prop);
        }
    }

    internal abstract class DirectElements{}
}
