using UnityEngine;

namespace jp.lilxyzw.editortoolbox.runtime
{
    [Docs(
        "SceneMSAA",
        "If you add this component to the Main Camera, anti-aliasing will also be applied in the Scene view."
    )]
    [DocsHowTo("Simply attach this component to the Main Camera. Anti-aliasing is always applied, allowing you to adjust materials in a way that is close to how they will actually appear.")]
    [ExecuteAlways, ImageEffectAllowedInSceneView, DisallowMultipleComponent]
    [AddComponentMenu(ConstantValues.COMPONENTS_BASE + nameof(SceneMSAA))]
    [HelpURL(ConstantValues.URL_DOCS_COMPONENT + nameof(SceneMSAA))]
    internal class SceneMSAA : EditorOnlyBehaviour
    {
        [ImageEffectUsesCommandBuffer] void OnRenderImage(RenderTexture src, RenderTexture dst) => Graphics.Blit(src, dst);
    }
}
