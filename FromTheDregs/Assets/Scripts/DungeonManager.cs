using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;	
using UnityEngine.UI;
using UnityEngine;



public class DungeonManager : MonoBehaviour {

	public enum LoadState {
		Unloaded,
		Unloading,
		Loading,
		Loaded
	}

	LoadState _loadState = LoadState.Unloaded;

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

	Vector2Int entrancePosition = new Vector2Int(-1, -1);
	public Vector2Int exitPosition = new Vector2Int(-1, -1);
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
		_loadState = LoadState.Unloaded;
	}

	void Start () {
		Load();
	}

	void Load() {
		_loadState = LoadState.Loading;
		InitializeGrid();
		InitializeObjectPools();
		SpawnBiomes();
		CreateMainPath();
		_limitRendering = true;
		_loadState = LoadState.Loaded;
		Debug.Log(Time.realtimeSinceStartup);
	}

	void Unload() {
		_loadState = LoadState.Unloading;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Space)) {
			if(_loadState == LoadState.Loaded) {
				Unload();
			}
		}
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
					tile.baseTerrain = new BaseTerrain(nearestBiome, hexColor);

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
								BaseUnit player = new BaseUnit(true, BaseUnit.StatPreset.Human, BaseUnit.SpritePreset.wizard, tile);
								player.tile = tile;
								tile.SpawnUnit(player);
								combatManager.BeginTurnLoop();
								_renderOrigin = tile.position;
								entrancePosition = tile.position;
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

		ExportMap();

		// Load textures
		for(int y = 0; y < dimension; y++) {
			for(int x = 0; x < dimension; x++) {
				if(_tiles[x, y] != null) {
					if(_tiles[x, y].baseTerrain != null) {
						_tiles[x, y].baseTerrain.Initialize(spriteManager, this, x, y);
					}
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
		int _terrainIndex = 0;
		int _decorationIndex = 0;
		int _unitIndex = 0;
		int offsetX = 0;
		int offsetY = 0;

		for(int y = 0; y < dimension; y++) {
			for(int x = 0; x < dimension; x++) {
				if(_tiles[x, y] != null) {
					if(	x <  _renderOrigin.x + renderDistance &&
						x >  _renderOrigin.x - renderDistance &&
						y <  _renderOrigin.y + renderDistance &&
						y >  _renderOrigin.y - renderDistance) {

						if(offsetX == 0) {
							offsetX = x;
						}

						if(offsetY == 0) {
							offsetY = y;
						}
						
						if(_terrainPool[x, y] == null) {
							if(_terrainStack.Count > _terrainIndex) {
								if(_tiles[x, y].baseTerrain != null) {
									_terrainPool[x, y] = _terrainStack[_terrainIndex++];
									_terrainPool[x, y].Transfer(_tiles[x, y], _tiles[x, y].baseTerrain);
									_terrainPool[x, y].gameObject.name = "Terrain (" + x + ", " + y + ")"; 
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

		SortTerrainObjects();
	}

	void SortTerrainObjects() {
		
		List<TerrainBehaviour> terrainObjects = new List<TerrainBehaviour>();

		// Get a list of valid terrain objects
		for(int i = 0; i < _terrainLayer.transform.childCount; i++) {
			TerrainBehaviour tb = _terrainLayer.transform.GetChild(i).GetComponent<TerrainBehaviour>();
			if(tb != null) {
				if(tb.tile != null) {
					terrainObjects.Add(tb);
				} else {
					tb.transform.SetAsFirstSibling();
				}
			}
		}

		// Sort terrain by depth (ascending insertion sort)
		if(terrainObjects.Count >= 2) {
			int i = 1;
			while(i < terrainObjects.Count) {
				int j = i;
				
				while(j > 0 && (dimension * terrainObjects[j - 1].tile.position.y + terrainObjects[j - 1].tile.position.x) > (dimension * terrainObjects[j].tile.position.y + terrainObjects[j].tile.position.x)) {
					int siblingNdx = terrainObjects[j - 1].transform.GetSiblingIndex();
					TerrainBehaviour tempBehaviour = terrainObjects[j - 1];

					// Swap transform depth
					terrainObjects[j - 1].transform.SetSiblingIndex(terrainObjects[j].transform.GetSiblingIndex());
					terrainObjects[j].transform.SetSiblingIndex(siblingNdx);
					
					// Swap behaviour order
					terrainObjects[j - 1] = terrainObjects[j];
					terrainObjects[j] = tempBehaviour;

					j--;
				}
				i++;
			}
		}
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
		biomes = new List<Biome>();

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

	void ExportMap() {
		Texture2D texture = new Texture2D(dimension, dimension, TextureFormat.RGB24, false);

		for(int y = 0; y < dungeonDimension; y++) {
			for(int x = 0; x < dungeonDimension; x++) {

				bool hasTexture = true;
				Chunk chunk = _chunks[x, y];
				Sprite src = null;


				if(chunk != null) {
					src = chunk.template;
				}

				if(src == null) {
					hasTexture = false;
				}
				

				for(int j = 0; j < chunkDimension; j++) {
					for(int i = 0; i < chunkDimension; i++) {
						
						Color color = Color.black;
						if(hasTexture) {

							color = src.texture.GetPixel(i, j);
							if(	color.r == 0f &&
								color.g == 0f &&
								color.b == 1f) {

								if(x == entrancePosition.x/chunkDimension && y == entrancePosition.y/chunkDimension ) {
									// do nothing
								} else if(x == exitPosition.x/chunkDimension  && y == exitPosition.y/chunkDimension ) {
									// do nothing
								} else {
									color = Color.green;
								}
							}
						}

						texture.SetPixel(x*chunkDimension + i, y*chunkDimension + j, color);
					}
				}

			}	
		}

		byte[] bytes = texture.EncodeToPNG();
		string dirPath = Application.dataPath + "/../map/";

		if(!Directory.Exists(dirPath)) {
			Directory.CreateDirectory(dirPath);
		}

		File.WriteAllBytes(dirPath + "recent" + ".png", bytes);
	}
}

