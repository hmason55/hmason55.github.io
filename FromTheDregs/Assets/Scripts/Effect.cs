using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect {

    public enum Preset {
    }

    public enum Conditions {
        BlockDamage,
        CastSpell,
        Cleanse,
        DealDamage,
        DurationExpire,
        EvadeSpell,
        KillSelf,
        KillTarget,
        PostTurnExpiration,
        ReceiveDamage,
    }

    public enum EffectType {
        CriticalDamage,
        Damage,
        DisplayHealth,
        InescapableDamage,
        Bleed,
        Block,
        Burn,
        Evade,
        Focus,
        Poison,
        Stun,
        UnblockableDamage,
    }

    public enum ScalingType {
        None,
        Strength,
        Dexterity,
        Intelligence,
        Block,
        PercentRemainingHP,
    }

    Dictionary<Conditions, int> _activationConditions;       public Dictionary<Conditions, int> activationConditions         {get{return _activationConditions;}}
    Dictionary<Conditions, int> _deactivationConditions;     public Dictionary<Conditions, int> deactivationConditions       {get{return _deactivationConditions;}}

    EffectType _effectType = EffectType.Damage;              public EffectType effectType      {get{return _effectType;}                set{_effectType = value;}}
    int _duration = 0;
    bool _stackable = false;                    public bool stackable                          {get{return _stackable;}                 set{_stackable = value;}}

    float _basePotency = 1.0f;                  public float basePotency                       {get{return _basePotency;}               set{_basePotency = value;}}

    int _currentHealth;
    public int currentHealth {
        get {return _currentHealth;}
        set {_currentHealth = value;}
    }

    int _totalHealth;
    public int totalHealth {
        get {return _totalHealth;}
        set {_totalHealth = value;}
    }

    int _initialPhysicalDamage;
    public int initialPhysicalDamage {
        get {return _initialPhysicalDamage;}
        set {_initialPhysicalDamage = value;}
    }

    int _initialSpellDamage;
    public int initialSpellDamage {
        get {return _initialSpellDamage;}
        set {_initialSpellDamage = value;}
    }

    int _initialBlockModifier;
    public int initialBlockModifier {
        get {return _initialBlockModifier;}
        set {_initialBlockModifier = value;}
    }

    int _initialPoisonModifier;
    public int initialPoisonModifier {
        get {return _initialPoisonModifier;}
        set {_initialPoisonModifier = value;}
    }

    int _initialBleedModifier;
    public int initialBleedModifier {
        get {return _initialBleedModifier;}
        set {_initialBleedModifier = value;}
    }
    
    ScalingType _primaryScalingType;            public ScalingType primaryScalingType          {get{return _primaryScalingType;}        set{_primaryScalingType = value;}}
    ScalingType _secondaryScalingType;          public ScalingType secondaryScalingType        {get{return _secondaryScalingType;}      set{_secondaryScalingType = value;}}
    ScalingType _tertiaryScalingType;           public ScalingType tertiaryScalingType         {get{return _tertiaryScalingType;}       set{_tertiaryScalingType = value;}}

    float _primaryScalingValue;                 public float primaryScalingValue               {get{return _primaryScalingValue;}       set{_primaryScalingValue = value;}}
    float _secondaryScalingValue;               public float secondaryScalingValue             {get{return _secondaryScalingValue;}     set{_secondaryScalingValue = value;}}
    float _tertiaryScalingValue;                public float tertiaryScalingValue              {get{return _tertiaryScalingValue;}      set{_tertiaryScalingValue = value;}}

    public Effect() {
        _activationConditions = new Dictionary<Conditions, int>();
        _deactivationConditions = new Dictionary<Conditions, int>();
    }

    public Effect(EffectType e, int d = 0) {
        _effectType = e;
        _duration = d;
        _activationConditions = new Dictionary<Conditions, int>();
        _deactivationConditions = new Dictionary<Conditions, int>();
    }

    public Effect(Preset p) {
        _activationConditions = new Dictionary<Conditions, int>();
        _deactivationConditions = new Dictionary<Conditions, int>();
        LoadPreset(p);
    }

    void LoadPreset(Preset p) {

    }

    public void SetPrimaryScaling(ScalingType type, float value) {
        _primaryScalingType = type;
        _primaryScalingValue = value;
    }

    public void SetSecondaryScaling(ScalingType type, float value) {
        _secondaryScalingType = type;
        _secondaryScalingValue = value;
    }

    public void SetTertiaryScaling(ScalingType type, float value) {
        _tertiaryScalingType = type;
        _tertiaryScalingValue = value;
    }

    public float GetPotency(Attributes attribs) {
        float totalPotency = _basePotency;

        switch(_primaryScalingType) {
            case ScalingType.Strength: 
                totalPotency += _primaryScalingValue * attribs.strength;
            break;

            case ScalingType.Dexterity:
                totalPotency += _primaryScalingValue * attribs.dexterity;
            break;

            case ScalingType.Intelligence:
                totalPotency += _primaryScalingValue * attribs.intelligence;
            break;
        }

        switch(_secondaryScalingType) {
            case ScalingType.Strength:
                totalPotency += _secondaryScalingValue * attribs.strength;
            break;

            case ScalingType.Dexterity:
                totalPotency += _secondaryScalingValue * attribs.dexterity;
            break;

            case ScalingType.Intelligence:
                totalPotency += _secondaryScalingValue * attribs.intelligence;
            break;
        }

        switch(_tertiaryScalingType) {
            case ScalingType.Strength:
                totalPotency += _tertiaryScalingValue * attribs.strength;
            break;
            
            case ScalingType.Dexterity:
                totalPotency += _tertiaryScalingValue * attribs.dexterity;
            break;

            case ScalingType.Intelligence:
                totalPotency += _tertiaryScalingValue * attribs.intelligence;
            break;
        }
        
        return totalPotency;
    }

    public bool Apply(BaseUnit target) {
        if(target == null) {return false;}

        bool applied = false;
        foreach(Effect effect in target.effects) {
            if(effect.effectType == _effectType) {
                if(effect.stackable && _stackable) {     // If stackable, stack
                    switch(effect.effectType) {

                        case EffectType.Bleed:
                            int bleedValue = _initialBleedModifier + _deactivationConditions[Conditions.DurationExpire];
                            if(effect.deactivationConditions.ContainsKey(Conditions.DurationExpire)) {
                                effect.deactivationConditions[Conditions.DurationExpire] += bleedValue;
                            }
                            applied = true;
                            //Debug.Log(effect.deactivationConditions[Conditions.DurationExpire] + " bleed");
                        break;

                        case EffectType.Block:
                            int blockValue = _initialBlockModifier + (int)effect.GetPotency(target.attributes);
                            if(effect.deactivationConditions.ContainsKey(Conditions.BlockDamage)) {
                                effect.deactivationConditions[Conditions.BlockDamage] += blockValue;
                            } 
                            applied = true;
                            //Debug.Log(effect.deactivationConditions[Conditions.BlockDamage] + " block");
                        break;

                        case EffectType.Poison:
                            int poisonValue = _initialPoisonModifier + _deactivationConditions[Conditions.DurationExpire];
                            if(effect.deactivationConditions.ContainsKey(Conditions.DurationExpire)) {
                                effect.deactivationConditions[Conditions.DurationExpire] += poisonValue;
                            }
                            applied = true;
                            //Debug.Log(effect.deactivationConditions[Conditions.DurationExpire] + " poison");
                        break;
                    }
                    return applied;
                } else {
                    return applied;
                }
            }
        }
        
        if(!applied) {
            switch(_effectType) {
                case EffectType.Bleed:
                    int bleedValue = _initialBleedModifier + _deactivationConditions[Conditions.DurationExpire];
                    _deactivationConditions[Conditions.DurationExpire] = bleedValue;
                    applied = true;
                    //Debug.Log(_deactivationConditions[Conditions.DurationExpire] + " bleed");
                break;

                case EffectType.Block:
                    int blockValue = _initialBlockModifier + (int)GetPotency(target.attributes);
                    _deactivationConditions[Conditions.BlockDamage] = blockValue;
                    applied = true;
                    //Debug.Log(_deactivationConditions[Conditions.BlockDamage] + " block");
                break;

                case EffectType.DisplayHealth:
                    
                    applied = true;
                break;

                case EffectType.Poison:
                    int poisonValue = _initialPoisonModifier + _deactivationConditions[Conditions.DurationExpire];
                    _deactivationConditions[Conditions.DurationExpire] = poisonValue;
                    applied = true;
                    //Debug.Log(_deactivationConditions[Conditions.DurationExpire] + " poison");
                break;

                case EffectType.Stun:
                    int stunValue = 1;
                    _deactivationConditions[Conditions.PostTurnExpiration] = stunValue;
                    applied = true;
                    //Debug.Log(_deactivationConditions[Conditions.PostTurnExpiration] + " stun");
                break;
            }
        }

        target.effects.Add(this);
        return applied;
    }


}
