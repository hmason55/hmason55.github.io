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

		switch(_biomeType) {
			case BiomeType.dungeon:
			break;

			case BiomeType.forsaken:
			break;
		}
	}

	public BaseUnit.SpritePreset GetEnemySpritePreset() {
		float rollSpawnRate = Random.Range(0f, 100f);
		int rollEnemyType = 0;
		if(rollSpawnRate <= commonEnemySpawnRate) {
			rollEnemyType = Random.Range(0, commonEnemySpawnTable.Count);
			return commonEnemySpawnTable[rollEnemyType];
		} else if(rollSpawnRate <= commonEnemySpawnRate + rareEnemySpawnRate) {
			rollEnemyType = Random.Range(0, rareEnemySpawnTable.Count);
			return rareEnemySpawnTable[rollEnemyType];
		}
		return BaseUnit.SpritePreset.none;
	}
}
