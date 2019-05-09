using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A unit's attributes determine its combat effectiveness.
/// </summary>
[System.Serializable]
public class Attributes {

    public enum Preset {
        None,
		Human,
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

    int _baseHealth;         
	public int baseHealth {
		get {return CalcBaseHealth();}
	}
	
    int _baseEssence;
	public int baseEssence {
		get {return CalcBaseEssence();}
		//set {_baseEssence = value;}}
    }

    int _baseStrength;
	public int baseStrength {
		get {return _baseStrength;}
		set {_baseStrength = value;}
	}

	int _baseDexterity;
	public int baseDexterity {
		get {return _baseDexterity;}
		set {_baseDexterity = value;}
	}

	int _baseIntelligence;
	public int baseIntelligence {
		get {return _baseIntelligence;}
		set {_baseIntelligence = value;}
	}

	int _baseConstitution;
	public int baseConstitution {
		get {return _baseConstitution;}
		set {
			_baseConstitution = value;
			CalcBaseHealth();
		}
	}

	int _baseWisdom;
	public int baseWisdom {
		get {return _baseWisdom;}
		set {_baseWisdom = value;}
	
	}

	int _baseCharisma;
	public int baseCharisma {
		get {return _baseCharisma;}
		set {_baseCharisma = value;}
	}

	int _baseSpeed;
	public int baseSpeed {
		get {return _baseSpeed;}
		set {_baseSpeed = value;}
	}

	bool _overrideHealthScaling;
	public bool overrideHealthScaling {
		get {return _overrideHealthScaling;}
		set {_overrideHealthScaling = value;}
	}

	bool _overrideEssenceScaling;
	public bool overrideEssenceScaling {
		get {return _overrideEssenceScaling;}
		set {_overrideEssenceScaling = value;}
	}
    
	int _strength;
	/// <summary>
	/// A unit's total strength.
	/// </summary>
	/// /// <value>baseStrength + strengthModifiers</value>
	public int strength {
		get {return _baseStrength;}
	}

    int _dexterity;
	/// <summary>
	/// A unit's total dexterity.
	/// </summary>
	/// /// <value>baseDexterity + dexterityModifiers</value>
	public int dexterity {
		get {return _baseDexterity;}
	}
    int _intelligence;

	/// <summary>
	/// A unit's total intelligence.
	/// </summary>
	/// <value>baseIntelligence + intelligenceModifiers</value>
	public int intelligence {
		get {return _baseIntelligence;}
	}

	int _constitution;
	/// <summary>
	/// A unit's total constitution.
	/// </summary>
	/// <value>baseConstitution + constitutionModifiers</value>
	public int constitution	{
		get {return _baseConstitution;}
	}

    int _wisdom;
	/// <summary>
	/// A unit's total wisdom.
	/// </summary>
	/// <value>baseWisdom + wisdomModifiers</value>
	public int wisdom {
		get {return _baseWisdom;}
	}

    int _charisma;
	/// <summary>
	/// A unit's total charisma.
	/// </summary>
	/// <value>baseCharisma + charismaModifiers</value>
	public int charisma {
		get {return _baseCharisma;}
	}

    int _speed;
	/// <summary>
	/// A unit's total speed.
	/// </summary>
	/// <value>baseSpeed + speedModifiers</value>
	public int speed {
		get {return _baseSpeed;}
	}

	int _level;
	/// <summary>
	/// A unit's combat level is determined by the sum of all other combat attributes.
	/// </summary>
	/// <value></value>
	public int level {
		get {return Level();}
	}

    int _currentHealth;
	public int currentHealth {
		get {return _currentHealth;}
		set {
				_currentHealth = value;
				if(_currentHealth > _totalHealth) {
					_currentHealth = _totalHealth;
				}
			}
	}

    int _totalHealth;
	public int totalHealth {
		get {return CalcBaseHealth();}
		//set {_hpTotal = value;}}
	}            

    int _currentEssence;
	public int currentEssence {
		get {return _currentEssence;}
		set {_currentEssence = value;}
	}

