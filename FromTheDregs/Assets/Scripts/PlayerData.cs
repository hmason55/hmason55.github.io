using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData  {
    public static PlayerData current;
    public int slot = -1;

    public PlayerData() {
        _character = new Character();
    }

    public Character character {
        get {return _character;}
        set {_character = value;}
    }

    // Character data
    Character _character;

    // Data tracking

    // Unlockables
}
