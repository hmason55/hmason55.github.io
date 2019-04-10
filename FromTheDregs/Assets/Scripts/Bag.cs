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

	public enum EquipmentBonus {
		PhysicalDamage,
		SpellDamage,
		BlockModifier,
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

	/// <summary>
	/// A list of items in the bag.
	/// </summary>
	/// <value></value>
	public List<BaseItem> items {
		get {return _items;}
	}

	/// <summary>
	/// A reference to the item in the bag currently equipped to the neck.
	/// </summary>
	/// <value></value>
	public BaseItem neck {
		get {return _neck;}
	}

	/// <summary>
	/// A reference to the item in the bag currently equipped to the head.
	/// </summary>
	/// <value></value>
	public BaseItem head {
		get {return _head;}
	}

	/// <summary>
	/// A reference to the item in the bag currently equipped to the finger.
	/// </summary>
	/// <value></value>
	public BaseItem finger {
		get {return _finger;}
	}

	/// <summary>
	/// A reference to the item in the bag currently equipped to the right hand.
	/// </summary>
	/// <value></value>
	public BaseItem primary {
		get {return _primary;}
	}

	/// <summary>
	/// A reference to the item in the bag currently equipped to the body.
	/// </summary>
	/// <value></value>
	public BaseItem body {
		get {return _body;}
	}

	/// <summary>
	/// A reference to the item in the bag currently equipped to the left hand.
	/// </summary>
	/// <value></value>
	public BaseItem secondary {
		get {return _secondary;}
	}

	/// <summary>
	/// A reference to the item in the bag currently equipped to the hands.
	/// </summary>
	/// <value></value>
	public BaseItem hands {
		get {return _hands;}
	}

	/// <summary>
	/// A reference to the item in the bag currently equipped to the legs.
	/// </summary>
	/// <value></value>
	public BaseItem legs {
		get {return _legs;}
	}

	/// <summary>
	/// A reference to the item in the bag currently equipped to the feet.
	/// </summary>
	/// <value></value>
	public BaseItem feet {
		get {return _feet;}
	}
	#endregion

	/// <summary>
	/// Adds an item to the bag.
	/// </summary>
	/// <param name="item">The item to add to the bag.</param>
	/// <returns>True if the item was added to the bag, otherwise returns False.</returns>
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

	/// <summary>
	/// Removes an item from the bag.
	/// </summary>
	/// <param name="item">The item to remove from the bag.</param>
	/// <returns>The slot number of which the item was removed, otherwise returns -1.</returns>
	public int Remove(BaseItem item) {
		int slot = FindItemSlot(item);
		if(slot > -1) {
			_items.RemoveAt(slot);
			_items.Add(null);
		}
		return slot;
	}

	/// <summary>
	/// Removes an item at a specific slot in the bag.
	/// </summary>
	/// <param name="ndx">The slot index to remove an item at.</param>
	/// <returns>True if the item was found and removed from the bag, otherwise returns -1.</returns>
	public bool RemoveAt(int ndx) {
		if(ndx > -1 && ndx < _items.Count) {
			_items.RemoveAt(ndx);
			_items.Add(null);
			return true;
		}
		return false;
	}

	/// <summary>
	/// Uses a consumable item from the bag.
	/// </summary>
	/// <param name="id">The ID of the item to use.</param>
	/// <returns>True if the item was used, otherwise returns False.</returns>
	public bool Consume(BaseItem.ID id) {
		int slot = FindItemWithID(id);
		if(slot > -1) {
			if(_items[slot].quantity > 1) {
				_items[slot].quantity -= 1;
			} else {
				_items.RemoveAt(slot);
				_items.Add(null);
			}
			return true;
		}
		return false;
	}

	/// <summary>
	/// Creates a bag object.
	/// </summary>
	/// <param name="b">The type of bag.</param>
	/// <param name="itm">A list of items to add to the bag.</param>
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

	/// <summary>
	/// Creates a bag object.
	/// </summary>
	/// <param name="b">The type of bag.</param>
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

	/// <summary>
	/// Equips an item from the bag.
	/// </summary>
	/// <param name="slot">The slot of the item to equip.</param>
	/// <returns>True if the item was equipped, otherwise returns False.</returns>
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

	/// <summary>
	/// Unequips an item from the bag and all items of the same category.
	/// </summary>
	/// <returns>True if the item was unequipped, otherwise returns False.</returns>
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

	/// <summary>
	/// Removes key items from the bag and prepares it for saving.
	/// </summary>
	public void Format() {
		for(int i = _items.Count-1; i >= 0; i--) {
			if(_items[i] != null) {

				// Remove Keys
				if(items[i].category == BaseItem.Category.Key) {
					RemoveAt(i);
				}
			}
		}
	}

	/// <summary>
	/// Finds an item within the bag.
	/// </summary>
	/// <returns>The slot number of the item when found, otherwise returns -1.</returns>
	public int FindItemSlot(BaseItem item) {
		int ndx = -1;
		for(int i = 0; i < _items.Count; i++) {
			if(_items[i] == item) {
				return i;
			}
		}
		return ndx;
	}

	/// <summary>
	/// Finds an item within the bag by its ID.
	/// </summary>
	/// <returns>The slot number of the item when found, otherwise returns -1.</returns>
	public int FindItemWithID(BaseItem.ID id) {
		int ndx = -1;
		for(int i = 0; i < _items.Count; i++) {
			if(_items[i] != null) {
				if(_items[i].id == id) {
					return i;
				}
			}
		}
		return ndx;
	}

	/// <summary>
	/// Finds an key item that has the given keycode.
	/// </summary>
	/// <returns>The slot number of the key when found, otherwise returns -1.</returns>
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

	/// <returns>An array of the currently equipped items in the bag.</returns>
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

	public int GetEquipmentBonus(EquipmentBonus bonusType) {
		int bonus = 0;
		foreach(BaseItem item in EquipmentList()) {
			if(item != null) {
				switch(bonusType) {
					case EquipmentBonus.PhysicalDamage:
						bonus += item.physicalDamage;
					break;

					case EquipmentBonus.SpellDamage:
						bonus += item.spellDamage;
					break;

					case EquipmentBonus.BlockModifier:
						bonus += item.blockModifier;
					break;
				}
			}
			
		}
		return bonus;
	}

}
