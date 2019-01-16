using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bag {

	public static int NUM_SLOTS = 32;

	BaseItem _neck;
	BaseItem _head;
	BaseItem _finger;
	BaseItem _primary;
	BaseItem _body;
	BaseItem _secondary;
	BaseItem _hands;
	BaseItem _legs;
	BaseItem _feet;

	List<BaseItem> _items;

	#region Accessors
	public List<BaseItem> items {
		get {return _items;}
	}

	public BaseItem neck {
		get {return _neck;}
	}

	public BaseItem head {
		get {return _head;}
	}

	public BaseItem finger {
		get {return _finger;}
	}

	public BaseItem primary {
		get {return _primary;}
	}

	public BaseItem body {
		get {return _body;}
	}

	public BaseItem secondary {
		get {return _secondary;}
	}

	public BaseItem hands {
		get {return _hands;}
	}

	public BaseItem legs {
		get {return _legs;}
	}

	public BaseItem feet {
		get {return _feet;}
	}

	public int equipmentAttack {
		get {
			int sum = 0;
			foreach(BaseItem item in EquipmentList()) {
				if(item != null) {
					sum += item.attack;
				}
			}
			return sum;
		}
	}

	public int equipmentDefense {
		get {
			int sum = 0;
			foreach(BaseItem item in EquipmentList()) {
				if(item != null) {
					sum += item.defense;
				}
			}
			return sum;
		}
	}

	public int equipmentHealthTotal {
		get {
			int sum = 0;
			foreach(BaseItem item in EquipmentList()) {
				if(item != null) {
					sum += item.healthTotal;
				}
			}
			return sum;
		}
	}

	public int equipmentHealthRecovery {
		get {
			int sum = 0;
			foreach(BaseItem item in EquipmentList()) {
				if(item != null) {
					sum += item.healthRecovery;
				}
			}
			return sum;
		}
	}

	public int equipmentEssenceTotal {
		get {
			int sum = 0;
			foreach(BaseItem item in EquipmentList()) {
				if(item != null) {
					sum += item.essenceTotal;
				}
			}
			return sum;
		}
	}

	public int equipmentEssenceRecovery {
		get {
			int sum = 0;
			foreach(BaseItem item in EquipmentList()) {
				if(item != null) {
					sum += item.essenceRecovery;
				}
			}
			return sum;
		}
	}

	public int equipmentMovementSpeed{
		get {
			int sum = 0;
			foreach(BaseItem item in EquipmentList()) {
				if(item != null) {
					sum += item.movementSpeed;
				}
			}
			return sum;
		}
	}
	#endregion

	public bool Add(BaseItem item) {
		for(int i = 0; i < _items.Count; i++) {
			if(_items[i] == null) {
				_items[i] = item;
				return true;
			}
		}
		return false;
	}

	public Bag(List<BaseItem> itm) {
		_items = new List<BaseItem>();
		for(int i = 0; i < NUM_SLOTS; i++) {
			if(i < itm.Count) {
				_items.Add(itm[i]);
			} else {
				_items.Add(null);
			}
		}
	}

	public Bag() {
		_items = new List<BaseItem>();
		for(int i = 0; i < NUM_SLOTS; i++) {
			_items.Add(null);
		}
	}

	public bool Equip(int slot) {
		if(slot < 0 || slot >= NUM_SLOTS) {return false;}
		if(_items[slot] == null) {return false;}

		switch(_items[slot].category) {
			case BaseItem.Category.Neck_Jewelry:
				_neck = _items[slot];
				return true;

			case BaseItem.Category.Head_Armor:
				_head = _items[slot];
				return true;

			case BaseItem.Category.Finger_Jewelry:
				_finger = _items[slot];
				return true;

			case BaseItem.Category.Primary_Weapon:
				_primary = _items[slot];
				return true;

			case BaseItem.Category.Body_Armor:
				_body = _items[slot];
				return true;
						
			case BaseItem.Category.Secondary_Weapon:
				_secondary = _items[slot];
				return true;
			
			case BaseItem.Category.Hand_Armor:
				_hands = _items[slot];
				return true;
			
			case BaseItem.Category.Leg_Armor:
				_legs = _items[slot];
				return true;
			
			case BaseItem.Category.Foot_Armor:
				_feet = _items[slot];
				return true;
		}

		return false;
	}

	public bool Unequip(BaseItem.Category category) {
		switch(category) {
			case BaseItem.Category.Neck_Jewelry:
				_neck = null;
				return true;

			case BaseItem.Category.Head_Armor:
				_head = null;
				return true;

			case BaseItem.Category.Finger_Jewelry:
				_finger = null;
				return true;

			case BaseItem.Category.Primary_Weapon:
				_primary = null;
				return true;

			case BaseItem.Category.Body_Armor:
				_body = null;
				return true;
			
			case BaseItem.Category.Secondary_Weapon:
				_secondary = null;
				return true;
			
			case BaseItem.Category.Hand_Armor:
				_hands = null;
				return true;
			
			case BaseItem.Category.Leg_Armor:
				_legs = null;
				return true;
			
			case BaseItem.Category.Foot_Armor:
				_feet = null;
				return true;
		}

		return false;
	}

	public int FindItemSlot(BaseItem item) {
		int ndx = -1;
		for(int i = 0; i < _items.Count; i++) {
			if(_items[i] == item) {
				return i;
			}
		}
		return ndx;
	}

	BaseItem[] EquipmentList() {
		BaseItem[] equipment = {
			_neck,
			_head,
			_finger,
			_primary,
			_body,
			_secondary,
			_hands,
			_legs,
			_feet
		};

		return equipment;
	}
}
