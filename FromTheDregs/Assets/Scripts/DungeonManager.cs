using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

public class DungeonManager : MonoBehaviour {

	[SerializeField] AnimationController animationController;
	[SerializeField] SpriteManager spriteManager;
	[SerializeField] CombatManager combatManager;
	
	[SerializeField] GameObject _dungeonPrefab;
	[SerializeField] GameObject _terrainPrefab;
	[SerializeField] GameObject _decorationPrefab;
	[SerializeField] GameObject _unitPrefab;
	[SerializeField] int renderDistance = 5;
	public static int TileWidth = 48;
	public static int TileHeight = 48;

	public static int dungeonDimension = 3;
	public static int chunkDimension = 16;
	public static int dimension = dungeonDimension * chunkDimension;

	public Vector2Int exitPosition;
	[SerializeField] int minimumPathSize = 4;

	[SerializeField] int minimumBiomes = 0;
	[SerializeField] int maximumBiomes = 1;

	[Range(0f, 100f)] public float smallDecorationDensity;
	
	[Range(0f, 100f)] public float containerDecorationDensity;
	
	[Range(0f, 100f)] public float trapDecorationDensity;

	[SerializeField] GameObject _terrainLayer;
	[SerializeField] GameObject _decorationLayer;
	[SerializeField] GameObject _unitLayer;

	Chunk[,] _chunks;
	Tile[,] _tiles;
	TerrainBehaviour[,] _terrainPool;
	DecorationBehaviour[,] _decorationPool;
	UnitBehaviour[,] _unitPool;
	[HideInInspector][SerializeField] List<TerrainBehaviour> _terrainStack;
	[HideInInspector][SerializeField] List<DecorationBehaviour> _decorationStack;
	[HideInInspector][SerializeField] List<UnitBehaviour> _unitStack;

	Vector2Int _renderOrigin;


	[SerializeField] Biome.BiomeType biomeType = Biome.BiomeType.forsaken;
	Biome _biome;

	List<Biome> biomes;

	bool _limitRendering = false;
	public bool limitRendering {
		get {return _limitRendering;}
	}

	public Vector2Int renderOrigin {
		get {return _renderOrigin;}
		set {_renderOrigin = value;}
	}

	public Tile[,] tiles {
		get {return _tiles;}
	}

	void Awake() {
		biomes = new List<Biome>();
	}

	void Start () {
		InitializeGrid();
		InitializeObjectPools();
		SpawnBiomes();
		CreateMainPath();
		_limitRendering = true;
		Debug.Log(Time.realtimeSinceStartup);
	}
	
	void InitializeGrid() {
		int dimension = chunkDimension * dungeonDimension;
		_tiles = new Tile[dimension, dimension];
		_chunks = new Chunk[dungeonDimension, dungeonDimension];

		for(int y = 0; y < dimension; y++) {
			for(int x = 0; x < dimension; x++) {
				Tile tile = new Tile(x, y);
				tile.dungeonManager = this;
				tile.combatManager = combatManager;
				tile.animationController = animationController;
				tile.spriteManager = spriteManager;
				_tiles[x, y] = tile;
			}
		}
	}

	void InitializeObjectPools() {
		int dimension = chunkDimension * dungeonDimension;

		_terrainPool = new TerrainBehaviour[dimension, dimension];
		_decorationPool = new DecorationBehaviour[dimension, dimension];
		_unitPool = new UnitBehaviour[dimension, dimension];

		for(int y = 0; y < dimension; y++) {
			for(int x = 0; x < dimension; x++) {
				_terrainPool[x, y] = null;
				_decorationPool[x, y] = null;
				_unitPool[x, y] = null;
			}
		}
	}

	#region Editor Scripts
	public void BuildObjectPools() {
		ClearObjectPools();

		_terrainStack = new List<TerrainBehaviour>();
		_decorationStack = new List<DecorationBehaviour>();
		_unitStack = new List<UnitBehaviour>();

		for(int i = 0; i < 4*renderDistance*renderDistance - 4*renderDistance + 1; i++) {
			GameObject t = GameObject.Instantiate(_terrainPrefab);
			t.name = "Terrain";
			t.transform.SetParent(_terrainLayer.transform);
			t.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
			_terrainStack.Add(t.GetComponent<TerrainBehaviour>());

			GameObject d = GameObject.Instantiate(_decorationPrefab);
			d.name = "Decoration";
			d.transform.SetParent(_decorationLayer.transform);
			d.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
			_decorationStack.Add(d.GetComponent<DecorationBehaviour>());

			GameObject u = GameObject.Instantiate(_unitPrefab);
			u.name = "Unit";
			u.transform.SetParent(_unitLayer.transform);
			u.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
			_unitStack.Add(u.GetComponent<UnitBehaviour>());
		}
	}

