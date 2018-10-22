using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

public class DungeonGenerator : MonoBehaviour {



	[SerializeField] AnimationController animationController;
	[SerializeField] SpriteManager spriteManager;
	

	[SerializeField] GameObject _tilePrefab;
	[SerializeField] GameObject _dungeonPrefab;
	[SerializeField] int renderDistance = 5;
	public static int TileWidth = 48;
	public static int TileHeight = 48;

	public static int dungeonDimension = 3;
	public static int chunkDimension = 16;
	[SerializeField] int minimumPathSize = 4;

	[SerializeField] int minimumBiomes = 0;
	[SerializeField] int maximumBiomes = 1;

	[Range(0f, 100f)] public float smallDecorationDensity;
	
	[Range(0f, 100f)] public float containerDecorationDensity;
	
	[Range(0f, 100f)] public float trapDecorationDensity;

	[SerializeField] GameObject _dungeon;

	Tile[,] _tiles;
	[HideInInspector][SerializeField] Tile[] _serializedTiles;

	Chunk[,] _chunks;
	[HideInInspector][SerializeField] Chunk[] _serializedChunks;


	[SerializeField] Biome.BiomeType biome = Biome.BiomeType.forsaken;

	List<Biome> biomes;

	bool _limitRendering = false;
	public bool limitRendering {
		get {return _limitRendering;}
	}

	public Tile[,] tiles {
		get {return _tiles;}
	}

	void Awake() {
		
		biomes = new List<Biome>();
	}

	void Start () {
		
		InitTiles();
		//SpawnTiles();
		SpawnBiomes();
		CreateMainPath();
		_limitRendering = true;
		Debug.Log(Time.realtimeSinceStartup);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void InitTiles() {
		int dimension = chunkDimension * dungeonDimension;
		_tiles = new Tile[dimension, dimension];
		_chunks = new Chunk[dungeonDimension, dungeonDimension];

		for(int y = 0; y < dimension; y++) {
			for(int x = 0; x < dimension; x++) {
				_tiles[x, y] = _serializedTiles[y * dimension + x];
			}
		}
	}

	public void BuildTileGrid() {
		// Remove existing dungeon
		DestroyImmediate(_dungeon);
		_dungeon = GameObject.Instantiate(_dungeonPrefab);
		_dungeon.name = "Dungeon";
		_dungeon.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform);
		_dungeon.transform.SetAsFirstSibling();

		// Create serialized array
		_serializedTiles = new Tile[(int)Mathf.Pow(dungeonDimension * chunkDimension, 2)];

		for(int y = 0; y < chunkDimension * dungeonDimension; y++) {
			for(int x = 0; x < chunkDimension * dungeonDimension; x++) {

				GameObject tileGO = GameObject.Instantiate(_tilePrefab);
				tileGO.name = "Tile (" + x +  ", " + y + ")";
				Tile tile = tileGO.GetComponent<Tile>();

				// Set References
				tile.animationController = animationController;
				tile.dungeonGenerator = this;
				tile.spriteManager = spriteManager;
				
				_serializedTiles[y * chunkDimension * dungeonDimension + x] = tile;

				// Transform
				RectTransform tileRT = tileGO.GetComponent<RectTransform>();
				tileGO.transform.SetParent(_dungeon.transform);
				tileRT.anchoredPosition = new Vector3(x * TileWidth, y * TileHeight, 0f);
				tileRT.localScale = new Vector3(1f, 1f, 1f);
			}
		}

		

	}

	public void CheckTileSize() {

	}

