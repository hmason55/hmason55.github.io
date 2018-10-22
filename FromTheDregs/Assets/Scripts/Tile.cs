using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class Tile : MonoBehaviour {
	public bool renderFlag = false;

	[HideInInspector][SerializeField] SpriteManager _spriteManager;

	[HideInInspector][SerializeField] DungeonGenerator _dungeonGenerator;
	[HideInInspector][SerializeField] AnimationController _animationController;

	[SerializeField] Terrain _terrain;

	[SerializeField] Decoration _decoration;
	[SerializeField] Unit _unit;

	Vector2Int _position;

	public Terrain terrain {
		set {_terrain = value;}
		get {return _terrain;}
	}
	public Decoration decoration {
		set {_decoration = value;}
		get {return _decoration;}
	}

	public Unit unit {
		set {_unit = value;}
		get {return _unit;}
	}

	public SpriteManager spriteManager {
		set {_spriteManager = value;}
		get {return _spriteManager;}
	}

	public DungeonGenerator dungeonGenerator {
		set {_dungeonGenerator = value;}
		get {return _dungeonGenerator;}
	}

	public AnimationController animationController {
		set {_animationController = value;}
		get {return _animationController;}
	}

	public Vector2Int position {
		set {_position = value;}
		get {return _position;}
	}

	void LateUpdate() {
		if(_unit != null) {
			if(_unit.baseUnit != null) {
				_unit.IncrementAnimation();
				_unit.UpdateSprite();
			}
		}
	}

	public void SpawnUnit(BaseUnit baseUnit) {
		if(_unit != null) {
			if(_unit.baseUnit == null) {
				_unit.baseUnit = baseUnit;
				_unit.baseUnit.tile = this;
				_unit.GetComponent<Image>().enabled = true;
				_unit.LoadSprites();
				GameObject.FindObjectOfType<CameraController>().target = _position;
			} else {
				Debug.Log("A unit already exists at " + _position);
			}
		}
	}

	public void EnableRendering() {
		if(_terrain.sprite != null) {
			_terrain.GetComponent<Image>().enabled = true;
		}
		
		if(_decoration.sprite != null) {
			_decoration.GetComponent<Image>().enabled = true;
		}

		if(_unit.baseUnit != null) {
			_unit.GetComponent<Image>().enabled = true;
		}
	}

	public void DisableRendering() {
		_terrain.GetComponent<Image>().enabled = false;
		_decoration.GetComponent<Image>().enabled = false;
		_unit.GetComponent<Image>().enabled = false;
	}
}
