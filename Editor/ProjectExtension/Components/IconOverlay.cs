using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace jp.lilxyzw.editortoolbox
{
    [Tooltip("Overlay any image on the icon.")]
    internal class IconOverlay : IProjectExtensionComponent
    {
        public int Priority => -1600;

        public void OnGUI(ref Rect currentRect, string guid, string path, string name, string extension, Rect fullRect)
        {
            if(!ProjectExtension.isIconGUI || !IconOverlayData.Dic.TryGetValue(guid, out var icon) || !icon) return;
            var rect = fullRect;
            rect.height = rect.width;

            var color = GUI.color;
            GUI.color = Color.clear;
            EditorGUI.DrawTextureTransparent(rect, icon);
            GUI.color = color;
        }
    }

    // ディレクトリが空だとUnityのバグでエラーになるので Assets/../ は必須
    [FilePath("Assets/../jp.lilxyzw.editortoolbox.IconOverlayData.asset", FilePathAttribute.Location.ProjectFolder)]
    internal class IconOverlayData : ScriptableSingleton<IconOverlayData>
    {
        public IconOverlayItem[] items = new IconOverlayItem[]{};
        private static Dictionary<string, Texture2D> m_dic;
        internal static Dictionary<string, Texture2D> Dic => m_dic ??= InitDic();
        private static HashSet<Texture2D> tempIcons = new();

        private static Dictionary<string, Texture2D> InitDic()
        {
            return instance.items.Where(i => i.target && i.icon).ToDictionary(
                i => AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(i.target)),
                i => GetClipIcon(i));
        }

        private static Texture2D GetClipIcon(IconOverlayItem item)
        {
            if(!item.clipBottom && !item.clipTop && item.scale == new Vector2(1,1) && item.offset == Vector2.zero) return item.icon;

            var objIcon = AssetPreview.GetMiniThumbnail(item.target);

            var material = new Material(Shader.Find("Hidden/_lil/CopyAlpha"));
            material.SetTexture("_TextureMain", item.icon);
            material.SetTexture("_TextureBack", objIcon);
            material.SetFloat("clipBottom", item.clipBottom ? 1 : 0);
            material.SetFloat("clipTop", item.clipTop ? 1 : 0);
            material.SetTextureScale("_TextureMain", item.scale);
            material.SetTextureOffset("_TextureMain", item.offset);

            RenderTexture bufRT = RenderTexture.active;
            RenderTexture texR = RenderTexture.GetTemporary(objIcon.width, objIcon.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            RenderTexture.active = texR;
            Graphics.Blit(null, texR, material);
            var tex = new Texture2D(objIcon.width, objIcon.height, TextureFormat.RGBA32, false, false);
            tex.ReadPixels(new Rect(0, 0, texR.width, texR.height), 0, 0);
            tex.Apply();
            RenderTexture.active = bufRT;
            RenderTexture.ReleaseTemporary(texR);
            tempIcons.Add(tex);

            return tex;
        }

        [Serializable]
        public class IconOverlayItem : DirectElements
        {
            public Object target;
            public Texture2D icon;
            public bool clipBottom;
            public bool clipTop;
            public Vector2 scale = new(1,1);
            public Vector2 offset = Vector2.zero;
        }

        [CustomEditor(typeof(IconOverlayData))]
        private class IconOverlayEditor : EditorWindow
        {
            [MenuItem(Common.MENU_HEAD + "Icon Overlay Editor")]
            static void Init() => GetWindow(typeof(IconOverlayEditor)).Show();

            void OnGUI()
            {
                var wide = EditorGUIUtility.wideMode;
                EditorGUIUtility.wideMode = true;
                using var serializedObject = new SerializedObject(instance);
                EditorGUI.BeginChangeCheck();
                serializedObject.UpdateIfRequiredOrScript();
                SerializedProperty iterator = serializedObject.GetIterator();
                iterator.NextVisible(true); // m_Script
                while(iterator.NextVisible(false))
                    L10n.PropertyField(iterator);

                if(EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    instance.Save(true);

                    foreach(var icon in tempIcons) DestroyImmediate(icon);
                    tempIcons.Clear();
                    m_dic = InitDic();
                    EditorApplication.RepaintProjectWindow();
                }
                EditorGUIUtility.wideMode = wide;
            }
        }
    }
}