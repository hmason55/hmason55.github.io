using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Decoration Sprite", menuName = "Asset References/Sprites/Decoration Sprite")]
public class DecorationSprite : ScriptableObject {
    public List<Sprite> animation;
    public List<Sprite> highlightAnimation;
}
