using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : MonoBehaviour {

	//public List<Spell> spells;
	
	BaseUnit _baseUnit;
	Spell _activeSpell;
	public BaseUnit baseUnit {
		set {_baseUnit = value;}
		get {return _baseUnit;}
	}

	public Spell activeSpell {
		get {return _activeSpell;}
		set {_activeSpell = value;}
	}

}
