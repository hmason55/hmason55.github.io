using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loot Tables", menuName = "Asset References/Loot Table Library")]
public class LootTableLibrary : ScriptableObject {
    public LootTable skeleton;
    public LootTable skeletonSummoner;
    public LootTable skeletonThrall;
    public LootTable skeletonWarrior;
}
