using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if (UNITY_EDITOR) 
[CustomEditor(typeof(DungeonGenerator))]
public class DungeonEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        
        DungeonGenerator myScript = (DungeonGenerator)target;
        if(GUILayout.Button("Build Tile Grid")) {
            myScript.BuildTileGrid();
        }

		if(GUILayout.Button("Check Size")) {
            myScript.CheckTileSize();
        }
		EditorUtility.SetDirty(myScript);
    }
}
#endif
