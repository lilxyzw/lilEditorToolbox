using System;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal class EditorGUIWrap : WrapBase
    {
        private static readonly Type type = typeof(EditorGUI);
        internal static readonly Func<Rect,int,string[],int> AdvancedPopup = GetFunc<Rect,int,string[],int>(type, "AdvancedPopup");
    }
}
