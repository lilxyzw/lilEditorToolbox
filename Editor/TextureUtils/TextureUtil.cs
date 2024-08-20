#if UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace jp.lilxyzw.editortoolbox
{
    internal class TextureUtil
    {
        private const string menuPathAssets                 = "Assets/_lil/lilEditorToolbox/";
        private const string menuPathConvertNormal          = menuPathAssets + "[Texture] Convert normal map (DirectX <-> OpenGL)";
        private const string menuPathS2MG                   = menuPathAssets + "[Texture] Smoothness/Smoothness -> MetallicGlossMap";
        private const string menuPathMS2MG                  = menuPathAssets + "[Texture] Smoothness/Metallic, Smoothness (, Occlusion, Detail) -> MetallicGlossMap (MaskMap)";
        private const string menuPathPR2S                   = menuPathAssets + "[Texture] Perceptual Roughness/Roughness -> Smoothness";
        private const string menuPathPR2MG                  = menuPathAssets + "[Texture] Perceptual Roughness/Roughness -> MetallicGlossMap";
        private const string menuPathMPR2MG                 = menuPathAssets + "[Texture] Perceptual Roughness/Metallic, Roughness (, Occlusion, Detail) -> MetallicGlossMap (MaskMap)";
        private const string menuPathR2S                    = menuPathAssets + "[Texture] Roughness/Roughness -> Smoothness";
        private const string menuPathR2MG                   = menuPathAssets + "[Texture] Roughness/Roughness -> MetallicGlossMap";
        private const string menuPathMR2MG                  = menuPathAssets + "[Texture] Roughness/Metallic, Roughness (, Occlusion, Detail) -> MetallicGlossMap (MaskMap)";

        private const int menuPriorityAssets = 1100;
        private const int menuPriorityConvertNormal         = menuPriorityAssets + 0; // [Texture] Convert normal map (DirectX <-> OpenGL)
        private const int menuPriorityS2MG                  = menuPriorityAssets + 1; // [Texture] Smoothness/Smoothness -> MetallicGlossMap
        private const int menuPriorityMS2MG                 = menuPriorityAssets + 2; // [Texture] Smoothness/Metallic and Smoothness -> MetallicGlossMap
        private const int menuPriorityPR2S                  = menuPriorityAssets + 3; // [Texture] Perceptual Roughness/Roughness -> Smoothness
        private const int menuPriorityPR2MG                 = menuPriorityAssets + 4; // [Texture] Perceptual Roughness/Roughness -> MetallicGlossMap
        private const int menuPriorityMPR2MG                = menuPriorityAssets + 5; // [Texture] Perceptual Roughness/Metallic and Roughness -> MetallicGlossMap
        private const int menuPriorityR2S                   = menuPriorityAssets + 6; // [Texture] Roughness/Roughness -> Smoothness
        private const int menuPriorityR2MG                  = menuPriorityAssets + 7; // [Texture] Roughness/Roughness -> MetallicGlossMap
        private const int menuPriorityMR2MG                 = menuPriorityAssets + 8; // [Texture] Roughness/Metallic and Roughness -> MetallicGlossMap

        //------------------------------------------------------------------------------------------------------------------------------
        // [Texture] Convert normal map (DirectX <-> OpenGL)
        [MenuItem(menuPathConvertNormal, false, menuPriorityConvertNormal)]
        private static void ConvertNormal()
        {
            foreach(Object obj in Selection.objects)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                if(!CheckAssetIsTexture(obj)) return;

                GetTexture2D(path, out Texture2D tex, out Color[] pixels);

                for(int i = 0; i < pixels.Length; i++)
                {
                    pixels[i].g = 1.0f - pixels[i].g;
                }

                SaveTexture2D(path, tex, pixels);
            }
        }

        [MenuItem(menuPathConvertNormal, true, menuPriorityConvertNormal)]
        private static bool CheckConvertNormal(){ return CheckAssetIsTexture(); }

        //------------------------------------------------------------------------------------------------------------------------------
        // [Texture] Roughness <-> Smoothness
        [MenuItem(menuPathR2S, false, menuPriorityR2S)]
        private static void R2S()
        {
            foreach(Object obj in Selection.objects)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                if(!CheckAssetIsTexture(obj)) return;

                GetTexture2D(path, out Texture2D tex, out Color[] pixels);

                for(int i = 0; i < pixels.Length; i++)
                {
                    pixels[i].r = 1.0f - Mathf.Sqrt(pixels[i].r);
                    pixels[i].g = 1.0f - pixels[i].g;
                    pixels[i].b = 1.0f - pixels[i].b;
                }

                SaveTexture2D(path, tex, pixels);
            }
        }

        [MenuItem(menuPathR2S, true, menuPriorityR2S)]
        private static bool CheckR2S(){ return CheckAssetIsTexture(); }

        //------------------------------------------------------------------------------------------------------------------------------
        // [Texture] Perceptual Roughness <-> Smoothness
        [MenuItem(menuPathPR2S, false, menuPriorityPR2S)]
        private static void PR2S()
        {
            foreach(Object obj in Selection.objects)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                if(!CheckAssetIsTexture(obj)) return;

                GetTexture2D(path, out Texture2D tex, out Color[] pixels);

                for(int i = 0; i < pixels.Length; i++)
                {
                    pixels[i].r = 1.0f - pixels[i].r;
                    pixels[i].g = 1.0f - pixels[i].g;
                    pixels[i].b = 1.0f - pixels[i].b;
                }

                SaveTexture2D(path, tex, pixels);
            }
        }

        [MenuItem(menuPathPR2S, true, menuPriorityPR2S)]
        private static bool CheckPR2S(){ return CheckAssetIsTexture(); }

        //------------------------------------------------------------------------------------------------------------------------------
        // [Texture] Roughness -> MetallicGlossMap
        [MenuItem(menuPathR2MG, false, menuPriorityR2MG)]
        private static void R2MG()
        {
            foreach(Object obj in Selection.objects)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                if(!CheckAssetIsTexture(obj)) return;

                GetTexture2D(path, out Texture2D tex, out Color[] pixels);

                for(int i = 0; i < pixels.Length; i++)
                {
                    pixels[i].a = 1.0f - Mathf.Sqrt(pixels[i].r);
                    pixels[i].r = 1.0f;
                    pixels[i].g = 1.0f;
                    pixels[i].b = 1.0f;
                }

                SaveTexture2D(path, tex, pixels);
            }
        }

        [MenuItem(menuPathR2MG, true, menuPriorityR2MG)]
        private static bool CheckR2MG(){ return CheckAssetIsTexture(); }

        //------------------------------------------------------------------------------------------------------------------------------
        // [Texture] Perceptual Roughness -> MetallicGlossMap
        [MenuItem(menuPathPR2MG, false, menuPriorityPR2MG)]
        private static void PR2MG()
        {
            foreach(Object obj in Selection.objects)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                if(!CheckAssetIsTexture(obj)) return;

                GetTexture2D(path, out Texture2D tex, out Color[] pixels);

                for(int i = 0; i < pixels.Length; i++)
                {
                    pixels[i].a = 1.0f - pixels[i].r;
                    pixels[i].r = 1.0f;
                    pixels[i].g = 1.0f;
                    pixels[i].b = 1.0f;
                }

                SaveTexture2D(path, tex, pixels);
            }
        }

        [MenuItem(menuPathPR2MG, true, menuPriorityPR2MG)]
        private static bool CheckPR2MG(){ return CheckAssetIsTexture(); }

        //------------------------------------------------------------------------------------------------------------------------------
        // [Texture] Smoothness -> MetallicGlossMap
        [MenuItem(menuPathS2MG, false, menuPriorityS2MG)]
        private static void S2MG()
        {
            foreach(Object obj in Selection.objects)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                if(!CheckAssetIsTexture(obj)) return;

                GetTexture2D(path, out Texture2D tex, out Color[] pixels);

                for(int i = 0; i < pixels.Length; i++)
                {
                    pixels[i].a = pixels[i].r;
                    pixels[i].r = 1.0f;
                    pixels[i].g = 1.0f;
                    pixels[i].b = 1.0f;
                }

                SaveTexture2D(path, tex, pixels);
            }
        }

        [MenuItem(menuPathS2MG, true, menuPriorityS2MG)]
        private static bool CheckS2MG(){ return CheckAssetIsTexture(); }

        //------------------------------------------------------------------------------------------------------------------------------
        // [Texture] Metallic & Roughness -> MetallicGlossMap
        [MenuItem(menuPathMR2MG, false, menuPriorityMR2MG)]
        private static void MR2MG()
        {
            if(Selection.objects.Length < 2) return;
            string pathM = null;
            string pathR = null;
            string pathO = null;
            string pathD = null;
            foreach(Object obj in Selection.objects)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                if(!CheckAssetIsTexture(obj)) continue;
                if(CheckMetallic(path)) pathM = path;
                if(CheckRoughness(path)) pathR = path;
                if(CheckOcclusion(path)) pathO = path;
                if(CheckDetail(path)) pathD = path;
            }

            if(string.IsNullOrEmpty(pathM) || string.IsNullOrEmpty(pathR)) return;

            GetTexture2D(pathM, out Texture2D texM, out Color[] pixelsM);
            GetTexture2D(pathR, out Texture2D texR, out Color[] pixelsR);

            for(int i = 0; i < pixelsM.Length; i++)
            {
                pixelsM[i].a = 1.0f - Mathf.Sqrt(pixelsR[i].r);
                pixelsM[i].g = 1.0f;
                pixelsM[i].b = 1.0f;
            }
            CopyOcculusion(pathO, ref pixelsM);
            CopyDetail(pathD, ref pixelsM);

            Object.DestroyImmediate(texR);
            SaveTexture2D(pathM, texM, pixelsM);
        }

        [MenuItem(menuPathMR2MG, true, menuPriorityMR2MG)]
        private static bool CheckMR2MG(){ return CheckAssetIsTexture(); }

        //------------------------------------------------------------------------------------------------------------------------------
        // [Texture] Metallic & Perceptual Roughness -> MetallicGlossMap
        [MenuItem(menuPathMPR2MG, false, menuPriorityMPR2MG)]
        private static void MPR2MG()
        {
            if(Selection.objects.Length < 2) return;
            string pathM = null;
            string pathR = null;
            string pathO = null;
            string pathD = null;
            foreach(Object obj in Selection.objects)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                if(!CheckAssetIsTexture(obj)) continue;
                if(CheckMetallic(path)) pathM = path;
                if(CheckRoughness(path)) pathR = path;
                if(CheckOcclusion(path)) pathO = path;
                if(CheckDetail(path)) pathD = path;
            }

            if(string.IsNullOrEmpty(pathM) || string.IsNullOrEmpty(pathR)) return;

            GetTexture2D(pathM, out Texture2D texM, out Color[] pixelsM);
            GetTexture2D(pathR, out Texture2D texR, out Color[] pixelsR);

            for(int i = 0; i < pixelsM.Length; i++)
            {
                pixelsM[i].a = 1.0f - pixelsR[i].r;
                pixelsM[i].g = 1.0f;
                pixelsM[i].b = 1.0f;
            }
            CopyOcculusion(pathO, ref pixelsM);
            CopyDetail(pathD, ref pixelsM);

            Object.DestroyImmediate(texR);
            SaveTexture2D(pathM, texM, pixelsM);
        }

        [MenuItem(menuPathMPR2MG, true, menuPriorityMPR2MG)]
        private static bool CheckMPR2MG(){ return CheckAssetIsTexture(); }

        //------------------------------------------------------------------------------------------------------------------------------
        // [Texture] Metallic & Smoothness -> MetallicGlossMap
        [MenuItem(menuPathMS2MG, false, menuPriorityMS2MG)]
        private static void MS2MG()
        {
            if(Selection.objects.Length < 2) return;
            string pathM = null;
            string pathS = null;
            string pathO = null;
            string pathD = null;
            foreach(Object obj in Selection.objects)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                if(!CheckAssetIsTexture(obj)) continue;
                if(CheckMetallic(path)) pathM = path;
                if(CheckSmoothness(path)) pathS = path;
                if(CheckOcclusion(path)) pathO = path;
                if(CheckDetail(path)) pathD = path;
            }

            if(string.IsNullOrEmpty(pathM) || string.IsNullOrEmpty(pathS)) return;

            GetTexture2D(pathM, out Texture2D texM, out Color[] pixelsM);
            GetTexture2D(pathS, out Texture2D texS, out Color[] pixelsS);

            for(int i = 0; i < pixelsM.Length; i++)
            {
                pixelsM[i].a = pixelsS[i].r;
                pixelsM[i].g = 1.0f;
                pixelsM[i].b = 1.0f;
            }
            CopyOcculusion(pathO, ref pixelsM);
            CopyDetail(pathD, ref pixelsM);

            Object.DestroyImmediate(texS);
            SaveTexture2D(pathM, texM, pixelsM);
        }

        [MenuItem(menuPathMS2MG, true, menuPriorityMS2MG)]
        private static bool CheckMS2MG(){ return CheckAssetIsTexture(); }

        //------------------------------------------------------------------------------------------------------------------------------
        // Format checker
        private static bool CheckExtension(string extension)
        {
            if(Selection.objects == null || Selection.objects.Length == 0) return false;
            foreach(Object obj in Selection.objects)
            {
                string assetPath = AssetDatabase.GetAssetPath(obj);
                if(assetPath.EndsWith(extension, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        private static bool CheckAssetIsTexture(Object obj)
        {
            return obj is Texture2D;
        }

        private static bool CheckAssetIsTexture()
        {
            if(Selection.objects == null || Selection.objects.Length == 0) return false;
            foreach(Object obj in Selection.objects)
            {
                if(obj is Texture2D) return true;
            }
            return false;
        }

        //------------------------------------------------------------------------------------------------------------------------------
        // Name checker
        private static bool CheckMetallic(string path)
        {
            return path.IndexOf("metallic", StringComparison.OrdinalIgnoreCase) >= 0 ||
                path.IndexOf("metalness", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool CheckRoughness(string path)
        {
            return path.IndexOf("roughness", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool CheckSmoothness(string path)
        {
            return path.IndexOf("smoothness", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool CheckOcclusion(string path)
        {
            return path.IndexOf("occlusion", StringComparison.OrdinalIgnoreCase) >= 0 ||
                path.IndexOf("AO") >= 0;
        }

        private static bool CheckDetail(string path)
        {
            return path.IndexOf("detail", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static void CopyOcculusion(string path, ref Color[] pixels)
        {
            if(string.IsNullOrEmpty(path)) return;
            GetTexture2D(path, out Texture2D texO, out Color[] pixelsO);
            for(int i = 0; i < pixels.Length; i++)
            {
                pixels[i].g = pixelsO[i].r;
            }
        }

        private static void CopyDetail(string path, ref Color[] pixels)
        {
            if(string.IsNullOrEmpty(path)) return;
            GetTexture2D(path, out Texture2D texD, out Color[] pixelsD);
            for(int i = 0; i < pixels.Length; i++)
            {
                pixels[i].b = pixelsD[i].r;
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------
        // Load and save
        private static void GetTexture2D(string path, out Texture2D tex, out Color[] pixels)
        {
            tex = new Texture2D(2, 2, TextureFormat.ARGB32, true, true);
            byte[] bytes = File.ReadAllBytes(Path.GetFullPath(path));
            if(!tex.LoadImage(bytes))
            {
                tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                GetReadableTexture(ref tex);
            }
            if(tex != null) pixels = tex.GetPixels();
            else            pixels = null;
        }

        private static void GetReadableTexture(ref Texture2D tex)
        {
            if(tex == null) return;

            #if UNITY_2018_3_OR_NEWER
            if(!tex.isReadable)
            #endif
            {
                RenderTexture bufRT = RenderTexture.active;
                RenderTexture texR = RenderTexture.GetTemporary(tex.width, tex.height);
                Graphics.Blit(tex, texR);
                RenderTexture.active = texR;
                tex = new Texture2D(texR.width, texR.height);
                tex.ReadPixels(new Rect(0, 0, texR.width, texR.height), 0, 0);
                tex.Apply();
                RenderTexture.active = bufRT;
                RenderTexture.ReleaseTemporary(texR);
            }
        }

        private static void SaveTexture2D(string path, Texture2D tex, Color[] pixels)
        {
            tex = new Texture2D(tex.height, tex.width, TextureFormat.ARGB32, true, true);
            tex.SetPixels(pixels);
            tex.Apply(false);
            SaveToPng(path, "_conv", tex);
            Object.DestroyImmediate(tex);
            AssetDatabase.Refresh();
        }

        private static string SaveToPng(string path, string add, Texture2D tex)
        {
            string savePath = Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path) + add + ".png";
            File.WriteAllBytes(savePath, tex.EncodeToPNG());
            return savePath;
        }
    }
}
#endif