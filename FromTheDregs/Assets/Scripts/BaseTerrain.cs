using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTerrain {
	public enum TerrainType {
		wall_top,
		wall_side,
		ground
	}

	bool _walkable = false;
	
	Biome _biome;
	TerrainType _terrainType = TerrainType.ground;
	Sprite _sprite;

	public bool walkable {
		get {return _walkable;}
		set {_walkable = value;}
	}

	public Biome biome {
		get {return _biome;}
		set {_biome = value;}
	}

	public TerrainType terrainType {
		get {return _terrainType;}
		set {_terrainType = value;}
	}

	public Sprite sprite {
		get {return _sprite;}
	}

	public BaseTerrain(Biome b, SpriteManager s, string c) {
		_biome = b;
		//ParseColor(s, c);
	}

	public BaseTerrain(Biome b, SpriteManager s, string c, DungeonManager d, int x, int y) {
		_biome = b;
		ParseColor(s, c, d, x, y);
	}

	void ParseColor(SpriteManager spriteManager, string color, DungeonManager dungeonManager, int x, int y) {
	//string colorAsString = ColorUtility.ToHtmlStringRGB(color);
		switch(color) {
			case "000000":	// Black
				_walkable = false;
				_terrainType = TerrainType.wall_top;

				if(y > 0) {
					
					Tile t1 = dungeonManager.tiles[x, y];
					Tile t2 = dungeonManager.tiles[x, y-1];
					if(t2.baseTerrain != null) {
						if(t2.baseTerrain.terrainType == TerrainType.ground) {
							_terrainType = TerrainType.wall_side;
						}
					}
				}

				LoadTexture(spriteManager);
			break;
			
			case "FF0000":  // Red
			case "0000FF":	// Blue
			case "FFFF00":	// Yellow
			case "FF00FF": 	// Magenta
			case "00FF00":	// Green
				_walkable = true;
				_terrainType = TerrainType.ground;
				LoadTexture(spriteManager);
			break;
		}
	}

	public void LoadTexture(SpriteManager spriteManager) {
		List<Sprite> sprites = new List<Sprite>();

		switch(_biome.biomeType) {
			case Biome.BiomeType.dungeon:
				switch(_terrainType) {
					case BaseTerrain.TerrainType.ground:
						sprites = spriteManager.biomeDungeon.ground;
					break;

					case BaseTerrain.TerrainType.wall_side:
						sprites = spriteManager.biomeDungeon.wallSide;
					break;

					case BaseTerrain.TerrainType.wall_top:
						sprites = spriteManager.biomeDungeon.wallTop;
					break;
				}
			break;

			case Biome.BiomeType.forsaken:
				switch(_terrainType) {
					case BaseTerrain.TerrainType.ground:
						sprites = spriteManager.biomeForsaken.ground;
					break;

					case BaseTerrain.TerrainType.wall_side:
						sprites = spriteManager.biomeForsaken.wallSide;
					break;

					case BaseTerrain.TerrainType.wall_top:
						sprites = spriteManager.biomeForsaken.wallTop;
					break;
				}
			break;
		}

		if(sprites != null) {
			int variation = Random.Range(0, sprites.Count-1);
			_sprite = sprites[variation];
		}
	}
}
