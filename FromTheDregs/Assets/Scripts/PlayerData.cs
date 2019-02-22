using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData  {
    public static PlayerData current;
    public int slot = -1;

    public PlayerData() {
        _character = new Character();
        _attributes = new Attributes();
        _bag = new Bag(Bag.BagType.Bag);
    }

    public Character character {
        get {return _character;}
        set {_character = value;}
    }

    public Attributes attributes {
        get {return _attributes;}
        set {_attributes = value;}
    }

    public Bag bag {
        get {return _bag;}
        set {_bag = value;}
    }
    // Character data
    Character _character;

    Attributes _attributes;

    Bag _bag;

    // Data tracking

    // Unlockables
}
