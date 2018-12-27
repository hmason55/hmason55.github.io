using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BagBehaviour : MonoBehaviour {

	SpriteManager _spriteManager;

	[SerializeField] Text _itemInfoTitle;
	[SerializeField] Text _itemInfoAttributes;

	[SerializeField] BagItemBehaviour _equipmentNeck;
	[SerializeField] BagItemBehaviour _equipmentHead;
	[SerializeField] BagItemBehaviour _equipmentFinger;
	[SerializeField] BagItemBehaviour _equipmentPrimary;
	[SerializeField] BagItemBehaviour _equipmentBody;
	[SerializeField] BagItemBehaviour _equipmentSecondary;
	[SerializeField] BagItemBehaviour _equipmentHands;
	[SerializeField] BagItemBehaviour _equipmentLegs;
	[SerializeField] BagItemBehaviour _equipmentFeet;


	
	void Awake() {
		_spriteManager = GameObject.FindObjectOfType<SpriteManager>();
		UpdateEquipmentImages();
	}

	void UpdateEquipmentImages() {
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

		Debug.Log("Equip from equipment slot");

		switch(bagItem.item.category) {
			case BaseItem.Category.Head_Armor:
				if(_equipmentHead.bagItemReference != null) {
					_equipmentHead.bagItemReference.equipped = false;
					_equipmentHead.bagItemReference.UpdateImage();
				}

				if(_equipmentHead != bagItem) {
					Debug.Log("Equip from bag slot");
					_equipmentHead.item = bagItem.item;
					_equipmentHead.equipped = true;
					_equipmentHead.bagItemReference = bagItem;
					_equipmentHead.bagItemReference.equipped = true;

					_equipmentHead.UpdateImage();

					if(_equipmentHead.bagItemReference != null) {
						_equipmentHead.bagItemReference.UpdateImage();
					}
				} else {
					Debug.Log("Equip -> unequip");
				}


			break;
		}
	}

	public void SwapItem() {

	}

	public void UnequipItem(BagItemBehaviour bagItem) {
		if(bagItem == null) {return;}
		if(bagItem.item == null) {return;}
		
		switch(bagItem.item.category) {
			case BaseItem.Category.Head_Armor:

				if(_equipmentHead == bagItem) {
					Debug.Log("Unequip from equipment slot");
					//_equipmentHead.equipped = false;
					//_equipmentHead.item = null;
					//_equipmentHead.bagItemReference = null;
				}

				if(_equipmentHead.bagItemReference != null) {
					Debug.Log("Unequip from bag slot");
					_equipmentHead.bagItemReference.equipped = false;
					_equipmentHead.bagItemReference.UpdateImage();
					_equipmentHead.bagItemReference = null;

					_equipmentHead.equipped = false;
					_equipmentHead.item = null;
					_equipmentHead.UpdateImage();
				}
			break;
		}
	}

	public void UpdateEquipmentStats() {

	}

	public void UpdateItemInfo(BaseItem item) {
		if(item != null) {
			_itemInfoTitle.text = item.NameToString();
			_itemInfoAttributes.text = item.AttributesToString();
		}
		
	}
}
