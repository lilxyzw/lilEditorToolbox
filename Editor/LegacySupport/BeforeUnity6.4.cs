#if !UNITY_6000_4_OR_NEWER
// 古いUnityでEntityIdが使えないのでその対応
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal static class ProjectWindowUtil
    {
        public static void CreateAssetWithTextContent(string filename, string content) => UnityEditor.ProjectWindowUtil.CreateAssetWithContent(filename, content);
        public static void ShowCreatedAsset(Object o) => UnityEditor.ProjectWindowUtil.ShowCreatedAsset(o);
        public static void CreateAsset(Object asset, string pathName) => UnityEditor.ProjectWindowUtil.CreateAsset(asset, pathName);
    }

    internal static class EditorUtility
    {
        public static Object EntityIdToObject(EntityId entityId) => UnityEditor.EditorUtility.InstanceIDToObject(entityId.i);
        public static EntityId GetEntityId(this Object obj) => obj.GetInstanceID();
        public static void CompressTexture(Texture2D texture, TextureFormat format, TextureCompressionQuality quality) => UnityEditor.EditorUtility.CompressTexture(texture, format, quality);
        public static string SaveFilePanel(string title, string directory, string defaultName, string extension) => UnityEditor.EditorUtility.SaveFilePanel(title, directory, defaultName, extension);
        public static string SaveFolderPanel(string title, string folder, string defaultName) => UnityEditor.EditorUtility.SaveFolderPanel(title, folder, defaultName);
        public static void DisplayCustomMenu(Rect position, GUIContent[] options, int selected, UnityEditor.EditorUtility.SelectMenuItemFunction callback, object userData) => UnityEditor.EditorUtility.DisplayCustomMenu(position, options, selected, callback, userData);
        public static bool DisplayDialog(string title, string message, string ok) => UnityEditor.EditorUtility.DisplayDialog(title, message, ok);
    }

    public struct EntityId
    {
        public int i;
        public static EntityId FromULong(ulong value) => new(){i = (int)value};
        public static EntityId None => new(){i=0};
        public override bool Equals(object obj) => i == ((EntityId)obj).i;
        public static implicit operator int(EntityId entityId) => entityId.i;
        public static implicit operator EntityId(int intValue) => new(){i = intValue};
        public override readonly int GetHashCode() => i;
    }

    internal static class DragAndDrop
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
