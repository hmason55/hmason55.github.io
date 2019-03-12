using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DecorationBehaviour : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	BaseDecoration _baseDecoration;
	Tile _tile;
	[SerializeField] RectTransform _rectTransform;
	[SerializeField] Image _image;
	[SerializeField] Image _lockImage;
	Outline _outline;
	bool _renderFlag = false;

	public BaseDecoration baseDecoration {
		get {return _baseDecoration;}
		set {_baseDecoration = value;}
	}

	public Tile tile {
		get {return _tile;}
		set {_tile = value;}
	}

	public bool renderFlag {
		get {return _renderFlag;}
		set {_renderFlag = value;}
	}

	public Image lockImage {
		get {return _lockImage;}
		set {_lockImage = value;}
	}

	public void OnPointerClick(PointerEventData eventData) {

		BagBehaviour bagBehaviour = GameObject.FindObjectOfType<BagBehaviour>();
		SpriteManager spriteManager = GameObject.FindObjectOfType<SpriteManager>();
		DungeonManager dungeonManager = GameObject.FindObjectOfType<DungeonManager>();
		LoadingUI loadingUI = GameObject.FindObjectOfType<LoadingUI>();

		switch(_baseDecoration.decorationType) {
			case BaseDecoration.DecorationType.Container:
				ContainerBehaviour containerBehaviour = GameObject.FindObjectOfType<ContainerBehaviour>();
				if(bagBehaviour != null && containerBehaviour != null) {
					bagBehaviour.defaultAction = BagBehaviour.Actions.Give;
					bagBehaviour.ShowUI(true);

					containerBehaviour.SyncBag(_baseDecoration.bag);
					containerBehaviour.defaultAction = ContainerBehaviour.Actions.Take;
				}
			break;

			case BaseDecoration.DecorationType.Exit:
				if(_baseDecoration.locked) {
					if(_baseDecoration.Unlock(bagBehaviour.bag)) {
						
						bagBehaviour.SyncUnit();
						_lockImage.raycastTarget = false;
						_lockImage.enabled = false;
						
						Debug.Log("The exit is unlocked!");
					} else {
						Debug.Log("This lock needs a different key.");
					}
				} else if(!_baseDecoration.traversable){
					_baseDecoration.traversable = true;
					if(spriteManager != null) {
						_baseDecoration.LoadTexture(spriteManager);
						_image.sprite = _baseDecoration.sprite;
					}
					_rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, 96f);
				} else {
					dungeonManager.Reload(dungeonManager.zone);
				}
			break;

			case BaseDecoration.DecorationType.CavernDoor:
				PlayerData.current.currentZone = DungeonManager.Zone.A1;
				PlayerData.current.targetZone = DungeonManager.Zone.A1;
				SaveLoadData.Save();
				loadingUI.FadeIn(3f);
			break;

			case BaseDecoration.DecorationType.CryptDoor:
			case BaseDecoration.DecorationType.HedgeDoor:

			break;

			case BaseDecoration.DecorationType.DungeonDoor:
				Debug.Log("This lock needs a different key.");
			break;

		}

	}

	public void OnPointerEnter(PointerEventData eventData) {
		_image.sprite = _baseDecoration.highlightSprite;
		if(_baseDecoration.locked) {
			_lockImage.sprite = _baseDecoration.lockHighlightSprite;
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		_image.sprite = _baseDecoration.sprite;
		if(_baseDecoration.locked) {
			_lockImage.sprite = _baseDecoration.lockSprite;
		}
	}

	public void Transfer(Tile t, BaseDecoration b) {
		float x = t.position.x * DungeonManager.TileWidth;
		float y = t.position.y * DungeonManager.TileWidth;
		_rectTransform.anchoredPosition = new Vector2(x, y);

		_renderFlag = true;
		_tile = t;
		_tile.decoration = this;

		if(b != null) {
			_baseDecoration = b;
			_image.sprite = _baseDecoration.sprite;
			_image.enabled = true;

			switch(_baseDecoration.decorationType) {
				case BaseDecoration.DecorationType.Container:
				case BaseDecoration.DecorationType.Exit:
				case BaseDecoration.DecorationType.CavernDoor:
				case BaseDecoration.DecorationType.CryptDoor:
				case BaseDecoration.DecorationType.HedgeDoor:
				case BaseDecoration.DecorationType.DungeonDoor:
					_image.raycastTarget = true;
				break;

				default:
					_image.raycastTarget = false;
				break;
			}

			_lockImage.sprite = _baseDecoration.lockSprite;

			if(_baseDecoration.locked) {
				_lockImage.enabled = true;
				_lockImage.raycastTarget = true;
			} else {
				_lockImage.enabled = false;
				_lockImage.raycastTarget = false;
			}
		}
		
	}

	public void Clear() {
		_rectTransform.anchoredPosition = new Vector2(-256f, -256f);
		_renderFlag = false;
		_tile = null;
		_baseDecoration = null;
		_image.sprite = null;
		_image.enabled = false;
		_lockImage.sprite = null;
		_lockImage.enabled = false;
	}

	void UpdateSprite() {
	//GetComponent<Image>().sprite = _baseDecoration.sprite;
	}
}
