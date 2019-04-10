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
        DurationExpire,
        EvadeSpell,
        KillSelf,
        KillTarget,
        ReceiveDamage,
        DealDamage,
    }

    public enum EffectType {
        Damage,
        CriticalDamage,
        InescapableDamage,
        UnblockableDamage,
        Bleed,
        Block,
        Burn,
        Evade,
        Focus,
        Stun,
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
        switch(p) {

        }
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

        bool applied = false;
        foreach(Effect e in target.effects) {
            if(e.effectType == _effectType) {
                if(e.stackable && _stackable) {     // If stackable, stack
                    switch(e.effectType) {
                        case EffectType.Block:
                            int blockValue = _initialBlockModifier + (int)e.GetPotency(target.attributes);
                            if(e.deactivationConditions.ContainsKey(Conditions.BlockDamage)) {
                                e.deactivationConditions[Conditions.BlockDamage] += blockValue;
                            } 
                            applied = true;
                            Debug.Log(e.deactivationConditions[Conditions.BlockDamage] + " block");
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
                case EffectType.Block:
                    int blockValue = _initialBlockModifier + (int)GetPotency(target.attributes);
                    _deactivationConditions[Conditions.BlockDamage] = blockValue;
                    applied = true;
                    Debug.Log(_deactivationConditions[Conditions.BlockDamage] + " block");
                break;
            }
        }

        target.effects.Add(this);
        return applied;
    }


}
