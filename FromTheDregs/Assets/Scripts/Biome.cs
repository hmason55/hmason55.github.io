using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biome {

	BiomeType _biomeType;
	int _x;
	int _y;
	int _radius;

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
	}
}