    int _totalEssence;
	public int totalEssence {
		get {return _totalEssence;}
		set {_totalEssence = value;}
	}

    int _recoveryEssence;
	public int recoveryEssence {
		get {return _recoveryEssence;}
		set {_recoveryEssence = value;}
	}
    
    int _experience;
	public int experience {
		get {return _experience;}
		set {_experience = value;}
	}

    Preset _preset;
	public Preset preset {
		get {return _preset;}
	}

    int _alliance;
	public int alliance {
		get {return _alliance;}
		set {_alliance = value;}
	}

    int _aggroRadius;
	public int aggroRadius {
		get {return _aggroRadius;}
		set {_aggroRadius = value;}
	}
    
	
    public Attributes(Preset p) {
        _preset = p;
        AssignPreset();
        Init();
    }

    void Init() {
		CalcBaseHealth();
		CalcBaseEssence();
		
        _totalHealth = _baseHealth;
        _currentHealth = _totalHealth;

        _totalEssence = _baseEssence;
        _currentEssence = _totalEssence;

        _recoveryEssence = _totalEssence;
    }

	int Level() {

		int l = _baseStrength + 
				_baseDexterity + 
				_baseIntelligence + 
				_baseConstitution + 
				_baseWisdom +
				_baseCharisma
				-7;

		if(l < 1) {return 1;}
		return l;
	}

	int CalcBaseHealth() {
		if(_overrideHealthScaling) {return _baseHealth;}

		float a = 30f;
		float b = 0.5f;
		float threshold = 15f;

		int total = Mathf.RoundToInt(a);
		for(int i = 1; i < _baseConstitution; i++) {
			float delta = a - Mathf.Pow(constitution, b);

			if(delta <= threshold) {
				delta = threshold;
			}

			total += Mathf.RoundToInt(delta);
		}

		_baseHealth = total;
		return _baseHealth;
	}

	int CalcBaseEssence() {
		if(_overrideEssenceScaling) {return _baseEssence;}

		float a = 0.25f;
		float b = 0.005f;
		float threshold = 0.1f;

		int total = 4+Mathf.RoundToInt(a);
		for(int i = 1; i < level; i++) {
			float delta = a - wisdom*b;

			if(delta <= threshold) {
				delta = threshold;
			}

			total += Mathf.RoundToInt(delta);
		}

		_baseEssence = total;
		return _baseEssence;
	}

	public int ToNextLevel() {
		float a = 25f;
		float b = 2.05f;
		return Mathf.RoundToInt(a + Mathf.Pow(level+1, b));
	}

