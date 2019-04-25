using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Asset Library", menuName = "Asset References/Asset Library")]
public class AssetLibrary : ScriptableObject {
    public SpriteLibrary sprites;
}