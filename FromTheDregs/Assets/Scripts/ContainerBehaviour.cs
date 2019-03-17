using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ContainerBehaviour : MonoBehaviour {

	public enum Actions {
		Buy,
		Take
	}


    Actions _defaultAction = Actions.Take;
    SpriteManager _spriteManager;
    BaseUnit _baseUnit;
    [SerializeField] TransferUI _transferUI;
    [SerializeField] GameObject _bagPanel;
    [SerializeField] GameObject _bagSlotGrid;
	[SerializeField] Text _titleText;

    List<BagItemBehaviour> _bagSlots;
	Bag _bag;
    bool _hidden = true;
	string _name = "Container Name";

	public bool hidden {
		get {return _hidden;}
	}

	public string name {
		get {return _name;}
		set {_name = value;}
	}
	
    public Bag bag {
		get {return _bag;}
		set {
			_bag = value;
			//SyncBag(_bag);
		}
	}

	public Actions defaultAction {
		get {return _defaultAction;}
		set {_defaultAction = value;}
	}

    public TransferUI transferUI {
		get {return _transferUI;}
		set {_transferUI = value;}
	}

    void Awake() {
		_spriteManager = GameObject.FindObjectOfType<SpriteManager>();

		_bagSlots = new List<BagItemBehaviour>();
		for(int i = 0; i < _bagSlotGrid.transform.childCount; i++) {
			_bagSlots.Add(_bagSlotGrid.transform.GetChild(i).GetComponent<BagItemBehaviour>());
		}

		UpdateSlotImages();
	}

    public void SyncBag(Bag b) {
        _bag = b;
		
		_titleText.text = _name;
		if(_bag.bagType == Bag.BagType.Bag) {
			for(int i = 0; i < Bag.BAG_SLOTS; i++) {
				_bagSlots[i].item = _bag.items[i];
			}
			
		} else {
			for(int i = 0; i < Bag.CONTAINER_SLOTS; i++) {
				_bagSlots[i].item = _bag.items[i];
			}
		}

		UpdateSlotImages();
    }

    void UpdateSlotImages() {
        for(int i = 0; i < _bagSlots.Count; i++) {
			_bagSlots[i].UpdateImage();
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
			_transferUI.HideUI();
			_hidden = false;
		}
	}

	public void HideUI() {
		if(!_hidden) {
			_bagPanel.SetActive(false);
			_hidden = true;

			ItemTooltip tooltip = GameObject.FindObjectOfType<ItemTooltip>();
			if(tooltip != null) {
				tooltip.Reset();
			}
		}
	}
}