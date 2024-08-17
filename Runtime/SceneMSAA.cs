using UnityEngine;

namespace jp.lilxyzw.editortoolbox.runtime
{
    [ExecuteAlways, ImageEffectAllowedInSceneView]
    internal class SceneMSAA : MonoBehaviour
    {
        [ImageEffectUsesCommandBuffer] void OnRenderImage(RenderTexture src, RenderTexture dst) => Graphics.Blit(src, dst);
    }
}
