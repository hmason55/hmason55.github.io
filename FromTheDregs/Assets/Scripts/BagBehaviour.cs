using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BagBehaviour : MonoBehaviour {

	public enum Actions {
		Destroy,
		Give,
		Sell,
		Use
	}

	SpriteManager _spriteManager;
	BaseUnit _baseUnit;

	[SerializeField] Actions _defaultAction = Actions.Use;

	[SerializeField] GameObject _bagEquipmentPanel;
	[SerializeField] GameObject _bagPanel;
	[SerializeField] GameObject _bagSlotGrid;
	
	[SerializeField] Text _titleText;

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

	public Actions defaultAction {
		get {return _defaultAction;}
		set {_defaultAction = value;}
	}

	void Awake() {
		_spriteManager = GameObject.FindObjectOfType<SpriteManager>();

		_bagSlots = new List<BagItemBehaviour>();
		for(int i = 0; i < _bagSlotGrid.transform.childCount; i++) {
			_bagSlots.Add(_bagSlotGrid.transform.GetChild(i).GetComponent<BagItemBehaviour>());
		}
		
		UpdateSlotImages();
	}

	public void SyncEquipment() {

		BagItemBehaviour[] equippedItems = {
			_equipmentNeck,
			_equipmentHead,
			_equipmentFinger,
			_equipmentPrimary,
			_equipmentSecondary,
			_equipmentHands,
			_equipmentLegs,
			_equipmentFeet
		};

		for(int i = Bag.BAG_SLOTS-1; i >= 0; i--) {
			if(_bagSlots[i].item != null) {

				for(int j = 0; j < equippedItems.Length; j++) {
					if(equippedItems[j].item != null) {
						if(_bagSlots[i].item.id == equippedItems[j].item.id) {
							EquipItem(_bagSlots[i]);
						}
					}
				}

			}
		}
	}

	public void SyncUnit() {
		_bag = _baseUnit.bag;

		for(int i = 0; i < Bag.BAG_SLOTS; i++) {
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
		_baseUnit.SyncHotbar();
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
					b.equipped = -1;
				}
			}
		}

		equipmentItem.equipped = bagItem.transform.GetSiblingIndex();
		bagItem.equipped = bagItem.transform.GetSiblingIndex();
		_baseUnit.UpdateSpells();
		SyncUnit();
	}

	public void SwapItem() {

	}

	public void UnequipItem(BagItemBehaviour bagItem) {
		if(bagItem == null) {return;}
		if(bagItem.item == null) {return;}
		if(!_bag.Unequip(bagItem.item.category)) {return;}

		BagItemBehaviour equipmentItem = GetEquipmentType(bagItem.item.category);

		equipmentItem.equipped = -1;

		if(equipmentItem == bagItem) {
			int slot = _bag.FindItemSlot(equipmentItem.item);
			if(slot > -1) {
				_bagSlots[slot].equipped = -1;
			}
		} else {
			bagItem.equipped = -1;
		}

		_baseUnit.UpdateSpells();
		SyncUnit();
	}

	public void ConsumeItem(BagItemBehaviour bagItem) {
		if(bagItem == null) {return;}
		if(bagItem.item == null) {return;}
		
		bool consumed = false;
		switch(bagItem.item.id) {
			case BaseItem.ID.Potion_of_Return:
				if(!_baseUnit.inCombat) {
					consumed = _bag.Consume(bagItem.item.id);
					if(consumed) {
						SyncUnit();
						_baseUnit.Return();
						return;
					}
				} else {
					AnnouncementManager.Display("This item cannot be used while in combat.", Color.red);
				}
			break;
		}
	}

	public bool GiveItem(BagItemBehaviour bagItem, ContainerBehaviour containerBehaviour) {
		if(bagItem == null) {return false;}
		if(bagItem.item == null) {return false;}
		if(containerBehaviour == null) {return false;}
		if(containerBehaviour.bag == null) {return false;}

		if(containerBehaviour.bag.Add(bagItem.item)) {
			int slot = _bag.Remove(bagItem.item);
			bagItem.item = null;

			for(int i = slot; i < _bagSlots.Count; i++) {
				if(i < _bagSlots.Count-1) {
					_bagSlots[i].equipped = _bagSlots[i+1].equipped-1;
				} else {
					_bagSlots[i].equipped = -1;
				}

				if(_bagSlots[i].equipped < -1) {
					_bagSlots[i].equipped = -1;
				}
			}

			SyncUnit();
			containerBehaviour.SyncBag(containerBehaviour.bag);
			AnnouncementManager.Display("Item Given.", Color.white, 5f);
			return true;
		}

		return false;
	}

	public bool TakeItem(BagItemBehaviour bagItem, ContainerBehaviour containerBehaviour) {
		if(bagItem == null) {return false;}
		if(bagItem.item == null) {return false;}
		if(containerBehaviour == null) {return false;}
		if(containerBehaviour.bag == null) {return false;}

		if(_bag.Add(bagItem.item)) {
			SyncUnit();

			containerBehaviour.bag.Remove(bagItem.item);
			bagItem.item = null;
			containerBehaviour.SyncBag(containerBehaviour.bag);
			Debug.Log("Item Taken.");
			return true;
		}

		return false;
	}

	public bool SellItem(BagItemBehaviour bagItem, ContainerBehaviour containerBehaviour) {
		if(bagItem == null) {return false;}
		if(bagItem.item == null) {return false;}
		if(bagItem.item.category == BaseItem.Category.Currency) {return false;}
		if(containerBehaviour == null) {return false;}
		if(containerBehaviour.bag == null) {return false;}

		
		int slot = _bag.Remove(bagItem.item);
		if(slot > -1) {
			int itemValue = bagItem.item.value;
			bagItem.item = null;

			for(int i = slot; i < _bagSlots.Count; i++) {
				if(i < _bagSlots.Count-1) {
					_bagSlots[i].equipped = _bagSlots[i+1].equipped-1;
				} else {
					_bagSlots[i].equipped = -1;
				}

				if(_bagSlots[i].equipped < -1) {
					_bagSlots[i].equipped = -1;
				}
			}

			if(itemValue > 0) {
				BaseItem gold = new BaseItem(BaseItem.ID.Gold);
				gold.quantity = itemValue;
				_bag.Add(gold);
			}
		
			SyncUnit();
			Debug.Log("Item Sold.");
			return true;
		}

		return false;
	}

	public bool BuyItem(BagItemBehaviour bagItem, ContainerBehaviour containerBehaviour) {
		if(bagItem == null) {return false;}
		if(bagItem.item == null) {return false;}
		if(containerBehaviour == null) {return false;}
		if(containerBehaviour.bag == null) {return false;}
		
		if(_bag.Add(bagItem.item)) {
			int itemNdx = _bag.FindItemWithID(BaseItem.ID.Gold);
			if(itemNdx > -1) {
				int value = bagItem.item.value;
				if(bagItem.item.category != BaseItem.Category.Runestone) {
					value *= 2;
				}

				if(_bag.items[itemNdx].quantity > value) {
					_bag.items[itemNdx].quantity -= value;
				} else if(_bag.items[itemNdx].quantity == value) {
					_bag.RemoveAt(itemNdx);
				} else {
					return false;
				}
			} else {
				return false;
			}

			SpendAttributes(bagItem.item.id);


			SyncUnit();

			//containerBehaviour.bag.Remove(bagItem.item);
			//bagItem.item = null;
			containerBehaviour.SyncBag(containerBehaviour.bag);
			Debug.Log("Item purchased.");
			return true;
		}

		return false;
	}

	void SpendAttributes(BaseItem.ID id) {
		switch(id) {
			case BaseItem.ID.Rune_of_Constitution:
				_bag.RemoveAt(_bag.FindItemWithID(BaseItem.ID.Rune_of_Constitution));
				_baseUnit.attributes.baseConstitution++;
				AnnouncementManager.Display("You feel healthier.", Color.green);
				SaveLoadData.Save();
				_baseUnit.SetAsInterfaceTarget();
			break;

			case BaseItem.ID.Rune_of_Dexterity:
				_bag.RemoveAt(_bag.FindItemWithID(BaseItem.ID.Rune_of_Dexterity));
				_baseUnit.attributes.baseDexterity++;
				AnnouncementManager.Display("You feel more agile.", Color.green);
				SaveLoadData.Save();
				_baseUnit.SetAsInterfaceTarget();
			break;

			case BaseItem.ID.Rune_of_Intelligence:
				_bag.RemoveAt(_bag.FindItemWithID(BaseItem.ID.Rune_of_Intelligence));
				_baseUnit.attributes.baseIntelligence++;
				AnnouncementManager.Display("You feel more perceptive.", Color.green);
				SaveLoadData.Save();
				_baseUnit.SetAsInterfaceTarget();
			break;

			case BaseItem.ID.Rune_of_Strength:
				_bag.RemoveAt(_bag.FindItemWithID(BaseItem.ID.Rune_of_Strength));
				_baseUnit.attributes.baseStrength++;
				AnnouncementManager.Display("You feel stronger.", Color.green);
				SaveLoadData.Save();
				_baseUnit.SetAsInterfaceTarget();
			break;
		}
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
		
	}

	public void ToggleUI() {
		if(hidden) {
			ShowUI();
		} else {
			HideUI();
		}
	}

	public void ShowUI(bool showContainer = false) {
		if(_hidden) {
			_bagPanel.SetActive(true);
			_bagEquipmentPanel.SetActive(true);
			_hidden = false;
		}

		SyncEquipment();

		if(showContainer) {
			GetComponent<ContainerBehaviour>().ShowUI();
		}
	}

	public void HideUI() {
		if(!_hidden) {
			_bagPanel.SetActive(false);
			_bagEquipmentPanel.SetActive(false);
			GetComponent<ContainerBehaviour>().HideUI();
			_hidden = true;
			
			ItemTooltip tooltip = GameObject.FindObjectOfType<ItemTooltip>();
			if(tooltip != null) {
				tooltip.Reset();
			}
		}
	}
}
