using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    [Tooltip("Displays the layer and tag of an object.")]
    internal class LayerAndTag : IHierarchyExtensionComponent
    {
        private static GUIStyle m_styleMiniText;
        private static GUIStyle StyleMiniText => m_styleMiniText != null ? m_styleMiniText : m_styleMiniText = new GUIStyle(EditorStyles.label){fontSize = (int)(EditorStyles.label.fontSize*0.75f)};

        public int Priority => 200;

        public void OnGUI(ref Rect currentRect, GameObject gameObject, int instanceID, Rect fullRect)
        {
            var layer = new GUIContent(LayerMask.LayerToName(gameObject.layer));
            var tag = new GUIContent(gameObject.tag);
            var width = 64f;
            //var width = EditorStyles.miniLabel.CalcSize(layer).x;
            currentRect.x -= width;
            currentRect.width = width;
            var rectMini = currentRect;
            rectMini.height = currentRect.height*0.6f;

            GUI.enabled = gameObject.layer != 0;
            GUI.Label(rectMini, layer, StyleMiniText);
            rectMini.y += currentRect.height*0.5f;

            GUI.enabled = gameObject.tag != "Untagged";
            GUI.Label(rectMini, tag, StyleMiniText);
            GUI.enabled = true;

            currentRect.x -= 8;
        }
    }
}
