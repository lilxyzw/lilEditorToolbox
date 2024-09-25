using System.Linq;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal class ComponentsDrawer : IHierarchyExtensionComponent
    {
        private const int ICON_SIZE = 16;

        public int Priority => 1100;

        public void OnGUI(ref Rect currentRect, GameObject gameObject, int instanceID, Rect fullRect)
        {
            var components = gameObject.GetComponents<Component>().Where(c => c is not Transform).ToArray();
            if(components.Length > 0)
            {
                currentRect.x -= ICON_SIZE * components.Length;
                currentRect.width = ICON_SIZE;
                var xmin = currentRect.x;

                foreach(var component in components)
                {
                    switch(component)
                    {
                        case Renderer c: GUI.enabled = c.enabled; break;
                        case Behaviour c: GUI.enabled = c.enabled; break;
                        case Collider c: GUI.enabled = c.enabled; break;
                    }
                    if(component) GUI.Box(currentRect, AssetPreview.GetMiniThumbnail(component), GUIStyle.none);
                    else          GUI.Box(currentRect, HierarchyExtension.MissingScriptIcon(), GUIStyle.none);
                    GUI.enabled = true;

                    if((component is Renderer || component is Behaviour || component is Collider) && GUI.Button(currentRect, Texture2D.blackTexture, GUIStyle.none))
                    {
                        using var so = new SerializedObject(component);
                        using var m_Enabled = so.FindProperty("m_Enabled");
                        m_Enabled.boolValue = !m_Enabled.boolValue;
                        so.ApplyModifiedProperties();
                    }

                    currentRect.x += ICON_SIZE;
                }

                currentRect.x = xmin - 8;
            }
        }
    }
}
