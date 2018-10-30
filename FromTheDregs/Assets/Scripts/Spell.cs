using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell {

	public enum Preset {
		
		BurningHands,
		Fireball,
		Move
	}

	public enum EffectDirection {
		Up,
		Right,
		Down,
		Left,
	}

	public enum EffectShape {
		Line,
		Cone45,
		Cone180,
		Cone270,
		Circle
	}

	public enum TargetUnitType {
		None,
		Self,
		Enemy,
		Ally,
		All
	}

	// Base
	BaseUnit _caster;
	Tile[,] _tiles;
	Preset _preset;
	string _spellName;
	int _damage = 5;
	int _essenceCost = 1;
	bool _requireCastConfirmation = false;
	bool _autoRecast = false;
	bool _createsProjectile = false;
	bool _createsEffect = true;


	// Casting
	int _castRadius = 0;
	bool _castThroughWalls = false;
	bool _castOnWalls = false;
	bool _castOnUnits = false;
	bool _castRequiresLineOfSight =  false;
	bool _castRequiresTarget = false;
	bool _castCanTargetSelf = false;
	TargetUnitType _castTargetUnitType = TargetUnitType.Enemy;
	GameObject _castParticle;
	string _castParticlePath;


	// Projectile
	int _projCount;
	float _projSpeed;
	List<GameObject> _projectiles;
	string _projParticlePath;


	// Effect
	Vector2Int _effectOrigin;
	int _effectRadius = 0;
	bool _effectIgnoresWalls = false;
	bool _effectRequiresLineOfSight = false;
	EffectShape _effectShape = EffectShape.Cone45;
	EffectDirection _effectDirection = EffectDirection.Right;
	TargetUnitType _effectTargetUnitType = TargetUnitType.Enemy;
	GameObject _effectParticle;
	string _effectParticlePath;

	#region Accessors
	public string spellName {
		get {return _spellName;}
	}

	public int essenceCost {
		get {return _essenceCost;}
	}

	public bool requireCastConfirmation {
		get {return _requireCastConfirmation;}
	}

	public Vector2Int effectOrigin {
		get {return _effectOrigin;}
		set {_effectOrigin = value;}
	}

	#endregion

	// Constructors
	public Spell(Preset spell) {
		CreateFromPreset(spell);
	}

	public Spell(BaseUnit caster, Preset spell) {
		_caster = caster;
		_tiles = _caster.tile.dungeonGenerator.tiles;
		CreateFromPreset(spell);
	}

	#region Spells
	public void CreateFromPreset(Preset spell) {
		_preset = spell;
		switch(spell) {
			#region Burning Hands
			case Preset.BurningHands:	
				_spellName = "Burning Hands";

				_damage = 1;
				_essenceCost = 2;
				_autoRecast = false;
				_requireCastConfirmation = true;
				_createsProjectile = false;
				_createsEffect = true;
				
				_castParticlePath = "Prefabs/Effects/Fire Casting";
				_castRadius = 1;
				_castThroughWalls = false;
				_castOnWalls = false;
				_castOnUnits = true;
				_castRequiresTarget = false;
				_castRequiresLineOfSight = true;
				_castCanTargetSelf = false;
				_castTargetUnitType = TargetUnitType.All;

				_effectParticlePath = "Prefabs/Effects/Burning Hands";
				_effectRadius = 1;
				_effectIgnoresWalls = false;
				_effectRequiresLineOfSight = true;
				_effectShape = Spell.EffectShape.Cone45;
				_effectDirection = EffectDirection.Up;
				_effectTargetUnitType = TargetUnitType.Enemy;
			break;
			#endregion

			#region Fireball
			case Preset.Fireball:	
				_spellName = "Fireball";

				_damage = 1;
				_essenceCost = 0;
				_autoRecast = false;
				_requireCastConfirmation = true;
				_createsProjectile = true;
				_createsEffect = true;
				
				_castParticlePath = "Prefabs/Effects/Fire Casting";
				_castRadius = 5;
				_castThroughWalls = false;
				_castOnWalls = false;
				_castOnUnits = true;
				_castRequiresTarget = false;
				_castRequiresLineOfSight = true;
				_castCanTargetSelf = false;
				_castTargetUnitType = TargetUnitType.All;

				_projCount = 1;
				_projParticlePath = "Prefabs/Effects/Fireball";
				_projSpeed = 196f;

				_effectParticlePath = "Prefabs/Effects/Fireball Impact";
				_effectRadius = 1;
				_effectIgnoresWalls = false;
				_effectRequiresLineOfSight = true;
				_effectShape = Spell.EffectShape.Circle;
				_effectDirection = EffectDirection.Up;
				_effectTargetUnitType = TargetUnitType.All;
			break;
			#endregion

			#region Move
			case Preset.Move:
				_spellName = "Move";

				_damage = 0;
				_essenceCost = 1;
				_autoRecast = true;
				_requireCastConfirmation = false;
				_createsProjectile = false;
				_createsEffect = false;
				
				_castParticlePath = "";
				_castRadius = 1;
				_castThroughWalls = false;
				_castOnWalls = false;
				_castOnUnits = false;
				_castRequiresTarget = false;
				_castRequiresLineOfSight = true;
				_castCanTargetSelf = false;
				_castTargetUnitType = TargetUnitType.None;
			break;
			#endregion
		}
	}
	#endregion

	public void ResetTiles() {
		int dimension = DungeonGenerator.dungeonDimension * DungeonGenerator.chunkDimension;
		for(int y = 0; y < dimension; y++) {
			for(int x = 0; x < dimension; x++) {
				Tile _tile = _caster.tile.dungeonGenerator.tiles[x, y];
				_tile.terrain.readyCast = false;
				_tile.terrain.confirmCast = false;
				_tile.terrain.image.color = Color.white;
			}
		}
	}

	int CheckManhattanDistance(int x1, int y1, int x2, int y2) {
		return Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2);
	}

	bool RayTrace(int x1, int y1, int x2, int y2) {
		int dx = Mathf.Abs(x2 - x1);
		int sx = -1;

		if(x1 < x2) {
			sx = 1;
		}

		int dy = Mathf.Abs(y2 - y1);
		int sy = -1;
		
		if(y1 < y2) {
			sy = 1;
		}

		int err = -dy/2;

		if(dx > dy) {
			err = dx/2;
		}

		while(true) {

			if(!_tiles[x1, y1].terrain.walkable) {
				return false;
			}

			if(x1 == x2 && y1 == y2) {
				return true;
			}

			int e2 = err;

			if(e2 > -dx) {
				err -= dy;
				x1 += sx;
			}

			if(e2 < dy) {
				err += dx;
				y1 += sy;
			}
		}
	}

	#region Casting
	public void ShowCastRange() {
		int dimension = DungeonGenerator.dungeonDimension * DungeonGenerator.chunkDimension;
		bool[,] visitedTiles = new bool[dimension, dimension];

		for(int y = 0; y < dimension; y++) {
			for(int x = 0; x < dimension; x++) {
				visitedTiles[x, y] = false;
				_tiles[x, y].terrain.image.color = new Color(0.5f, 0.5f, 0.5f);
			}
		}

		PopulateCastRange(_caster.tile.position.x, _caster.tile.position.y, visitedTiles, _caster.tile.position.x, _caster.tile.position.y);
		if(_castParticlePath != null) {
			SpawnCastParticles(_caster.tile.position, 0f);
		}
	}

	void PopulateCastRange(int x, int y, bool[,] visited, int ox, int oy) {

		// Check map bounds
		if(	x < 0 ||
		 	y < 0 ||
		 	x >= (DungeonGenerator.dungeonDimension * DungeonGenerator.chunkDimension)-1 || 
			y >= (DungeonGenerator.dungeonDimension * DungeonGenerator.chunkDimension)-1) { 
			return;
		}

		// Check casting bounds
		if(	x > ox+_castRadius || y > oy+_castRadius || 
			x < ox-_castRadius || y <  oy-_castRadius) {
			return;
		}

		if(visited[x, y] == true) {
			return;
		}

		visited[x, y] = true;


		if(CheckManhattanDistance(ox, oy, x, y) > _castRadius) {
			return;
		}

		Tile _tile = _caster.tile.dungeonGenerator.tiles[x, y];

		// Check wall collision
		if((!_castThroughWalls && !_castOnWalls) && !_tile.terrain.walkable) {
			return;
		}

		bool flagTile = false;

		// Allow self target
		if(x == ox && y == oy) {
			if(_castCanTargetSelf) {
				flagTile = true;
			}
		} else {

			// Allow casting on walls
			if(!_tile.terrain.walkable) {
				if(_castThroughWalls && _castOnWalls) {
					flagTile = true;
				} else if(_castOnWalls) {
					_tile.terrain.image.color = new Color(0.75f, 0.75f, 1f);
					return;
				}
			} else {

				// Require line of sight
				if(_castRequiresLineOfSight) {
					flagTile = RayTrace(ox, oy, x, y);
				} else {
					flagTile = true;
				}
				
			}
		}

		// Change tile color
		if(flagTile) {
			_tile.terrain.readyCast = true;
			_tile.terrain.confirmCast = false;
			_tile.terrain.image.color = new Color(0.75f, 0.75f, 1f);
		}

		PopulateCastRange(x+1, y  , visited, ox, oy);
		PopulateCastRange(x  , y-1, visited, ox, oy);
		PopulateCastRange(x-1, y  , visited, ox, oy);
		PopulateCastRange(x  , y+1, visited, ox, oy);
	}
	#endregion
	
	#region Effects
	public void ShowEffectRange(Vector2Int origin) {
		int dimension = DungeonGenerator.dungeonDimension * DungeonGenerator.chunkDimension;
		bool[,] visitedTiles = new bool[dimension, dimension];

		for(int y = 0; y < dimension; y++) {
			for(int x = 0; x < dimension; x++) {
				visitedTiles[x, y] = false;
				_tiles[x, y].terrain.image.color = new Color(0.5f, 0.5f, 0.5f);
			}
		}

		_effectOrigin = origin;

		if(_effectOrigin.x - _caster.tile.position.x < 0) {
			_effectDirection = EffectDirection.Left;
		} else if(_effectOrigin.x - _caster.tile.position.x > 0) {
			_effectDirection = EffectDirection.Right;
		} else if(_effectOrigin.y - _caster.tile.position.y < 0) {
			_effectDirection = EffectDirection.Down;
		} if(_effectOrigin.y - _caster.tile.position.y > 0) {
			_effectDirection = EffectDirection.Up;
		}

		PopulateEffectRange(_effectOrigin.x, _effectOrigin.y, visitedTiles, _effectOrigin.x, _effectOrigin.y);
	}

	void PopulateEffectRange(int x, int y, bool[,] visited, int ox, int oy) {

		// Check map bounds
		if(	x < 0 ||
		 	y < 0 ||
		 	x >= (DungeonGenerator.dungeonDimension * DungeonGenerator.chunkDimension)-1 || 
			y >= (DungeonGenerator.dungeonDimension * DungeonGenerator.chunkDimension)-1) { 
			return;
		}

		// Check casting bounds
		if(	x > ox+_effectRadius || y > oy+_effectRadius || 
			x < ox-_effectRadius || y <  oy-_effectRadius) {
			return;
		}

		if(visited[x, y] == true) {
			return;
		}

		visited[x, y] = true;

		Tile _tile = _caster.tile.dungeonGenerator.tiles[x, y];
		bool flagTile = false;

		switch(_effectDirection) {
			case EffectDirection.Left:	// Left
				switch(_effectShape) {
					case EffectShape.Cone45:	// Cone 45deg
						if(Mathf.Abs(y - oy) >	Mathf.Abs(x - ox) || (x - ox > 0)) {
							return;
						}
					break;
				}
			break;

			case EffectDirection.Down:	// Down
				switch(_effectShape) {
					case EffectShape.Cone45:	// Cone 45deg					
						if(Mathf.Abs(y - oy) <	Mathf.Abs(x - ox) || (y - oy > 0)) {
							return;
						}
					break;
				}
			break;

			case EffectDirection.Right:	// Right
				switch(_effectShape) {
					case EffectShape.Cone45:	// Cone 45deg					
						if(Mathf.Abs(y - oy) >	Mathf.Abs(x - ox) || (x - ox < 0)) {
							return;
						}
					break;
				}
			break;

			case EffectDirection.Up:	// Up
				switch(_effectShape) {
					case EffectShape.Cone45:	// Cone 45deg					
						if(Mathf.Abs(y - oy) <	Mathf.Abs(x - ox) || (y - oy < 0)) {
							return;
						}
					break;
				}
			break;
		}


		if(CheckManhattanDistance(ox, oy, x, y) > _effectRadius) {
			//return;
		}

		

		// Check wall collision
		if(!_effectIgnoresWalls && !_tile.terrain.walkable) {
			return;
		}

		

		// Allow effects through walls
		if(!_tile.terrain.walkable) {
			if(_effectIgnoresWalls) {
				flagTile = true;
			}
		} else {

			// Require line of sight
			if(_effectRequiresLineOfSight) {
				flagTile = RayTrace(ox, oy, x, y);
			} else {
				flagTile = true;
			}
			
		}

		// Allow self target
		/* if(x == ox && y == oy) {
			if(_effectTargetUnitType == TargetUnitType.Self) {
				flagTile = true;
			}
		} else {

				// Allow effects through walls
		}*/

		end:
		// Change tile color
		if(flagTile) {
			_tile.terrain.readyCast = false;
			_tile.terrain.confirmCast = true;
			_tile.terrain.image.color = new Color(1f, 0.75f, 0.75f);
		}
		

		
		PopulateEffectRange(x+1, y  , visited, ox, oy);
		PopulateEffectRange(x  , y-1, visited, ox, oy);
		PopulateEffectRange(x-1, y  , visited, ox, oy);
		PopulateEffectRange(x  , y+1, visited, ox, oy);
	}
	#endregion


	public void ConfirmSpellCast() {
		float zrot = 0f;
		switch(_effectDirection) {
			case EffectDirection.Right:
				zrot = 270f;
			break;

			case EffectDirection.Down:
				zrot = 180f;
			break;

			case EffectDirection.Left:
				zrot = 90f;
			break;

			case EffectDirection.Up:
				zrot = 0f;
			break;
		}

		OnCast();

		DestroyCastParticles();

		if(_createsProjectile) {
			_projectiles = new List<GameObject>();
			for(int i = 0; i < _projCount; i++) {
				SpawnProjectileParticles(_caster.tile.position, effectOrigin, 0f);
			}
		} else if(_createsEffect) {
			SpawnEffectParticles(_effectOrigin, zrot);
		}

		Hotbar hotbar = GameObject.FindObjectOfType<Hotbar>();
		Hotkey[] hotkeys = hotbar.GetComponentsInChildren<Hotkey>();
		foreach(Hotkey hotkey in hotkeys) {
			if(hotkey.preset == _preset && _autoRecast) {
				hotkey.PreviewCast();
			} else {
				hotkey.showCastRange = false;
			}
		}

		if(!_autoRecast) {
			ResetTiles();
		}
	}
	
	#region Particles
	void SpawnCastParticles(Vector2Int position, float zrotation) {
		GameObject castParticleGO = GameObject.Instantiate(Resources.Load<GameObject>(_castParticlePath));
		CameraController dungeon = GameObject.FindObjectOfType<CameraController>();
		castParticleGO.transform.SetParent(dungeon.transform);
		castParticleGO.transform.SetAsLastSibling();

		_castParticle = castParticleGO;

		RectTransform tileRT = _tiles[position.x, position.y].GetComponent<RectTransform>();

		RectTransform castRT = castParticleGO.GetComponent<RectTransform>();

		castRT.anchoredPosition = new Vector2(tileRT.anchoredPosition.x + DungeonGenerator.TileWidth/2, tileRT.anchoredPosition.y + DungeonGenerator.TileHeight/2);
		castRT.localEulerAngles = new Vector3(0f, 0f, zrotation);
	}

	public void DestroyCastParticles() {
		if(_castParticle != null) {
			ParticleSystem ps = _castParticle.GetComponent<ParticleSystem>();
			ps.Stop();
			GameObject.Destroy(_castParticle, ps.main.startLifetime.constant);
			_castParticle = null;
		}
	}

	void SpawnProjectileParticles(Vector2Int start, Vector2Int end, float zrotation) {
		GameObject projParticleGO = GameObject.Instantiate(Resources.Load<GameObject>(_projParticlePath));
		CameraController dungeon = GameObject.FindObjectOfType<CameraController>();
		projParticleGO.transform.SetParent(dungeon.transform);
		projParticleGO.transform.SetAsLastSibling();

		Projectile proj = projParticleGO.GetComponent<Projectile>();
		float theta = Mathf.Atan2(end.y - start.y, end.x - start.x);
		float distance = Mathf.Sqrt((end.x - start.x)*(end.x - start.x) + (end.y - start.y)*(end.y - start.y)) * DungeonGenerator.TileWidth;
		proj.velocity = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)) * _projSpeed;
		proj.spell = this;
		proj.end = end;
		_projectiles.Add(projParticleGO);
		GameObject.Destroy(projParticleGO, distance/_projSpeed);

		RectTransform tileRT = _tiles[start.x, start.y].GetComponent<RectTransform>();
		RectTransform projRT = projParticleGO.GetComponent<RectTransform>();
		projRT.anchoredPosition = new Vector2(tileRT.anchoredPosition.x + DungeonGenerator.TileWidth/2, tileRT.anchoredPosition.y + DungeonGenerator.TileHeight/2);
		projRT.localEulerAngles = new Vector3(0f, 0f, theta * Mathf.Rad2Deg - 90f);
	}

	public void SpawnEffectParticles(Vector2Int position, float zrotation) {
		GameObject effectParticleGO = GameObject.Instantiate(Resources.Load<GameObject>(_effectParticlePath));
		CameraController dungeon = GameObject.FindObjectOfType<CameraController>();
		effectParticleGO.transform.SetParent(dungeon.transform);
		effectParticleGO.transform.SetAsLastSibling();

		RectTransform tileRT = _tiles[position.x, position.y].GetComponent<RectTransform>();

		RectTransform effectRT = effectParticleGO.GetComponent<RectTransform>();

		effectRT.anchoredPosition = new Vector2(tileRT.anchoredPosition.x + DungeonGenerator.TileWidth/2, tileRT.anchoredPosition.y + DungeonGenerator.TileHeight/2);
		effectRT.localEulerAngles = new Vector3(0f, 0f, zrotation);
	}
	#endregion

	void OnCast() {
		switch(_preset) {
			case Preset.Move:
				Move();
			break;
		}
	}


	void Move() {
		Debug.Log(_effectOrigin);
		_caster.tile.unit.baseUnit.Move(_effectOrigin.x - _caster.tile.position.x, _effectOrigin.y - _caster.tile.position.y);
	}


}
