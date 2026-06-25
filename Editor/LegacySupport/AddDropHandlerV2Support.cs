#if !UNITY_6000_3_OR_NEWER
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal static partial class DragAndDrop
    {
        public static string[] paths => UnityEditor.DragAndDrop.paths;
        public static Object[] objectReferences{
            get => UnityEditor.DragAndDrop.objectReferences;
            set => UnityEditor.DragAndDrop.objectReferences = value;
        }
        public static DragAndDropVisualMode visualMode{
            get => UnityEditor.DragAndDrop.visualMode;
            set => UnityEditor.DragAndDrop.visualMode = value;
        }
        public static void AddDropHandlerV2(UnityEditor.DragAndDrop.ProjectBrowserDropHandler handler) => UnityEditor.DragAndDrop.AddDropHandler(handler);
        public static void StartDrag(string title) => UnityEditor.DragAndDrop.StartDrag(title);
        public static void PrepareStartDrag() => UnityEditor.DragAndDrop.PrepareStartDrag();
        public static void AcceptDrag() => UnityEditor.DragAndDrop.AcceptDrag();
    }
}
#endif
