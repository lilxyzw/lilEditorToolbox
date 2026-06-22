#if !UNITY_6000_3_OR_NEWER
using jp.lilxyzw.editortoolbox;
[assembly: ExportsToolbarExtensionComponent(
    typeof(LockReloadAssembliesButton),
    typeof(AddInspectorTabButton)
)]
#endif
