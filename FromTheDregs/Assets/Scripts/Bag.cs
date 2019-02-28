using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bag {

	public static int BAG_SLOTS = 32;
	public static int CONTAINER_SLOTS = 16;

	public enum BagType {
		Bag,
		Container,
		Shop
	}
	
	BagType _bagType = BagType.Bag;

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
	public BagType bagType {
		get {return _bagType;}
		set {_bagType = value;}
	}

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
			} else if(_items[i].id == item.id) {
				if(_items[i].IsStackable()) {
					_items[i].quantity += item.quantity;
					return true;
				}
			}
		}
		return false;
	}

	public int Remove(BaseItem item) {
		int slot = FindItemSlot(item);
		if(slot > -1) {
			_items.RemoveAt(slot);
			_items.Add(null);
		}
		return slot;
	}

	public bool RemoveAt(int ndx) {
		if(ndx > -1 && ndx < _items.Count) {
			_items.RemoveAt(ndx);
			_items.Add(null);
			return true;
		}
		return false;
	}

	public Bag(BagType b, List<BaseItem> itm) {
		_items = new List<BaseItem>();

		_bagType = b;

		if(_bagType == BagType.Bag) {
			for(int i = 0; i < BAG_SLOTS; i++) {
				if(i < itm.Count) {
					_items.Add(itm[i]);
				} else {
					_items.Add(null);
				}
			}
		} else {
			for(int i = 0; i < CONTAINER_SLOTS; i++) {
				if(i < itm.Count) {
					_items.Add(itm[i]);
				} else {
					_items.Add(null);
				}
			}
		}
	}

	public Bag(BagType b) {
		_items = new List<BaseItem>();

		if(b == BagType.Bag) {
			for(int i = 0; i < BAG_SLOTS; i++) {
				_items.Add(null);
			}
		} else {
			for(int i = 0; i < CONTAINER_SLOTS; i++) {
				_items.Add(null);
			}
		}
	}

	public bool Equip(int slot) {
		if(slot < 0 || slot >= BAG_SLOTS) {return false;}
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

	public int FindKey(string keycode) {
		int ndx = -1;
		if(keycode == "XXXXXX") {return ndx;}
		for(int i = 0; i < _items.Count; i++) {
			if(_items[i] != null) {
				if(items[i].keycode == keycode) {
					return i;
				}
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
