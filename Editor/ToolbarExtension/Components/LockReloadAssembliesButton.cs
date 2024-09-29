using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace jp.lilxyzw.editortoolbox
{
    internal class LockReloadAssembliesButton : IToolbarExtensionComponent
    {
        public int Priority => 0;
        public bool InLeftSide => true;

        private static ToolbarToggle root;
        public VisualElement GetRootElement()
        {
            if(root != null) return root;
            root = new(){text = "Assemblies Unlocked"};
            root.RegisterValueChangedCallback(e => {
                LockReloadAssemblies.ToggleLock();
                root.text = LockReloadAssemblies.isLocked ? "Assemblies Locked" : "Assemblies Unlocked";
            });
            return root;
        }
    }
}
