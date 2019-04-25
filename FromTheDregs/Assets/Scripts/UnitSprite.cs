using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Unit Sprite", menuName = "Asset References/Sprites/Unit Sprite")]
public class UnitSprite : ScriptableObject {
    public List<Sprite> hit;
    public List<Sprite> idle;
}
