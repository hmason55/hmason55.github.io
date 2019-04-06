using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveset {
    int[] _pattern;
    int _index;

    public int[] pattern {
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
        _pattern = new int[] {-1};
    }

    public Moveset(int[] pattern, int startIndex = 0) {
        _index = startIndex;
        _pattern = pattern;
    }

    public void Next() {
        _index++;
        LimitIndex();
    }

    void LimitIndex() {
        if(_index > _pattern.Length-1 || _index < 0) {
            _index = 0;
        }
    }
}
