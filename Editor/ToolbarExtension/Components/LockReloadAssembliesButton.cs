using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace jp.lilxyzw.editortoolbox
{
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
}
