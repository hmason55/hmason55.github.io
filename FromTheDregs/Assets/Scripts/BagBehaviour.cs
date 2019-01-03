using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BagBehaviour : MonoBehaviour {

	public enum Actions {
		Equip,
		Consume,
		Sell,
		Destroy
	}

	SpriteManager _spriteManager;
	BaseUnit _baseUnit;

	[SerializeField] GameObject _bagEquipmentPanel;
	[SerializeField] GameObject _bagPanel;
	[SerializeField] GameObject _bagItemPanel;
	[SerializeField] GameObject _bagSlotGrid;


	[SerializeField] Text _itemInfoTitle;
	[SerializeField] Text _itemInfoCategory;
	[SerializeField] Text _itemInfoAttributeCategories;
	[SerializeField] Text _itemInfoAttributeValues;
	[SerializeField] Text _equipmentAttributeCategories;
	[SerializeField] Text _equipmentAttributeValues;

	[SerializeField] BagItemBehaviour _equipmentNeck;
	[SerializeField] BagItemBehaviour _equipmentHead;
	[SerializeField] BagItemBehaviour _equipmentFinger;
	[SerializeField] BagItemBehaviour _equipmentPrimary;
	[SerializeField] BagItemBehaviour _equipmentBody;
	[SerializeField] BagItemBehaviour _equipmentSecondary;
	[SerializeField] BagItemBehaviour _equipmentHands;
	[SerializeField] BagItemBehaviour _equipmentLegs;
	[SerializeField] BagItemBehaviour _equipmentFeet;

	List<BagItemBehaviour> _bagSlots;

	Bag _bag;

	bool _hidden = true;

	public bool hidden {
		get {return _hidden;}
	}
	
	public BaseUnit baseUnit {
		get {return _baseUnit;}
		set {
			_baseUnit = value; 
			SyncUnit();
		}
	}

	public Bag bag {
		get {return _bag;}
		set {_bag = value;}
	}

	void Awake() {
		_spriteManager = GameObject.FindObjectOfType<SpriteManager>();

		_bagSlots = new List<BagItemBehaviour>();
		for(int i = 0; i < _bagSlotGrid.transform.childCount; i++) {
			_bagSlots.Add(_bagSlotGrid.transform.GetChild(i).GetComponent<BagItemBehaviour>());
		}

		UpdateSlotImages();
	}

	void SyncUnit() {
		_bag = _baseUnit.bag;

		for(int i = 0; i < Bag.NUM_SLOTS; i++) {
			_bagSlots[i].item = _bag.items[i];
		}

		_equipmentNeck.item = _bag.neck;
		_equipmentHead.item = _bag.head;
		_equipmentFinger.item = _bag.finger;
		_equipmentPrimary.item = _bag.primary;
		_equipmentBody.item = _bag.body;
		_equipmentSecondary.item = _bag.secondary;
		_equipmentHands.item = _bag.hands;
		_equipmentLegs.item = _bag.legs;
		_equipmentFeet.item = _bag.feet;

		UpdateSlotImages();
		UpdateEquipmentStats();
	}

	void UpdateSlotImages() {
		for(int i = 0; i < _bagSlots.Count; i++) {
			_bagSlots[i].UpdateImage();
		}

		_equipmentNeck.UpdateImage();
		_equipmentHead.UpdateImage();
		_equipmentFinger.UpdateImage();
		_equipmentPrimary.UpdateImage();
		_equipmentBody.UpdateImage();
		_equipmentSecondary.UpdateImage();
		_equipmentHands.UpdateImage();
		_equipmentLegs.UpdateImage();
		_equipmentFeet.UpdateImage();
	}

	public void EquipItem(BagItemBehaviour bagItem) {
		if(bagItem == null) {return;}
		if(bagItem.item == null) {return;}
		if(!_bag.Equip(bagItem.transform.GetSiblingIndex())) {return;}

		BagItemBehaviour equipmentItem = GetEquipmentType(bagItem.item.category);

		// Unequip items of this type from inventory
		foreach(BagItemBehaviour b in _bagSlots) {
			if(b.item != null) {
				if(b.item.category == bagItem.item.category) {
					b.equipped = false;
				}
			}
		}

		equipmentItem.equipped = true;
		bagItem.equipped = true;

		SyncUnit();
	}

	public void SwapItem() {

	}

	public void UnequipItem(BagItemBehaviour bagItem) {
		if(bagItem == null) {return;}
		if(bagItem.item == null) {return;}
		if(!_bag.Unequip(bagItem.item.category)) {return;}

		BagItemBehaviour equipmentItem = GetEquipmentType(bagItem.item.category);

		equipmentItem.equipped = false;

		if(equipmentItem == bagItem) {
			int slot = _bag.FindItemSlot(equipmentItem.item);
			if(slot > -1) {
				_bagSlots[slot].equipped = false;
			}
		} else {
			bagItem.equipped = false;
		}

		SyncUnit();
	}

	BagItemBehaviour GetEquipmentType(BaseItem.Category category) {
		
		switch(category) {
			case BaseItem.Category.Neck_Jewelry:
				return _equipmentNeck;

			case BaseItem.Category.Head_Armor:
				return _equipmentHead;

			case BaseItem.Category.Finger_Jewelry:
				return _equipmentFinger;

			case BaseItem.Category.Primary_Weapon:
				return _equipmentPrimary;

			case BaseItem.Category.Body_Armor:
				return _equipmentBody;

			case BaseItem.Category.Secondary_Weapon:
				return _equipmentSecondary;

			case BaseItem.Category.Hand_Armor:
				return _equipmentHands;

			case BaseItem.Category.Leg_Armor:
				return _equipmentLegs;

			case BaseItem.Category.Foot_Armor:
				return _equipmentFeet;
		}

		return null;
	}

	public void UpdateEquipmentStats() {
		if(_bag == null) {return;}

		string categories = "";

		categories += "Attack\n";
		categories += "Defense\n";
		categories += "\n";
		categories += "Health Total\n";
		categories += "Health Recovery\n";
		categories += "\n";
		categories += "Essence Total\n";
		categories += "Essence Recovery\n";
		categories += "\n";
		categories += "Movement Speed\n";

		string values = "";

		values += _bag.equipmentAttack + "\n";
		values += _bag.equipmentDefense + "\n";
		values += "\n";
		values += _bag.equipmentHealthTotal + "\n";
		values += _bag.equipmentHealthRecovery + "\n";
		values += "\n";
		values += _bag.equipmentEssenceTotal + "\n";
		values += _bag.equipmentEssenceRecovery + "\n";
		values += "\n";
		values += _bag.equipmentMovementSpeed + "\n";
		
		_equipmentAttributeCategories.text = categories;
		_equipmentAttributeValues.text = values;
	}

	public void UpdateItemInfo(BaseItem item) {
		if(item != null) {
			_itemInfoTitle.text = item.NameToString();
			_itemInfoCategory.text = item.CategoryToString();
			_itemInfoAttributeCategories.text = item.AttributeCategoriesToString();
			_itemInfoAttributeValues.text = item.AttributeValuesToString();
		}
		
	}

	public void ToggleUI() {
		if(hidden) {
			ShowUI();
		} else {
			HideUI();
		}
	}

	public void ShowUI() {
		if(_hidden) {
			_bagPanel.SetActive(true);
			_bagEquipmentPanel.SetActive(true);
			_bagItemPanel.SetActive(true);
			_hidden = false;
		}
	}

	public void HideUI() {
		if(!_hidden) {
			_bagPanel.SetActive(false);
			_bagEquipmentPanel.SetActive(false);
			_bagItemPanel.SetActive(false);
			_hidden = true;
		}
	}
}
