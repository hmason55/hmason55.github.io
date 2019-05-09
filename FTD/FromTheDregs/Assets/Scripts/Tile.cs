using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class Tile {
	public bool renderFlag = false;

	SpriteManager _spriteManager;
	DungeonManager _dungeonManager;
	CombatManager _combatManager;
	AnimationController _animationController;

	TerrainBehaviour _terrain;
	DecorationBehaviour _decoration;
	UnitBehaviour _unit;

	BaseTerrain _baseTerrain;
	BaseDecoration _baseDecoration;
	BaseUnit _baseUnit;

	Vector2Int _position;
	
	public SpriteManager spriteManager {
		set {_spriteManager = value;}
		get {return _spriteManager;}
	}

	public DungeonManager dungeonManager {
		set {_dungeonManager = value;}
		get {return _dungeonManager;}
	}

	public CombatManager combatManager {
		set {_combatManager = value;}
		get {return _combatManager;}
	}

	public AnimationController animationController {
		set {_animationController = value;}
		get {return _animationController;}
	}

	public TerrainBehaviour terrain {
		set {_terrain = value;}
		get {return _terrain;}
	}
	public DecorationBehaviour decoration {
		set {_decoration = value;}
		get {return _decoration;}
	}

	public UnitBehaviour unit {
		set {_unit = value;}
		get {return _unit;}
	}

	public BaseTerrain baseTerrain {
		set {_baseTerrain = value;}
		get {return _baseTerrain;}
	}
	public BaseDecoration baseDecoration {
		set {_baseDecoration = value;}
		get {return _baseDecoration;}
	}

	public BaseUnit baseUnit {
		set {_baseUnit = value;}
		get {return _baseUnit;}
	}

	public Vector2Int position {
		set {_position = value;}
		get {return _position;}
	}

	public Tile(int x, int y) {
		position = new Vector2Int(x, y);
	}

	public void AnimateUnit() {
		if(_baseUnit != null) {
			_baseUnit.IncrementAnimation();
			if(_unit != null) {
				_unit.UpdateSprite();
			}
		}
	}

	public void AnimateDecoration() {
		if(_baseDecoration != null) {
			if(_baseDecoration.animated) {
				_baseDecoration.IncrementAnimation();
			}
			
			if(_decoration != null) {
				_decoration.UpdateSprite();
			}
			
		}
	}

	public void SpawnUnit(BaseUnit baseUnit) {
		if(_baseUnit == null) {
			_baseUnit = baseUnit;
			_baseUnit.tile = this;
			if(_unit != null) {
				_unit.baseUnit = baseUnit;
				
			}
			_baseUnit.LoadSprites(_spriteManager);
		} else {
			Debug.LogWarning("A unit already exists at " + _position);
		}
	}

	public void EnableRendering() {
		if(_baseTerrain.sprite != null) {
			_terrain.GetComponent<Image>().enabled = true;
		}
		
		if(_baseDecoration != null) {
			_decoration.GetComponent<Image>().enabled = true;
		}

		if(_baseUnit != null) {
			_unit.GetComponent<Image>().enabled = true;
		}
	}

	public void DisableRendering() {
		_terrain.GetComponent<Image>().enabled = false;
		_decoration.GetComponent<Image>().enabled = false;
		_unit.GetComponent<Image>().enabled = false;
	}
}
