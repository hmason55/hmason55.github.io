using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Sprites", menuName = "Asset References/Sprite Library")]
public class SpriteLibrary : ScriptableObject {
    public StatusSpriteLibrary statuses;
    public UnitSpriteLibrary units;
    
}