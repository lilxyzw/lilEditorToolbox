using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace jp.lilxyzw.editortoolbox
{
    [Tooltip("Locks the assembly to reduce the wait time for script compilation. This is useful if you frequently rewrite scripts.")]
    #if UNITY_6000_3_OR_NEWER
    internal class LockReloadAssembliesButton
    {
        [UnityEditor.Toolbars.MainToolbarElement("lilEditorToolbox/Lock Reload Assemblies Button", defaultDockPosition = UnityEditor.Toolbars.MainToolbarDockPosition.Left)]
        private static UnityEditor.Toolbars.MainToolbarToggle Create()
        {
            var content = new UnityEditor.Toolbars.MainToolbarContent(L10n.L("Assemblies Unlocked"));
            return new(content, LockReloadAssemblies.isLocked, (v) => {
                LockReloadAssemblies.ToggleLock();
                content.text = LockReloadAssemblies.isLocked ? L10n.L("Assemblies Locked") : L10n.L("Assemblies Unlocked");
            });
        }
    }
    #else
    internal class LockReloadAssembliesButton : IToolbarExtensionComponent
    {
        public int Priority => 0;
        public bool InLeftSide => true;

        public VisualElement GetRootElement()
        {
            var root = new ToolbarToggle (){text = L10n.L("Assemblies Unlocked")};
            root.RegisterValueChangedCallback(e => {
                LockReloadAssemblies.ToggleLock();
                root.text = LockReloadAssemblies.isLocked ? L10n.L("Assemblies Locked") : L10n.L("Assemblies Unlocked");
            });
            return root;
        }
    }
    #endif
}
