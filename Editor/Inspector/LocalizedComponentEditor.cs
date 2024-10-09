using jp.lilxyzw.editortoolbox.runtime;
using UnityEditor;

namespace jp.lilxyzw.editortoolbox
{
    [CustomEditor(typeof(EditorOnlyBehaviour), true)] [CanEditMultipleObjects]
    internal class LocalizedComponentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();
            var iter = serializedObject.GetIterator();
            iter.NextVisible(true);
            while(iter.NextVisible(false)) L10n.PropertyField(iter);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
