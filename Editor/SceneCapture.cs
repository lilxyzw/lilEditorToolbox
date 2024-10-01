using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal class SceneCapture : EditorWindow
    {
        public bool autoHeight = true;
        public int width = 3840;
        public int height = 2160;

        public bool captureTarget = false;

        [MenuItem(Common.MENU_HEAD + "Scene Capture")]
        static void Init() => GetWindow(typeof(SceneCapture)).Show();

        void OnGUI()
        {
            var sceneView = SceneView.lastActiveSceneView;
            if(!sceneView || !sceneView.camera)
            {
                EditorGUILayout.LabelField(L10n.L("Scene View is not open."));
                return;
            }

            captureTarget = EditorGUILayout.ToggleLeft(L10n.L("Capture only selected objects"), captureTarget);

            if(captureTarget && !Selection.activeGameObject)
            {
                EditorGUILayout.LabelField(L10n.L("No GameObject is selected."));
                return;
            }

            var aspect = sceneView.position.height / sceneView.position.width;
            autoHeight = EditorGUILayout.ToggleLeft(L10n.L("Fit Aspect Ratio to Scene View"), autoHeight);
            width = EditorGUILayout.IntField(L10n.L("Width"), width);
            if(autoHeight)
            {
                GUI.enabled = false;
                EditorGUILayout.IntField(L10n.L("Height"), (int)(width * aspect));
                GUI.enabled = true;
            }
            else
            {
                height = EditorGUILayout.IntField(L10n.L("Height"), height);
            }

            if(GUILayout.Button(L10n.L("Capture"))) Capture();
        }

        private void Capture()
        {
            var path = EditorUtility.SaveFilePanel("Save capture", "", "", "png");
            if(string.IsNullOrEmpty(path)) return;

            var currentRT = RenderTexture.active;

            var sceneView = SceneView.lastActiveSceneView;
            var aspect = sceneView.position.height / sceneView.position.width;
            var heightO = autoHeight ? (int)(width * aspect) : height;

            var renderTexture = RenderTexture.GetTemporary(width, heightO, 24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Default);
            renderTexture.antiAliasing = 8;
            RenderTexture.active = renderTexture;

            var camera = Instantiate(sceneView.camera);
            camera.targetTexture = renderTexture;
            camera.allowHDR = true;
            camera.allowMSAA = true;

            var objs = new List<GameObject>();
            if(captureTarget)
            {
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = Color.clear;
                camera.cullingMask = 1 << 31;
                foreach(var gameObject in Selection.gameObjects)
                {
                    var obj = Instantiate(gameObject);
                    obj.SetActive(true);
                    foreach(var t in obj.GetComponentsInChildren<Transform>()) t.gameObject.layer = 31;
                    objs.Add(obj);
                }
            }

            camera.Render();

            var renderTexture2 = RenderTexture.GetTemporary(width, heightO, 24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Default);
            Graphics.Blit(renderTexture, renderTexture2, new Material(Shader.Find("Hidden/_lil/FixColor")));

            var tex = new Texture2D(width, heightO, TextureFormat.RGBA32, false, false);
            tex.ReadPixels(new Rect(0, 0, width, heightO), 0, 0);
            tex.Apply();
            File.WriteAllBytes(path, tex.EncodeToPNG());

            RenderTexture.active = currentRT;

            foreach(var obj in objs) DestroyImmediate(obj);
            DestroyImmediate(camera.gameObject);
            DestroyImmediate(tex);

            EditorUtility.DisplayDialog("Scene Capture", L10n.L("Complete!"), L10n.L("OK"));
        }
    }
}
