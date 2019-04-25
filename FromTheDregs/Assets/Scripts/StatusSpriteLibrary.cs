using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Statuses", menuName = "Asset References/Sprites/Status Library")]
public class StatusSpriteLibrary : ScriptableObject {
    public Sprite bleed;
    public Sprite block;
    public Sprite health;
    public Sprite poison;
}