    void AssignPreset() {
        switch(_preset) {
            case Preset.None:
                _baseStrength =     1;
				_baseDexterity =    1;
				_baseIntelligence = 1;
				_baseConstitution = 1;
				_baseWisdom =       1;
				_baseCharisma =     1;

				_baseSpeed =        1;
				_baseEssence =      4;
				_aggroRadius = 		4;
            break;

			case Attributes.Preset.Giant_Spider:
				_baseStrength =     8;
				_baseDexterity =    9;
				_baseIntelligence = 1;
				_baseConstitution = 8;
				_baseWisdom =       1;
				_baseCharisma =     1;

				_overrideHealthScaling = true;
				_baseHealth = 120;

				_baseSpeed =        3;
				_baseEssence =      3;
				_aggroRadius = 		4;
			break;

			case Attributes.Preset.Giant_Widow:
				_baseStrength =     8;
				_baseDexterity =    10;
				_baseIntelligence = 1;
				_baseConstitution = 10;
				_baseWisdom =       1;
				_baseCharisma =     1;

				_overrideHealthScaling = true;
				_baseHealth = 180;

				_baseSpeed =        3;
				_baseEssence =      3;
				_aggroRadius = 		4;
			break;


			case Preset.Mage:
				_baseStrength =     1;
				_baseDexterity =    1;
				_baseIntelligence = 2;
				_baseConstitution = 1;
				_baseWisdom =       2;
				_baseCharisma =     1;

				_baseSpeed =        1;
				_baseEssence =      4;
				_aggroRadius = 		4;
			break;

			case Preset.Rogue:
				_baseStrength =     1;
				_baseDexterity =    2;
				_baseIntelligence = 1;
				_baseConstitution = 1;
				_baseWisdom =       1;
				_baseCharisma =     2;

				_baseSpeed =        1;
				_baseEssence =      4;
				_aggroRadius = 		4;
			break;

			case Preset.Warrior:
				_baseStrength =     2;
				_baseDexterity =    1;
				_baseIntelligence = 1;
				_baseConstitution = 2;
				_baseWisdom =       1;
				_baseCharisma =     1;

				_baseSpeed =        1;
				_baseEssence =      4;
				_aggroRadius = 		4;
			break;

			case Attributes.Preset.Human:
				_baseStrength =     1;
				_baseDexterity =    1;
				_baseIntelligence = 1;
				_baseConstitution = 1;
				_baseWisdom =       1;
				_baseCharisma =     1;

				_baseSpeed =        1;
				_baseEssence =      4;
				_aggroRadius = 		4;
			break;

			case Attributes.Preset.Skeleton:
				_baseStrength =     2;
				_baseDexterity =    4;
				_baseIntelligence = 1;
				_baseConstitution = 1;
				_baseWisdom =       1;
				_baseCharisma =     1;

				_overrideHealthScaling = true;
				_baseHealth = 40;

				_baseSpeed =        3;
				_baseEssence =      2;
				_aggroRadius = 		3;
			break;

			case Attributes.Preset.Skeleton_Summoner:
				_baseStrength =     1;
				_baseDexterity =    2;
				_baseIntelligence = 10;
				_baseConstitution = 4;
				_baseWisdom =       1;
				_baseCharisma =     1;

				_overrideHealthScaling = true;
				_baseHealth = 90;

				_baseSpeed =        2;
				_baseEssence =      2;
				_aggroRadius = 		3;
			break;

			case Attributes.Preset.Skeleton_Thrall:
				_baseStrength =     2;
				_baseDexterity =    4;
				_baseIntelligence = 1;
				_baseConstitution = 1;
				_baseWisdom =       1;
				_baseCharisma =     1;

				_overrideHealthScaling = true;
				_baseHealth = 25;

				_baseSpeed =        2;
				_baseEssence =      2;
				_aggroRadius = 		3;
			break;

			case Attributes.Preset.Skeleton_Warrior:
				_baseStrength =     6;
				_baseDexterity =    5;
				_baseIntelligence = 1;
				_baseConstitution = 4;
				_baseWisdom =       1;
				_baseCharisma =     1;

				_overrideHealthScaling = true;
				_baseHealth = 120;

				_baseSpeed =        2;
				_baseEssence =      2;
				_aggroRadius = 		3;
			break;

			case Attributes.Preset.Green_Slime:
				_baseStrength =     2;
				_baseDexterity =    1;
				_baseIntelligence = 1;
				_baseConstitution = 2;
				_baseWisdom =       1;
				_baseCharisma =     1;

				_overrideHealthScaling = true;
				_baseHealth = 120;

				_baseSpeed =        1;
				_baseEssence =      2;
				_aggroRadius = 		2;
			break;

			case Attributes.Preset.Spiderling:
				_baseStrength =     2;
				_baseDexterity =    4;
				_baseIntelligence = 1;
				_baseConstitution = 3;
				_baseWisdom =       1;
				_baseCharisma =     1;

				_overrideHealthScaling = true;
				_baseHealth = 40;

				_baseSpeed =        3;
				_baseEssence =      3;
				_aggroRadius = 		4;
			break;


			case Attributes.Preset.Widowling:
				_baseStrength =     2;
				_baseDexterity =    4;
				_baseIntelligence = 1;
				_baseConstitution = 3;
				_baseWisdom =       1;
				_baseCharisma =     1;

				_overrideHealthScaling = true;
				_baseHealth = 50;

				_baseSpeed =        3;
				_baseEssence =      3;
				_aggroRadius = 		4;
			break;
		}
    }
}
