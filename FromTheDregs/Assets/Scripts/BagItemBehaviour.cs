using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class BagItemBehaviour : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler {

	
	[SerializeField] Image _image;
	[SerializeField] Text _text;
	[SerializeField] BaseItem.Category _slotCategory;

	[SerializeField] Sprite _defaultSprite;
	[SerializeField] Sprite _equippedSprite;

	SpriteManager _spriteManager;
	BagBehaviour _bagBehaviour;

	BagItemBehaviour _bagItemReference;

	BaseItem _item;

	[SerializeField] bool _equipped = false;

	public BaseItem item {
		get {return _item;}
		set {_item = value;}
	}

	public Image image {
		get {return _image;}
		set {_image = value;}
	}

	public BagItemBehaviour bagItemReference {
		get {return _bagItemReference;}
		set {_bagItemReference = value;}
	}

	public bool equipped {
		get {return _equipped;}
		set {_equipped = value;}
	}

	void Awake() {
		_spriteManager = GameObject.FindObjectOfType<SpriteManager>();
		_bagBehaviour = GameObject.FindObjectOfType<BagBehaviour>();
		if(_slotCategory == BaseItem.Category.Unknown) {
			_item = new BaseItem(BaseItem.Category.Body_Armor, 1);
		}
		
		UpdateImage();
	}

	
	public void OnPointerClick(PointerEventData eventData) {
		if(_equipped) {
			_bagBehaviour.UnequipItem(this);
		} else {
			_bagBehaviour.EquipItem(this);
		}
		
	}

	
	public void OnPointerEnter(PointerEventData eventData) {
		UpdateItemInfo();
	}


	public void UpdateImage() {
		if(_item != null) {
			_item.LoadSprite(_spriteManager);

			if(_equipped) {
				GetComponent<Image>().sprite = _equippedSprite;
			} else {
				GetComponent<Image>().sprite = _defaultSprite;
			}

			if(_item.sprite != null) {
				_image.sprite = _item.sprite;
			} else {
				_image.GetComponent<Outline>().enabled = true;
				_image.GetComponent<Shadow>().enabled = true;
				_image.sprite = _spriteManager.items.unknown;
			}
		} else {
			_image.GetComponent<Outline>().enabled = false;
			_image.GetComponent<Shadow>().enabled = false;

			_equipped = false;
			GetComponent<Image>().sprite = _defaultSprite;

			switch(_slotCategory) {
				case BaseItem.Category.Neck_Jewelry:
					_image.sprite = _spriteManager.items.unknownNeck;
				break;

				case BaseItem.Category.Head_Armor:
					_image.sprite = _spriteManager.items.unknownHead;
				break;

				case BaseItem.Category.Finger_Jewelry:
					_image.sprite = _spriteManager.items.unknownFinger;
				break;

				case BaseItem.Category.Primary_Weapon:
					_image.sprite = _spriteManager.items.unknownPrimary;
				break;

				case BaseItem.Category.Body_Armor:
					_image.sprite = _spriteManager.items.unknownBody;
				break;

				case BaseItem.Category.Secondary_Weapon:
					_image.sprite = _spriteManager.items.unknownSecondary;
				break;

				case BaseItem.Category.Hand_Armor:
					_image.sprite = _spriteManager.items.unknownHands;
				break;

				case BaseItem.Category.Leg_Armor:
					_image.sprite = _spriteManager.items.unknownLegs;
				break;

				case BaseItem.Category.Foot_Armor:
					_image.sprite = _spriteManager.items.unknownFeet;
				break;
			}
		}
	}
	
	public void UpdateItemInfo() {
		if(_bagBehaviour == null) {return;}
		_bagBehaviour.UpdateItemInfo(_item);
	}
}
