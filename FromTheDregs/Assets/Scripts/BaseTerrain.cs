using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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

	public BaseTerrain(Biome b, string c) {
		_biome = b;
		ParseColor(c);
	}

	void ParseColor(string color) {
	//string colorAsString = ColorUtility.ToHtmlStringRGB(color);
		switch(color) {
			case "000000":	// Black
				_walkable = false;
				_terrainType = TerrainType.wall_top;
			break;
			
			case "FFFF00":	// Yellow
				_walkable = false;
				_terrainType = TerrainType.ground;
			break;

			case "FF0000":  // Red
			case "0000FF":	// Blue
			case "FF00FF": 	// Magenta
			case "00FF00":	// Green
				_walkable = true;
				_terrainType = TerrainType.ground;
			break;
		}
	}
	
	public void Initialize(SpriteManager spriteManager, DungeonManager dungeonManager, int x, int y) {
		if(_terrainType == TerrainType.wall_top && y > 0) {
			Tile t1 = dungeonManager.tiles[x, y];
			Tile t2 = dungeonManager.tiles[x, y-1];
			if(t2.baseTerrain != null) {
				if(t2.baseTerrain.terrainType == TerrainType.ground) {
					_terrainType = TerrainType.wall_side;
				}
			}
		}
		LoadTexture(spriteManager);
		if(dungeonManager.tiles[x, y].terrain != null) {
			dungeonManager.tiles[x, y].terrain.UpdateSprite();
		}
	}

	public void LoadTexture(SpriteManager spriteManager) {
		List<Sprite> sprites = new List<Sprite>();

		switch(_biome.biomeType) {
			// Cavern
			case Biome.BiomeType.cavern:
				switch(_terrainType) {
					case BaseTerrain.TerrainType.ground:
						sprites = spriteManager.biomeCavern.ground;
					break;

					case BaseTerrain.TerrainType.wall_side:
						sprites = spriteManager.biomeCavern.wallSide;
					break;

					case BaseTerrain.TerrainType.wall_top:
						sprites = spriteManager.biomeCavern.wallTop;
					break;
				}
			break;

			// Crypt
			case Biome.BiomeType.crypt:
				switch(_terrainType) {
					case BaseTerrain.TerrainType.ground:
						sprites = spriteManager.biomeCrypt.ground;
					break;

					case BaseTerrain.TerrainType.wall_side:
						sprites = spriteManager.biomeCrypt.wallSide;
					break;

					case BaseTerrain.TerrainType.wall_top:
						sprites = spriteManager.biomeCrypt.wallTop;
					break;
				}
			break;

			// Dungeon
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

			// Forsaken
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

			
			// Ruins
			case Biome.BiomeType.ruins:
				switch(_terrainType) {
					case BaseTerrain.TerrainType.ground:
						sprites = spriteManager.biomeRuins.ground;
					break;

					case BaseTerrain.TerrainType.wall_side:
						sprites = spriteManager.biomeRuins.wallSide;
					break;

					case BaseTerrain.TerrainType.wall_top:
						sprites = spriteManager.biomeRuins.wallTop;
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
