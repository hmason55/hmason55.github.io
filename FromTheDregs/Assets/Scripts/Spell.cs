using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell {

	public enum Preset {
		BurningHands
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
		Self,
		Enemy,
		Ally,
		All
	}

	// Base
	BaseUnit _caster;
	Tile[,] _tiles;
	string _spellName;
	int _damage = 5;

	// Casting
	string _castParticlePath;
	int _castRadius = 0;
	bool _castThroughWalls = false;
	bool _castOnWalls = false;
	bool _castRequiresLineOfSight =  false;
	bool _castRequiresTarget = false;
	bool _castCanTargetSelf = false;
	TargetUnitType _castTargetUnitType = TargetUnitType.Enemy;
	

	// Effect
	string _effectParticlePath;
	Vector2Int _effectOrigin;
	int _effectRadius = 0;
	bool _effectIgnoresWalls = false;
	bool _effectRequiresLineOfSight = false;
	EffectShape _effectShape = EffectShape.Cone45;
	EffectDirection _effectDirection = EffectDirection.Right;
	TargetUnitType _effectTargetUnitType = TargetUnitType.Enemy;

	#region Accessors
	public string spellName {
		get {return _spellName;}
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

	public void CreateFromPreset(Preset spell) {
		switch(spell) {
			case Preset.BurningHands:
				_spellName = "Burning Hands";
				
				_castParticlePath = "";
				_castRadius = 1;
				_castThroughWalls = false;
				_castOnWalls = false;
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
		}
	}

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


		SpawnEffectParticles(_effectOrigin, zrot);
		ResetTiles();
	}

	void SpawnEffectParticles(Vector2Int position, float zrotation) {
		GameObject effectParticleGO = GameObject.Instantiate(Resources.Load<GameObject>(_effectParticlePath));
		CameraController dungeon = GameObject.FindObjectOfType<CameraController>();
		effectParticleGO.transform.SetParent(dungeon.transform);
		effectParticleGO.transform.SetAsLastSibling();

		RectTransform tileRT = _tiles[position.x, position.y].GetComponent<RectTransform>();

		RectTransform effectRT = effectParticleGO.GetComponent<RectTransform>();

		effectRT.anchoredPosition = new Vector2(tileRT.anchoredPosition.x + DungeonGenerator.TileWidth/2, tileRT.anchoredPosition.y + DungeonGenerator.TileHeight/2);
		effectRT.localEulerAngles = new Vector3(0f, 0f, zrotation);
	}


}
