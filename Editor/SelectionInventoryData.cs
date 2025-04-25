using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    [FilePath("./jp.lilxyzw.editortoolbox.SelectionInventoryData.asset", FilePathAttribute.Location.ProjectFolder)]
    internal class SelectionInventoryData : ScriptableSingleton<SelectionInventoryData>
    {
        public List<Object> objects;
        internal void Save() => Save(true);
    }
}