	void SpawnTiles(List<Vector2Int> nodes) {
		// Spawn tiles
		for(int i = 0; i < nodes.Count; i++) {
			Vector2Int node = nodes[i];

			int offsetX = node.x * chunkDimension * TileWidth;
			int offsetY = node.y * chunkDimension * TileHeight;
			Color[] pixels = _chunks[node.x, node.y].template.texture.GetPixels();

			for(int y = 0; y < chunkDimension; y++) {
				for(int x = 0; x < chunkDimension; x++) {
					Tile tile = _tiles[(node.x * chunkDimension + x), (node.y * chunkDimension + y)];
					tile.position = new Vector2Int(node.x * chunkDimension + x, node.y * chunkDimension + y);

					// Biome
					tile.terrain.biome = biome;
					tile.decoration.biome = biome;

					foreach(Biome b in biomes) {
						if(CheckDistance(x * TileWidth + offsetX, y * TileHeight + offsetY, b.x, b.y) < b.radius) {
							tile.terrain.biome = b.biomeType;
							tile.decoration.biome = b.biomeType;
						}
					}

					Color color = pixels[(chunkDimension * y + x)];

					// Parse Chunk Template
					tile.terrain.ParseColor(color);
					string hexColor = ColorUtility.ToHtmlStringRGB(color);

					// Decorations
					if(hexColor == "0000FF" && i <= 1) {
						
						if(i == 0) {
							// Entrance	
							tile.decoration.decorationType = Decoration.DecorationType.entrance;
							tile.SpawnUnit(new BaseUnit(true, BaseUnit.StatPreset.Human, BaseUnit.SpritePreset.knight, tile));
							Debug.Log("Spawned at " + tile.position);
						} else if(i == 1) {
							// Exit
							tile.decoration.decorationType = Decoration.DecorationType.exit;
						}
						
						tile.decoration.Init(tile.terrain.walkable);
					} else if(hexColor == "FFFF00") {

						// Container decorations
						float containerDecorationRoll = Random.Range(0f, 100f);
						if(containerDecorationRoll <= containerDecorationDensity) {
							tile.decoration.decorationType = Decoration.DecorationType.container;
							tile.decoration.Init(tile.terrain.walkable);
						}
					} else if(hexColor == "FF00FF") {

						// Trap decorations
						float containerDecorationRoll = Random.Range(0f, 100f);
						if(containerDecorationRoll <= trapDecorationDensity) {
							tile.decoration.decorationType = Decoration.DecorationType.trap;
							tile.decoration.Init(tile.terrain.walkable);
						}
					}


					if(tile.decoration.decorationType == Decoration.DecorationType.none) {
						// Small decorations
						float smallDecorationRoll = Random.Range(0f, 100f);
						if(smallDecorationRoll <= smallDecorationDensity) {
							tile.decoration.decorationType = Decoration.DecorationType.small;
							tile.decoration.Init(tile.terrain.walkable);
						}
					}

				}
			}
		}

		for(int y = 0; y < dungeonDimension * chunkDimension; y++) {
			for(int x = 0; x < dungeonDimension * chunkDimension; x++) {

				if(y > 0) {
					Tile tile = _tiles[x, y];

					if(tile != null) {
						if(tile.terrain.terrainType == Terrain.TerrainType.wall_top) {

							if(_tiles[x, y-1] != null) {
								if(_tiles[x, y-1].terrain.terrainType == Terrain.TerrainType.ground) {
									tile.terrain.terrainType = Terrain.TerrainType.wall_side;
									tile.terrain.LoadTexture();
								}
							}

						}
					}

				}

			}
		}

		Debug.Log("Spawned " + nodes.Count + " rooms.");
	}

	void SpawnUnit(int x, int y, Unit unit) {
		_tiles[x, y].unit = unit;
	}

