using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Hotbar : MonoBehaviour {

	//public List<Spell> spells;
	[SerializeField] TapController _tapController;
	[SerializeField] EssenceUI _essenceUI;
	[SerializeField] CastOptionsUI _castOptionsUI;
	List<Hotkey> _hotkeys;
	BaseUnit _baseUnit;
	Spell _activeSpell;

	public TapController tapController {
		get {return _tapController;}
		set {_tapController = value;}
	}

	public EssenceUI essenceUI {
		get {return _essenceUI;}
		set {_essenceUI = value;}
	}

	public BaseUnit baseUnit {
		set {_baseUnit = value;}
		get {return _baseUnit;}
	}

	public Spell activeSpell {
		get {return _activeSpell;}
		set {_activeSpell = value;}
	}

	public CastOptionsUI castOptionsUI {
		get {return _castOptionsUI;}
	}

	void Awake() {
		InitHotkeys();
	}

	void InitHotkeys() {
		_hotkeys = new List<Hotkey>();
		for(int i = 0; i < transform.childCount; i++) {
			Hotkey hotkey = transform.GetChild(i).GetComponent<Hotkey>();
			if(hotkey != null) {
				_hotkeys.Add(hotkey);
			}
		}
	}

	public void SyncUnit(BaseUnit b) {
		_baseUnit = b;
		for(int i = 0; i < _hotkeys.Count; i++) {
			if(i < _baseUnit.spells.Count) {
				Hotkey hotkey = _hotkeys[i];
				hotkey.gameObject.SetActive(true);
				hotkey.preset = _baseUnit.spells[i];
			} else {
				_hotkeys[i].gameObject.SetActive(false);
			}
		}
	}

	public void CancelPreview() {
		_tapController.image.raycastTarget = true;
		_activeSpell.ResetTiles();
		_activeSpell.DestroyCastParticles();
		_essenceUI.ResetAll();
	}

	public void ReadyCast(Vector2Int position) {
		if(_activeSpell.requireCastConfirmation) {
			_activeSpell.ResetTiles();
			_activeSpell.ShowEffectRange(position);
		} else {
			_activeSpell.effectOrigin = position;
			ConfirmCast(true);
		}
		//_essenceUI.PreviewUsage(_baseUnit.currentEssence, _activeSpell.essenceCost);
	}

	public void ConfirmCast(bool recast = false) {
		_activeSpell.ConfirmSpellCast();
		_baseUnit.Cast(_activeSpell.essenceCost);
		_essenceUI.SetFilledEssence(_baseUnit.currentEssence);
		
		if(!recast || _activeSpell.essenceCost > _baseUnit.currentEssence) {	// Cancel recast if insufficient essence or recast is disabled.
			_activeSpell.ResetTiles();
			_tapController.image.raycastTarget = true;
			_castOptionsUI.HideUI();
		}
	}

}
