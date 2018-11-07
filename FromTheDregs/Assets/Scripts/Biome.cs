using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biome {

	BiomeType _biomeType;
	int _x;
	int _y;
	int _radius;

	float commonEnemySpawnRate = 100f;
	float rareEnemySpawnRate = 0f;


	List<BaseUnit.SpritePreset> commonEnemySpawnTable;
	List<BaseUnit.SpritePreset> rareEnemySpawnTable;

	public enum BiomeType {
		forsaken,
		dungeon
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

	public Biome(int x, int y, int radius) {
		_x = x;
		_y = y;
		_radius = radius;
		_biomeType = BiomeType.forsaken;

		commonEnemySpawnTable = new List<BaseUnit.SpritePreset>();
		rareEnemySpawnTable = new List<BaseUnit.SpritePreset>();

		switch(_biomeType) {
			case BiomeType.dungeon:

			break;

			case BiomeType.forsaken:
				commonEnemySpawnRate = 75f;
				commonEnemySpawnTable.Add(BaseUnit.SpritePreset.greenslime);
				commonEnemySpawnTable.Add(BaseUnit.SpritePreset.spidersmall);
				commonEnemySpawnTable.Add(BaseUnit.SpritePreset.spider);
				commonEnemySpawnTable.Add(BaseUnit.SpritePreset.widowsmall);
				
				rareEnemySpawnRate = 25f;
				rareEnemySpawnTable.Add(BaseUnit.SpritePreset.widow);
			break;
		}
	}

	public BaseUnit GetEnemySpawn() {
		float rollSpawnRate = Random.Range(0f, 100f);
		int rollEnemyType = 0;

		BaseUnit.SpritePreset spritePreset = BaseUnit.SpritePreset.direrat;
		if(rollSpawnRate <= commonEnemySpawnRate) {
			rollEnemyType = Random.Range(0, commonEnemySpawnTable.Count);
			spritePreset = commonEnemySpawnTable[rollEnemyType];
		} else if(rollSpawnRate <= commonEnemySpawnRate + rareEnemySpawnRate) {
			rollEnemyType = Random.Range(0, rareEnemySpawnTable.Count);
			spritePreset = rareEnemySpawnTable[rollEnemyType];
		}

		BaseUnit baseUnit = new BaseUnit(false, spritePreset);
		return baseUnit;
	}
}
