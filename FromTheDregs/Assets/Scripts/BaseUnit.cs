﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BaseUnit {

	public enum StatPreset {
		Human,
		Spider
	}

	public enum SpritePreset {
		none,
		direrat,
		knight,
		wizard,
		greenslime,
		sandbehemoth,
		sandworm,
		spider,
		spidersmall,
		widow,
		widowsmall
	}

	public enum Size {
		Tiny,
		Small,
		Medium,
		Large,
		Huge,
		Gargantuan,
		Colossal
	}

	#region Stats
	int _baseStrength;
	int _baseDexterity;
	int _baseIntelligence;
	int _baseConstitution;
	int _baseWisdom;
	int _baseCharisma;
	int _baseSpeed;
	
	
	int _baseHitPoints;
	int _hpScaling;
	int _armorClass;
	Size _size;

	int _modStrength;
	int _modDexterity;
	int _modIntelligence;
	int _modConstitution;
	int _modWisdom;
	int _modCharisma;
	int _modSpeed;
	#endregion

	
	int _baseEssence;
	int _turnEssence;
	int _currentEssence;
	int _hitPoints;

	bool _playerControlled = false;

	Tile _tile;

	SpritePreset _spritePreset = SpritePreset.knight;

	StatPreset _statPreset = StatPreset.Human;

	public bool playerControlled {
		set {_playerControlled = value;}
		get {return _playerControlled;}
	}

	public Tile tile {
		set {_tile = value;}
		get {return _tile;}
	}

	public StatPreset statPreset {
		set {_statPreset = value;}
		get {return _statPreset;}
	}

	public SpritePreset spritePreset {
		set {_spritePreset = value;}
		get {return _spritePreset;}
	}

	public int baseEssence {
		get {return _baseEssence;}
	}
	public int turnEssence {
		get {return _turnEssence;}
	}
	public int currentEssence {
		get {return _currentEssence;}
	}

	public int armorClass {
		get {return _armorClass;}
	}

	public Size size {
		get {return _size;}
	}

	public int modStrength {
		get {return _modStrength;}
	}

	public int modDexterity {
		get {return _modDexterity;}
	}

	public int modIntelligence {
		get {return _modIntelligence;}
	}

	public BaseUnit(bool player, StatPreset stats, SpritePreset sprite, Tile tile) {
		_playerControlled = player;
		_statPreset = stats;
		_spritePreset = sprite;
		_tile = tile;
		EvaluateStatPreset();
		UpdateModifiers();
		UpdateArmorClass();

		// Set starting health
		_baseHitPoints = _hpScaling + _modConstitution;
		UpdateHitPoints();

		_currentEssence = _baseEssence;

		if(playerControlled) {
			SetAsCameraTarget();
			SetAsTapControllerTarget();
			SetAsHotbarTarget();
		}
	}

	void EvaluateStatPreset() {
		switch(_statPreset) {
			case StatPreset.Human:
				_baseStrength = 10;
				_baseDexterity = 10;
				_baseIntelligence = 10;
				_baseConstitution = 10;
				_baseWisdom = 10;
				_baseCharisma = 10;
				_baseSpeed = 3;
				_baseEssence = 4;
				_hpScaling = 8;
				_size = Size.Medium;
			break;

			case StatPreset.Spider:
				_baseStrength = 10;
				_baseDexterity = 15;
				_baseIntelligence = 0;
				_baseConstitution = 12;
				_baseWisdom = 10;
				_baseCharisma = 2;
				_baseSpeed = 5;
				_baseEssence = 4;
				_size = Size.Small;
			break;
		}
	}

	void UpdateModifiers() {
		_modStrength = (_baseStrength - 10) / 2;
		_modDexterity = (_baseDexterity - 10) / 2;
		_modIntelligence = (_baseIntelligence - 10) / 2;
		_modConstitution = (_baseConstitution - 10) / 2;
		_modWisdom = (_baseWisdom - 10) / 2;
		_modCharisma = (_baseCharisma - 10) / 2;
		_modSpeed = (_baseDexterity - 10) / 3;
	}

	void UpdateHitPoints() {
		_hitPoints = _baseHitPoints;
	}

	void UpdateArmorClass() {
		_armorClass = 10 + _modDexterity;
	}

	public void Move(int dx, int dy) {
		int x = _tile.position.x + dx;
		int y = _tile.position.y + dy;
		int mapWidth = DungeonGenerator.chunkDimension * DungeonGenerator.dungeonDimension;
		int mapHeight = DungeonGenerator.chunkDimension * DungeonGenerator.dungeonDimension;

		if(x < 0 || x >= mapWidth || y < 0 || y >= mapHeight) {
			Debug.Log("Out of bounds " + x + ", " + y);
			return;
		}

		Tile nextTile = _tile.dungeonGenerator.tiles[x, y];
		if(nextTile != null) {
			if(nextTile.terrain.walkable && nextTile.unit.baseUnit == null) {
				_tile.unit.TransferUnit(nextTile.unit);
				nextTile.unit.baseUnit = this;
				_tile.unit.baseUnit = null;
				_tile = nextTile;
				SetAsCameraTarget();
				SetAsHotbarTarget();
				Debug.Log("Moved " + dx + ", " + dy);
			} else {
				return;
			}
		}
	}

	public void Cast(int essenceCost) {
		_currentEssence -= essenceCost;
	}

	public void SetAsCameraTarget() {
		CameraController cameraController = GameObject.FindObjectOfType<CameraController>();
		cameraController.target = _tile.position;
		if(_playerControlled) {
			if(_tile.dungeonGenerator.limitRendering) {
				_tile.dungeonGenerator.RenderTiles();
			}
			
			cameraController.MoveToTarget();
		}
	}

	public void SetAsTapControllerTarget() {
		TapController tapController = GameObject.FindObjectOfType<TapController>();
		if(tapController != null) {
			tapController.baseUnit = this;
		}
	}

	public void SetAsHotbarTarget() {
		Hotbar hotbar = GameObject.FindObjectOfType<Hotbar>();
		if(hotbar != null) {
			hotbar.baseUnit = this;
		}
	}

	public void SpawnDamageText(string text, Color color) {
		GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Damage Text"));
		DamageText damageText = go.GetComponent<DamageText>();
		damageText.Init(_tile.GetComponent<RectTransform>().anchoredPosition, text, color);
	}

}
