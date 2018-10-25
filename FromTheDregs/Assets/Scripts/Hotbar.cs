using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Hotbar : MonoBehaviour {

	//public List<Spell> spells;
	[SerializeField] TapController _tapController;
	[SerializeField] EssenceUI _essenceUI;
	[SerializeField] CastOptionsUI _castOptionsUI;
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

	public void CancelPreview() {
		_tapController.image.raycastTarget = true;
		_activeSpell.ResetTiles();
		_essenceUI.ResetAll();
	}

	public void ReadyCast(Vector2Int position) {
		_activeSpell.ResetTiles();
		_activeSpell.ShowEffectRange(position);
		//_essenceUI.PreviewUsage(_baseUnit.currentEssence, _activeSpell.essenceCost);
	}

	public void ConfirmCast() {
		_activeSpell.ResetTiles();
		_activeSpell.ConfirmSpellCast();
		_baseUnit.Cast(_activeSpell.essenceCost);
		_essenceUI.SetFilledEssence(_baseUnit.currentEssence);
		_tapController.image.raycastTarget = true;
		_castOptionsUI.HideUI();
	}

}
