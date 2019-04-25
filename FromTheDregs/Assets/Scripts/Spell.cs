using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell {

	public enum Preset {
		Bite,
		Bleed,
		Block,
		BurningHands,
		FeintSwipe,
		Fireball,
		Move,
		Poison,
		PoisonFang,
		SeveringStrike,
		Skeletal_Claws,
		Slash,
	}

	public enum DamageType {
		Bleed,
		Fire,
		Ice,
		Lightning,
		Physical,
		Poison,
		Spell,
	}

	public enum Scaling {
		Strength,
		Dexterity,
		Intelligence
	}

	public enum EffectDirection {
		Up,
		Right,
		Down,
		Left,
	}

	public enum EffectShape {
		Line,
		Cone45,
		Cone180,
		Cone270,
		Circle
	}

	public enum TargetUnitType {
		None,
		Self,
		Enemy,
		Ally,
		All
	}

	public enum IntentType {
		Movement,
		Attack,
		Block,
		Buff,
		Debuff,
		Unknown
	}

	// Base
	BaseUnit _caster;
	Tile[,] _tiles;
	Preset _preset;
	string _spellName;
	//int _damageDice = 1;
	//int _damageSides = 4;
	//float _damageMultiplier = 1.0f;
	Effect _damageEffect;
	int _essenceCost = 1;
	int _chargesRemaining = 0;
	int _chargesMax;
	bool _requireCastConfirmation = false;
	bool _autoRecast = false;
	bool _createsCastParticle = false;
	bool _createsProjectile = false;
	bool _createsEffect = true;
	DamageType _damageType;
	Scaling _scaling = Scaling.Strength;
	string _damageSoundPath;
	string _blockSoundPath = "Sounds/sfx/block_impact_0";
	List<Effect> _spellCasterEffects;
	List<Effect> _spellTargetEffects;
	IntentType _intentType;


	// Casting
	int _castRadius = 0;
	bool _castThroughWalls = false;
	bool _castOnWalls = false;
	bool _castOnUnits = false;
	bool _castRequiresLineOfSight =  false;
	bool _castRequiresTarget = false;
	bool _castCanTargetSelf = false;
	TargetUnitType _castTargetUnitType = TargetUnitType.Enemy;
	GameObject _castParticle;
	string _castParticlePath;
	string _castSoundPath;


	// Projectile
	int _projCount;
	float _projSpeed;
	float _projPreSpawnDelay;
	float _projPostSpawnDelay;
	List<GameObject> _projectiles;
	string _projParticlePath;
	string _projSoundPath;


	// Effect
	Vector2Int _effectOrigin;
	float _effectPreSpawnDelay;
	float _effectDamageDelay;
	int _effectRadius = 0;
	bool _effectRotateToDirection = true;
	bool _effectIgnoresWalls = false;
	bool _effectRequiresLineOfSight = false;
	EffectShape _effectShape = EffectShape.Cone45;
	EffectDirection _effectDirection = EffectDirection.Right;
	TargetUnitType _effectTargetUnitType = TargetUnitType.Enemy;
	GameObject _effectParticle;
	List<Tile> _hitTiles;
	string _effectParticlePath;
	string _effectSoundPath;
	float _effectSoundDelay;

	#region Accessors

	public int id;
	public Preset preset {
		get {return _preset;}
		set {_preset = value;}
	}
	public BaseUnit caster {
		get {return _caster;}
	}
	public string spellName {
		get {return _spellName;}
	}

	public DamageType damageType {
		get {return _damageType;}
	}

	public int essenceCost {
		get {return _essenceCost;}
		set {_essenceCost = value;}
	}

	public int castRadius {
		get {return _castRadius;}
		set {_castRadius = value;}
	}

	public int chargesRemaining {
		get {return _chargesRemaining;}
		set {_chargesRemaining = value;}
	}

	public int chargesMax {
		get {return _chargesMax;}
	}

	public int effectRadius {
		get {return _effectRadius;}
	}

	public bool requireCastConfirmation {
		get {return _requireCastConfirmation;}
	}

	public TargetUnitType castTargetUnitType {
		get {return _castTargetUnitType;}
	}

	public int projCount {
		get {return _projCount;}
	}

	public float projPreSpawnDelay {
		get {return _projPreSpawnDelay;}
	}

	public float projPostSpawnDelay {
		get {return _projPostSpawnDelay;}
	}

	public Vector2Int effectOrigin {
		get {return _effectOrigin;}
		set {_effectOrigin = value;}
	}

	public float effectPreSpawnDelay {
		get {return _effectPreSpawnDelay;}
	}

	public float effectDamageDelay {
		get {return _effectDamageDelay;}
	}

	public List<Tile> hitTiles {
		get {return _hitTiles;}
		set {_hitTiles = value;}
	}

	public List<Effect> targetEffects {
		get {return _spellTargetEffects;}
	}

	public List<Effect> casterEffects {
		get {return _spellCasterEffects;}
	}

	public IntentType intentType {
		get {return _intentType;}
	}

	public string intentValue {
		get {

			switch(_intentType) {
				case IntentType.Attack:
				case IntentType.Debuff:
					int a = 0;

					foreach(Effect e in _spellTargetEffects) {

						switch(e.effectType) {
							case Effect.EffectType.Damage:
							case Effect.EffectType.CriticalDamage:
							case Effect.EffectType.InescapableDamage:
							case Effect.EffectType.UnblockableDamage:
								a += EstimateDamage(e);

							break;
						}
						
					}
					if(_caster.spellCharges == 1) {
						return a.ToString();
					} else if(_caster.spellCharges > 1) {
						return a.ToString() + "x" + _caster.spellCharges;
					}
				break;
				
				case IntentType.Block:
					int b = 0;
					foreach(Effect e in _spellCasterEffects) {
						switch(e.effectType) {
							case Effect.EffectType.Block:
								b += EstimateBlock(e);
							break;
						}
						
					}
					if(_caster.spellCharges == 1) {
						return b.ToString();
					} else if(_caster.spellCharges > 1) {
						return b.ToString() + "x" + _caster.spellCharges;
					}
				break;
			}
			return "";
		}
	}
	#endregion

	// Constructors
	public Spell(Preset spell) {
		id = Random.Range(0, 1000);
		CreateFromPreset(spell);
	}

	public Spell(BaseUnit caster, Preset spell) {
		id = Random.Range(0, 1000);
		_caster = caster;
		_tiles = _caster.tile.dungeonManager.tiles;
		CreateFromPreset(spell);
	}


	#region Spells
	public void CreateFromPreset(Preset spell, bool sync = false) {
		_preset = spell;
		_hitTiles = new List<Tile>();
		_spellCasterEffects = new List<Effect>();
		_spellTargetEffects = new List<Effect>();
		Effect damage;

		switch(spell) {
			
			#region Bite
			case Preset.Bite:	
				_spellName = "Bite";

				_essenceCost = 2;
				_damageType = DamageType.Physical;
				_damageSoundPath = "Sounds/sfx/impact_damage_0";
				_autoRecast = false;
				_requireCastConfirmation = true;
				_createsCastParticle = false;
				_createsProjectile = false;
				_createsEffect = true;
				_scaling = Scaling.Strength;
				_intentType = IntentType.Attack;

				
				damage = new Effect(Effect.EffectType.Damage);
				damage.SetPrimaryScaling(Effect.ScalingType.Strength, 1.00f);
				_spellTargetEffects.Add(damage);
				
				_castParticlePath = "";
				_castRadius = 1;
				_castThroughWalls = false;
				_castOnWalls = false;
				_castOnUnits = true;
				_castRequiresTarget = true;
				_castRequiresLineOfSight = true;
				_castCanTargetSelf = false;
				_castTargetUnitType = TargetUnitType.Enemy;

				_effectParticlePath = "Prefabs/Effects/Bite Impact";
				_effectSoundPath = "Sounds/sfx/bite_impact_0";
				_effectSoundDelay = 0.15f;
				_effectDamageDelay = 0.75f;
				_effectRadius = 0;
				_effectRotateToDirection = false;
				_effectIgnoresWalls = false;
				_effectRequiresLineOfSight = true;
				_effectShape = Spell.EffectShape.Circle;
				_effectDirection = EffectDirection.Up;
				_effectTargetUnitType = TargetUnitType.Enemy;
			break;
			#endregion

			#region Bleed
			case Preset.Bleed:	
				_spellName = "Bleed";

				_essenceCost = 0;
				_damageType = DamageType.Bleed;
				_damageSoundPath = "Sounds/sfx/impact_damage_0";
				_autoRecast = false;
				_requireCastConfirmation = true;
				_createsCastParticle = false;
				_createsProjectile = false;
				_createsEffect = true;
				_scaling = Scaling.Strength;
				_intentType = IntentType.Attack;
				
				_castParticlePath = "";
				_castRadius = 1;
				_castThroughWalls = false;
				_castOnWalls = false;
				_castOnUnits = true;
				_castRequiresTarget = true;
				_castRequiresLineOfSight = true;
				_castCanTargetSelf = false;
				_castTargetUnitType = TargetUnitType.Enemy;

				_effectParticlePath = "Prefabs/Effects/Bleed Impact";
				_effectSoundPath = "Sounds/sfx/impact_damage_0";
				_effectSoundDelay = 0.15f;
				_effectDamageDelay = 0.25f;
				_effectRadius = 0;
				_effectRotateToDirection = false;
				_effectIgnoresWalls = false;
				_effectRequiresLineOfSight = true;
				_effectShape = Spell.EffectShape.Circle;
				_effectDirection = EffectDirection.Up;
				_effectTargetUnitType = TargetUnitType.All;
			break;
			#endregion

			#region Block
			case Preset.Block:	
				_spellName = "Block";

				_essenceCost = 2;
				//_damageType = DamageType.Piercing;
				//_damageSoundPath = "Sounds/sfx/impact_damage_0";
				_autoRecast = false;
				_requireCastConfirmation = true;
				_createsCastParticle = false;
				_createsProjectile = false;
				_createsEffect = true;
				_scaling = Scaling.Strength;
				_intentType = IntentType.Block;

				Effect block = new Effect(Effect.EffectType.Block);
				block.stackable = true;
				block.SetPrimaryScaling(Effect.ScalingType.Strength, 0.25f);
				block.SetSecondaryScaling(Effect.ScalingType.Dexterity, 0.10f);
				block.deactivationConditions[Effect.Conditions.DurationExpire] = 1;
				_spellCasterEffects.Add(block);

				//damage = new Effect(Effect.EffectType.Block);
				//damage.SetPrimaryScaling(Effect.ScalingType.Dexterity, 0.80f);
				//_spellTargetEffects.Add(damage);

				
				_castParticlePath = "";
				_castRadius = 0;
				_castThroughWalls = false;
				_castOnWalls = false;
				_castOnUnits = true;
				_castRequiresTarget = true;
				_castRequiresLineOfSight = true;
				_castCanTargetSelf = true;
				_castTargetUnitType = TargetUnitType.Self;

				_effectParticlePath = "Prefabs/Effects/Block Apply";
				_effectSoundPath = "Sounds/sfx/grant_block_0";
				_effectSoundDelay = 0.15f;
				_effectDamageDelay = 0.80f;
				_effectRadius = 0;
				_effectRotateToDirection = false;
				_effectIgnoresWalls = false;
				_effectRequiresLineOfSight = true;
				_effectShape = Spell.EffectShape.Circle;
				_effectDirection = EffectDirection.Up;
				_effectTargetUnitType = TargetUnitType.Self;
			break;
			#endregion

			#region Feint Swipe
			case Preset.FeintSwipe:	
				_spellName = "Feint Swipe";

				_essenceCost = 2;
				_damageType = DamageType.Physical;
				_damageSoundPath = "Sounds/sfx/impact_damage_0";
				_autoRecast = false;
				_requireCastConfirmation = true;
				_createsCastParticle = false;
				_createsProjectile = false;
				_createsEffect = true;
				_scaling = Scaling.Strength;
				_intentType = IntentType.Attack;

				Effect crit = new Effect(Effect.EffectType.Focus);
				crit.deactivationConditions.Add(Effect.Conditions.DealDamage, 1);
				_spellCasterEffects.Add(crit);

				damage = new Effect(Effect.EffectType.Damage);
				damage.SetPrimaryScaling(Effect.ScalingType.Dexterity, 0.75f);
				_spellTargetEffects.Add(damage);

				
				_castParticlePath = "";
				_castRadius = 1;
				_castThroughWalls = false;
				_castOnWalls = false;
				_castOnUnits = true;
				_castRequiresTarget = true;
				_castRequiresLineOfSight = true;
				_castCanTargetSelf = false;
				_castTargetUnitType = TargetUnitType.All;

				_effectParticlePath = "Prefabs/Effects/Slash Impact";
				_effectSoundPath = "Sounds/sfx/slash_impact_0";
				_effectSoundDelay = 0.15f;
				_effectDamageDelay = 0.80f;
				_effectRadius = 0;
				_effectRotateToDirection = false;
				_effectIgnoresWalls = false;
				_effectRequiresLineOfSight = true;
				_effectShape = Spell.EffectShape.Circle;
				_effectDirection = EffectDirection.Up;
				_effectTargetUnitType = TargetUnitType.Enemy;
			break;
			#endregion

			#region Fireball
			case Preset.Fireball:	
				_spellName = "Fireball";

				_damageType = DamageType.Fire;
				_essenceCost = 4;
				_autoRecast = false;
				_requireCastConfirmation = true;
				_createsCastParticle = true;
				_createsProjectile = true;
				_createsEffect = true;
				_scaling = Scaling.Intelligence;
				_intentType = IntentType.Attack;

				damage = new Effect(Effect.EffectType.Damage);
				damage.SetPrimaryScaling(Effect.ScalingType.Intelligence, 0.80f);
				_spellTargetEffects.Add(damage);
				
				_castParticlePath = "Prefabs/Effects/Fire Casting";
				_castRadius = 5;
				_castThroughWalls = false;
				_castOnWalls = false;
				_castOnUnits = true;
				_castRequiresTarget = false;
				_castRequiresLineOfSight = true;
				_castCanTargetSelf = false;
				_castTargetUnitType = TargetUnitType.All;

				_projCount = 1;
				_projParticlePath = "Prefabs/Effects/Fireball";
				_projSpeed = 192f;

				_effectParticlePath = "Prefabs/Effects/Fireball Impact";
				_effectRadius = 1;
				_effectIgnoresWalls = false;
				_effectRequiresLineOfSight = true;
				_effectShape = Spell.EffectShape.Circle;
				_effectDirection = EffectDirection.Up;
				_effectTargetUnitType = TargetUnitType.All;
			break;
			#endregion

			#region Move
			case Preset.Move:
				_spellName = "Move";
				//_essenceCost = 1;
				
				if(!sync) {	// Base settings
					//_chargesMax = 0;
				} else {
					if(_caster != null) {
						//_chargesMax = _caster.attributes.speed - 1;
					}
				}
				
				if(!sync) {	// Base settings
					_essenceCost = 1;
				}

				_autoRecast = true;
				_requireCastConfirmation = false;
				_createsCastParticle = false;
				_createsProjectile = false;
				_createsEffect = false;
				
				_castRadius = 1;
				_castThroughWalls = false;
				_castOnWalls = false;
				_castOnUnits = false;
				_castRequiresTarget = false;
				_castRequiresLineOfSight = true;
				_castCanTargetSelf = false;
				_castTargetUnitType = TargetUnitType.None;
			break;
			#endregion

			#region Poison
			case Preset.Poison:	
				_spellName = "Poison";

				_essenceCost = 0;
				_damageType = DamageType.Poison;
				_damageSoundPath = "Sounds/sfx/impact_damage_0";
				_autoRecast = false;
				_requireCastConfirmation = true;
				_createsCastParticle = false;
				_createsProjectile = false;
				_createsEffect = true;
				_scaling = Scaling.Strength;
				_intentType = IntentType.Attack;
				
				_castParticlePath = "";
				_castRadius = 1;
				_castThroughWalls = false;
				_castOnWalls = false;
				_castOnUnits = true;
				_castRequiresTarget = true;
				_castRequiresLineOfSight = true;
				_castCanTargetSelf = false;
				_castTargetUnitType = TargetUnitType.Enemy;

				_effectParticlePath = "Prefabs/Effects/Poison Impact";
				_effectSoundPath = "Sounds/sfx/impact_damage_0";
				_effectSoundDelay = 0.15f;
				_effectDamageDelay = 0.25f;
				_effectRadius = 0;
				_effectRotateToDirection = false;
				_effectIgnoresWalls = false;
				_effectRequiresLineOfSight = true;
				_effectShape = Spell.EffectShape.Circle;
				_effectDirection = EffectDirection.Up;
				_effectTargetUnitType = TargetUnitType.All;
			break;
			#endregion

			#region Poison Fang
			case Preset.PoisonFang:	
				_spellName = "Poison Fang";

				_essenceCost = 2;
				_damageType = DamageType.Physical;
				_damageSoundPath = "Sounds/sfx/impact_damage_0";
				_autoRecast = false;
				_requireCastConfirmation = true;
				_createsCastParticle = false;
				_createsProjectile = false;
				_createsEffect = true;
				_scaling = Scaling.Strength;
				_intentType = IntentType.Debuff;


				damage = new Effect(Effect.EffectType.Damage);
				damage.SetPrimaryScaling(Effect.ScalingType.Dexterity, 0.75f);
				_spellTargetEffects.Add(damage);

				Effect poison = new Effect(Effect.EffectType.Poison);
				poison.stackable = true;
				poison.deactivationConditions.Add(Effect.Conditions.DurationExpire, 3);
				_spellTargetEffects.Add(poison);

				

				_castParticlePath = "";
				_castRadius = 1;
				_castThroughWalls = false;
				_castOnWalls = false;
				_castOnUnits = true;
				_castRequiresTarget = true;
				_castRequiresLineOfSight = true;
				_castCanTargetSelf = false;
				_castTargetUnitType = TargetUnitType.Enemy;

				_effectParticlePath = "Prefabs/Effects/Poison Fang Impact";
				_effectSoundPath = "Sounds/sfx/slash_impact_0";
				_effectSoundDelay = 0.15f;
				_effectDamageDelay = 0.75f;
				_effectRadius = 0;
				_effectRotateToDirection = false;
				_effectIgnoresWalls = false;
				_effectRequiresLineOfSight = true;
				_effectShape = Spell.EffectShape.Circle;
				_effectDirection = EffectDirection.Up;
				_effectTargetUnitType = TargetUnitType.Enemy;
			break;
			#endregion

			#region Severing Strike
			case Preset.SeveringStrike:	
				_spellName = "Severing Strike";

				_essenceCost = 2;
				_damageType = DamageType.Physical;
				_damageSoundPath = "Sounds/sfx/impact_damage_0";
				_autoRecast = false;
				_requireCastConfirmation = true;
				_createsCastParticle = false;
				_createsProjectile = false;
				_createsEffect = true;
				_scaling = Scaling.Strength;
				_intentType = IntentType.Debuff;
				
				damage = new Effect(Effect.EffectType.Damage);
				damage.SetPrimaryScaling(Effect.ScalingType.Strength, 1.25f);
				_spellTargetEffects.Add(damage);

				Effect bleed = new Effect(Effect.EffectType.Bleed);
				bleed.stackable = true;
				bleed.deactivationConditions.Add(Effect.Conditions.DurationExpire, 3);
				_spellTargetEffects.Add(bleed);

				
				_castParticlePath = "";
				_castRadius = 1;
				_castThroughWalls = false;
				_castOnWalls = false;
				_castOnUnits = true;
				_castRequiresTarget = true;
				_castRequiresLineOfSight = true;
				_castCanTargetSelf = false;
				_castTargetUnitType = TargetUnitType.Enemy;

				_effectParticlePath = "Prefabs/Effects/Severing Strike Impact";
				_effectSoundPath = "Sounds/sfx/slash_impact_0";
				_effectSoundDelay = 0.15f;
				_effectDamageDelay = 0.80f;
				_effectRadius = 0;
				_effectRotateToDirection = false;
				_effectIgnoresWalls = false;
				_effectRequiresLineOfSight = true;
				_effectShape = Spell.EffectShape.Circle;
				_effectDirection = EffectDirection.Up;
				_effectTargetUnitType = TargetUnitType.Enemy;
			break;
			#endregion

			#region Skeletal Claws
			case Preset.Skeletal_Claws:	
				_spellName = "Skeletal Claws";

				_essenceCost = 2;
				_damageType = DamageType.Physical;
				_damageSoundPath = "Sounds/sfx/impact_damage_0";
				_autoRecast = false;
				_requireCastConfirmation = true;
				_createsCastParticle = false;
				_createsProjectile = false;
				_createsEffect = true;
				_scaling = Scaling.Strength;
				_intentType = IntentType.Attack;

				
				damage = new Effect(Effect.EffectType.Damage);
				damage.SetPrimaryScaling(Effect.ScalingType.Dexterity, 1.00f);
				_spellTargetEffects.Add(damage);
				
				_castParticlePath = "";
				_castRadius = 1;
				_castThroughWalls = false;
				_castOnWalls = false;
				_castOnUnits = true;
				_castRequiresTarget = true;
				_castRequiresLineOfSight = true;
				_castCanTargetSelf = false;
				_castTargetUnitType = TargetUnitType.Enemy;

				_effectParticlePath = "Prefabs/Effects/Claws Impact";
				_effectSoundPath = "Sounds/sfx/slash_impact_0";
				_effectSoundDelay = 0.15f;
				_effectDamageDelay = 0.75f;
				_effectRadius = 0;
				_effectRotateToDirection = false;
				_effectIgnoresWalls = false;
				_effectRequiresLineOfSight = true;
				_effectShape = Spell.EffectShape.Circle;
				_effectDirection = EffectDirection.Up;
				_effectTargetUnitType = TargetUnitType.Enemy;
			break;
			#endregion

			#region Slash
			case Preset.Slash:	
				_spellName = "Slash";

				_essenceCost = 2;
				_damageType = DamageType.Physical;
				_damageSoundPath = "Sounds/sfx/impact_damage_0";
				_autoRecast = false;
				_requireCastConfirmation = true;
				_createsCastParticle = false;
				_createsProjectile = false;
				_createsEffect = true;
				_scaling = Scaling.Strength;
				_intentType = IntentType.Attack;

				damage = new Effect(Effect.EffectType.Damage);
				damage.SetPrimaryScaling(Effect.ScalingType.Strength, 1.50f);
				_spellTargetEffects.Add(damage);
				
				_castParticlePath = "";
				_castRadius = 1;
				_castThroughWalls = false;
				_castOnWalls = false;
				_castOnUnits = true;
				_castRequiresTarget = true;
				_castRequiresLineOfSight = true;
				_castCanTargetSelf = false;
				_castTargetUnitType = TargetUnitType.Enemy;

				_effectParticlePath = "Prefabs/Effects/Slash Impact";
				_effectSoundPath = "Sounds/sfx/slash_impact_0";
				_effectSoundDelay = 0.15f;
				_effectDamageDelay = 0.80f;
				_effectRadius = 0;
				_effectRotateToDirection = false;
				_effectIgnoresWalls = false;
				_effectRequiresLineOfSight = true;
				_effectShape = Spell.EffectShape.Circle;
				_effectDirection = EffectDirection.Up;
				_effectTargetUnitType = TargetUnitType.Enemy;
			break;
			#endregion
		}
	}
	#endregion

	public void ResetTiles() {
		for(int y = 0; y < DungeonManager.dimension; y++) {
			for(int x = 0; x < DungeonManager.dimension; x++) {
				Tile _tile = _caster.tile.dungeonManager.tiles[x, y];
				if(_tile.terrain != null) {
					_tile.terrain.readyCast = false;
					_tile.terrain.confirmCast = false;
					_tile.terrain.image.color = Color.white;
				}
			}
		}
	}

	int CheckManhattanDistance(int x1, int y1, int x2, int y2) {
		return Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2);
	}

	float CheckTrueDistance(Vector2Int start, Vector2Int end) {
		return Mathf.Sqrt((end.x - start.x)*(end.x - start.x) + (end.y - start.y)*(end.y - start.y)) * DungeonManager.TileWidth;
	}

	bool RayTrace(int x1, int y1, int x2, int y2) {
		int dx = Mathf.Abs(x2 - x1);
		int sx = -1;

		if(x1 < x2) {
			sx = 1;
		}

		int dy = Mathf.Abs(y2 - y1);
		int sy = -1;
		
		if(y1 < y2) {
			sy = 1;
		}

		int err = -dy/2;

		if(dx > dy) {
			err = dx/2;
		}

		while(true) {

			if(!_tiles[x1, y1].baseTerrain.walkable) {
				return false;
			}

			if(x1 == x2 && y1 == y2) {
				return true;
			}

			int e2 = err;

			if(e2 > -dx) {
				err -= dy;
				x1 += sx;
			}

			if(e2 < dy) {
				err += dx;
				y1 += sy;
			}
		}
	}

	#region Casting
	public void ShowCastRange() {
		bool[,] visitedTiles = new bool[DungeonManager.dimension, DungeonManager.dimension];

		for(int y = 0; y < DungeonManager.dimension; y++) {
			for(int x = 0; x < DungeonManager.dimension; x++) {
				visitedTiles[x, y] = false;
				if(_tiles[x, y].terrain != null) {
					_tiles[x, y].terrain.image.color = new Color(1f, 1f, 1f);
				}
			}
		}

		PopulateCastRange(_caster.tile.position.x, _caster.tile.position.y, visitedTiles, _caster.tile.position.x, _caster.tile.position.y);
		if(_createsCastParticle) {
			SpawnCastParticles(_caster.tile.position, 0f);
		}
	}

	void PopulateCastRange(int x, int y, bool[,] visited, int ox, int oy) {

		// Check map bounds
		if(	x < 0 ||
		 	y < 0 ||
		 	x >= (DungeonManager.dimension)-1 || 
			y >= (DungeonManager.dimension)-1) { 
			return;
		}

		// Check casting bounds
		if(	x > ox+_castRadius || y > oy+_castRadius || 
			x < ox-_castRadius || y <  oy-_castRadius) {
			return;
		}

		if(visited[x, y] == true) {
			return;
		}

		visited[x, y] = true;


		if(CheckManhattanDistance(ox, oy, x, y) > _castRadius) {
			return;
		}

		Tile _tile = _caster.tile.dungeonManager.tiles[x, y];

		// Check wall collision
		if(_tile.baseTerrain != null) {
			if((!_castThroughWalls && !_castOnWalls) && !_tile.baseTerrain.walkable) {
				return;
			}
		}


		bool flagTile = false;

		// Allow self target
		if(x == ox && y == oy) {
			if(_castCanTargetSelf) {
				flagTile = true;
			}
		} else {

			// Allow unit target
			if(_tile.baseUnit != null) {
				if(_castRequiresTarget) {
					switch(_castTargetUnitType) {
						case TargetUnitType.None:
							return;
						
						case TargetUnitType.Enemy:
							if(_tile.baseUnit.attributes.alliance != _caster.attributes.alliance) {
								flagTile = true;
							} else {
								flagTile = false;
							}
						break;

						case TargetUnitType.All:
							flagTile = true;
						break;


					}
				} else {
					flagTile = true;
				}
			} else {
				if(_castRequiresTarget) {
					return;
				}
			}

			// Allow casting on walls
			if(!_tile.baseTerrain.walkable) {
				if(_castThroughWalls && _castOnWalls) {
					flagTile = true;
				} else if(_castOnWalls) {
					_tile.terrain.image.color = new Color(0.85f, 0.85f, 1f);
					return;
				}
			} else {

				// Require line of sight
				if(_castRequiresLineOfSight) {
					flagTile = RayTrace(ox, oy, x, y);
				} else {
					flagTile = true;
				}
				
			}
		}

		// Change tile color
		if(flagTile) {
			_tile.terrain.readyCast = true;
			_tile.terrain.confirmCast = false;
			_tile.terrain.image.color = new Color(0.85f, 0.85f, 1f);
		}

		PopulateCastRange(x+1, y  , visited, ox, oy);
		PopulateCastRange(x  , y-1, visited, ox, oy);
		PopulateCastRange(x-1, y  , visited, ox, oy);
		PopulateCastRange(x  , y+1, visited, ox, oy);
	}
	#endregion
	
	#region Effects
	public void ShowEffectRange(Vector2Int origin) {
		bool[,] visitedTiles = new bool[DungeonManager.dimension, DungeonManager.dimension];

		if(_caster.playerControlled) {
			for(int y = 0; y < DungeonManager.dimension; y++) {
				for(int x = 0; x < DungeonManager.dimension; x++) {
					visitedTiles[x, y] = false;
					if(_tiles[x, y].terrain != null) {
						_tiles[x, y].terrain.image.color = new Color(1f, 1f, 1f);
					}
				}
			}
		}

		_effectOrigin = origin;

		if(_effectOrigin.x - _caster.tile.position.x < 0) {
			_effectDirection = EffectDirection.Left;
		} else if(_effectOrigin.x - _caster.tile.position.x > 0) {
			_effectDirection = EffectDirection.Right;
		} else if(_effectOrigin.y - _caster.tile.position.y < 0) {
			_effectDirection = EffectDirection.Down;
		} if(_effectOrigin.y - _caster.tile.position.y > 0) {
			_effectDirection = EffectDirection.Up;
		}

		_hitTiles = new List<Tile>();
		PopulateEffectRange(_effectOrigin.x, _effectOrigin.y, visitedTiles, _effectOrigin.x, _effectOrigin.y);
	}

	void PopulateEffectRange(int x, int y, bool[,] visited, int ox, int oy) {

		// Check map bounds
		if(	x < 0 ||
		 	y < 0 ||
		 	x >= (DungeonManager.dimension)-1 || 
			y >= (DungeonManager.dimension)-1) { 
			return;
		}

		// Check casting bounds
		if(	x > ox+_effectRadius || y > oy+_effectRadius || 
			x < ox-_effectRadius || y <  oy-_effectRadius) {
			return;
		}

		if(visited[x, y] == true) {
			return;
		}

		visited[x, y] = true;

		Tile _tile = _caster.tile.dungeonManager.tiles[x, y];
		bool flagTile = false;

		switch(_effectDirection) {
			case EffectDirection.Left:	// Left
				switch(_effectShape) {
					case EffectShape.Cone45:	// Cone 45deg
						if(Mathf.Abs(y - oy) >	Mathf.Abs(x - ox) || (x - ox > 0)) {
							return;
						}
					break;

					case EffectShape.Circle:	// Circle
						if(CheckManhattanDistance(ox, oy, x, y) > _effectRadius) {
							return;
						}
					break;
				}
			break;

			case EffectDirection.Down:	// Down
				switch(_effectShape) {
					case EffectShape.Cone45:	// Cone 45deg					
						if(Mathf.Abs(y - oy) <	Mathf.Abs(x - ox) || (y - oy > 0)) {
							return;
						}
					break;

					case EffectShape.Circle:	// Circle
						if(CheckManhattanDistance(ox, oy, x, y) > _effectRadius) {
							return;
						}
					break;
				}
			break;

			case EffectDirection.Right:	// Right
				switch(_effectShape) {
					case EffectShape.Cone45:	// Cone 45deg					
						if(Mathf.Abs(y - oy) >	Mathf.Abs(x - ox) || (x - ox < 0)) {
							return;
						}
					break;

					case EffectShape.Circle:	// Circle
						if(CheckManhattanDistance(ox, oy, x, y) > _effectRadius) {
							return;
						}
					break;
				}
			break;

			case EffectDirection.Up:	// Up
				switch(_effectShape) {
					case EffectShape.Cone45:	// Cone 45deg					
						if(Mathf.Abs(y - oy) <	Mathf.Abs(x - ox) || (y - oy < 0)) {
							return;
						}
					break;

					case EffectShape.Circle:	// Circle
						if(CheckManhattanDistance(ox, oy, x, y) > _effectRadius) {
							return;
						}
					break;
				}
			break;
		}

		// Check wall collision
		if(!_effectIgnoresWalls && !_tile.baseTerrain.walkable) {
			return;
		}

		// Allow effects through walls
		if(!_tile.baseTerrain.walkable) {
			if(_effectIgnoresWalls) {
				flagTile = true;
			}
		} else {

			// Require line of sight
			if(_effectRequiresLineOfSight) {
				flagTile = RayTrace(ox, oy, x, y);
			} else {
				flagTile = true;
			}
			
		}

		// Change tile color
		if(flagTile) {
			if(_caster.playerControlled) {
				_tile.terrain.image.color = new Color(1f, 0.85f, 0.85f);
			}
			
			if(_tile.terrain != null) {
				_tile.terrain.readyCast = false;
				_tile.terrain.confirmCast = true;
			}
			
			_hitTiles.Add(_tile);
		}
		

		
		PopulateEffectRange(x+1, y  , visited, ox, oy);
		PopulateEffectRange(x  , y-1, visited, ox, oy);
		PopulateEffectRange(x-1, y  , visited, ox, oy);
		PopulateEffectRange(x  , y+1, visited, ox, oy);
	}
	#endregion


	public float ConfirmSpellCast() {
		
		float zrot = 0f;
		if(_effectRotateToDirection) {
			switch(_effectDirection) {
				case EffectDirection.Right:
					zrot = 270f;
				break;

				case EffectDirection.Down:
					zrot = 180f;
				break;

				case EffectDirection.Left:
					zrot = 90f;
				break;

				case EffectDirection.Up:
					zrot = 0f;
				break;
			}
		}

		float castDelay = OnCast();

		DestroyCastParticles();

		if(_caster.tile.unit != null) {
			if(_createsProjectile) {
				_projectiles = new List<GameObject>();
				_caster.tile.unit.SpawnSpellProjectiles(this);
			} else if(_createsEffect) {
				_caster.tile.unit.SpawnSpellEffect(this);
			}
		}

		if(_caster.playerControlled) {
			ResetTiles();

			Hotbar hotbar = GameObject.FindObjectOfType<Hotbar>();
			Hotkey[] hotkeys = hotbar.GetComponentsInChildren<Hotkey>();
			foreach(Hotkey hotkey in hotkeys) {
				if(hotkey.preset == _preset && _autoRecast) {
					hotkey.PreviewCast();
				} else {
					hotkey.showCastRange = false;
				}
			}
		}
		
		return _projPreSpawnDelay + _effectPreSpawnDelay + _effectDamageDelay + castDelay;
	}
	
	#region Particles
	void SpawnCastParticles(Vector2Int position, float zrotation) {
		GameObject castParticleGO = GameObject.Instantiate(Resources.Load<GameObject>(_castParticlePath));
		CameraController dungeon = GameObject.FindObjectOfType<CameraController>();
		castParticleGO.transform.SetParent(dungeon.transform);
		castParticleGO.transform.SetAsLastSibling();

		_castParticle = castParticleGO;

		RectTransform unitRT = _tiles[position.x, position.y].unit.GetComponent<RectTransform>();

		RectTransform castRT = castParticleGO.GetComponent<RectTransform>();

		castRT.anchoredPosition = new Vector2(unitRT.anchoredPosition.x + DungeonManager.TileWidth/2, unitRT.anchoredPosition.y + DungeonManager.TileHeight/2);
		castRT.localEulerAngles = new Vector3(0f, 0f, zrotation);
		castRT.localScale = new Vector3(0.25f, 0.25f, 0.25f);
	}

	public void DestroyCastParticles() {
		if(_castParticle != null) {
			ParticleSystem ps = _castParticle.GetComponent<ParticleSystem>();
			ps.Stop();
			GameObject.Destroy(_castParticle, ps.main.startLifetime.constant);
			_castParticle = null;
		}
	}

	public void SpawnProjectileParticles(Vector2Int start, Vector2Int end, float zrotation) {
		GameObject projParticleGO = GameObject.Instantiate(Resources.Load<GameObject>(_projParticlePath));
		GameObject effectsLayer = GameObject.FindGameObjectWithTag("Effects Layer");
		projParticleGO.transform.SetParent(effectsLayer.transform);
		projParticleGO.transform.SetAsLastSibling();

		Projectile proj = projParticleGO.GetComponent<Projectile>();
		float theta = Mathf.Atan2(end.y - start.y, end.x - start.x);
		float distance = Mathf.Sqrt((end.x - start.x)*(end.x - start.x) + (end.y - start.y)*(end.y - start.y)) * DungeonManager.TileWidth;
		proj.velocity = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)) * _projSpeed;
		proj.spell = this;
		proj.end = end;
		_projectiles.Add(projParticleGO);
		GameObject.Destroy(projParticleGO, distance/_projSpeed);

		RectTransform unitRT = _tiles[start.x, start.y].unit.GetComponent<RectTransform>();
		RectTransform projRT = projParticleGO.GetComponent<RectTransform>();
		projRT.anchoredPosition = new Vector2(unitRT.anchoredPosition.x + DungeonManager.TileWidth/2, unitRT.anchoredPosition.y + DungeonManager.TileHeight/2);
		projRT.localEulerAngles = new Vector3(0f, 0f, theta * Mathf.Rad2Deg - 90f);
		projRT.localScale = new Vector3(1f, 1f, 1f);
	}

	public void SpawnEffectParticles(Vector2Int position, float zrotation) {
		GameObject effectsLayer = GameObject.FindGameObjectWithTag("Effects Layer");
		if(effectsLayer == null) {return;}

		GameObject effectParticleGO = GameObject.Instantiate(Resources.Load<GameObject>(_effectParticlePath));
		effectParticleGO.transform.SetParent(effectsLayer.transform);
		effectParticleGO.transform.SetAsLastSibling();

		RectTransform unitRT = _tiles[position.x, position.y].unit.GetComponent<RectTransform>();
		

		RectTransform effectRT = effectParticleGO.GetComponent<RectTransform>();

		effectRT.anchoredPosition = new Vector2(unitRT.anchoredPosition.x + DungeonManager.TileWidth/2, unitRT.anchoredPosition.y + DungeonManager.TileWidth/2);
		effectRT.localEulerAngles = new Vector3(0f, 0f, zrotation);
		effectRT.localScale = new Vector3(1f, 1f, 1f);
	}
	#endregion

	#region Sounds
	public void PlayEffectSound(Vector2Int position) {
		if(_effectSoundPath == null) {return;}
		AudioManager audioManager = GameObject.FindObjectOfType<AudioManager>();
		audioManager.LoadSound(_effectSoundPath);
		audioManager.PlaySound(0.5f, _effectSoundDelay);
	}

	public void PlayDamageSound(Vector2Int position) {
		if(_damageSoundPath == null) {return;}
		AudioManager audioManager = GameObject.FindObjectOfType<AudioManager>();
		audioManager.LoadSound(_damageSoundPath);
		audioManager.PlaySound();
	}

	public void PlayBlockSound(Vector2Int position) {
		if(_damageSoundPath == null) {return;}
		AudioManager audioManager = GameObject.FindObjectOfType<AudioManager>();
		audioManager.LoadSound(_blockSoundPath);
		audioManager.PlaySound();
	}
	#endregion

	public int EstimateDamage(Effect e) {
		float baseDamage = 1.0f;
		float critMult = 0f;
		float additionalDamage = 0f;

		// Calculate damage
		foreach(Effect casterEffect in _caster.effects) {
			switch(casterEffect.effectType) {
				case Effect.EffectType.Focus:
					critMult = 0.5f;
				break;


			}
		}
		
		int damage = 0;
		switch(_damageType) {
			case DamageType.Physical:
				if(_caster.bag != null) {
					damage = _caster.bag.GetEquipmentBonus(Bag.EquipmentBonus.PhysicalDamage);
				}
			break;

			case DamageType.Spell:
				if(_caster.bag != null) {
					damage = _caster.bag.GetEquipmentBonus(Bag.EquipmentBonus.SpellDamage);
				}
			break;
		}

		damage += (int)(e.GetPotency(_caster.attributes) * (baseDamage + critMult + additionalDamage));
		return damage;
	}

	public int EstimateBlock(Effect e) {

		// Calculate block
		int block = 0;

		if(_caster.bag != null) {
			block = _caster.bag.GetEquipmentBonus(Bag.EquipmentBonus.BlockModifier);
			e.initialBlockModifier = block;
		}

		block += (int)(e.GetPotency(_caster.attributes));

		return block;
	}

	public void DealDamage(Effect e, bool sound = false) {
		float baseDamage = 1.0f;
		float critMult = 0f;
		float additionalDamage = 0f;

		// Calculate damage
		foreach(Effect casterEffect in _caster.effects) {
			switch(casterEffect.effectType) {
				case Effect.EffectType.Focus:
					critMult = 0.5f;
				break;


			}
		}

		
		int damage = 0;
		switch(_damageType) {
			case DamageType.Physical:
				if(caster.bag != null) {
					damage = caster.bag.GetEquipmentBonus(Bag.EquipmentBonus.PhysicalDamage);
				}
			break;

			case DamageType.Spell:
				if(caster.bag != null) {
					damage = caster.bag.GetEquipmentBonus(Bag.EquipmentBonus.SpellDamage);
				}
			break;
		}

		damage += (int)(e.GetPotency(_caster.attributes) * (baseDamage + critMult + additionalDamage));

		foreach(Tile tile in _hitTiles) {
			if(tile.unit.baseUnit != null) {
				int totalDamage = damage;
				int blockedAmount = 0;

				foreach(Effect effect in tile.unit.baseUnit.effects) {
					switch(effect.effectType) {
						case Effect.EffectType.Block:
							if(effect.deactivationConditions.ContainsKey(Effect.Conditions.BlockDamage)) {
								blockedAmount = effect.deactivationConditions[Effect.Conditions.BlockDamage];
							}

							Debug.Log(blockedAmount + " block");

							if(totalDamage <= blockedAmount) {
								blockedAmount = totalDamage;
								totalDamage = 0;
							} else {
								totalDamage -= blockedAmount;
							}

							
						break;
					}
				}

				tile.unit.baseUnit.TickStatusImmediate(Effect.Conditions.BlockDamage, blockedAmount);
				//Debug.Log(effect.deactivationConditions[Effect.Conditions.BlockDamage] + " block remaining");
					
				if(sound) {
					if(totalDamage > 0) {
						PlayDamageSound(tile.position);
					} else {
						totalDamage = 0;
						PlayBlockSound(tile.position);
					}
				}
				
				tile.unit.baseUnit.TickStatusImmediate(Effect.Conditions.ReceiveDamage, totalDamage);
				tile.unit.baseUnit.ReceiveDamage(_caster, totalDamage, _damageType);

				
				// Deactivate effects that require "deal damage" for deactivation.
				_caster.TickStatusImmediate(Effect.Conditions.DealDamage);
			}
		}
	}

	public void ApplyStatus(Effect e, bool sound = false) {
		foreach(Tile tile in _hitTiles) {
			if(tile.unit.baseUnit != null) {
				if(sound) {
					PlayDamageSound(tile.position);
				}
				
				//int damage = (int)e.GetPotency(_caster.attributes);
				tile.unit.baseUnit.ReceiveStatus(this, e);
				//tile.unit.baseUnit.ReceiveDamage(_caster, damage, _damageType);
			}
		}
	}

	float OnCast() {
		float spellDuration = 0.25f;

		if(_createsProjectile) {
			for(int i = 0; i < _projCount; i++) {
				spellDuration += _projPreSpawnDelay;
				spellDuration += _projPostSpawnDelay;
			}

			float dist = CheckTrueDistance(_caster.tile.position, _effectOrigin);
			spellDuration += dist/_projSpeed;
			//Debug.Log(dist/_projSpeed);
		}

		if(_createsEffect) {
			spellDuration += 1f;
			spellDuration += _effectPreSpawnDelay;
			spellDuration += _effectDamageDelay;
		}

		if(_caster.tile.unit != null) {
			if(_caster.playerControlled) {
				_caster.tile.unit.SetInputCooldown(_caster, spellDuration, _autoRecast);
			}
		}
		
		switch(_preset) {
			case Preset.Move:
				Move();
			break;
		}

		return spellDuration;
	}

	public void SyncWithCaster(BaseUnit caster) {
		if(_caster != null) {
			CreateFromPreset(_preset, true);
		} else {
			if(caster != null) {
				_caster = caster;
				_tiles = _caster.tile.dungeonManager.tiles;
				CreateFromPreset(_preset);
			}
		}
	}

	void Move() {
		_caster.Move(_effectOrigin.x - _caster.tile.position.x, _effectOrigin.y - _caster.tile.position.y);

		if(_caster.inCombat) {
			_essenceCost = 1;
			/* 
			if(_chargesRemaining > 0) {
				_chargesRemaining -= 1;
				_essenceCost = 0;
			} else {
				_chargesRemaining = _chargesMax;
				_essenceCost = 1;
			}*/
		} else {
			_essenceCost = 0;
		}
	}


}
