using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal class MaterialShader : IProjectExtensionComponent
    {
        public int Priority => 2;

        public void OnGUI(ref Rect currentRect, string guid, string path, string name, string extension, Rect fullRect)
        {
            if(extension != ".mat" || ProjectExtension.GUIDToObject(guid) is not Material material) return;

            if(material.shader && material.shader.isSupported) GUIHelper.DrawLabel(ref currentRect, material.shader.name);
            else GUIHelper.DrawLabel(ref currentRect, "ERROR MATERIAL");
        }
    }
}
