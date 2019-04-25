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
			//Tile t1 = dungeonManager.tiles[x, y];
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
			
			

			if(y > 0) {
				Tile t = dungeonManager.tiles[x, y-1];

				if(t.baseTerrain != null) {
					if(t.baseTerrain.terrainType == TerrainType.wall_side) {
						_render = true;
						return;
					}
				}

				if(x > 0) {
					Tile tl = dungeonManager.tiles[x-1, y-1];

					if(tl.baseTerrain != null) {
						if(tl.baseTerrain.terrainType == TerrainType.wall_side) {
							_render = true;
							return;
						}
					}
				}

				if(x < DungeonManager.dimension-1) {
					Tile tr = dungeonManager.tiles[x+1, y-1];

					if(tr.baseTerrain != null) {
						if(tr.baseTerrain.terrainType == TerrainType.wall_side) {
							_render = true;
							return;
						}
					}
				}
			}

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
	}

	public void Shade(SpriteManager spriteManager, DungeonManager dungeonManager, int x, int y) {
		_shaded = true;
		if((_terrainType == TerrainType.wall_top || _terrainType == TerrainType.wall_side)) {
			
			Tile[] samples = {
				null,	// Top
				null,	// Bottom
				null,	// Left
				null,	// Right
				null,	// Top Left
				null,	// Top Right
				null,	// Bottom Left
				null	// Bottom Right
			};

			bool[] adjacency = {
				false,	// Top
				false,	// Bottom
				false,	// Left
				false,	// Right
				false,	// Top Left
				false,	// Top Right
				false,	// Bottom Left
				false	// Bottom Right
			};


			if(x == 0 && y == DungeonManager.dimension-1) {
				// Top Left Corner

				// Ignore Top
				samples[1] = dungeonManager.tiles[x  , y-1];	// Bottom
				// Ignore Left
				samples[3] = dungeonManager.tiles[x+1, y  ];	// Right
				// Ignore Top Left
				// Ignore Top Right
				// Ignore Bottom Left
				samples[7] = dungeonManager.tiles[x+1, y-1];	// Bottom Right

			} else if(x == DungeonManager.dimension-1 && y == DungeonManager.dimension-1) {
				// Top Right Corner

				// Ignore Top
				samples[1] = dungeonManager.tiles[x  , y-1];	// Bottom
				samples[2] = dungeonManager.tiles[x-1, y  ];	// Left
				// Ignore Right
				// Ignore Top Left
				// Ignore Top Right
				samples[6] = dungeonManager.tiles[x-1, y-1];	// Bottom Left
				// Ignore Bottom Right

			} else if(x == 0 && y == 0) {
				// Bottom Left Corner

				samples[0] = dungeonManager.tiles[x  , y+1];	// Top
				// Ignore Bottom
				// Ignore Left
				samples[3] = dungeonManager.tiles[x+1, y  ];	// Right
				// Ignore Top Left
				samples[5] = dungeonManager.tiles[x+1, y+1];	// Top Right
				// Ignore Bottom Left
				// Ignore Bottom Right

			} else if(x == DungeonManager.dimension-1 && y == 0) {
				// Bottom Right Corner

				samples[0] = dungeonManager.tiles[x  , y+1];	// Top
				// Ignore Bottom
				samples[2] = dungeonManager.tiles[x-1, y  ];	// Left
				// Ignore Right
				samples[4] = dungeonManager.tiles[x-1, y+1];	// Top Left
				// Ignore Top Right
				// Ignore Bottom Left
				// Ignore Bottom Right

			} else if(y == DungeonManager.dimension-1) {
				// Top

				// Ignore Top
				samples[1] = dungeonManager.tiles[x  , y-1];	// Bottom
				samples[2] = dungeonManager.tiles[x-1, y  ];	// Left
				samples[3] = dungeonManager.tiles[x+1, y  ];	// Right
				// Ignore Top Left
				// Ignore Top Right
				samples[6] = dungeonManager.tiles[x-1, y-1];	// Bottom Left
				samples[7] = dungeonManager.tiles[x+1, y-1];	// Bottom Right

			} else if(y == 0) {
				// Bottom

				samples[0] = dungeonManager.tiles[x  , y+1];	// Top
				// Ignore Bottom
				samples[2] = dungeonManager.tiles[x-1, y  ];	// Left
				samples[3] = dungeonManager.tiles[x+1, y  ];	// Right
				samples[4] = dungeonManager.tiles[x-1, y+1];	// Top Left
				samples[5] = dungeonManager.tiles[x+1, y+1];	// Top Right
				// Ignore Bottom Left
				// Ignore Bottom Right

			} else if(x == 0) {
				// Left

				samples[0] = dungeonManager.tiles[x  , y+1];	// Top
				samples[1] = dungeonManager.tiles[x  , y-1];	// Bottom
				// Ignore Left
				samples[3] = dungeonManager.tiles[x+1, y  ];	// Right
				// Ignore Top Left
				samples[5] = dungeonManager.tiles[x+1, y+1];	// Top Right
				// Ignore Bottom Left
				samples[7] = dungeonManager.tiles[x+1, y-1];	// Bottom Right

			} else if(x == DungeonManager.dimension-1) {
				// Right

				samples[0] = dungeonManager.tiles[x  , y+1];	// Top
				samples[1] = dungeonManager.tiles[x  , y-1];	// Bottom
				samples[2] = dungeonManager.tiles[x-1, y  ];	// Left
				// Ignore Right
				samples[4] = dungeonManager.tiles[x-1, y+1];	// Top Left
				// Ignore Top Right
				samples[6] = dungeonManager.tiles[x-1, y-1];	// Bottom Left
				// Ignore Bottom Right

			} else {
				// Center, Ignore None
				samples[0] = dungeonManager.tiles[x  , y+1];	// Top
				samples[1] = dungeonManager.tiles[x  , y-1];	// Bottom
				samples[2] = dungeonManager.tiles[x-1, y  ];	// Left
				samples[3] = dungeonManager.tiles[x+1, y  ];	// Right
				samples[4] = dungeonManager.tiles[x-1, y+1];	// Top Left
				samples[5] = dungeonManager.tiles[x+1, y+1];	// Top Right
				samples[6] = dungeonManager.tiles[x-1, y-1];	// Bottom Left
				samples[7] = dungeonManager.tiles[x+1, y-1];	// Bottom Right
			}

			for(int i = 0; i < samples.Length; i++) {
				if(samples[i] != null) {
					if(samples[i].baseTerrain != null) {
						adjacency[i] = !samples[i].baseTerrain.render;
					} else {
						adjacency[i] = true;
					}
				} else {
					adjacency[i] = true;
				}
			}
			
			if(!adjacency[0] && !adjacency[2] && adjacency[4]) {
				// Top Left Inner
				_shadedSprite = spriteManager.borderShaded[4];
				_shaded = true;

			}else if(!adjacency[0] && !adjacency[3] && adjacency[5]) {
				// Top Right Inner
				_shadedSprite = spriteManager.borderShaded[5];
				_shaded = true;

			} else if(!adjacency[1] && !adjacency[2] && adjacency[6]) {
				// Bottom Left Inner
				_shadedSprite = spriteManager.borderShaded[6];
				_shaded = true;

			} else if(!adjacency[1] && !adjacency[3] && adjacency[7]) {
				// Bottom Right Inner
				_shadedSprite = spriteManager.borderShaded[7];
				_shaded = true;

			}  else if(adjacency[0] && adjacency[2]) {
				// Top Left Outer
				_shadedSprite = spriteManager.borderShaded[8];
				_shaded = true;

			} else if(adjacency[0] && adjacency[3]) {
				// Top Right Outer
				_shadedSprite = spriteManager.borderShaded[9];
				_shaded = true;

			} else if(adjacency[1] && adjacency[2]) {
				// Bottom Left Outer
				_shadedSprite = spriteManager.borderShaded[10];
				_shaded = true;

			} else if(adjacency[1] && adjacency[3]) {
				// Bottom Right Outer
				_shadedSprite = spriteManager.borderShaded[11];
				_shaded = true;

			} else if(adjacency[0]) {
				// Top
				_shadedSprite = spriteManager.borderShaded[0];
				_shaded = true;

			} else if(adjacency[1]) {
				// Bottom
				_shadedSprite = spriteManager.borderShaded[1];
				_shaded = true;

			} else if(adjacency[2]) {
				// Left
				_shadedSprite = spriteManager.borderShaded[2];
				_shaded = true;

			} else if(adjacency[3]) {
				// Right
				_shadedSprite = spriteManager.borderShaded[3];
				_shaded = true;

			} else {
				_shaded = false;
			}

		} else {
			_shaded = false;
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
