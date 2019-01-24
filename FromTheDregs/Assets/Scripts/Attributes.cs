using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attributes {
    public enum Preset {
        None,
		Human,
		DireRat,
		DireRatSmall,
		Slime,
		Spider,
		SpiderSmall,
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
    int _baseHitPoints;         public int baseHitPoints            {get {return _baseHitPoints;}           set {_baseHitPoints = value;}}
    int _baseEssence;           public int baseEssence              {get {return _baseEssence;}             set {_baseEssence = value;}}
    /*Private-------------------Accessors---------------------------Get-------------------------------------Set------------------------------*/
    int _baseStrength;          public int baseStrength             {get {return _baseStrength;}            set {_baseStrength = value;}}
	int _baseDexterity;         public int baseDexterity            {get {return _baseDexterity;}           set {_baseDexterity = value;}}
	int _baseIntelligence;      public int baseIntelligence         {get {return _baseIntelligence;}        set {_baseIntelligence = value;}}
	int _baseConstitution;      public int baseConstitution         {get {return _baseConstitution;}        set {_baseConstitution = value;}}
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
    int _strength;              public int strength                 {get {return _baseStrength + _modStrength;}}
    int _dexterity;             public int dexterity                {get {return _baseDexterity + _modDexterity;}}
    int _intelligence;          public int intelligence             {get {return _baseIntelligence + _modIntelligence;}}
    int _wisdom;                public int wisdom                   {get {return _baseWisdom + _modWisdom;}}
    int _charisma;              public int charisma                 {get {return _baseCharisma + _modCharisma;}}
    int _speed;                 public int speed                    {get {return _baseSpeed + _modSpeed;}}
    /*Private-------------------Accessors---------------------------Get-------------------------------------Set------------------------------*/
    int _hpCurrent;             public int hpCurrent                {get {return _hpCurrent;}               set {_hpCurrent = value;}}
    int _hpTotal;               public int hpTotal                  {get {return _hpTotal;}                 set {_hpTotal = value;}}
    int _hpScaling;             public int hpScaling                {get {return _hpScaling;}               set {_hpScaling = value;}}
    /*Private-------------------Accessors---------------------------Get-------------------------------------Set------------------------------*/
    int _esCurrent;             public int esCurrent                {get {return _esCurrent;}               set {_esCurrent = value;}}
    int _esTotal;               public int esTotal                  {get {return _esTotal;}               set {_esTotal = value;}}
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
        _baseHitPoints = _baseConstitution;
        _hpTotal = _baseHitPoints + _modHitPoints + _hpScaling;
        _hpCurrent = _hpTotal;

        _baseEssence = 4;
        _esTotal = _baseEssence + _modEssence;
        _esCurrent = _esTotal;
        _esRecovery = 4;

        _experience = 0;
        _aggroRadius = 4;
        
        //UpdateModifiers();
    }

    public void UpdateModifiers() {
        _modStrength =      (_baseStrength - 10)        / 2;
		_modDexterity =     (_baseDexterity - 10)       / 2;
		_modIntelligence =  (_baseIntelligence - 10)    / 2;
		_modConstitution =  (_baseConstitution - 10)    / 2;
		_modWisdom =        (_baseWisdom - 10)          / 2;
		_modCharisma =      (_baseCharisma - 10)        / 2;
		_modSpeed =         (_baseDexterity - 10)       / 3;
    }

    void AssignPreset() {
        switch(_preset) {
            case Preset.None:
                _baseStrength =     10;
				_baseDexterity =    10;
				_baseIntelligence = 10;
				_baseConstitution = 10;
				_baseWisdom =       10;
				_baseCharisma =     10;
				_baseSpeed =        2;
				_baseEssence =      4;
				_hpScaling =        10;	//8 default
				_size = Size.Medium;
            break;

			case Attributes.Preset.Human:
				_baseStrength =     10;
				_baseDexterity =    10;
				_baseIntelligence = 10;
				_baseConstitution = 10;
				_baseWisdom =       10;
				_baseCharisma =     10;
				_baseSpeed =        2;
				_baseEssence =      4;
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
				_baseEssence = 2;
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
				_baseEssence = 2;
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
				_baseEssence = 2;
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
				_baseEssence = 2;
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
				_baseEssence = 2;
				_hpScaling = 11;
				_size = Size.Small;
			break;
		}
    }
}
