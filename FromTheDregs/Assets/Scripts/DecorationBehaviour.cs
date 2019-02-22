using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DecorationBehaviour : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	BaseDecoration _baseDecoration;
	Tile _tile;
	RectTransform _rectTransform;
	Image _image;
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
	
	void Awake() {
		_image = GetComponent<Image>();
		_rectTransform = GetComponent<RectTransform>();
	}

	public void OnPointerClick(PointerEventData eventData) {
		BagBehaviour bagBehaviour = GameObject.FindObjectOfType<BagBehaviour>();
		ContainerBehaviour containerBehaviour = GameObject.FindObjectOfType<ContainerBehaviour>();
		if(bagBehaviour != null && containerBehaviour != null) {
			bagBehaviour.defaultAction = BagBehaviour.Actions.Give;
			bagBehaviour.ShowUI(true);

			containerBehaviour.SyncBag(_baseDecoration.bag);
			containerBehaviour.defaultAction = ContainerBehaviour.Actions.Take;
		}
	}

	public void OnPointerEnter(PointerEventData eventData) {
		_image.sprite = _baseDecoration.highlightSprite;
	}

	public void OnPointerExit(PointerEventData eventData) {
		_image.sprite = _baseDecoration.sprite;
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
			if(_baseDecoration.decorationType == BaseDecoration.DecorationType.Container) {
				_image.raycastTarget = true;
			} else {
				_image.raycastTarget = false;
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
	}

	void UpdateSprite() {
	//GetComponent<Image>().sprite = _baseDecoration.sprite;
	}
}
