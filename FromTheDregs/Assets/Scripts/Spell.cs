using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell {

	public enum Preset {
		Bite,
		BurningHands,
		Fireball,
		Move
	}

	public enum DamageType {
		Slashing,
		Piercing,
		Crushing,
		Fire,
		Ice,
		Lightning
	}

	public enum Scaling {
		Strength,
		Dexterity,
		Intelligence
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
	int _damageDice = 1;
	int _damageSides = 4;
	int _essenceCost = 1;
	int _chargesRemaining = 1;
	int _chargesMax;
	bool _requireCastConfirmation = false;
	bool _autoRecast = false;
	bool _createsCastParticle = false;
	bool _createsProjectile = false;
	bool _createsEffect = true;
	DamageType _damageType;
	Scaling _scaling = Scaling.Strength;
	int _modSizeDamage = 0;
	string _damageSoundPath;


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
	string _castSoundPath;


	// Projectile
	int _projCount;
	float _projSpeed;
	float _projPreSpawnDelay;
	float _projPostSpawnDelay;
	List<GameObject> _projectiles;
	string _projParticlePath;
	string _projSoundPath;


	// Effect
	Vector2Int _effectOrigin;
	float _effectPreSpawnDelay;
	float _effectDamageDelay;
	int _effectRadius = 0;
	bool _effectRotateToDirection = true;
	bool _effectIgnoresWalls = false;
	bool _effectRequiresLineOfSight = false;
	EffectShape _effectShape = EffectShape.Cone45;
	EffectDirection _effectDirection = EffectDirection.Right;
	TargetUnitType _effectTargetUnitType = TargetUnitType.Enemy;
	GameObject _effectParticle;
	List<Tile> _hitTiles;
	string _effectParticlePath;
	string _effectSoundPath;
	float _effectSoundDelay;

	#region Accessors
	public BaseUnit caster {
		get {return _caster;}
	}
	public string spellName {
		get {return _spellName;}
	}

	public DamageType damageType {
		get {return _damageType;}
	}

	public int essenceCost {
		get {return _essenceCost;}
	}

	public int chargesRemaining {
		get {return _chargesRemaining;}
	}

	public int chargesMax {
		get {return _chargesMax;}
	}

	public bool requireCastConfirmation {
		get {return _requireCastConfirmation;}
	}

	public int projCount {
		get {return _projCount;}
	}

	public float projPreSpawnDelay {
		get {return _projPreSpawnDelay;}
	}

	public float projPostSpawnDelay {
		get {return _projPostSpawnDelay;}
	}

	public Vector2Int effectOrigin {
		get {return _effectOrigin;}
		set {_effectOrigin = value;}
	}

	public float effectPreSpawnDelay {
		get {return _effectPreSpawnDelay;}
	}

	public float effectDamageDelay {
		get {return _effectDamageDelay;}
	}

	public List<Tile> hitTiles {
		get {return _hitTiles;}
		set {_hitTiles = value;}
	}
	#endregion

	// Constructors
	public Spell(Preset spell) {
		CreateFromPreset(spell);
	}

	public Spell(BaseUnit caster, Preset spell) {
		_caster = caster;
		_tiles = _caster.tile.dungeonManager.tiles;
		CreateFromPreset(spell);
	}

	#region Spells
	public void CreateFromPreset(Preset spell) {
		_preset = spell;
		_hitTiles = new List<Tile>();

		switch(spell) {
			
			#region Bite
			case Preset.Bite:	
				_spellName = "Bite";

				_essenceCost = 2;
				_damageType = DamageType.Piercing;
				_damageSoundPath = "Sounds/sfx/impact_damage_0";
				_autoRecast = false;
				_requireCastConfirmation = true;
				_createsCastParticle = false;
				_createsProjectile = false;
				_createsEffect = true;
				_scaling = Scaling.Strength;
				_modSizeDamage = 0;
				
				_castParticlePath = "";
				_castRadius = 1;
				_castThroughWalls = false;
				_castOnWalls = false;
				_castOnUnits = true;
				_castRequiresTarget = true;
				_castRequiresLineOfSight = true;
				_castCanTargetSelf = false;
				_castTargetUnitType = TargetUnitType.All;

				_effectParticlePath = "Prefabs/Effects/Bite Impact";
				_effectSoundPath = "Sounds/sfx/bite_impact_0";
				_effectSoundDelay = 0.15f;
				_effectDamageDelay = 0.75f;
				_effectRadius = 0;
				_effectRotateToDirection = false;
				_effectIgnoresWalls = false;
				_effectRequiresLineOfSight = true;
				_effectShape = Spell.EffectShape.Circle;
				_effectDirection = EffectDirection.Up;
				_effectTargetUnitType = TargetUnitType.Enemy;
			break;
			#endregion

			#region Burning Hands
			case Preset.BurningHands:	
				_spellName = "Burning Hands";

				_damageDice = 1;
				_damageSides = 4;
				_damageType = DamageType.Fire;
				_essenceCost = 2;
				_autoRecast = false;
				_requireCastConfirmation = true;
				_createsCastParticle = true;
				_createsProjectile = false;
				_createsEffect = true;
				_scaling = Scaling.Intelligence;
				_modSizeDamage = 0;
				
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
				_effectDamageDelay = 1.25f;
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

				_damageDice = 1;
				_damageSides = 6;
				_damageType = DamageType.Fire;
				_essenceCost = 2;
				_autoRecast = false;
				_requireCastConfirmation = true;
				_createsCastParticle = true;
				_createsProjectile = true;
				_createsEffect = true;
				_scaling = Scaling.Intelligence;
				_modSizeDamage = 0;
				
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
				_projSpeed = 192f;

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

				_essenceCost = 0;
				_autoRecast = true;
				_requireCastConfirmation = false;
				_createsCastParticle = false;
				_createsProjectile = false;
				_createsEffect = false;
				
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
		for(int y = 0; y < DungeonManager.dimension; y++) {
			for(int x = 0; x < DungeonManager.dimension; x++) {
				Tile _tile = _caster.tile.dungeonManager.tiles[x, y];
				if(_tile.terrain != null) {
					_tile.terrain.readyCast = false;
					_tile.terrain.confirmCast = false;
					_tile.terrain.image.color = Color.white;
				}
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

			if(!_tiles[x1, y1].baseTerrain.walkable) {
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
		bool[,] visitedTiles = new bool[DungeonManager.dimension, DungeonManager.dimension];

		for(int y = 0; y < DungeonManager.dimension; y++) {
			for(int x = 0; x < DungeonManager.dimension; x++) {
				visitedTiles[x, y] = false;
				if(_tiles[x, y].terrain != null) {
					_tiles[x, y].terrain.image.color = new Color(0.5f, 0.5f, 0.5f);
				}
			}
		}

		PopulateCastRange(_caster.tile.position.x, _caster.tile.position.y, visitedTiles, _caster.tile.position.x, _caster.tile.position.y);
		if(_createsCastParticle) {
			SpawnCastParticles(_caster.tile.position, 0f);
		}
	}

	void PopulateCastRange(int x, int y, bool[,] visited, int ox, int oy) {

		// Check map bounds
		if(	x < 0 ||
		 	y < 0 ||
		 	x >= (DungeonManager.dimension)-1 || 
			y >= (DungeonManager.dimension)-1) { 
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

		Tile _tile = _caster.tile.dungeonManager.tiles[x, y];

		// Check wall collision
		if(_tile.baseTerrain != null) {
			if((!_castThroughWalls && !_castOnWalls) && !_tile.baseTerrain.walkable) {
				return;
			}
		}


		bool flagTile = false;

		// Allow self target
		if(x == ox && y == oy) {
			if(_castCanTargetSelf) {
				flagTile = true;
			}
		} else {

			// Allow unit target
			if(_tile.baseUnit != null) {
				if(_castRequiresTarget) {
					switch(_castTargetUnitType) {
						case TargetUnitType.None:
							return;
						
						case TargetUnitType.Enemy:
							if(_tile.baseUnit.combatAlliance != _caster.combatAlliance) {
								flagTile = true;
							} else {
								flagTile = false;
							}
						break;

						case TargetUnitType.All:
							flagTile = true;
						break;


					}
				} else {
					flagTile = true;
				}
			} else {
				if(_castRequiresTarget) {
					return;
				}
			}

			// Allow casting on walls
			if(!_tile.baseTerrain.walkable) {
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
		bool[,] visitedTiles = new bool[DungeonManager.dimension, DungeonManager.dimension];

		if(_caster.playerControlled) {
			for(int y = 0; y < DungeonManager.dimension; y++) {
				for(int x = 0; x < DungeonManager.dimension; x++) {
					visitedTiles[x, y] = false;
					if(_tiles[x, y].terrain != null) {
						_tiles[x, y].terrain.image.color = new Color(0.5f, 0.5f, 0.5f);
					}
				}
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

		_hitTiles = new List<Tile>();
		PopulateEffectRange(_effectOrigin.x, _effectOrigin.y, visitedTiles, _effectOrigin.x, _effectOrigin.y);
	}

	void PopulateEffectRange(int x, int y, bool[,] visited, int ox, int oy) {

		// Check map bounds
		if(	x < 0 ||
		 	y < 0 ||
		 	x >= (DungeonManager.dimension)-1 || 
			y >= (DungeonManager.dimension)-1) { 
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

		Tile _tile = _caster.tile.dungeonManager.tiles[x, y];
		bool flagTile = false;

		switch(_effectDirection) {
			case EffectDirection.Left:	// Left
				switch(_effectShape) {
					case EffectShape.Cone45:	// Cone 45deg
						if(Mathf.Abs(y - oy) >	Mathf.Abs(x - ox) || (x - ox > 0)) {
							return;
						}
					break;

					case EffectShape.Circle:	// Circle
						if(CheckManhattanDistance(ox, oy, x, y) > _effectRadius) {
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

					case EffectShape.Circle:	// Circle
						if(CheckManhattanDistance(ox, oy, x, y) > _effectRadius) {
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

					case EffectShape.Circle:	// Circle
						if(CheckManhattanDistance(ox, oy, x, y) > _effectRadius) {
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

					case EffectShape.Circle:	// Circle
						if(CheckManhattanDistance(ox, oy, x, y) > _effectRadius) {
							return;
						}
					break;
				}
			break;
		}


		//if(CheckManhattanDistance(ox, oy, x, y) > _effectRadius) {
			//return;
		//}

		

		// Check wall collision
		if(!_effectIgnoresWalls && !_tile.baseTerrain.walkable) {
			return;
		}

		

		// Allow effects through walls
		if(!_tile.baseTerrain.walkable) {
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
			if(_caster.playerControlled) {
				_tile.terrain.image.color = new Color(1f, 0.75f, 0.75f);
			}

			_tile.terrain.readyCast = false;
			_tile.terrain.confirmCast = true;
			_hitTiles.Add(_tile);
		}
		

		
		PopulateEffectRange(x+1, y  , visited, ox, oy);
		PopulateEffectRange(x  , y-1, visited, ox, oy);
		PopulateEffectRange(x-1, y  , visited, ox, oy);
		PopulateEffectRange(x  , y+1, visited, ox, oy);
	}
	#endregion


	public void ConfirmSpellCast() {
		
		float zrot = 0f;
		if(_effectRotateToDirection) {
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
		}

		OnCast();

		DestroyCastParticles();

		if(_createsProjectile) {
			_projectiles = new List<GameObject>();
			_caster.tile.unit.SpawnSpellProjectiles(this);
		} else if(_createsEffect) {
			_caster.tile.unit.SpawnSpellEffect(this);
		}

		if(_caster.playerControlled) {
			ResetTiles();

			Hotbar hotbar = GameObject.FindObjectOfType<Hotbar>();
			Hotkey[] hotkeys = hotbar.GetComponentsInChildren<Hotkey>();
			foreach(Hotkey hotkey in hotkeys) {
				if(hotkey.preset == _preset && _autoRecast) {
					hotkey.PreviewCast();
				} else {
					hotkey.showCastRange = false;
				}
			}
		}
	}
	
	#region Particles
	void SpawnCastParticles(Vector2Int position, float zrotation) {
		GameObject castParticleGO = GameObject.Instantiate(Resources.Load<GameObject>(_castParticlePath));
		CameraController dungeon = GameObject.FindObjectOfType<CameraController>();
		castParticleGO.transform.SetParent(dungeon.transform);
		castParticleGO.transform.SetAsLastSibling();

		_castParticle = castParticleGO;

		RectTransform unitRT = _tiles[position.x, position.y].unit.GetComponent<RectTransform>();

		RectTransform castRT = castParticleGO.GetComponent<RectTransform>();

		castRT.anchoredPosition = new Vector2(unitRT.anchoredPosition.x + DungeonManager.TileWidth/2, unitRT.anchoredPosition.y + DungeonManager.TileHeight/2);
		castRT.localEulerAngles = new Vector3(0f, 0f, zrotation);
		castRT.localScale = new Vector3(0.25f, 0.25f, 0.25f);
	}

	public void DestroyCastParticles() {
		if(_castParticle != null) {
			ParticleSystem ps = _castParticle.GetComponent<ParticleSystem>();
			ps.Stop();
			GameObject.Destroy(_castParticle, ps.main.startLifetime.constant);
			_castParticle = null;
		}
	}

	public void SpawnProjectileParticles(Vector2Int start, Vector2Int end, float zrotation) {
		GameObject projParticleGO = GameObject.Instantiate(Resources.Load<GameObject>(_projParticlePath));
		CameraController dungeon = GameObject.FindObjectOfType<CameraController>();
		projParticleGO.transform.SetParent(dungeon.transform);
		projParticleGO.transform.SetAsLastSibling();

		Projectile proj = projParticleGO.GetComponent<Projectile>();
		float theta = Mathf.Atan2(end.y - start.y, end.x - start.x);
		float distance = Mathf.Sqrt((end.x - start.x)*(end.x - start.x) + (end.y - start.y)*(end.y - start.y)) * DungeonManager.TileWidth;
		proj.velocity = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)) * _projSpeed;
		proj.spell = this;
		proj.end = end;
		_projectiles.Add(projParticleGO);
		GameObject.Destroy(projParticleGO, distance/_projSpeed);

		RectTransform unitRT = _tiles[start.x, start.y].unit.GetComponent<RectTransform>();
		RectTransform projRT = projParticleGO.GetComponent<RectTransform>();
		projRT.anchoredPosition = new Vector2(unitRT.anchoredPosition.x + DungeonManager.TileWidth/2, unitRT.anchoredPosition.y + DungeonManager.TileHeight/2);
		projRT.localEulerAngles = new Vector3(0f, 0f, theta * Mathf.Rad2Deg - 90f);
		projRT.localScale = new Vector3(0.25f, 0.25f, 0.25f);
	}

	public void SpawnEffectParticles(Vector2Int position, float zrotation) {
		GameObject effectParticleGO = GameObject.Instantiate(Resources.Load<GameObject>(_effectParticlePath));
		CameraController dungeon = GameObject.FindObjectOfType<CameraController>();
		effectParticleGO.transform.SetParent(dungeon.transform);
		effectParticleGO.transform.SetAsLastSibling();

		RectTransform unitRT = _tiles[position.x, position.y].unit.GetComponent<RectTransform>();

		RectTransform effectRT = effectParticleGO.GetComponent<RectTransform>();

		effectRT.anchoredPosition = new Vector2(unitRT.anchoredPosition.x + DungeonManager.TileWidth/2, unitRT.anchoredPosition.y + DungeonManager.TileHeight/2);
		effectRT.localEulerAngles = new Vector3(0f, 0f, zrotation);
		effectRT.localScale = new Vector3(0.25f, 0.25f, 0.25f);
	}
	#endregion

	#region Sounds
	public void PlayEffectSound(Vector2Int position) {
		if(_effectSoundPath == null) {return;}
		AudioManager audioManager = GameObject.FindObjectOfType<AudioManager>();
		audioManager.LoadSound(_effectSoundPath);
		audioManager.PlaySound(0.5f, _effectSoundDelay);
	}

	public void PlayDamageSound(Vector2Int position) {
		if(_damageSoundPath == null) {return;}
		AudioManager audioManager = GameObject.FindObjectOfType<AudioManager>();
		audioManager.LoadSound(_damageSoundPath);
		audioManager.PlaySound();
	}
	#endregion

	public void DealDamage() {
		foreach(Tile tile in _hitTiles) {
			if(tile.unit.baseUnit != null) {
				PlayDamageSound(tile.position);
				tile.unit.baseUnit.ReceiveDamage(_caster, CalcSpellDamage(), _damageType);
			}
		}
	}

	void OnCast() {
		switch(_preset) {
			case Preset.Move:
				Move();
			break;
		}
	}

	public int CalcSpellDamage() {
		CalcModSizeDamage();
		int totalDamage = 0;
		switch(_scaling) {
			case Scaling.Strength:
				totalDamage = Dice.Roll(_damageDice, _damageSides) + _caster.modStrength + _modSizeDamage;
			break;

			case Scaling.Dexterity:
				totalDamage = Dice.Roll(_damageDice, _damageSides) + _caster.modDexterity + _modSizeDamage;
			break;

			case Scaling.Intelligence:
				totalDamage = Dice.Roll(_damageDice, _damageSides) + _caster.modIntelligence + _modSizeDamage;
			break;
		}

		return totalDamage;
	}

	void CalcModSizeDamage() {
		switch(_preset) {
			case Preset.Bite:
				_modSizeDamage = Dice.Roll(1+((int)_caster.size)/2, 1+((int)_caster.size));
			break;
			default:
				_modSizeDamage = 0;
			break;
		}
	}

	void Move() {
		_caster.Move(_effectOrigin.x - _caster.tile.position.x, _effectOrigin.y - _caster.tile.position.y);
	}


}