	public void ClearObjectPools() {
		for(int i = _terrainLayer.transform.childCount-1; i >= 0; i--) {
			DestroyImmediate(_terrainLayer.transform.GetChild(i).gameObject);
		}

		for(int i = _decorationLayer.transform.childCount-1; i >= 0; i--) {
			DestroyImmediate(_decorationLayer.transform.GetChild(i).gameObject);
		}

		for(int i = _unitLayer.transform.childCount-1; i >= 0; i--) {
			DestroyImmediate(_unitLayer.transform.GetChild(i).gameObject);
		}
	}
	#endregion

	void InitializeTiles(List<Vector2Int> nodes) {

		int dimension = chunkDimension * dungeonDimension;
		
		for(int i = 0; i < nodes.Count; i++) {
			Vector2Int node = nodes[i];
			Color[] pixels = _chunks[node.x, node.y].template.texture.GetPixels();
			
			int offsetX = node.x * chunkDimension * TileWidth;
			int offsetY = node.y * chunkDimension * TileHeight;

			for(int y = 0; y < chunkDimension; y++) {
				for(int x = 0; x < chunkDimension; x++) {

					Tile tile = _tiles[(node.x * chunkDimension + x), (node.y * chunkDimension + y)];
					//tile.position = new Vector2Int(node.x * chunkDimension + x, node.y * chunkDimension + y);

					// Initialize Biome
					Biome nearestBiome = _biome;
					foreach(Biome b in biomes) {
						if(CheckDistance(x * TileWidth + offsetX, y * TileHeight + offsetY, b.x, b.y) < b.radius) {
							nearestBiome = b;
						}
					}

					string hexColor = ColorUtility.ToHtmlStringRGB(pixels[(chunkDimension * y + x)]);

					// Initialize Terrain
					tile.baseTerrain = new BaseTerrain(nearestBiome, spriteManager, hexColor, this, (node.x * chunkDimension + x), (node.y * chunkDimension + y));

					switch(hexColor) {
						case "FF0000":	// Enemy (Red)
							BaseUnit enemy = nearestBiome.GetEnemySpawn();
							enemy.tile = tile;
							tile.SpawnUnit(enemy);
						break;

						case "FFFF00":	// Container (Yellow)
							float containerDecorationRoll = Random.Range(0f, 100f);
							if(containerDecorationRoll <= containerDecorationDensity) {
								tile.baseDecoration = new BaseDecoration(nearestBiome, BaseDecoration.DecorationType.Container, spriteManager);
							}
						break;

						case "0000FF":	// Entrance / Exit (Blue)
							if(i == 0) {
								tile.baseDecoration = new BaseDecoration(nearestBiome, BaseDecoration.DecorationType.Entrance, spriteManager);
								// Spawn player here
								tile.SpawnUnit(new BaseUnit(true, BaseUnit.StatPreset.Human, BaseUnit.SpritePreset.wizard, tile));
								combatManager.BeginCombat();
								_renderOrigin = tile.position;
							} else if(i == 1) {
								tile.baseDecoration = new BaseDecoration(nearestBiome,  BaseDecoration.DecorationType.Exit, spriteManager);
								exitPosition = tile.position;
							}
						break;

						case "FF00FF":	// Traps (Magenta)
							float trapDecorationRoll = Random.Range(0f, 100f);
							if(trapDecorationRoll <= trapDecorationDensity) {
								tile.baseDecoration = new BaseDecoration(nearestBiome, BaseDecoration.DecorationType.Trap, spriteManager);
							}
						break;

						case "00FF00":	// Decorations (Green)
							float smallDecorationRoll = Random.Range(0f, 100f);
							if(smallDecorationRoll <= smallDecorationDensity) {
								tile.baseDecoration = new BaseDecoration(nearestBiome, BaseDecoration.DecorationType.Small, spriteManager);
								//tile.decoration.Init(tile.terrain.walkable);
							}
						break;
					}



					// Initialize Unit



					//BaseDecoration;;
					
					//
					//
					//_tiles[x, y] = _serializedTiles[y * dimension + x];
					//
					//_tiles[x, y] = new Tile();
				}
			}
		}

		UpdateObjectPool();
	}

	public void UpdateObjectPool() {
		FreeObjects();
		AllocateObjects();
	}

