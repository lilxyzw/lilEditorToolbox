using System.Linq;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal class ChildrenDrawer : IHierarchyExtensionComponent
    {
        private const int ICON_SIZE_CHILD = 8;

        public int Priority => 1200;

        public void OnGUI(ref Rect currentRect, GameObject gameObject, int instanceID, Rect fullRect)
        {
            var componentsChild = gameObject.GetComponentsInChildren<Component>(true).Where(c => c && c is not Transform && c.gameObject != gameObject).GroupBy(c => c.GetType());
            if(componentsChild.Count() > 0)
            {
                var icons = componentsChild.OrderBy(c => c.Key.FullName).Select(c => AssetPreview.GetMiniThumbnail(c.First())).Distinct().ToArray();

                currentRect.x -= ICON_SIZE_CHILD * icons.Length;
                currentRect.width = ICON_SIZE_CHILD;
                var xmin = currentRect.x;

                foreach(var icon in icons)
                {
                    GUI.Box(currentRect, icon, GUIStyle.none);
                    currentRect.x += ICON_SIZE_CHILD;
                }

                currentRect.x = xmin;
            }

            if(gameObject.GetComponentsInChildren<Component>(true).Any(c => !c))
            {
                currentRect.x -= ICON_SIZE_CHILD;
                currentRect.width = ICON_SIZE_CHILD;
                GUI.Box(currentRect, HierarchyExtension.MissingScriptIcon(), GUIStyle.none);
            }
        }
    }
}
