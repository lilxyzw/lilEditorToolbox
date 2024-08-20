using System.IO;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    public class ExtensionDrawer : IProjectExtensionComponent
    {
        public int Priority => 0;

        public void OnGUI(ref Rect currentRect, string guid, string path, string name, string extension, Rect fullRect)
        {
            if(!File.Exists(path) || fullRect.x > 16) return;
            currentRect.width = EditorStyles.label.CalcSize(new GUIContent(extension)).x;
            GUI.Label(currentRect, extension, EditorStyles.label);
            currentRect.x += currentRect.width;
        }
    }
}
