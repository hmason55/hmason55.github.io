using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BaseUnit {

	public enum Preset {
		Giant_Spider,
		Giant_Widow,
		Green_Slime,
		Mage,
		Rogue,
		Skeleton,
		Skeleton_Summoner,
		Skeleton_Thrall,
		Skeleton_Warrior,
		Spiderling,
		Warrior,
		Widowling,

	}

	#region Stats

	int _baseSpeed;
	
	int _baseHitPoints;

	int _modSpeed;
	#endregion
	
	int _aggroRadius;
	int _baseEssence;
	int _turnEssence;
	int _experience;
	int _currentEssence;
	int _currentHitPoints;
	int _hitPoints;
	List<Spell.Preset> _spells;
	
	List<Spell.Preset> _innateSpells;
	//List<Spell.Preset> _equippedSpells;
	Turn _myTurn;
	bool _inCombat;
	bool _isCasting;
	
	Attributes _attributes;
	Character _character;
	LootTable _lootTable;
	bool _playerControlled = false;
	Bag _bag;
	List<Effect> _effects;

	Tile _tile;
	Preset _preset;

	Attributes.Preset _statPreset = Attributes.Preset.Human;

	string _deathParticlesPath;
	bool _useCustomSprites;
	
	Sprite[] _idleAnimation;
	Sprite[] _idleSkinAnimation;
	Sprite[] _idleArmorAnimation;
	Sprite[] _idleSecondaryAnimation;
	Sprite[] _idlePrimaryAnimation;
	Sprite[] _hitAnimation;
	static int IdleAnimationLength = 4;
	static int HitAnimationLength = 1;
	int _animationFrame = -1;
	int _hitFrame = -1;
	int _hitFrameSkip = 8;

	Moveset _moveset;

	Sprite _sprite;
	Sprite _spriteSkin;
	Sprite _spriteArmor;
	Sprite _spriteSecondary;
	Sprite _spritePrimary;
	
	Sprite _shadowSprite;

	Spell _intentSpell;

	bool _tickedStatuses;

	int _spellCharges = 1;

	#region Accessors

	public bool inCombat {
		set {_inCombat = value;}
		get {return _inCombat;}
	}

	public bool isCasting {
		get {return _isCasting;}
		set {_isCasting = value;}
	}

	public bool playerControlled {
		set {_playerControlled = value;}
		get {return _playerControlled;}
	}

	public bool tickedStatuses {
		get {return _tickedStatuses;}
		set {_tickedStatuses = value;}
	}

	public Spell intentSpell {
		get {return _intentSpell;}
		set {_intentSpell = value;}
	}

	public Character character {
		set {_character = value;}
		get {return _character;}
	}

	public Tile tile {
		set {_tile = value;}
		get {return _tile;}
	}
	
	public Sprite sprite {
		set {_sprite = value;}
		get {return _sprite;}
	}

	public Sprite spriteSkin {
		set {_spriteSkin = value;}
		get {return _spriteSkin;}
	}

	public Sprite spriteArmor {
		set {_spriteArmor = value;}
		get {return _spriteArmor;}
	}

	public Sprite spriteSecondary {
		set {_spriteSecondary = value;}
		get {return _spriteSecondary;}
	}

	public Sprite spritePrimary {
		set {_spritePrimary = value;}
		get {return _spritePrimary;}
	}

	public Sprite shadowSprite {
		set {_shadowSprite = value;}
		get {return _shadowSprite;}
	}

	public Attributes attributes {
		set {_attributes = value;}
		get {return _attributes;}
	}

	public Preset preset {
		get {return _preset;}
	}

	public string deathParticlesPath {
		set {_deathParticlesPath = value;}
		get {return _deathParticlesPath;}
	}

	public List<Spell.Preset> spells {
		get {return _spells;}
	}

	public Turn myTurn {
		get {return _myTurn;}
		set {_myTurn = value;}
	}

	public Bag bag {
		get {return _bag;}
		set {_bag = value;}
	}

	public List<Effect> effects {
		get {return _effects;}
	}
	public bool useCustomSprites {
		get {return _useCustomSprites;}
		set {_useCustomSprites = value;}
	}

	public Sprite[] idleAnimation {
		get {return _idleAnimation;}
		set {_idleAnimation = value;}
	}

	public Sprite[] idleSkinAnimation {
		get {return _idleSkinAnimation;}
		set {_idleSkinAnimation = value;}
	}

	public Sprite[] idleArmorAnimation {
		get {return _idleArmorAnimation;}
		set {_idleArmorAnimation = value;}
	}

	public Sprite[] idleSecondaryAnimation {
		get {return _idleSecondaryAnimation;}
		set {_idleSecondaryAnimation = value;}
	}

	public Sprite[] idlePrimaryAnimation {
		get {return _idlePrimaryAnimation;}
		set {_idlePrimaryAnimation = value;}
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

	public int spellCharges {
		get {return _spellCharges;}
		set {_spellCharges = value;}
	}

	public LootTable lootTable {
		get {return _lootTable;}
	}

	#endregion

	// Used for preset units.
	public BaseUnit(bool player, Preset preset) {
		_playerControlled = player;
		_preset = preset;
		_effects = new List<Effect>();
		_innateSpells = new List<Spell.Preset>();

		switch(_preset) {
			
			case Preset.Giant_Spider:
				_statPreset = Attributes.Preset.Giant_Spider;
				_innateSpells.Add(Spell.Preset.Bite);
				_innateSpells.Add(Spell.Preset.Block);
				_moveset = new Moveset(new int[,] {
				// Index, Charges
					{0, 1},
					{0, 2},
					{1, 2},
					{0, 3},
					{1, 1},
				}, Random.Range(0, 1));
			break;

			case Preset.Giant_Widow:
				_statPreset = Attributes.Preset.Giant_Widow;
				_innateSpells.Add(Spell.Preset.Bite);
				_innateSpells.Add(Spell.Preset.Poison_Fang);
				_innateSpells.Add(Spell.Preset.Block);
				_moveset = new Moveset(new int[,] {
				// Index, Charges
					{0, 3},
					{1, 1},
					{0, 2},
					{2, 2},
					{1, 2},
				}, Random.Range(0, 1));
			break;

			case Preset.Green_Slime:
			break;

			case Preset.Skeleton:
				_statPreset = Attributes.Preset.Skeleton;
				_innateSpells.Add(Spell.Preset.Skeletal_Claws);
				_innateSpells.Add(Spell.Preset.Block);
				_moveset = new Moveset(new int[,] {
				// Index, Charges
					{0, 1},
					{1, 2},
					{0, 2},
					{1, 2},
					{0, 3},
				}, Random.Range(0, 1));
			break;

			case Preset.Skeleton_Summoner:
			break;

			case Preset.Skeleton_Thrall:
				_statPreset = Attributes.Preset.Skeleton_Thrall;
				_innateSpells.Add(Spell.Preset.Skeletal_Claws);
				_moveset = new Moveset();
			break;

			case Preset.Skeleton_Warrior:
				_statPreset = Attributes.Preset.Skeleton_Warrior;
				_innateSpells.Add(Spell.Preset.Slash);
				_innateSpells.Add(Spell.Preset.Slash);
				_innateSpells.Add(Spell.Preset.Severing_Strike);
				_innateSpells.Add(Spell.Preset.Block);
				_moveset = new Moveset();
			break;

			case Preset.Spiderling:
				_statPreset = Attributes.Preset.Spiderling;
				_innateSpells.Add(Spell.Preset.Bite);
				_innateSpells.Add(Spell.Preset.Block);
				_moveset = new Moveset(new int[,] {
				// Index, Charges
					{0, 2},
					{1, 1},
					{0, 1},
					{1, 1},
					{0, 3},
				}, Random.Range(0, 1));
			break;

			case Preset.Widowling:
				_statPreset = Attributes.Preset.Widowling;
				_innateSpells.Add(Spell.Preset.Bite);
				_innateSpells.Add(Spell.Preset.Poison_Fang);
				_innateSpells.Add(Spell.Preset.Block);
				_moveset = new Moveset(new int[,] {
				// Index, Charges
					{0, 1},
					{1, 1},
					{0, 2},
					{1, 1},
					{2, 1},
				}, Random.Range(0, 2));
			break;
		}

		_lootTable = new LootTable(_preset);
		_attributes = new Attributes(_statPreset);
		Init();
		LoadFallbackSprite();
	}

	// Used for custom units.
	public BaseUnit(bool player, Attributes.Preset attribs, Preset preset, Tile tile, bool customSprite = false, bool preview = false) {
		_playerControlled = player;
		_attributes = new Attributes(attribs);
		_preset = preset;
		_effects = new List<Effect>();
		_innateSpells = new List<Spell.Preset>();
		_tile = tile;
		_useCustomSprites = customSprite;

		if(preview) {return;}
		Init();
	}

	void Init() {

		SetDeathEffect();

		// Equip items
		_bag = new Bag(Bag.BagType.Bag);
		
		if(_playerControlled) {
			
			_preset = PlayerData.current.unitPreset;
			if(PlayerData.current.character != null) {
				_character = PlayerData.current.character;
			}

			if(PlayerData.current.attributes != null) {
				_attributes = PlayerData.current.attributes;
			}

			if(PlayerData.current.bag != null) {
				_bag = PlayerData.current.bag;
			}
			//_bag.Add(new BaseItem(BaseItem.ID.Gold, 48));
			//_bag.Add(new BaseItem(BaseItem.ID.Cotton_Hood));
			//_bag.Add(new BaseItem(BaseItem.ID.Cotton_Tunic));
			/*
			_bag.Add(new BaseItem(BaseItem.Category.Head_Armor, BaseItem.ArmorWeight.Heavy, 2));
			_bag.Add(new BaseItem(BaseItem.Category.Body_Armor, BaseItem.ArmorWeight.Heavy, 2));
			_bag.Add(new BaseItem(BaseItem.Category.Hand_Armor, BaseItem.ArmorWeight.Heavy, 2));
			_bag.Add(new BaseItem(BaseItem.Category.Leg_Armor, BaseItem.ArmorWeight.Heavy, 2));
			_bag.Add(new BaseItem(BaseItem.Category.Foot_Armor, BaseItem.ArmorWeight.Heavy, 2));

			_bag.Add(new BaseItem(BaseItem.Category.Head_Armor, BaseItem.ArmorWeight.Light, 3));
			_bag.Add(new BaseItem(BaseItem.Category.Body_Armor, BaseItem.ArmorWeight.Light, 3));
			_bag.Add(new BaseItem(BaseItem.Category.Hand_Armor, BaseItem.ArmorWeight.Light, 3));
			_bag.Add(new BaseItem(BaseItem.Category.Leg_Armor, BaseItem.ArmorWeight.Light, 3));
			_bag.Add(new BaseItem(BaseItem.Category.Foot_Armor, BaseItem.ArmorWeight.Light, 3));

			_bag.Add(new BaseItem(BaseItem.Category.Neck_Jewelry, 1));
			_bag.Add(new BaseItem(BaseItem.Category.Neck_Jewelry, 2));
			_bag.Add(new BaseItem(BaseItem.Category.Neck_Jewelry, 3));

			_bag.Add(new BaseItem(BaseItem.Category.Finger_Jewelry, 1));
			_bag.Add(new BaseItem(BaseItem.Category.Finger_Jewelry, 2));
			_bag.Add(new BaseItem(BaseItem.Category.Finger_Jewelry, 3));
			*/

			//tile.combatManager.turnQueue.Add(new Turn(this, _modSpeed));
			UpdateSpells();
			SetAsCameraTarget();
			SetAsInterfaceTarget();
			_attributes.alliance = 0;
		} else {
			UpdateSpells();
			_attributes.alliance = 1;
		}

		_effects.Add(new Effect(Effect.EffectType.DisplayHealth, -1));
		UpdateDisplayHealth();
	}

	void SetDeathEffect() {
		switch(_attributes.preset) {
			case Attributes.Preset.Green_Slime:
				_deathParticlesPath = "Prefabs/Effects/Death Green Particles";
			break;

			default:
				_deathParticlesPath = "Prefabs/Effects/Death Blood Particles";
			break;
		}

		switch(_preset) {
			case Preset.Skeleton:
			case Preset.Skeleton_Summoner:
			case Preset.Skeleton_Thrall:
			case Preset.Skeleton_Warrior:
				_deathParticlesPath = "Prefabs/Effects/Bone Death Particles";
			break;
		}
	}

	public void UpdateSpells() {
		_spells = new List<Spell.Preset>();

		// Add innate spells.
		foreach(Spell.Preset spell in _innateSpells) {
			_spells.Add(spell);
		}

		// Add spells from equipment.
		if(_bag != null) {
			if(_bag.primary != null) {
				foreach(Spell.Preset spell in _bag.primary.spells) {
					_spells.Add(spell);
				}
			}

			if(_bag.secondary != null) {
				foreach(Spell.Preset spell in _bag.secondary.spells) {
					_spells.Add(spell);
				}
			}
		}
	}

	public Spell SelectAISpell() {
		UpdateSpells();

		int spellIndex = _moveset.pattern[_moveset.index, 0];
		if(spellIndex == -1 || spellIndex > spells.Count) {
			spellIndex = Random.Range(0, spells.Count-1);
		}

		Spell s = new Spell(this, spells[spellIndex]);
		_spellCharges = _moveset.pattern[_moveset.index, 1];

		_moveset.Next();
		return s;
	}

	public void BeginTurn() {
		
		_tickedStatuses = false;
		if(_tile.unit != null) {
			_tile.unit.TickStatus(Effect.Conditions.DurationExpire);
		} else {
			TickStatusImmediate(Effect.Conditions.DurationExpire);
		}
		
		_attributes.currentEssence += _attributes.recoveryEssence;
		if(_attributes.currentEssence > _attributes.totalEssence) {
			_attributes.currentEssence = _attributes.totalEssence;
		}
	}

	public bool Move(int dx, int dy) {
		if(tile.combatManager.turnQueue.Length == 0) {return false;}
		if(tile.combatManager.turnQueue.queue[0].baseUnit != this) {Debug.Log("Not Your Turn!"); return false;}

		int x = _tile.position.x + dx;
		int y = _tile.position.y + dy;
		int mapWidth = DungeonManager.dimension;
		int mapHeight = DungeonManager.dimension;

		if(x < 0 || x >= mapWidth || y < 0 || y >= mapHeight) {
			Debug.LogWarning("Out of bounds " + x + ", " + y);
			return false;
		}

		Tile nextTile = _tile.dungeonManager.tiles[x, y];
		if(nextTile != null) {
			if(nextTile.baseTerrain != null) {
				if(nextTile.baseTerrain.walkable && nextTile.baseUnit == null) {
					if(_inCombat) {
						Cast(1);
					}
					Vector2Int prevPosition = _tile.position;

					nextTile.baseUnit = this;
					_tile.baseUnit = null;
					
					if(_tile.unit != null) {
						_tile.unit.baseUnit = null;
						_tile.unit.Unset();
						//_tile.unit.UpdateSprite();
					}

					_tile = nextTile;

					// Move camera to target
					SetAsCameraTarget();
					if(_playerControlled) {
						SetAsInterfaceTarget();
					}
					tile.combatManager.CheckCombatStatus(tile.combatManager.GetAllBaseUnits());

					if(_tile.unit != null) {
						_tile.unit.baseUnit = this;
						//_tile.unit.unitImage.enabled = true;
						//_tile.unit.shadowImage.enabled = true;
						_tile.unit.ShowUnit();
						_tile.unit.UpdateSprite();
						//_tile.unit.GetComponent<RectTransform>().localPosition = new Vector3(prevPosition.x, prevPosition.y, 0f) * DungeonManager.dimension;
						_tile.unit.MoveFromTo(prevPosition, _tile.position, 0.5f);
						//Debug.Log(tile.position);
						//_tile.unit.image.sprite = _tile.baseUnit.sprite;
					}	
					
					if(_playerControlled && !_inCombat) {
						//tile.combatManager.turnQueue.EndTurn();
						//tile.combatManager.turnQueue.NextTurn();
						//tile.combatManager.turnQueue.Add(new Turn(this, _modSpeed));
					}
					
					//Debug.Log("Moved " + dx + ", " + dy);
					return true;
				}
			}
		}

		return false;
	}

	public void Cast(int essenceCost) {
		if(_inCombat) {
			_attributes.currentEssence -= essenceCost;
		}
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
		SyncHotbar();

		BagBehaviour bagBehaviour = GameObject.FindObjectOfType<BagBehaviour>();
		if(bagBehaviour != null) {
			bagBehaviour.baseUnit = this;
		}

		AttributesUI attributesUI = GameObject.FindObjectOfType<AttributesUI>();
		if(attributesUI != null) {
			attributesUI.baseUnit = this;
		}

		HitpointUI hitpointUI = GameObject.FindObjectOfType<HitpointUI>();
		if(hitpointUI != null) {
			hitpointUI.baseUnit = this;
			hitpointUI.UpdateHitpoints(_attributes.currentHealth, _attributes.totalHealth);
		}

		EssenceUI essenceUI = GameObject.FindObjectOfType<EssenceUI>();
		if(essenceUI != null) {
			essenceUI.baseUnit = this;
		}
	}

	public void SyncHotbar() {
		Hotbar hotbar = GameObject.FindObjectOfType<Hotbar>();
		if(hotbar != null) {
			hotbar.SyncUnit(this);
			hotbar.UpdateHotkeys();
		}
	}

	public bool ReceiveDamage(BaseUnit dealer, int damage, Spell.DamageType type) {
		Color color = Color.white;
		switch(type) {
			case Spell.DamageType.Bleed:
				color = new Color(192f/255f, 32f/255f, 0f);
			break;

			case Spell.DamageType.Fire:
				color = new Color(1f, 165f/255f, 0f);
			break;

			case Spell.DamageType.Poison:
				color = new Color(64f/255f, 192f/255f, 0f);
			break;
		}

		_tile.baseUnit.BeginHitAnimation();
		SpawnDamageText(damage.ToString(), color);

		_attributes.currentHealth -= damage;
		if(_playerControlled) {
			HitpointUI hitpointUI = GameObject.FindObjectOfType<HitpointUI>();
			if(hitpointUI.baseUnit == this) {
				hitpointUI.UpdateHitpoints(_attributes.currentHealth, _attributes.totalHealth);
			}
		}

		UpdateDisplayHealth();

		if(_attributes.currentHealth > 0) {
			
			// Aggro this unit if it's not in combat
			if(dealer != null) {
				CombatManager cm = _tile.combatManager;

				if(_attributes.alliance != dealer.attributes.alliance) {
					_inCombat = true;
					if(!cm.turnQueue.UnitInQueue(this)) {
						cm.turnQueue.Add(new Turn(this, _attributes.speed));
					}

					dealer.inCombat = true;
					if(!cm.turnQueue.UnitInQueue(dealer)) {
						cm.turnQueue.Add(new Turn(dealer, dealer.attributes.speed));
					}

					if(!cm.inCombat) {
						cm.BeginCombat();
						cm.CheckCombatStatus(new List<BaseUnit> {this, dealer});
					}
				}
			}

		} else {
			
			if(dealer != null) {
				dealer.GrantExperience(10);
			}
			Kill();
		}

		if(damage > 0) {
			return true;
		} else {
			return false;
		}
	}

	public bool HasStatusType(Effect.EffectType effectType) {
		foreach(Effect effect in _effects) {
			if(effect.effectType == effectType) {
				return true;
			}
		}
		return false;
	}

	public void ReceiveStatus(Spell spell, Effect effect) {
		
		Color color = Color.white;
		string text = "";

		switch(effect.effectType) {
			
			case Effect.EffectType.Bleed:
				color = new Color(192f/255f, 32f/255f, 0f);

				
				if(spell.caster.bag != null) {
					effect.initialBleedModifier = spell.caster.bag.GetEquipmentBonus(Bag.EquipmentBonus.BleedModifier);
				}

				int bleedValue = effect.initialBleedModifier + effect.deactivationConditions[Effect.Conditions.DurationExpire];
				// No scaling
				//bleedValue += (int)e.GetPotency(spell.caster.attributes);
			
				text = "+" + bleedValue;
			break;

			case Effect.EffectType.Block:
				color = new Color(0f, 0.5f, 1f);

				int blockValue = 0;
				if(spell.caster.bag != null) {
					blockValue = spell.caster.bag.GetEquipmentBonus(Bag.EquipmentBonus.BlockModifier);
					effect.initialBlockModifier = blockValue;
				}

				blockValue += (int)effect.GetPotency(spell.caster.attributes);
			
				text = "+" + blockValue;
			break;

			case Effect.EffectType.Focus:
				color = new Color(0f, 1f, 1f);
				text = "+Focus";
			break;

			case Effect.EffectType.Poison:
				color = new Color(64f/255f, 192f/255f, 0f);

				
				if(spell.caster.bag != null) {
					effect.initialPoisonModifier = spell.caster.bag.GetEquipmentBonus(Bag.EquipmentBonus.PoisonModifier);
				}

				int poisonValue = effect.initialBleedModifier + effect.deactivationConditions[Effect.Conditions.DurationExpire];
				// No scaling
				//bleedValue += (int)e.GetPotency(spell.caster.attributes);
			
				text = "+" + poisonValue;
			break;

			case Effect.EffectType.Stun:
				color = new Color(1f, 1f, 0f);
				text = "Stunned";
			break;
		}

		if(!effect.Apply(tile.unit.baseUnit)) {return;}

		SpawnDamageText(text.ToString(), color);
	}


	public void TickStatusImmediate(Effect.Conditions condition, int amount = 1) {
		for(int i = _effects.Count-1; i >= 0; i--) {
			TickStatusSingle(_effects[i], condition, amount);
		}
	}

	public float TickStatusSingle(Effect effect, Effect.Conditions condition, int amount = 1) {
		float duration = 0f;
		if(effect.deactivationConditions.ContainsKey(condition)) {
			switch(effect.effectType) {
				case Effect.EffectType.Bleed:
					if(ReceiveDamage(this, effect.deactivationConditions[Effect.Conditions.DurationExpire], Spell.DamageType.Bleed)) {
						if(_tile.unit != null) {
							Spell bleed = new Spell(this, Spell.Preset.Bleed);
							bleed.effectOrigin = _tile.position;
							_tile.unit.SpawnSpellEffect(bleed);
							duration = 1.5f;
						}
					}
				break;

				case Effect.EffectType.Poison:
					if(ReceiveDamage(this, effect.deactivationConditions[Effect.Conditions.DurationExpire], Spell.DamageType.Poison)) {
						if(_tile.unit != null) {
							Spell poison = new Spell(this, Spell.Preset.Poison);
							poison.effectOrigin = _tile.position;
							_tile.unit.SpawnSpellEffect(poison);
							duration = 1.5f;
						}
					}
				break;
			}
			
			effect.deactivationConditions[condition] -= amount;
			if(effect.deactivationConditions[condition] <= 0) {
				RemoveStatus(effect);
			}
		}
		return duration;
	}

	public void RemoveStatusByCondition(Effect.EffectType effectType, Effect.Conditions condition, int amount = 1) {
		for(int i = _effects.Count-1; i >= 0; i--) {
			if(_effects[i].effectType == effectType) {
				if(_effects[i].deactivationConditions.ContainsKey(condition)) {
					_effects[i].deactivationConditions[condition] -= amount;
				}

				if(_effects[i].deactivationConditions[condition] <= 0) {
					RemoveStatus(_effects[i]);
				}
			}
		}
	}

	public void RemoveStatus(Effect effect) {
		for(int i = _effects.Count-1; i >= 0; i--) {
			if(effect.Equals(_effects[i])) {
				_effects.RemoveAt(i);
			}
		}

		Color color = Color.white;
		string text = "";
		switch(effect.effectType) {
			case Effect.EffectType.Block:
				//color = new Color(0f, 1f, 1f);
				//text = "-Block";
			break;

			case Effect.EffectType.Focus:
				color = new Color(0f, 1f, 1f);
				text = "-Focus";
			break;
		}

		SpawnDamageText(text.ToString(), color);
	}

	void UpdateDisplayHealth() {
		foreach(Effect e in _effects) {
			if(e.effectType == Effect.EffectType.DisplayHealth) {
				e.currentHealth = _attributes.currentHealth;
				e.totalHealth = _attributes.totalHealth;
			}
		}
	}

	public void Kill() {
		// Save bag and death location here.
		if(_playerControlled) {
			_attributes.currentHealth = _attributes.totalHealth;

			PlayerData.current.retrievalZone = PlayerData.current.currentZone;
			PlayerData.current.retrievalBag = new Bag(Bag.BagType.Container);

			// Set target zone to hub.
			PlayerData.current.currentZone = DungeonManager.Zone.Hub;
			PlayerData.current.targetZone = DungeonManager.Zone.Hub;

			// Move currency and trophies to retrieval bag.
			for(int i = _bag.items.Count-1; i >= 0; i--) {
				if(_bag.items[i] != null) {
					if(	_bag.items[i].category == BaseItem.Category.Currency || 
						_bag.items[i].category == BaseItem.Category.Trophy) {

						BaseItem item = _bag.items[i];
						PlayerData.current.retrievalBag.Add(item);
						_bag.RemoveAt(i);
					}
				}
			}

			PlayerData.current.bag = _bag;
			PlayerData.current.character = _character;

			// Restore health and essence.
			_attributes.currentHealth = _attributes.totalHealth;
			_attributes.currentEssence = _attributes.totalEssence;

			PlayerData.current.attributes = _attributes;
			PlayerData.current.retrievalMode = true;
			SaveLoadData.Save();
		}
		
		_tile.combatManager.turnQueue.RemoveTurns(this);
		_tile.unit.Kill();
	}

	public void Return() {
		// Restore health and essence.
		_attributes.currentHealth = _attributes.totalHealth;
		_attributes.currentEssence = _attributes.totalEssence;

		// Set target zone to hub.
		PlayerData.current.currentZone = DungeonManager.Zone.Hub;
		PlayerData.current.targetZone = DungeonManager.Zone.Hub;

		// Save data
		PlayerData.current.bag = _bag;
		PlayerData.current.character = _character;
		PlayerData.current.attributes = _attributes;
		SaveLoadData.Save();

		_tile.unit.Return();
	}

	public void GrantExperience(int exp) {
		_experience += exp;
	}

	public void SpawnDamageText(string text, Color color) {
		GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Damage Text"));
		DamageText damageText = go.GetComponent<DamageText>();
		//go.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
		if(_tile.unit != null) {
			damageText.Init(_tile.unit.GetComponent<RectTransform>().anchoredPosition, text, color);
			//damageText.ResetScale();
		}
	}

	public void LoadSprites(SpriteManager spriteManager) {
		if(_useCustomSprites) {
			LoadCustomSprite(spriteManager);
		} else {
			LoadFallbackSprite();
		}
	}

	void LoadCustomSprite(SpriteManager spriteManager) {
		_idleAnimation = new Sprite[IdleAnimationLength];
		
		switch(_preset) {
			default:
				_shadowSprite = spriteManager.shadowMedium;
			break;
		}

		LoadCustomPrimary(spriteManager);
		LoadCustomSecondary(spriteManager);
		LoadCustomArmor(spriteManager);
	}

	public void LoadCustomPrimary(SpriteManager spriteManager) {
		_idlePrimaryAnimation = new Sprite[IdleAnimationLength];

		if(_bag != null) {
			// Primary Weapon
			if(_bag.primary != null) {
				switch(_bag.primary.id) {
					case BaseItem.ID.Gladius:
						_idlePrimaryAnimation = spriteManager.unitWeapons.gladius.ToArray();
					break;

					case BaseItem.ID.Staff_of_Flame:
						_idlePrimaryAnimation = spriteManager.unitWeapons.staff.ToArray();
					break;

				}
			}
		}

		// Fallback to empty sprite sheet.
		if(_idlePrimaryAnimation[0] == null) {
			for(int i = 0; i < IdleAnimationLength; i++) {
				_idlePrimaryAnimation[i] = spriteManager.items.unknown;
			}
		}
	}

	public void LoadCustomSecondary(SpriteManager spriteManager) {
		_idleSecondaryAnimation = new Sprite[IdleAnimationLength];
		
		if(_bag != null) {
			// Secondary Weapon
			if(_bag.secondary != null) {
				switch(_bag.secondary.id) {
					case BaseItem.ID.Dagger:
						_idleSecondaryAnimation = spriteManager.unitWeapons.dagger.ToArray();
					break;

					case BaseItem.ID.Novice_Tome:
						_idleSecondaryAnimation = spriteManager.unitWeapons.tome.ToArray();
					break;

					case BaseItem.ID.Parma:
						_idleSecondaryAnimation = spriteManager.unitWeapons.parma.ToArray();
					break;
				}
			}
		}

		// Fallback to empty sprite sheet.
		if(_idleSecondaryAnimation[0] == null) {
			for(int i = 0; i < IdleAnimationLength; i++) {
				_idleSecondaryAnimation[i] = spriteManager.items.unknown;
			}
		}
	}
	
	public void LoadCustomArmor(SpriteManager spriteManager) {
		_idleSkinAnimation = new Sprite[IdleAnimationLength];
		_idleArmorAnimation = new Sprite[IdleAnimationLength];

		Sprite[] skinTemplate = null;
		Sprite[] armorTemplate = null;

		Color[] skinPalette = null;
		Color[] armorPalette = null;

		if(_bag != null) {
			if(_bag.body != null) {
				switch(_bag.body.id) {
					case BaseItem.ID.Chainmail_Tunic:
						skinTemplate = spriteManager.unitArmor.chainmailSkin.ToArray();
						armorTemplate = spriteManager.unitArmor.chainmail.ToArray();
						armorPalette = ParseColor(Swatch.chainmailArmor);
					break;

					case BaseItem.ID.Cotton_Tunic:
						skinTemplate = spriteManager.unitArmor.cottonSkin.ToArray();
						armorTemplate = spriteManager.unitArmor.cotton.ToArray();
						armorPalette = ParseColor(Swatch.cottonArmor);
					break;

					case BaseItem.ID.Leather_Jack:
						skinTemplate = spriteManager.unitArmor.leatherSkin.ToArray();
						armorTemplate = spriteManager.unitArmor.leather.ToArray();
						armorPalette = ParseColor(Swatch.leatherArmor);
					break;
				}
			}
		}

		// Fallback to a default skin template.
		if(skinTemplate == null) {
			switch(_preset) {
				case Preset.Mage:
					skinTemplate = spriteManager.unitArmor.cottonSkin.ToArray();
				break;

				case Preset.Rogue:
					skinTemplate = spriteManager.unitArmor.leatherSkin.ToArray();
				break;

				default:
					skinTemplate = spriteManager.unitArmor.chainmailSkin.ToArray();
				break;
			}
			
		}
		
		// Fallback to a default armor template.
		if(armorTemplate == null) {
			
			switch(_preset) {
				case Preset.Mage:
					armorTemplate = spriteManager.unitArmor.cotton.ToArray();
					armorPalette = ParseColor(Swatch.cottonArmor);
				break;

				case Preset.Rogue:
					armorTemplate = spriteManager.unitArmor.leather.ToArray();
					armorPalette = ParseColor(Swatch.leatherArmor);
				break;

				default:
					armorTemplate = spriteManager.unitArmor.chainmail.ToArray();
					armorPalette = ParseColor(Swatch.chainmailArmor);
				break;
			}
		}
		
		// Prepare palette for the unit's skin.
		skinPalette = ParseColor(Swatch.GetSkinSwatch(_character.skinColor));
		
		// Apply swatches to the templates, then assign new sprites to the animation sheet.
		for(int i = 0; i < IdleAnimationLength; i++) {
			_idleSkinAnimation[i] = ApplySwatch(skinTemplate[i], skinPalette);
			_idleArmorAnimation[i] = ApplySwatch(armorTemplate[i], armorPalette);
		}
	}

	Sprite ApplySwatch(Sprite template, Color[] swatch) {
		if(template == null || swatch == null) {return null;}

		Texture2D texture = new Texture2D(48, 48);

		Color[] colors = template.texture.GetPixels();
		for(int i = 0; i < colors.Length; i++) {
			if(colors[i].a > 0f) {
				colors[i] = Swatch.SwapToColor(colors[i], swatch);
			}	
		}
		
		texture.SetPixels(colors);
		texture.Apply();

		Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 48, 48), new Vector2(0.5f, 0.5f), 48f);
		sprite.texture.filterMode = FilterMode.Point;
		return sprite;
	}

	Color[] ParseColor(string[] arr) {
		Color[] palette = new Color[Swatch.size];

		for(int i = 0; i < Swatch.size; i++) {
			if(i >= arr.Length) {
				ColorUtility.TryParseHtmlString(Swatch.template[i], out palette[i]);
			} else {
				ColorUtility.TryParseHtmlString(arr[i], out palette[i]);
			}
		}
		
		return palette;
	}

	void LoadFallbackSprite() {
		_idleAnimation = new Sprite[IdleAnimationLength];

		switch(_preset) {
			case Preset.Skeleton:
			case Preset.Skeleton_Thrall:
				_shadowSprite = AssetReference.sprites.units.shadows.medium;
				_idleAnimation = AssetReference.sprites.units.skeleton.idle.ToArray();
				_hitAnimation = AssetReference.sprites.units.skeleton.hit.ToArray();
			break;

			case Preset.Giant_Spider:
				_shadowSprite = AssetReference.sprites.units.shadows.large;
				_idleAnimation = AssetReference.sprites.units.giantSpider.idle.ToArray();
				_hitAnimation = AssetReference.sprites.units.giantSpider.hit.ToArray();
			break;

			case Preset.Spiderling:
				_shadowSprite = AssetReference.sprites.units.shadows.small;
				_idleAnimation = AssetReference.sprites.units.spiderling.idle.ToArray();
				_hitAnimation = AssetReference.sprites.units.spiderling.hit.ToArray();
			break;

			case Preset.Giant_Widow:
				_shadowSprite = AssetReference.sprites.units.shadows.large;
				_idleAnimation = AssetReference.sprites.units.giantWidow.idle.ToArray();
				_hitAnimation = AssetReference.sprites.units.giantWidow.hit.ToArray();
			break;

			case Preset.Widowling:
				_shadowSprite = AssetReference.sprites.units.shadows.small;
				_idleAnimation = AssetReference.sprites.units.widowling.idle.ToArray();
				_hitAnimation = AssetReference.sprites.units.widowling.hit.ToArray();
			break;

			case Preset.Warrior:
				_shadowSprite = AssetReference.sprites.units.shadows.medium;
				_idleAnimation = AssetReference.sprites.units.warrior.idle.ToArray();
				_hitAnimation = AssetReference.sprites.units.warrior.hit.ToArray();
			break;

			case Preset.Green_Slime:
			default:
				_shadowSprite = AssetReference.sprites.units.shadows.medium;
				_idleAnimation = AssetReference.sprites.units.greenSlime.idle.ToArray();
				_hitAnimation = AssetReference.sprites.units.greenSlime.hit.ToArray();
			break;
		}
	}

	public void BeginHitAnimation() {
		_hitFrame = 0;
		
	}

	public void EndHitAnimation() {
		_hitFrame = -1;
	}

	public void IncrementAnimation(int frame = -1) {
		if(_tile != null) {
			_animationFrame = _tile.animationController.animationFrame;
		} else {
			_animationFrame = frame;
		}

		if(_hitAnimation != null && _hitFrame > -1 && _hitFrame < _hitFrameSkip) {
			_sprite = _hitAnimation[0];
			_hitFrame++;
		} else if(_idleAnimation != null) {
			_hitFrame = -1;

			if(_animationFrame < 10) {
				if(_useCustomSprites) {
					_spriteSkin = _idleSkinAnimation[0];
					_spriteArmor = _idleArmorAnimation[0];
					_spriteSecondary = _idleSecondaryAnimation[0];
					_spritePrimary = _idlePrimaryAnimation[0];
				} else {
					_sprite = _idleAnimation[0];
				}
			} else if(_animationFrame < 11) {
				if(_useCustomSprites) {
					_spriteSkin = _idleSkinAnimation[1];
					_spriteArmor = _idleArmorAnimation[1];
					_spriteSecondary = _idleSecondaryAnimation[1];
					_spritePrimary = _idlePrimaryAnimation[1];
				} else {
					_sprite = _idleAnimation[1];
				}
			} else if(_animationFrame < 22) {
				_sprite = _idleAnimation[2];
				if(_useCustomSprites) {
					_spriteSkin = _idleSkinAnimation[2];
					_spriteArmor = _idleArmorAnimation[2];
					_spriteSecondary = _idleSecondaryAnimation[2];
					_spritePrimary = _idlePrimaryAnimation[2];
				}
			} else {
				_sprite = _idleAnimation[3];
				if(_useCustomSprites) {
					_spriteSkin = _idleSkinAnimation[3];
					_spriteArmor = _idleArmorAnimation[3];
					_spriteSecondary = _idleSecondaryAnimation[3];
					_spritePrimary = _idlePrimaryAnimation[3];
				}
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
