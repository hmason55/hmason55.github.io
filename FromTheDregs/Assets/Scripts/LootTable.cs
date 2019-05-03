using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable {

    [Range(0f, 100f)]
    float _commonDropRate;

    [Range(0f, 100f)]
    float _rareDropRate;

    List<BaseItem.ID> _commonItems;
    List<BaseItem.ID> _rareItems;

    Vector2Int _goldDropQuantity;

    public LootTable(BaseUnit.Preset preset) {
        _commonItems = new List<BaseItem.ID>();
        _rareItems = new List<BaseItem.ID>();

        switch(preset) {

            case BaseUnit.Preset.Giant_Spider:
                _commonDropRate = 25f;
                _commonItems.Add(BaseItem.ID.Potion_of_Curing);
                _commonItems.Add(BaseItem.ID.Potion_of_Return);

                _rareDropRate = 0f;

                _goldDropQuantity = new Vector2Int(10, 40);
            break;

            case BaseUnit.Preset.Giant_Widow:
                _commonDropRate = 25f;
                _commonItems.Add(BaseItem.ID.Potion_of_Curing);
                _commonItems.Add(BaseItem.ID.Potion_of_Return);

                _rareDropRate = 0f;

                _goldDropQuantity = new Vector2Int(20, 50);
            break;

            case BaseUnit.Preset.Skeleton:
                _commonDropRate = 25f;
                _commonItems.Add(BaseItem.ID.Potion_of_Clotting);
                _commonItems.Add(BaseItem.ID.Potion_of_Return);

                _rareDropRate = 0f;

                _goldDropQuantity = new Vector2Int(0, 7);
            break;

            case BaseUnit.Preset.Skeleton_Summoner:
                _commonDropRate = 25f;
                _commonItems.Add(BaseItem.ID.Potion_of_Clotting);
                _commonItems.Add(BaseItem.ID.Potion_of_Return);

                _rareDropRate = 10f;
                _rareItems.Add(BaseItem.ID.Staff_of_Flame);
                _rareItems.Add(BaseItem.ID.Novice_Tome);

                _goldDropQuantity = new Vector2Int(0, 12);
            break;

            case BaseUnit.Preset.Skeleton_Thrall:
                _commonDropRate = 10f;
                _commonItems.Add(BaseItem.ID.Potion_of_Clotting);
                _commonItems.Add(BaseItem.ID.Potion_of_Return);

                _rareDropRate = 0f;

                _goldDropQuantity = new Vector2Int(0, 2);
            break;

            case BaseUnit.Preset.Skeleton_Warrior:
                _commonDropRate = 25f;
                _commonItems.Add(BaseItem.ID.Potion_of_Clotting);
                _commonItems.Add(BaseItem.ID.Potion_of_Return);

                _rareDropRate = 10f;
                _rareItems.Add(BaseItem.ID.Gladius);
                _rareItems.Add(BaseItem.ID.Parma);

                _goldDropQuantity = new Vector2Int(0, 15);
            break;

            case BaseUnit.Preset.Spiderling:
                _commonDropRate = 25f;
                _commonItems.Add(BaseItem.ID.Potion_of_Curing);

                _rareDropRate = 0f;

                _goldDropQuantity = new Vector2Int(0, 5);
            break;

            case BaseUnit.Preset.Widowling:
                _commonDropRate = 25f;
                _commonItems.Add(BaseItem.ID.Potion_of_Curing);

                _rareDropRate = 0f;

                _goldDropQuantity = new Vector2Int(0, 8);
            break;
        }
    }

    public void SpawnLoot(Tile tile) {
        if(tile == null) {return;}

        Debug.Log("Spawning Loot");

        float roll = Random.Range(0f, 100f);
        BaseItem item;

        if(roll < _commonDropRate) {
            item = new BaseItem(RollCommon());
        } else if(roll < _commonDropRate + _rareDropRate) {
            item = new BaseItem(RollRare());
        } else {
            item = new BaseItem(BaseItem.ID.Gold, Random.Range(_goldDropQuantity.x, _goldDropQuantity.y));
        }

        if(item == null) {return;}
        if(item.quantity < 1) {return;}

        Debug.Log(item.id);

        bool lootExists = false;
        if(tile.baseDecoration != null) {
            if(tile.baseDecoration.decorationType == BaseDecoration.DecorationType.Loot) {
                lootExists = true;
            }
        }

        if(!lootExists) {
            tile.baseDecoration = new BaseDecoration(BaseDecoration.DecorationType.Loot, tile);
            GameObject.FindObjectOfType<DungeonManager>().AllocateObjects();
            if(tile.decoration != null) {
                tile.decoration.baseDecoration = tile.baseDecoration;
                tile.decoration.UpdateSprite();
            }
        }

        tile.baseDecoration.bag.Add(item);
    }

    public BaseItem.ID RollCommon() {
        return _commonItems[Random.Range(0, _commonItems.Count)];
    }

    public BaseItem.ID RollRare() {
        return _rareItems[Random.Range(0, _rareItems.Count)];
    }
}
