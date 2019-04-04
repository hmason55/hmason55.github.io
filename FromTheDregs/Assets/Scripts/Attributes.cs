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
		DireRat,
		DireRatSmall,
		Mage,
		Rogue,
		Slime,
		Spider,
		SpiderSmall,
		Warrior,
		Widow,
		WidowSmall
		
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
    
    /*Private-------------------Accessors---------------------------Get-------------------------------------Set------------------------------*/
    int _baseHitPoints;         
	public int baseHitPoints {
		get {return CalcBaseHP();}
	}
	
    int _baseEssence;           public int baseEssence              {get {return CalcBaseES();}}           	//set {_baseEssence = value;}}
    /*Private-------------------Accessors---------------------------Get-------------------------------------Set------------------------------*/
    int _baseStrength;          public int baseStrength             {get {return _baseStrength;}            set {_baseStrength = value;}}
	int _baseDexterity;         public int baseDexterity            {get {return _baseDexterity;}           set {_baseDexterity = value;}}
	int _baseIntelligence;      public int baseIntelligence         {get {return _baseIntelligence;}        set {_baseIntelligence = value;}}
	int _baseConstitution;      public int baseConstitution         {get {return _baseConstitution;}        set {_baseConstitution = value;  CalcBaseHP();}}
	int _baseWisdom;            public int baseWisdom               {get {return _baseWisdom;}              set {_baseWisdom = value;}}
	int _baseCharisma;          public int baseCharisma             {get {return _baseCharisma;}            set {_baseCharisma = value;}}
	int _baseSpeed;             public int baseSpeed                {get {return _baseSpeed;}               set {_baseSpeed = value;}}
	/*Private-------------------Accessors---------------------------Get-------------------------------------Set------------------------------*/
    int _modHitPoints;
    int _modEssence;
    /*Private-------------------Accessors---------------------------Get-------------------------------------Set------------------------------*/
	int _modStrength;
	int _modDexterity;
	int _modIntelligence;
	int _modConstitution;
	int _modWisdom;
	int _modCharisma;
	int _modSpeed;
    /*Private-------------------Accessors---------------------------Get-------------------------------------Set------------------------------*/
    
	int _strength;
	/// <summary>
	/// A unit's total strength.
	/// </summary>
	/// /// <value>baseStrength + strengthModifiers</value>
	public int strength {
		get {return _baseStrength + _modStrength;}
	}

    int _dexterity;
	/// <summary>
	/// A unit's total dexterity.
	/// </summary>
	/// /// <value>baseDexterity + dexterityModifiers</value>
	public int dexterity {
		get {return _baseDexterity + _modDexterity;}
	}
    int _intelligence;

	/// <summary>
	/// A unit's total intelligence.
	/// </summary>
	/// <value>baseIntelligence + intelligenceModifiers</value>
	public int intelligence {
		get {return _baseIntelligence + _modIntelligence;}
	}

	int _constitution;
	/// <summary>
	/// A unit's total constitution.
	/// </summary>
	/// <value>baseConstitution + constitutionModifiers</value>
	public int constitution	{
		get {return _baseConstitution + _modConstitution;}
	}

    int _wisdom;
	/// <summary>
	/// A unit's total wisdom.
	/// </summary>
	/// <value>baseWisdom + wisdomModifiers</value>
	public int wisdom {
		get {return _baseWisdom + _modWisdom;}
	}

    int _charisma;
	/// <summary>
	/// A unit's total charisma.
	/// </summary>
	/// <value>baseCharisma + charismaModifiers</value>
	public int charisma {
		get {return _baseCharisma + _modCharisma;}
	}

    int _speed;
	/// <summary>
	/// A unit's total speed.
	/// </summary>
	/// <value>baseSpeed + speedModifiers</value>
	public int speed {
		get {return _baseSpeed + _modSpeed;}
	}

	int _level;
	/// <summary>
	/// A unit's combat level is determined by the sum of all other combat attributes.
	/// </summary>
	/// <value></value>
	public int level {
		get {return Level();}
	}

    /*Private-------------------Accessors---------------------------Get-------------------------------------Set------------------------------*/
    int _hpCurrent;             public int hpCurrent                {get {return _hpCurrent;}               set {_hpCurrent = value;}}
    int _hpTotal;               public int hpTotal                  {get {return CalcBaseHP();}}            //set {_hpTotal = value;}}
    int _hpScaling;             public int hpScaling                {get {return _hpScaling;}               set {_hpScaling = value;}}
    /*Private-------------------Accessors---------------------------Get-------------------------------------Set------------------------------*/
    int _esCurrent;             public int esCurrent                {get {return _esCurrent;}               set {_esCurrent = value;}}
    int _esTotal;               public int esTotal                  {get {return _esTotal;}               	set {_esTotal = value;}}
    int _esRecovery;            public int esRecovery               {get {return _esRecovery;}              set {_esRecovery = value;}}
    /*Private-------------------Accessors---------------------------Get-------------------------------------Set------------------------------*/
    int _experience;            public int experience               {get {return _experience;}              set {_experience = value;}}
    Preset _preset;             public Preset preset                {get {return _preset;}}
	Size _size;
    int _alliance;              public int alliance                 {get {return _alliance;}                set {_alliance = value;}}
    int _aggroRadius;           public int aggroRadius              {get {return _aggroRadius;}             set {_aggroRadius = value;}}
    /*Private-------------------Accessors---------------------------Get-------------------------------------Set------------------------------*/
    public Attributes(Preset p) {
        _preset = p;
        AssignPreset();
        Init();
    }

    public Attributes() {
        AssignPreset();
        Init();
    }

    void Init() {
        CalcBaseHP();
        _hpTotal = _baseHitPoints + _modHitPoints;
        _hpCurrent = _hpTotal;

        _baseEssence = 8;
        _esTotal = _baseEssence + _modEssence;
        _esCurrent = _esTotal;
        _esRecovery = 4;

        _experience = 0;
        _aggroRadius = 4;
        
        //UpdateModifiers();
    }

    public void UpdateModifiers() {
        ///_modStrength =      (_baseStrength - 10)        / 2;
		//_modDexterity =     (_baseDexterity - 10)       / 2;
		//_modIntelligence =  (_baseIntelligence - 10)    / 2;
		//_modConstitution =  (_baseConstitution - 10)    / 2;
		//_modWisdom =        (_baseWisdom - 10)          / 2;
		//_modCharisma =      (_baseCharisma - 10)        / 2;
		//_modSpeed =         (_baseDexterity - 10)       / 3;
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

	int CalcBaseHP() {
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

		_baseHitPoints = total;
		return _baseHitPoints;
	}

	int CalcBaseES() {
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
				_baseEssence =      3;
				_hpScaling =        10;	//8 default
				_size = Size.Medium;
            break;

			case Preset.Mage:
				_baseStrength =     1;
				_baseDexterity =    1;
				_baseIntelligence = 2;
				_baseConstitution = 1;
				_baseWisdom =       2;
				_baseCharisma =     1;

				_baseSpeed =        1;
				_baseEssence =      3;
				_size = Size.Medium;
			break;

			case Preset.Rogue:
				_baseStrength =     1;
				_baseDexterity =    2;
				_baseIntelligence = 1;
				_baseConstitution = 1;
				_baseWisdom =       1;
				_baseCharisma =     2;

				_baseSpeed =        1;
				_baseEssence =      3;
				_size = Size.Medium;
			break;

			case Preset.Warrior:
				_baseStrength =     2;
				_baseDexterity =    1;
				_baseIntelligence = 1;
				_baseConstitution = 2;
				_baseWisdom =       1;
				_baseCharisma =     1;

				_baseSpeed =        1;
				_baseEssence =      3;
				_size = Size.Medium;
			break;

			case Attributes.Preset.Human:
				_baseStrength =     10;
				_baseDexterity =    10;
				_baseIntelligence = 10;
				_baseConstitution = 10;
				_baseWisdom =       10;
				_baseCharisma =     10;
				_baseSpeed =        10;
				_baseHitPoints = 	10;
				_baseEssence =      8;
				_hpScaling =        50;	//8 default
				_size = Size.Medium;
			break;

			case Attributes.Preset.Slime:
				_baseStrength = 10;
				_baseDexterity = 15;
				_baseIntelligence = 0;
				_baseConstitution = 12;
				_baseWisdom = 10;
				_baseCharisma = 2;
				_baseSpeed = 5;
				_baseEssence = 6;
				_hpScaling = 16;
				_size = Size.Small;
				_aggroRadius = 6;
			break;

			case Attributes.Preset.Spider:
				_baseStrength = 11;
				_baseDexterity = 17;
				_baseIntelligence = 0;
				_baseConstitution = 12;
				_baseWisdom = 10;
				_baseCharisma = 2;
				_baseSpeed = 5;
				_baseEssence = 6;
				_hpScaling = 16;
				_size = Size.Medium;
			break;

			case Attributes.Preset.SpiderSmall:
				_baseStrength = 10;
				_baseDexterity = 15;
				_baseIntelligence = 0;
				_baseConstitution = 12;
				_baseWisdom = 10;
				_baseCharisma = 2;
				_baseSpeed = 5;
				_baseEssence = 6;
				_hpScaling = 11;
				_size = Size.Small;
			break;

			case Attributes.Preset.Widow:
				_baseStrength = 11;
				_baseDexterity = 17;
				_baseIntelligence = 0;
				_baseConstitution = 12;
				_baseWisdom = 10;
				_baseCharisma = 2;
				_baseSpeed = 5;
				_baseEssence = 6;
				_hpScaling = 16;
				_size = Size.Medium;
			break;

			case Attributes.Preset.WidowSmall:
				_baseStrength = 10;
				_baseDexterity = 15;
				_baseIntelligence = 0;
				_baseConstitution = 12;
				_baseWisdom = 10;
				_baseCharisma = 2;
				_baseSpeed = 5;
				_baseEssence = 6;
				_hpScaling = 11;
				_size = Size.Small;
			break;
		}
    }
}
