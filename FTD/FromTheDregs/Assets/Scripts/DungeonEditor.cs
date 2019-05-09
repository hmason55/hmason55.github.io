using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if (UNITY_EDITOR) 
[CustomEditor(typeof(DungeonManager))]
public class DungeonEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        
        DungeonManager dungeonManager = (DungeonManager)target;
        if(GUILayout.Button("Build Object Pools")) {
            dungeonManager.BuildObjectPools();
        }

        if(GUILayout.Button("Clear Object Pools")) {
            dungeonManager.ClearObjectPools();
        }

		EditorUtility.SetDirty(dungeonManager);
    }
}
#endif
