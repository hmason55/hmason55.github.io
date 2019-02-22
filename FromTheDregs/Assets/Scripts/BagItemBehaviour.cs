using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class BagItemBehaviour : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	
	[SerializeField] Image _image;
	[SerializeField] Text _text;
	[SerializeField] BaseItem.Category _slotCategory;

	[SerializeField] Sprite _defaultSprite;
	[SerializeField] Text _quantityText;
	[SerializeField] Sprite _equippedSprite;
	[SerializeField] BagBehaviour _bagBehaviour;
	[SerializeField] ContainerBehaviour _containerBehaviour;
	
	SpriteManager _spriteManager;
	
	BagItemBehaviour _bagItemReference;
	

	BaseItem _item;

	[SerializeField] int _equipped = -1;
	[SerializeField] bool _isContainer = false;

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

	public int equipped {
		get {return _equipped;}
		set {_equipped = value;}
	}

	void Awake() {
		_spriteManager = GameObject.FindObjectOfType<SpriteManager>();
		_bagBehaviour = GameObject.FindObjectOfType<BagBehaviour>();
		_containerBehaviour = GameObject.FindObjectOfType<ContainerBehaviour>();

		UpdateImage();
	}

	
	public void OnPointerClick(PointerEventData eventData) {
		if(_item == null) {return;}

		if(_isContainer) {
			switch(_containerBehaviour.defaultAction) {
				case ContainerBehaviour.Actions.Buy:
				break;

				case ContainerBehaviour.Actions.Take:
					if(_bagBehaviour.TakeItem(this, _containerBehaviour)) {
						OnPointerExit(eventData);
						OnPointerEnter (eventData);
					}
				break;
			}
		} else {

			switch(_bagBehaviour.defaultAction) {
				case BagBehaviour.Actions.Give:
					if(_equipped > -1) {
						_bagBehaviour.UnequipItem(this);
					}

					if(_bagBehaviour.GiveItem(this, _containerBehaviour)) {
						UpdateImage();
						OnPointerExit(eventData);
						OnPointerEnter (eventData);
					}
				break;

				case BagBehaviour.Actions.Sell:
					if(_equipped > -1) {
						_bagBehaviour.UnequipItem(this);
					}

				break;

				case BagBehaviour.Actions.Use:
					if(_equipped > -1) {
						_bagBehaviour.UnequipItem(this);
					} else {
						if(_item.IsEquipment()) {
							_bagBehaviour.EquipItem(this);
						} else if(_item.IsConsumable()) {
							_bagBehaviour.ConsumeItem(this);
						}
					}
				break;
			}

		}
	}

	
	public void OnPointerEnter(PointerEventData eventData) {
		if(_item == null) {return;}

		ItemTooltip itemTooltip = GameObject.FindObjectOfType<ItemTooltip>();
		if(itemTooltip != null) {
			if(_isContainer) {
				itemTooltip.Snap(transform.position.x, transform.position.y);
			} else {
				itemTooltip.Snap(transform.position.x-440f, transform.position.y);
			}
			
			itemTooltip.UpdateTooltip(_item);
		}

		if(_isContainer) {
			if(_containerBehaviour.bag == null) {return;}
			if(_containerBehaviour.hidden) {return;}

			if(_containerBehaviour.bag.bagType == Bag.BagType.Shop) {
				_containerBehaviour.defaultAction = ContainerBehaviour.Actions.Buy;	
			} else {
				_containerBehaviour.defaultAction = ContainerBehaviour.Actions.Take;	
			}

			switch(_containerBehaviour.defaultAction) {
				case ContainerBehaviour.Actions.Buy:
					_containerBehaviour.transferUI.Preview(ContainerBehaviour.Actions.Buy, _item);
				break;

				case ContainerBehaviour.Actions.Take:
					_containerBehaviour.transferUI.Preview(ContainerBehaviour.Actions.Take, _item);
				break;
			}
		} else {
			
			if(_containerBehaviour.hidden) {
				if(_item.IsEquipment() || _item.IsConsumable()) {
					_bagBehaviour.defaultAction = BagBehaviour.Actions.Use;
				}
			} else {
				if(_containerBehaviour.bag == null) {return;}

				if(_containerBehaviour.bag.bagType == Bag.BagType.Shop) {
					_bagBehaviour.defaultAction = BagBehaviour.Actions.Sell;	
				} else {
					_bagBehaviour.defaultAction = BagBehaviour.Actions.Give;	
				}
			}
			

			switch(_bagBehaviour.defaultAction) {
				case BagBehaviour.Actions.Give:
					_containerBehaviour.transferUI.Preview(BagBehaviour.Actions.Give, _item);
				break;

				case BagBehaviour.Actions.Sell:
					_containerBehaviour.transferUI.Preview(BagBehaviour.Actions.Sell, _item);
				break;
			}
		}
	}


	public void OnPointerExit(PointerEventData eventData) {
		ItemTooltip itemTooltip = GameObject.FindObjectOfType<ItemTooltip>();
		if(itemTooltip != null) {
			itemTooltip.Reset();
		}

		_containerBehaviour.transferUI.HideUI();
	}


	public void UpdateImage() {
		if(_spriteManager == null) {
			_spriteManager = GameObject.FindObjectOfType<SpriteManager>();
		}

		if(_item != null) {
			if(_item.quantity > 1) {
				_quantityText.text = _item.quantity.ToString();
				_quantityText.enabled = true;
			} else {
				_quantityText.enabled = false;
			}

			_item.LoadSprite(_spriteManager);

			if(_equipped > -1) {
				GetComponent<Image>().sprite = _equippedSprite;
			} else {
				GetComponent<Image>().sprite = _defaultSprite;
			}

			if(_item.sprite != null) {
				_image.sprite = _item.sprite;
			} else {
				_image.sprite = _spriteManager.items.unknown;
			}
		} else {
			_quantityText.enabled = false;
			_equipped = -1;
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

				default:
					_image.sprite = _spriteManager.items.unknown;
				break;
			}
		}
	}

}
