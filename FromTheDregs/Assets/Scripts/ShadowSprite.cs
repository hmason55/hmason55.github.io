using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Shadow Sprite", menuName = "Asset References/Sprites/Units/Shadow Sprite")]
public class ShadowSprite : ScriptableObject {

    public Sprite large;
    public Sprite medium;
    public Sprite small;
}
