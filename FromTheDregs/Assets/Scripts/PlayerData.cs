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

        _currentZone = DungeonManager.Zone.Debug;
        _targetZone = DungeonManager.Zone.Debug;
        _retrievalZone = DungeonManager.Zone.Debug;
        _retrievalMode = false;
        _retrievalBag = new Bag(Bag.BagType.Container);
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

    
	public DungeonManager.Zone currentZone {
        get {return _currentZone;}
        set {_currentZone = value;}
    }

    public DungeonManager.Zone targetZone {
        get {return _targetZone;}
        set {_targetZone = value;}
    }

    public DungeonManager.Zone retrievalZone {
        get {return _retrievalZone;}
        set {_retrievalZone = value;}
    }

    public bool retrievalMode {
        get {return _retrievalMode;}
        set {_retrievalMode = value;}
    }

    public Bag retrievalBag {
        get {return _retrievalBag;}
        set {_retrievalBag = value;}
    }

    // Character data
    Character _character;

    Attributes _attributes;

    Bag _bag;

    DungeonManager.Zone _currentZone;
    DungeonManager.Zone _targetZone;
	DungeonManager.Zone _retrievalZone;

    bool _retrievalMode;
    Bag _retrievalBag;

    // Data tracking

    // Unlockables
}