	public void FreeObjects() {
		int dimension = chunkDimension * dungeonDimension;
		for(int y = 0; y < dimension; y++) {
			for(int x = 0; x < dimension; x++) {
				if(_tiles[x, y] != null) {
					if(!(x <  _renderOrigin.x + renderDistance &&
						 x >  _renderOrigin.x - renderDistance &&
						 y <  _renderOrigin.y + renderDistance &&
						 y >  _renderOrigin.y - renderDistance)) {
						
						if(_terrainPool[x, y] != null) {
							_terrainStack.Add(_terrainPool[x, y]);
							_terrainPool[x, y].Clear();
							_terrainPool[x, y] = null;
						}

						if(_decorationPool[x, y] != null) {
							_decorationStack.Add(_decorationPool[x, y]);
							_decorationPool[x, y].Clear();
							_decorationPool[x, y] = null;
						}

						if(_unitPool[x, y] != null) {
							_unitStack.Add(_unitPool[x, y]);
							_unitPool[x, y].Clear();
							_unitPool[x, y] = null;
						}
					}
				}
			}
		}
	}

	public void AllocateObjects() {
		int dimension = chunkDimension * dungeonDimension;
		int _terrainIndex = 0;
		int _decorationIndex = 0;
		int _unitIndex = 0;

		for(int y = 0; y < dimension; y++) {
			for(int x = 0; x < dimension; x++) {
				if(_tiles[x, y] != null) {
					if(	x <  _renderOrigin.x + renderDistance &&
						x >  _renderOrigin.x - renderDistance &&
						y <  _renderOrigin.y + renderDistance &&
						y >  _renderOrigin.y - renderDistance) {
						
						if(_terrainPool[x, y] == null) {
							if(_terrainStack.Count > _terrainIndex) {
								if(_tiles[x, y].baseTerrain != null) {
									_terrainPool[x, y] = _terrainStack[_terrainIndex++];
									_terrainPool[x, y].Transfer(_tiles[x, y], _tiles[x, y].baseTerrain);
									_terrainPool[x, y].transform.SetAsLastSibling();
								}
							}
						}

						if(_decorationPool[x, y] == null) {
							if(_decorationStack.Count > _decorationIndex) {
								if(_tiles[x, y].baseDecoration != null) {
									_decorationPool[x, y] = _decorationStack[_decorationIndex++];
									_decorationPool[x, y].Transfer(_tiles[x, y], _tiles[x, y].baseDecoration);
								}
							}
						}

						if(_unitPool[x, y] == null) {
							if(_unitStack.Count > _unitIndex) {
								if(_tiles[x, y].baseTerrain != null) {
									_unitPool[x, y] = _unitStack[_unitIndex++];
									_unitPool[x, y].Transfer(_tiles[x, y], tiles[x, y].baseUnit);
								}
							}
						}

					}
				}
			}
		}


		for(int i = _terrainStack.Count-1; i >= 0; i--) {
			if(_terrainStack[i].renderFlag) {
				_terrainStack.RemoveAt(i);
			}
		}

		for(int i = _decorationStack.Count-1; i >= 0; i--) {
			if(_decorationStack[i].renderFlag) {
				_decorationStack.RemoveAt(i);
			}
		}

		for(int i = _unitStack.Count-1; i >= 0; i--) {
			if(_unitStack[i].renderFlag) {
				_unitStack.RemoveAt(i);
			}
		}
		//Debug.Log(_unitStack.Count);
	}

	public void AllocateUnit(Vector2Int position) {
		if(_unitStack.Count == 0) {return;}

		if(_tiles[position.x, position.y] != null) {
			if(_tiles[position.x, position.y].unit == null) {
				_unitPool[position.x, position.y] = _unitStack[0];
				_unitStack.RemoveAt(0);
				//_unitPool[position.x, position.y].Transfer(_tiles[x, y], tiles[x, y].baseUnit);
			}
		}
	}

