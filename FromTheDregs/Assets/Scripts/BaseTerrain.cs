using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseTerrain {
	public enum TerrainType {
		wall_top,
		wall_side,
		ground,
		wall_hidden,
		ground_hidden,

	}

	bool _walkable = false;
	
	Biome _biome;
	TerrainType _terrainType = TerrainType.ground;
	Sprite _sprite;
	Sprite _shadedSprite;

	bool _hidden = false;
	bool _render = true;
	bool _shaded = false;

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

	public bool render {
		get {return _render;}
		set {_render = value;}
	}

	public bool shaded {
		get {return _shaded;}
		set {_shaded = value;}
	}

	public Sprite shadedSprite {
		get {return _shadedSprite;}
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

		if(_terrainType == TerrainType.ground) {

			if(y == DungeonManager.dimension-1) {
				if(dungeonManager.tiles[x, y-1] != null) {
					if(dungeonManager.tiles[x, y-1].baseTerrain.terrainType == TerrainType.ground) {
						_terrainType = TerrainType.wall_side;
					} else {
						_terrainType = TerrainType.wall_top;
					}
				} else {
					_terrainType = TerrainType.wall_top;
				}
			} else if(dungeonManager.tiles[x, y+1] == null) {
				_terrainType = TerrainType.wall_top;
			} else if(y == 0) {
				_terrainType = TerrainType.wall_top;
			} else if(dungeonManager.tiles[x, y-1] == null) {
				_terrainType = TerrainType.wall_top;
			} else if(x == 0) {
				_terrainType = TerrainType.wall_top;
			} else if(dungeonManager.tiles[x-1, y] == null) {
				_terrainType = TerrainType.wall_top;
			} else if(x == DungeonManager.dimension-1) {
				_terrainType = TerrainType.wall_top;
			} else if(dungeonManager.tiles[x+1, y] == null) {
				_terrainType = TerrainType.wall_top;
			}

			if(_terrainType == TerrainType.wall_top || _terrainType == TerrainType.wall_side) {
				_walkable = false;
			}
		}

		LoadTexture(spriteManager);
		if(dungeonManager.tiles[x, y].terrain != null) {
			dungeonManager.tiles[x, y].terrain.UpdateSprite();
		}
	}

	public void Optimize(SpriteManager spriteManager, DungeonManager dungeonManager, int x, int y) {

		

		if(_terrainType == TerrainType.wall_top) {
			List<Tile> samples = new List<Tile>();
			List<bool> adjacency = new List<bool>();

			if(x == 0 && y == 0) {
				samples.Add(dungeonManager.tiles[x  , y+1]);	// Top
				samples.Add(dungeonManager.tiles[x+1, y+1]);	// Top Right
				samples.Add(dungeonManager.tiles[x+1, y  ]);	// Right
			} else if(x == 0 && y == DungeonManager.dimension-1) {
				samples.Add(dungeonManager.tiles[x  , y-1]);	// Bottom
				samples.Add(dungeonManager.tiles[x+1, y-1]);	// Bottom Right
				samples.Add(dungeonManager.tiles[x+1, y  ]);	// Right
			} else if(x == DungeonManager.dimension-1 && y == 0) {
				samples.Add(dungeonManager.tiles[x  , y+1]);	// Top
				samples.Add(dungeonManager.tiles[x-1, y+1]);	// Top Left
				samples.Add(dungeonManager.tiles[x-1, y  ]);	// Left
			} else if(x == DungeonManager.dimension-1 && y == DungeonManager.dimension-1) {
				samples.Add(dungeonManager.tiles[x  , y-1]);	// Bottom
				samples.Add(dungeonManager.tiles[x-1, y-1]);	// Bottom Left
				samples.Add(dungeonManager.tiles[x-1, y  ]);	// Left
			} else if(x == 0) {
				samples.Add(dungeonManager.tiles[x+1, y+1]);	// Top Right
				samples.Add(dungeonManager.tiles[x+1, y-1]);	// Bottom Right
				samples.Add(dungeonManager.tiles[x+1, y  ]);	// Right
			} else if(x == DungeonManager.dimension-1) {
				samples.Add(dungeonManager.tiles[x-1, y+1]);	// Top Left
				samples.Add(dungeonManager.tiles[x-1, y-1]);	// Bottom Left
				samples.Add(dungeonManager.tiles[x-1, y  ]);	// Left
			} else if(y == 0) {
				samples.Add(dungeonManager.tiles[x  , y+1]);	// Top
				samples.Add(dungeonManager.tiles[x-1, y+1]);	// Top Left
				samples.Add(dungeonManager.tiles[x+1, y+1]);	// Top Right
			} else if(y == DungeonManager.dimension-1) {
				samples.Add(dungeonManager.tiles[x  , y-1]);	// Bottom
				samples.Add(dungeonManager.tiles[x-1, y-1]);	// Bottom Left
				samples.Add(dungeonManager.tiles[x+1, y-1]);	// Bottom Right
			} else {
				samples.Add(dungeonManager.tiles[x  , y+1]);	// Top
				samples.Add(dungeonManager.tiles[x  , y-1]);	// Bottom
				samples.Add(dungeonManager.tiles[x-1, y  ]);	// Left
				samples.Add(dungeonManager.tiles[x+1, y  ]);	// Right
				samples.Add(dungeonManager.tiles[x-1, y+1]);	// Top Left
				samples.Add(dungeonManager.tiles[x+1, y+1]);	// Top Right
				samples.Add(dungeonManager.tiles[x-1, y-1]);	// Bottom Left
				samples.Add(dungeonManager.tiles[x+1, y-1]);	// Bottom Right
			}


			for(int i = 0; i < samples.Count; i++) {
				adjacency.Add(true);
			}

			for(int i = 0; i < samples.Count; i++) {
				if(samples[i].baseTerrain != null) {
					if(samples[i].baseTerrain.terrainType == TerrainType.ground) {
						adjacency[i] = false;
					}
				}
			}
			
			_render = false;
			for(int i = 0; i < samples.Count; i++) {
				if(adjacency[i] == false) {
					_render = true;
				}
			}
		}
		
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

		if(render && sprites != null) {
			int variation = Random.Range(0, sprites.Count-1);
			_sprite = sprites[variation];
		}
	}
}
