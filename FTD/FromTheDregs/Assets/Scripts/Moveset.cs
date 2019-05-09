using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveset {

    // [Spell Index, Spell Charges]
    int[,] _pattern;
    int _index;
    
    public int[,] pattern {
        get {return _pattern;}
    }

    public int index {
        get {return _index;}
        set {
            _index = value;
            LimitIndex();
        }
    }

    public Moveset(int startIndex = 0) {
        _index = startIndex;
        _pattern = new int[,] {
            {-1, 1}
        };
    }

    public Moveset(int[,] pattern, int startIndex = 0) {
        _index = startIndex;
        _pattern = pattern;
    }

    public void Next() {
        _index++;
        LimitIndex();
    }

    void LimitIndex() {
        if(_index > _pattern.GetLength(0)-1 || _index < 0) {
            _index = 0;
        }
    }
}