	/* 
	public void BuildTileGrid() {

		// Remove existing dungeon
		DestroyImmediate(_dungeon);

		// Create a new dungeon
		_dungeon = GameObject.Instantiate(_dungeonPrefab);
		_dungeon.name = "Dungeon";
		_dungeon.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform);
		_dungeon.transform.SetAsFirstSibling();

		// Get render layer references
		CameraController cameraController = _dungeon.GetComponent<CameraController>();

		// Create serialized array
		_serializedTiles = new Tile[dungeonDimension*dungeonDimension * chunkDimension*chunkDimension];

		for(int y = 0; y < chunkDimension * dungeonDimension; y++) {
			for(int x = 0; x < chunkDimension * dungeonDimension; x++) {

				// Terrain Layer
				GameObject terrainGO = GameObject.Instantiate(_terrainPrefab);
				terrainGO.name = "Terrain (" + x +  ", " + y + ")";

				RectTransform terrainRT = terrainGO.GetComponent<RectTransform>();
				terrainRT.SetParent(cameraController.terrainLayer);
				terrainRT.anchoredPosition = new Vector3(x * TileWidth, y * TileHeight, 0f);
				terrainRT.localScale = new Vector3(1f, 1f, 1f);


				// Decoration Layer
				GameObject decorationGO = GameObject.Instantiate(_decorationPrefab);
				decorationGO.name = "Decoration (" + x +  ", " + y + ")";

				RectTransform decorationRT = decorationGO.GetComponent<RectTransform>();
				decorationRT.SetParent(cameraController.decorationLayer);
				decorationRT.anchoredPosition = new Vector3(x * TileWidth, y * TileHeight, 0f);
				decorationRT.localScale = new Vector3(1f, 1f, 1f);
				
				
				// Unit Layer
				GameObject unitGO = GameObject.Instantiate(_unitPrefab);
				unitGO.name = "Unit (" + x +  ", " + y + ")";

				RectTransform unitRT = unitGO.GetComponent<RectTransform>();
				unitRT.SetParent(cameraController.unitLayer);
				unitRT.anchoredPosition = new Vector3(x * TileWidth, y * TileHeight, 0f);
				unitRT.localScale = new Vector3(1f, 1f, 1f);
				

				// Create the tile.
				Tile tile = new Tile(	
					terrainGO.GetComponent<TerrainBehaviour>(),
				 	decorationGO.GetComponent<DecorationBehaviour>(), 
					unitGO.GetComponent<UnitBehaviour>()
				);

				if(x == 0 && y == 0) {
					Debug.Log(tile.terrain);
				}

				// Set tile references
				tile.animationController = animationController;
				tile.dungeonManager = this;
				tile.spriteManager = spriteManager;

				// Serialize
				_serializedTiles[y * chunkDimension * dungeonDimension + x] = tile;
			}
		}
		Debug.Log("Dungeon build complete.");
	}
	*/


	/* 
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

					Debug.Log(tile);
					Debug.Log(tile.terrain);
					tile.terrain.biome = _biome;
					tile.decoration.biome = _biome;

					foreach(Biome b in biomes) {
						if(CheckDistance(x * TileWidth + offsetX, y * TileHeight + offsetY, b.x, b.y) < b.radius) {
							tile.terrain.biome = b;
							tile.decoration.biome = b;
						}
					}

					Color color = pixels[(chunkDimension * y + x)];

					// Parse Chunk Template
					tile.terrain.ParseColor(color);
					string hexColor = ColorUtility.ToHtmlStringRGB(color);

					if(hexColor == "FF0000") {	// Red (Enemy)
						// Spawn enemy accoring to biome type
						BaseUnit enemy = tile.terrain.biome.GetEnemySpawn();
						enemy.tile = tile;
						tile.SpawnUnit(enemy);

					} else if(hexColor == "0000FF" && i <= 1) {	// Blue (Entrance/Exit)
						
						if(i == 0) {
							// Entrance	
							tile.decoration.decorationType = DecorationBehaviour.DecorationType.entrance;

							// Spawn player at entrance
							tile.SpawnUnit(new BaseUnit(true, BaseUnit.StatPreset.Human, BaseUnit.SpritePreset.wizard, tile));
							Debug.Log("Spawned at " + tile.position);
						} else if(i == 1) {
							// Exit
							tile.decoration.decorationType = DecorationBehaviour.DecorationType.exit;
						}
						
						tile.decoration.Init(tile.terrain.walkable);
					} else if(hexColor == "FFFF00") {		// Yellow	(Containers)

						// Container decorations
						float containerDecorationRoll = Random.Range(0f, 100f);
						if(containerDecorationRoll <= containerDecorationDensity) {
							tile.decoration.decorationType = DecorationBehaviour.DecorationType.container;
							tile.decoration.Init(tile.terrain.walkable);
						}
					} else if(hexColor == "FF00FF") {		// Magenta	(Traps)

						// Trap decorations
						float containerDecorationRoll = Random.Range(0f, 100f);
						if(containerDecorationRoll <= trapDecorationDensity) {
							tile.decoration.decorationType = DecorationBehaviour.DecorationType.trap;
							tile.decoration.Init(tile.terrain.walkable);
						}
					}


					if(tile.decoration.decorationType == DecorationBehaviour.DecorationType.none) {
						// Small decorations
						float smallDecorationRoll = Random.Range(0f, 100f);
						if(smallDecorationRoll <= smallDecorationDensity) {
							tile.decoration.decorationType = DecorationBehaviour.DecorationType.small;
							tile.decoration.Init(tile.terrain.walkable);
						}
					}

				}
			}
		}


		}

		Debug.Log("Spawned " + nodes.Count + " rooms.");
	}
	*/
	void SpawnUnit(int x, int y, UnitBehaviour unit) {
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

		InitializeTiles(nodes);
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
		// Global biome
		_biome = new Biome(0, 0, 0);
		_biome.biomeType = biomeType;


		// Smaller biomes
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

