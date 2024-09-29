using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal class TexturePacker : EditorWindow
    {
        public ChannelParam[] channelParams = {
            new(){mode = ChannelMode.R, def = 0},
            new(){mode = ChannelMode.G, def = 0},
            new(){mode = ChannelMode.B, def = 0},
            new(){mode = ChannelMode.A, def = 1}
        };
        private Material material;
        public Texture2D packed;

        private readonly Dictionary<Texture2D, Texture2D> rawTexs = new();

        [MenuItem(Common.MENU_HEAD + "Texture Packer")]
        static void Init()
        {
            var window = GetWindow(typeof(TexturePacker));
            window.position = new(window.position.x,window.position.y,1000,window.position.height);
            window.Show();
        }

        void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            GUIPerChannel("R Channel", channelParams[0]);
            GUIPerChannel("G Channel", channelParams[1]);
            GUIPerChannel("B Channel", channelParams[2]);
            GUIPerChannel("A Channel", channelParams[3]);
            EditorGUILayout.EndHorizontal();
            if(EditorGUI.EndChangeCheck())
            {
                material = new(Shader.Find("Hidden/_lil/TexturePacker"));
                material.SetTexture("_TextureR", GetUncompressedTexture(channelParams[0].tex));
                material.SetTexture("_TextureG", GetUncompressedTexture(channelParams[1].tex));
                material.SetTexture("_TextureB", GetUncompressedTexture(channelParams[2].tex));
                material.SetTexture("_TextureA", GetUncompressedTexture(channelParams[3].tex));
                material.SetVector("_BlendR", ModeToVector(channelParams[0]));
                material.SetVector("_BlendG", ModeToVector(channelParams[1]));
                material.SetVector("_BlendB", ModeToVector(channelParams[2]));
                material.SetVector("_BlendA", ModeToVector(channelParams[3]));
                material.SetVector("_IgnoreTexture", new Vector4(
                    channelParams[0].tex ? 0 : 1,
                    channelParams[1].tex ? 0 : 1,
                    channelParams[2].tex ? 0 : 1,
                    channelParams[3].tex ? 0 : 1
                ));
                material.SetVector("_Invert", new Vector4(
                    channelParams[0].mode == ChannelMode.OneMinusR || channelParams[0].mode == ChannelMode.OneMinusG || channelParams[0].mode == ChannelMode.OneMinusB || channelParams[0].mode == ChannelMode.OneMinusA?1:0,
                    channelParams[1].mode == ChannelMode.OneMinusR || channelParams[1].mode == ChannelMode.OneMinusG || channelParams[1].mode == ChannelMode.OneMinusB || channelParams[1].mode == ChannelMode.OneMinusA?1:0,
                    channelParams[2].mode == ChannelMode.OneMinusR || channelParams[2].mode == ChannelMode.OneMinusG || channelParams[2].mode == ChannelMode.OneMinusB || channelParams[2].mode == ChannelMode.OneMinusA?1:0,
                    channelParams[3].mode == ChannelMode.OneMinusR || channelParams[3].mode == ChannelMode.OneMinusG || channelParams[3].mode == ChannelMode.OneMinusB || channelParams[3].mode == ChannelMode.OneMinusA?1:0
                ));
                material.SetFloat("_DefaultR", channelParams[0].def);
                material.SetFloat("_DefaultG", channelParams[1].def);
                material.SetFloat("_DefaultB", channelParams[2].def);
                material.SetFloat("_DefaultA", channelParams[3].def);

                int width = 32;
                int height = 32;
                if(channelParams[0].tex){width = Mathf.Max(width,channelParams[0].tex.width); height = Mathf.Max(height,channelParams[0].tex.height);}
                if(channelParams[1].tex){width = Mathf.Max(width,channelParams[1].tex.width); height = Mathf.Max(height,channelParams[1].tex.height);}
                if(channelParams[2].tex){width = Mathf.Max(width,channelParams[2].tex.width); height = Mathf.Max(height,channelParams[2].tex.height);}
                if(channelParams[3].tex){width = Mathf.Max(width,channelParams[3].tex.width); height = Mathf.Max(height,channelParams[3].tex.height);}
                var currentRT = RenderTexture.active;
                var renderTexture = RenderTexture.GetTemporary(width, height);
                RenderTexture.active = renderTexture;
                Graphics.Blit(null, renderTexture, material);
                packed = new Texture2D(width, height, TextureFormat.RGBA32, false, false);
                packed.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                packed.Apply();
                RenderTexture.active = currentRT;
                //File.WriteAllBytes(path, tex.EncodeToPNG());
            }

            if(packed)
            {
                var rect = EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                float height4T = rect.height - 64;
                float width = packed.width;
                float height = packed.height;
                if(rect.width < width)
                {
                    height = height * rect.width / width;
                    width = rect.width;
                }
                if(height4T < height)
                {
                    width = width * height4T / height;
                    height = height4T;
                }
                EditorGUI.DrawTextureTransparent(new Rect(rect.x+(rect.width-width)/2,rect.y+16,width,height), packed);
                if(GUI.Button(new Rect(rect.x,rect.yMax-32,rect.width,32), "Export Texture"))
                {
                    var path = EditorUtility.SaveFilePanel("Save capture", "", "", "png");
                    if(string.IsNullOrEmpty(path)) return;
                    File.WriteAllBytes(path, packed.EncodeToPNG());
                }
            }
        }

        void OnDisable()
        {
            foreach(var kv in rawTexs)
                if(kv.Value) DestroyImmediate(kv.Value);
            rawTexs.Clear();
        }

        private Texture2D GetUncompressedTexture(Texture2D tex)
        {
            if(!tex) return tex;
            if(rawTexs.ContainsKey(tex)) return rawTexs[tex];
            var path = AssetDatabase.GetAssetPath(tex);
            if(string.IsNullOrEmpty(path)) return tex;

            var raw = new Texture2D(0,0, TextureFormat.RGBA32, false, !tex.isDataSRGB);
            if(raw.LoadImage(File.ReadAllBytes(path))) return rawTexs[tex] = raw;

            if(raw) DestroyImmediate(raw);
            return tex;
        }

        private void GUIPerChannel(string label, ChannelParam param)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            param.tex = EditorGUI.ObjectField(EditorGUILayout.GetControlRect(GUILayout.Width(96), GUILayout.Height(96)), param.tex, typeof(Texture2D), false) as Texture2D;
            if(param.tex)
            {
                param.mode = (ChannelMode)EditorGUILayout.EnumPopup("使うチャンネル", param.mode);
                if(param.mode == ChannelMode.Custom) param.blend = EditorGUILayout.Vector4Field("", param.blend);
            }
            else
            {
                param.def = EditorGUILayout.FloatField("チャンネルを埋める値", param.def);
            }
            EditorGUILayout.EndVertical();
        }

        private Vector4 ModeToVector(ChannelParam param)
        {
            switch(param.mode)
            {
                case ChannelMode.R : return new(1,0,0,0);
                case ChannelMode.G : return new(0,1,0,0);
                case ChannelMode.B : return new(0,0,1,0);
                case ChannelMode.A : return new(0,0,0,1);
                case ChannelMode.OneMinusR : return new(1,0,0,0);
                case ChannelMode.OneMinusG : return new(0,1,0,0);
                case ChannelMode.OneMinusB : return new(0,0,1,0);
                case ChannelMode.OneMinusA : return new(0,0,0,1);
                case ChannelMode.Gray : return new(0.333333f, 0.333333f, 0.333333f, 0f);
                case ChannelMode.Luminance : return new(0.2126729f, 0.7151522f, 0.0721750f, 0f);
                default : return param.blend;
            }
        }

        [Serializable]
        internal class ChannelParam
        {
            public Texture2D tex;
            public ChannelMode mode;
            public Vector4 blend;
            public float def = 0;
        }

        internal enum ChannelMode
        {
            R,
            G,
            B,
            A,
            OneMinusR,
            OneMinusG,
            OneMinusB,
            OneMinusA,
            Gray,
            Luminance,
            Custom
        }
    }
}