	void CreateMainPath() {

		seed:
		int startX = Random.Range(0, dungeonDimension-1);
		int startY = Random.Range(0, dungeonDimension-1);
		int endX = Random.Range(0, dungeonDimension-1);
		int endY = Random.Range(0, dungeonDimension-1);

	 	while(endX == startX && endY == startY) {
			endX = Random.Range(0, dungeonDimension-1);
			endY = Random.Range(0, dungeonDimension-1);
		}

		List<Vector2Int> nodes = new List<Vector2Int>();

		Debug.Log("Start (" + startX + ", " + startY + ")");
		nodes.Add(new Vector2Int(startX, startY));
		Debug.Log("End (" + endX + ", " + endY + ")");
		nodes.Add(new Vector2Int(endX, endY));

		int cursorX = startX;
		int cursorY = startY;

		int limit = 0;
		bool done = false;
		while(!done) {
			if(limit > 1000) {
				goto seed;
			} else {
				limit++;
			}
			List<Vector2Int> deltaPositions = new List<Vector2Int>();

			if(cursorX > 0) {
				deltaPositions.Add(new Vector2Int(-1, 0));
			}

			if(cursorX < dungeonDimension-1) {
				deltaPositions.Add(new Vector2Int(1, 0));
			}

			if(cursorY > 0) {
				deltaPositions.Add(new Vector2Int(0, -1));
			}

			if(cursorY < dungeonDimension-1) {
				deltaPositions.Add(new Vector2Int(0, 1));
			}

			Vector2Int nextPosition = deltaPositions[Random.Range(0, deltaPositions.Count)];

			if(!NodeExists(cursorX + nextPosition.x, cursorY + nextPosition.y, nodes)) {
				cursorX += nextPosition.x;
				cursorY += nextPosition.y;
				Debug.Log("Node (" + cursorX + ", " + cursorY + ")");
				nodes.Add(new Vector2Int(cursorX, cursorY));
			}

			if(Mathf.Abs(cursorX - endX) == 1 && cursorY == endY) {
				done = true;
			} else if (cursorX == endX && Mathf.Abs(cursorY - endY) == 1) {
				done = true;
			} else if(cursorX == endX && cursorY == endY) {
				done = true;
			}
		}

		if(nodes.Count <= minimumPathSize) {
			goto seed;
		}

		foreach(Vector2Int node in nodes) {

			string chunkPresetString = "";
			
			// Check Left
			if(NodeExists(node.x - 1, node.y, nodes)) {
				chunkPresetString += "1";
			} else {
				chunkPresetString += "0";
			}

			// Check Right
			if(NodeExists(node.x + 1, node.y, nodes)) {
				chunkPresetString += "1";
			} else {
				chunkPresetString += "0";
			}

			// Check Up
			if(NodeExists(node.x, node.y + 1, nodes)) {
				chunkPresetString += "1";
			} else {
				chunkPresetString += "0";
			}

			// Check Down
			if(NodeExists(node.x, node.y - 1, nodes)) {
				chunkPresetString += "1";
			} else {
				chunkPresetString += "0";
			}

			_chunks[node.x, node.y] = new Chunk(chunkPresetString);
		}

		SpawnTiles(nodes);
	}

	bool NodeExists(int x, int y, List<Vector2Int> nodes) {
		foreach(Vector2Int node in nodes) {
			if(node.x == x && node.y == y) {
				return true;
			}
		}
		return false;
	}

	public void RenderTiles() {

		// Reset render state of all tiles.
		for(int y = 0; y < chunkDimension * dungeonDimension; y++) {
			for(int x = 0; x < chunkDimension * dungeonDimension; x++) {
				_tiles[x, y].DisableRendering();
			}	
		}

		// Check distance from sources
		for(int y = 0; y < chunkDimension * dungeonDimension; y++) {
			for(int x = 0; x < chunkDimension * dungeonDimension; x++) {

				if(!_tiles[x, y].renderFlag) {
					BaseUnit baseUnit = _tiles[x, y].unit.baseUnit;
					if(baseUnit != null) {
						if(baseUnit.playerControlled) {

							for(int dy = -renderDistance; dy < renderDistance; dy++) {
								for(int dx = -renderDistance; dx < renderDistance; dx++) {
									if(x+dx >= 0 && x+dx < (chunkDimension*dungeonDimension-1) && y+dy >= 0 && y+dy < (chunkDimension*dungeonDimension-1)) {
										_tiles[x+dx, y+dy].EnableRendering();
									}
								}
							}

							
						}
					}
				}

			}	
		}

	}

	int CalcManhattanDistance(int x1, int y1, int x2, int y2) {
		return (x2 - x1) + (y2 - y1);
	}

	void SpawnBiomes() {
		int numBiomes = Random.Range(minimumBiomes, maximumBiomes);
		for(int i = 0; i < numBiomes; i++) {
			Debug.Log("Spawned biome");
			int x = Random.Range(0, dungeonDimension * chunkDimension * TileWidth);
			int y = Random.Range(0, dungeonDimension * chunkDimension * TileHeight);
			int radius = Random.Range(0, dungeonDimension * chunkDimension * TileWidth / 2);
			biomes.Add(new Biome(x, y, radius));
		}
	}

	int CheckDistance(int x1, int y1, int x2, int y2) {
		return (int)Mathf.Sqrt(Mathf.Pow(x2 - x1, 2) + Mathf.Pow(y2 - y1, 2));
	}
}
