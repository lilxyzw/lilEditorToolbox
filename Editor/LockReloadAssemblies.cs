using UnityEditor;

namespace jp.lilxyzw.editortoolbox
{
    internal static class LockReloadAssemblies
    {
        private const string MENU_PATH = Common.MENU_HEAD + "Lock Reload Assemblies";
        private static bool isLocked = false;
        [MenuItem(MENU_PATH)]
        private static void Lock()
        {
            isLocked = !isLocked;
            Menu.SetChecked(MENU_PATH, isLocked);
            if(isLocked) EditorApplication.LockReloadAssemblies();
            else EditorApplication.UnlockReloadAssemblies();
        }
    }
}
