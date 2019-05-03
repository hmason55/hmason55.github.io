using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biome {
	
	BiomeType _biomeType;
	int _x;
	int _y;
	int _radius;

	float _commonEnemySpawnRate = 100f;
	float _rareEnemySpawnRate = 0f;


	List<BaseUnit.Preset> _commonEnemySpawnTable;
	List<BaseUnit.Preset> _rareEnemySpawnTable;

	public enum BiomeType {
		cavern,
		crypt,
		dungeon,
		forsaken,
		ruins
	}

	public BiomeType biomeType {
		get {return _biomeType;}
		set {_biomeType = value;}
	}

	public int x {
		get {return _x;}
		set {_x = value;}
	}

	public int y {
		get {return _y;}
		set {_y = value;}
	}

	public int radius {
		get {return _radius;}
		set {_radius = value;}
	}

	public Biome(int x, int y, int radius, DungeonManager.Zone zone) {
		_x = x;
		_y = y;
		_radius = radius;
		_biomeType = BiomeType.forsaken;

		_commonEnemySpawnTable = new List<BaseUnit.Preset>();
		_rareEnemySpawnTable = new List<BaseUnit.Preset>();

		switch(zone) {
			case DungeonManager.Zone.A1:
				_commonEnemySpawnRate = 85f;
				_commonEnemySpawnTable.Add(BaseUnit.Preset.Spiderling);
				_commonEnemySpawnTable.Add(BaseUnit.Preset.Skeleton);

				_rareEnemySpawnRate = 15f;
				_rareEnemySpawnTable.Add(BaseUnit.Preset.Widowling);
				
			break;

			case DungeonManager.Zone.A2:
				_commonEnemySpawnRate = 70f;
				_commonEnemySpawnTable.Add(BaseUnit.Preset.Widowling);
				_commonEnemySpawnTable.Add(BaseUnit.Preset.Skeleton);

				_rareEnemySpawnRate = 30f;
				_rareEnemySpawnTable.Add(BaseUnit.Preset.Giant_Spider);
			break;

			case DungeonManager.Zone.A3:
				_commonEnemySpawnRate = 60f;
				_commonEnemySpawnTable.Add(BaseUnit.Preset.Giant_Spider);
				_commonEnemySpawnTable.Add(BaseUnit.Preset.Widowling);

				_rareEnemySpawnRate = 40f;
				_rareEnemySpawnTable.Add(BaseUnit.Preset.Giant_Widow);
			break;
		}
	}

	public BaseUnit GetEnemySpawn() {
		float rollSpawnRate = Random.Range(0f, 100f);
		int rollEnemyType = 0;

		BaseUnit.Preset preset = BaseUnit.Preset.Skeleton;
		if(rollSpawnRate <= _commonEnemySpawnRate) {
			rollEnemyType = Random.Range(0, _commonEnemySpawnTable.Count);
			preset = _commonEnemySpawnTable[rollEnemyType];
		} else if(rollSpawnRate <= _commonEnemySpawnRate + _rareEnemySpawnRate) {
			rollEnemyType = Random.Range(0, _rareEnemySpawnTable.Count);
			preset = _rareEnemySpawnTable[rollEnemyType];
		}

		BaseUnit baseUnit = new BaseUnit(false, preset);
		return baseUnit;
	}
}
