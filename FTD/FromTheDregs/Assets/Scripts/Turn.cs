﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn {

	int _priority = 99;
	BaseUnit _baseUnit;

	public int priority {
		get {return _priority;}
		set {_priority = value;}
	}
	public BaseUnit baseUnit {
		get {return _baseUnit;}
		set {_baseUnit = value;}
	}

	public Turn(BaseUnit b, int p) {
		if(!b.playerControlled) {
			b.intentSpell = b.SelectAISpell();
		}
		
		_baseUnit = b;
		_priority = p;
	}
}