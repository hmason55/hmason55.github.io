using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BaseUnit {

	public enum StatPreset {
		Human,
		DireRat,
		DireRatSmall,
		Slime,
		Spider,
		SpiderSmall,
		Widow,
		WidowSmall
	}

	public enum SpritePreset {
		none,
		direrat,
		direratsmall,
		greenslime,
		knight,
		sandbehemoth,
		sandworm,
		spider,
		spidersmall,
		widow,
		widowsmall,
		wizard
	}

	public enum Size {
		Tiny,
		Small,
		Medium,
		Large,
		Huge,
		Gargantuan,
		Colossal
	}

	#region Stats
	int _baseStrength;
	int _baseDexterity;
	int _baseIntelligence;
	int _baseConstitution;
	int _baseWisdom;
	int _baseCharisma;
	int _baseSpeed;
	
	int _baseHitPoints;
	int _hpScaling;
	int _armorClass;
	Size _size;

	int _modStrength;
	int _modDexterity;
	int _modIntelligence;
	int _modConstitution;
	int _modWisdom;
	int _modCharisma;
	int _modSpeed;
	#endregion
	
	int _baseEssence;
	int _turnEssence;
	int _experience;
	int _currentEssence;
	int _currentHitPoints;
	int _hitPoints;
	List<Spell.Preset> _spells;
	//List<Spell.Preset> _equippedSpells;
	Turn _myTurn;
	bool _inCombat;
	int _combatAlliance;

	bool _playerControlled = false;

	Tile _tile;

	SpritePreset _spritePreset = SpritePreset.knight;

	StatPreset _statPreset = StatPreset.Human;

	string _deathParticlesPath;

	Sprite[] _idleAnimation;
	Sprite[] _hitAnimation;
	static int IdleAnimationLength = 4;
	static int HitAnimationLength = 1;
	int _animationFrame = -1;
	int _hitFrame = -1;
	int _hitFrameSkip = 8;

	Sprite _sprite;
	Sprite _shadowSprite;

	public int combatAlliance {
		set {_combatAlliance = value;}
		get {return _combatAlliance;}
	}

	public bool inCombat {
		set {_inCombat = value;}
		get {return _inCombat;}
	}

	public bool playerControlled {
		set {_playerControlled = value;}
		get {return _playerControlled;}
	}

	public Tile tile {
		set {_tile = value;}
		get {return _tile;}
	}
	
	public Sprite sprite {
		set {_sprite = value;}
		get {return _sprite;}
	}

	public Sprite shadowSprite {
		set {_shadowSprite = value;}
		get {return _shadowSprite;}
	}


	public StatPreset statPreset {
		set {_statPreset = value;}
		get {return _statPreset;}
	}

	public SpritePreset spritePreset {
		set {_spritePreset = value;}
		get {return _spritePreset;}
	}

	public string deathParticlesPath {
		set {_deathParticlesPath = value;}
		get {return _deathParticlesPath;}
	}

	public int baseEssence {
		get {return _baseEssence;}
	}
	public int turnEssence {
		get {return _turnEssence;}
	}
	public int currentEssence {
		get {return _currentEssence;}
	}

	public int armorClass {
		get {return _armorClass;}
	}

	public Size size {
		get {return _size;}
	}

	public int modStrength {
		get {return _modStrength;}
	}

	public int modDexterity {
		get {return _modDexterity;}
	}

	public int modIntelligence {
		get {return _modIntelligence;}
	}

	public int modSpeed {
		get {return _modSpeed;}
	}

	public List<Spell.Preset> spells {
		get {return _spells;}
	}

	

	public Turn myTurn {
		get {return _myTurn;}
		set {_myTurn = value;}
	}

	public Sprite[] idleAnimation {
		get {return _idleAnimation;}
		set {_idleAnimation = value;}
	}

	public Sprite[] hitAnimation {
		get {return _hitAnimation;}
		set {_hitAnimation = value;}
	}

	public int animationFrame {
		get {return _animationFrame;}
		set {_animationFrame = value;}
	}

	public int hitFrame {
		get {return _hitFrame;}
		set {_hitFrame = value;}
	}



	public BaseUnit(bool player, SpritePreset sprite) {
		_playerControlled = player;
		_spritePreset = sprite;

		switch(sprite) {
			case SpritePreset.direrat:
				_statPreset = StatPreset.DireRat;
				_deathParticlesPath = "Prefabs/Effects/Death Blood Particles";
			break;

			case SpritePreset.direratsmall:
				_statPreset = StatPreset.DireRatSmall;
				_deathParticlesPath = "Prefabs/Effects/Death Blood Particles";
			break;

			case SpritePreset.greenslime:
				_statPreset = StatPreset.Slime;
				_deathParticlesPath = "Prefabs/Effects/Death Green Particles";
			break;

			case SpritePreset.spider:
				_statPreset = StatPreset.Spider;
				_deathParticlesPath = "Prefabs/Effects/Death Blood Particles";
			break;

			case SpritePreset.spidersmall:
				_statPreset = StatPreset.SpiderSmall;
				_deathParticlesPath = "Prefabs/Effects/Death Blood Particles";
			break;

			case SpritePreset.widow:
				_statPreset = StatPreset.Widow;
				_deathParticlesPath = "Prefabs/Effects/Death Blood Particles";
			break;

			case SpritePreset.widowsmall:
				_statPreset = StatPreset.WidowSmall;
				_deathParticlesPath = "Prefabs/Effects/Death Blood Particles";
			break;

			default:
				_statPreset = StatPreset.Human;
				_deathParticlesPath = "Prefabs/Effects/Death Blood Particles";
			break;
		}

		Init();
	}

	public BaseUnit(bool player, StatPreset stats, SpritePreset sprite, Tile tile) {
		_playerControlled = player;
		_statPreset = stats;
		_spritePreset = sprite;
		_tile = tile;
		Init();
	}

	void Init() {
		EvaluateStatPreset();
		UpdateModifiers();
		UpdateArmorClass();

		// Set starting health
		_baseHitPoints = _hpScaling + _modConstitution;
		UpdateHitPoints();
		_currentHitPoints = _hitPoints;
		_experience = 0;
		

		_currentEssence = _baseEssence;
		//_myTurn = new Turn(this, _modSpeed);

		if(_playerControlled) {
			tile.combatManager.turnQueue.Add(new Turn(this, _modSpeed));
			SetAsCameraTarget();
			SetAsInterfaceTarget();
			_combatAlliance = 0;
		} else {
			_combatAlliance = 1;
		}
	}

	void EvaluateStatPreset() {
		_spells = new List<Spell.Preset>();
		_spells.Add(Spell.Preset.Move);
		_spells.Add(Spell.Preset.Bite);

		switch(_statPreset) {
			case StatPreset.Human:
				_baseStrength = 10;
				_baseDexterity = 10;
				_baseIntelligence = 10;
				_baseConstitution = 10;
				_baseWisdom = 10;
				_baseCharisma = 10;
				_baseSpeed = 3;
				_baseEssence = 4;
				_hpScaling = 8;
				_size = Size.Medium;
			break;

			case StatPreset.Slime:
				_baseStrength = 10;
				_baseDexterity = 15;
				_baseIntelligence = 0;
				_baseConstitution = 12;
				_baseWisdom = 10;
				_baseCharisma = 2;
				_baseSpeed = 5;
				_baseEssence = 4;
				_hpScaling = 16;
				_size = Size.Small;
			break;

			case StatPreset.Spider:
				_baseStrength = 11;
				_baseDexterity = 17;
				_baseIntelligence = 0;
				_baseConstitution = 12;
				_baseWisdom = 10;
				_baseCharisma = 2;
				_baseSpeed = 5;
				_baseEssence = 4;
				_hpScaling = 16;
				_size = Size.Medium;
			break;

			case StatPreset.SpiderSmall:
				_baseStrength = 10;
				_baseDexterity = 15;
				_baseIntelligence = 0;
				_baseConstitution = 12;
				_baseWisdom = 10;
				_baseCharisma = 2;
				_baseSpeed = 5;
				_baseEssence = 3;
				_hpScaling = 11;
				_size = Size.Small;
			break;

			case StatPreset.Widow:
				_baseStrength = 11;
				_baseDexterity = 17;
				_baseIntelligence = 0;
				_baseConstitution = 12;
				_baseWisdom = 10;
				_baseCharisma = 2;
				_baseSpeed = 5;
				_baseEssence = 4;
				_hpScaling = 16;
				_size = Size.Medium;
			break;

			case StatPreset.WidowSmall:
				_baseStrength = 10;
				_baseDexterity = 15;
				_baseIntelligence = 0;
				_baseConstitution = 12;
				_baseWisdom = 10;
				_baseCharisma = 2;
				_baseSpeed = 5;
				_baseEssence = 3;
				_hpScaling = 11;
				_size = Size.Small;
			break;
		}
	}

	void UpdateModifiers() {
		_modStrength = (_baseStrength - 10) / 2;
		_modDexterity = (_baseDexterity - 10) / 2;
		_modIntelligence = (_baseIntelligence - 10) / 2;
		_modConstitution = (_baseConstitution - 10) / 2;
		_modWisdom = (_baseWisdom - 10) / 2;
		_modCharisma = (_baseCharisma - 10) / 2;
		_modSpeed = (_baseDexterity - 10) / 3;
	}

	void UpdateHitPoints() {
		_hitPoints = _baseHitPoints;
	}

	void UpdateArmorClass() {
		_armorClass = 10 + _modDexterity;
	}

	public void Move(int dx, int dy) {
		if(tile.combatManager.turnQueue.Length == 0) {return;}
		if(tile.combatManager.turnQueue.queue[0].baseUnit != this) {Debug.Log("Not Your Turn!"); return;}

		int x = _tile.position.x + dx;
		int y = _tile.position.y + dy;
		int mapWidth = DungeonManager.dimension;
		int mapHeight = DungeonManager.dimension;

		if(x < 0 || x >= mapWidth || y < 0 || y >= mapHeight) {
			Debug.Log("Out of bounds " + x + ", " + y);
			return;
		}

		Tile nextTile = _tile.dungeonManager.tiles[x, y];
		if(nextTile != null) {
			if(nextTile.baseTerrain != null) {
				if(nextTile.baseTerrain.walkable && nextTile.baseUnit == null) {
					nextTile.baseUnit = this;
					_tile.baseUnit = null;
					
					if(_tile.unit != null) {
						_tile.unit.baseUnit = null;
						_tile.unit.unitImage.enabled = false;
						_tile.unit.unitImage.sprite = null;

						_tile.unit.shadowImage.enabled = false;
						_tile.unit.shadowImage.sprite = null;
						_tile.unit.UpdateSprite();
					}

					_tile = nextTile;

					// Move camera to target
					SetAsCameraTarget();
					if(_playerControlled) {
						SetAsInterfaceTarget();
					}
					tile.combatManager.CheckCombatStatus();

					if(_tile.unit != null) {
						_tile.unit.baseUnit = this;
						_tile.unit.unitImage.enabled = true;
						_tile.unit.shadowImage.enabled = true;
						_tile.unit.UpdateSprite();
						//Debug.Log(tile.position);
						//_tile.unit.image.sprite = _tile.baseUnit.sprite;
					}	
					
					
					if(_playerControlled) {
						tile.combatManager.turnQueue.EndTurn();
						tile.combatManager.turnQueue.NextTurn();
						tile.combatManager.turnQueue.Add(new Turn(this, _modSpeed));
					}


					
					Debug.Log("Moved " + dx + ", " + dy);
					
				}
			}
		}
	}

	public void Cast(int essenceCost) {
		_currentEssence -= essenceCost;
	}

	public void SetAsCameraTarget() {
		CameraController cameraController = GameObject.FindObjectOfType<CameraController>();
		cameraController.target = _tile.position;
		if(_playerControlled) {
			//if(_tile.dungeonManager.limitRendering) {
				_tile.dungeonManager.renderOrigin = _tile.position;
				_tile.dungeonManager.UpdateObjectPool();
			//}
			
			cameraController.MoveToTarget();
		}
	}

	public void SetAsInterfaceTarget() {
		Hotbar hotbar = GameObject.FindObjectOfType<Hotbar>();
		if(hotbar != null) {
			hotbar.SyncUnit(this);
		}

		TapController tapController = GameObject.FindObjectOfType<TapController>();
		if(tapController != null) {
			tapController.baseUnit = this;
		}

		HitpointUI hitpointUI = GameObject.FindObjectOfType<HitpointUI>();
		if(hitpointUI != null) {
			hitpointUI.baseUnit = this;
		}

		EssenceUI essenceUI = GameObject.FindObjectOfType<EssenceUI>();
		if(essenceUI != null) {
			essenceUI.baseUnit = this;
		}
	}

	public void ReceiveDamage(BaseUnit dealer, int damage, Spell.DamageType type) {
		Color color = Color.white;
		switch(type) {
			case Spell.DamageType.Fire:
				color = new Color(1f, 165f/255f, 0f);
			break;
		}
		_tile.baseUnit.BeginHitAnimation();
		SpawnDamageText(damage.ToString(), color);
		if(_currentHitPoints-damage > 0) {
			_currentHitPoints -= damage;
			if(_playerControlled) {
				HitpointUI hitpointUI = GameObject.FindObjectOfType<HitpointUI>();
				if(hitpointUI.baseUnit == this) {
					Debug.Log("Equal!");
					hitpointUI.UpdateHitpoints(_currentHitPoints, _hitPoints);
				}
			}
		} else {
			if(dealer != null) {
				dealer.GrantExperience(10);
			}
			Kill();
		}
	}

	public void Kill() {
		_tile.combatManager.turnQueue.RemoveTurns(this);
		_tile.baseUnit = null;
		_tile.unit.Kill();
	}

	public void GrantExperience(int exp) {
		_experience += exp;
	}

	public void SpawnDamageText(string text, Color color) {
		GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Damage Text"));
		DamageText damageText = go.GetComponent<DamageText>();
		go.GetComponent<RectTransform>().localScale = new Vector3(0.25f, 0.25f, 0.25f);
		damageText.Init(_tile.unit.GetComponent<RectTransform>().anchoredPosition, text, color);
	}

	public void LoadSprites(SpriteManager spriteManager) {
		_idleAnimation = new Sprite[IdleAnimationLength];

		switch(spritePreset) {
			case BaseUnit.SpritePreset.knight:
				//_idleAnimation = _tile.spriteManager.unit.idle.ToArray();
			break;

			case BaseUnit.SpritePreset.direrat:
				_shadowSprite = spriteManager.shadowLarge;
				_idleAnimation = spriteManager.unitDireRat1.idle.ToArray();
				_hitAnimation = spriteManager.unitDireRat1.hit.ToArray();
			break;

			case BaseUnit.SpritePreset.direratsmall:
				_shadowSprite = spriteManager.shadowSmall;
				_idleAnimation = spriteManager.unitDireRatSmall1.idle.ToArray();
				_hitAnimation = spriteManager.unitDireRatSmall1.hit.ToArray();
			break;

			case BaseUnit.SpritePreset.sandbehemoth:
				_shadowSprite = spriteManager.shadowLarge;
				_idleAnimation = spriteManager.unitSandBehemoth1.idle.ToArray();
				_hitAnimation = spriteManager.unitSandBehemoth1.hit.ToArray();
			break;

			case BaseUnit.SpritePreset.spider:
				_shadowSprite = spriteManager.shadowLarge;
				_idleAnimation = spriteManager.unitSpider1.idle.ToArray();
				_hitAnimation = spriteManager.unitSpider1.hit.ToArray();
			break;

			case BaseUnit.SpritePreset.spidersmall:
				_shadowSprite = spriteManager.shadowSmall;
				_idleAnimation = spriteManager.unitSpiderSmall1.idle.ToArray();
				_hitAnimation = spriteManager.unitSpiderSmall1.hit.ToArray();
			break;

			case BaseUnit.SpritePreset.widow:
				_shadowSprite = spriteManager.shadowLarge;
				_idleAnimation = spriteManager.unitWidow1.idle.ToArray();
				_hitAnimation = spriteManager.unitWidow1.hit.ToArray();
			break;

			case BaseUnit.SpritePreset.widowsmall:
				_shadowSprite = spriteManager.shadowSmall;
				_idleAnimation = spriteManager.unitWidowSmall1.idle.ToArray();
				_hitAnimation = spriteManager.unitWidowSmall1.hit.ToArray();
			break;

			case BaseUnit.SpritePreset.wizard:
				_shadowSprite = spriteManager.shadowMedium;
				_idleAnimation = spriteManager.unitHumanWizard1.idle.ToArray();
				_hitAnimation = spriteManager.unitHumanWizard1.hit.ToArray();
			break;

			case BaseUnit.SpritePreset.greenslime:
			default:
				_shadowSprite = spriteManager.shadowMedium;
				_idleAnimation = spriteManager.unitGreenSlime1.idle.ToArray();
				_hitAnimation = spriteManager.unitGreenSlime1.hit.ToArray();
			break;
		}
	}

	public void BeginHitAnimation() {
		_hitFrame = 0;
		
	}

	public void EndHitAnimation() {
		_hitFrame = -1;
	}

	public void IncrementAnimation() {
		_animationFrame = _tile.animationController.animationFrame;

		if(_hitAnimation != null && _hitFrame > -1 && _hitFrame < _hitFrameSkip) {
			_sprite = _hitAnimation[0];
			_hitFrame++;
		} else if(_idleAnimation != null) {
			_hitFrame = -1;

			if(_animationFrame < 10) {
				_sprite = _idleAnimation[0];
			} else if(_animationFrame < 11) {
				_sprite = _idleAnimation[1];
			} else if(_animationFrame < 22) {
				_sprite = _idleAnimation[2];
			} else {
				_sprite = _idleAnimation[3];
			}
		}
	}

	#region AI
	void BotTurn() {
		if(_myTurn == null) {
			return;
		}

		if(_playerControlled) {
			//return;
		}


	}
	#endregion

	#region Pathfinding
	public List<PathNode> FindPath(Vector2Int startPosition, Vector2Int endPosition, bool ignoreUnits = false, int excludeNodesFromEnd = 0) {
		PathNode currentNode = null;
		PathNode startNode = new PathNode(startPosition);
		PathNode endNode = new PathNode(endPosition);
		List<PathNode> openNodes = new List<PathNode>();
		List<PathNode> closedNodes = new List<PathNode>();
		bool[,] walkableTiles = GetWalkableTiles(startPosition, endPosition, ignoreUnits);
		int distanceFromStart = 0;
		openNodes.Add(startNode);

		while(openNodes.Count > 0) {
			int lowest = int.MaxValue;
			foreach(PathNode node in openNodes) {
				int distance = node.totalDistance;
				if(distance < lowest) {
					lowest = distance;
					currentNode = node;
				}
			}

			closedNodes.Add(currentNode);
			openNodes.Remove(currentNode);

			foreach(PathNode node in closedNodes) {
				if(	node.position.x == endNode.position.x &&
					node.position.y == endNode.position.y) {
					goto createPath;
				}
			}

			List<PathNode> neighborNodes = GetWalkableNeighborNodes(currentNode.position, endPosition, walkableTiles,ignoreUnits);
			distanceFromStart++;

			foreach(PathNode neighborNode in neighborNodes) {

				// If closed nodes already contain this node then skip it
				foreach(PathNode node in closedNodes) {
					if(	node.position.x == neighborNode.position.x &&
						node.position.y == neighborNode.position.y) {
						goto nextNeighbor;
					}
				}

				// Check if it's in the open list
				bool inOpenNodes = false;
				foreach(PathNode node in openNodes) {
					if(	node.position.x == neighborNode.position.x &&
						node.position.y == neighborNode.position.y) {
						inOpenNodes = true;
						break;
					}
				}

				if(inOpenNodes) {
					// Check if this is a better path
					if(distanceFromStart + neighborNode.trueDistanceFromEnd < neighborNode.totalDistance) {
						neighborNode.distanceFromStart = distanceFromStart;
						neighborNode.totalDistance = neighborNode.distanceFromStart + neighborNode.trueDistanceFromEnd;
						neighborNode.parentNode = currentNode;
					}
				} else {
					neighborNode.distanceFromStart = distanceFromStart;
					neighborNode.trueDistanceFromEnd = GetTrueNodeDistance(neighborNode, endNode);
					neighborNode.totalDistance = neighborNode.distanceFromStart + neighborNode.trueDistanceFromEnd;
					neighborNode.parentNode = currentNode;
					openNodes.Insert(0, neighborNode);
				}

				nextNeighbor:{}
			}
		}

		// Create path
		createPath:
		List<PathNode> path = new List<PathNode>();
		while(currentNode != null) {
			path.Add(currentNode);
			currentNode = currentNode.parentNode;
		}

		Debug.Log("Path");
		for(int i = 0; i < path.Count; i++) {
			Debug.Log(path[i].position);
		}

		if(excludeNodesFromEnd > 0) {
			List<PathNode>pathFinal = new List<PathNode>();
			for(int i = 0; i < path.Count; i++) {
				if(i > excludeNodesFromEnd-1) {
					pathFinal.Add(path[i]);
				}
			}
			return pathFinal;
		} else {
			return path;
		}
	}

	bool[,] GetWalkableTiles(Vector2Int start, Vector2Int end, bool ignoreUnits = false) {
		Tile[,] tiles = _tile.dungeonManager.tiles;
		bool[,] walkableTiles = new bool[DungeonManager.dimension, DungeonManager.dimension];
		for(int y = 0; y < DungeonManager.dimension; y++) {
			for(int x = 0; x < DungeonManager.dimension; x++) {
				if(tiles[x, y] == null) {
					walkableTiles[x, y] = false;
					continue;
				}
				
				if(tiles[x, y].baseTerrain == null) {
					walkableTiles[x, y] = false;
					continue;
				}

				if(!tiles[x, y].baseTerrain.walkable) {
					walkableTiles[x, y] = false;
					continue;
				}

				// End is always valid
				if(	x == end.x && y == end.y) {
					walkableTiles[x, y] = true;
               		continue;	
				}

				if(ignoreUnits) {
					walkableTiles[x, y] = true;
					continue;	
				} else {
					if(tiles[x, y].baseUnit == null) {
						walkableTiles[x, y] = true;
						continue;	
					}
				}

				// Start is always valid
				if(	x == start.x && y == start.y) {
					walkableTiles[x, y] = true;
               		continue;	
				}

			}
		}
		return walkableTiles;
	}

	List<PathNode> GetWalkableNeighborNodes(Vector2Int position, Vector2Int end, bool[,] walkableTiles, bool ignoreUnits = false) {
		List<PathNode> testNodes = new List<PathNode>();
		if(position.y > 0) {
			testNodes.Add(new PathNode(new Vector2Int(position.x, position.y-1)));
		}

		if(position.y < DungeonManager.dimension-1) {
			testNodes.Add(new PathNode(new Vector2Int(position.x, position.y+1)));
		}

		if(position.x > 0) {
			testNodes.Add(new PathNode(new Vector2Int(position.x-1, position.y)));
		}

		if(position.x < DungeonManager.dimension-1) {
			testNodes.Add(new PathNode(new Vector2Int(position.x+1, position.y)));
		}

		Tile[,] tiles = _tile.dungeonManager.tiles;

		// Validate test nodes
		for(int i = testNodes.Count-1; i >= 0; i--) {
			int x = testNodes[i].position.x;
			int y = testNodes[i].position.y;

			if(	x == end.x && y == end.y) {
				continue;
			}

			if(!walkableTiles[x, y]) {
				testNodes.RemoveAt(i);
				continue;
			}

			if(!ignoreUnits) {
				if(tiles[x, y].baseUnit != null) {
					testNodes.RemoveAt(i);
					continue;
				}
			}
		}
		return testNodes;
	}

	int GetTrueNodeDistance(PathNode a, PathNode b) {
		return Mathf.Abs(b.position.x - a.position.x) + Mathf.Abs(b.position.y - a.position.y);
	}
	#endregion
}
