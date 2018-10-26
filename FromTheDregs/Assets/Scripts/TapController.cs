using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class TapController : MonoBehaviour, IPointerClickHandler {

	[SerializeField] Image _image;
	Vector2Int _origin;
	float _width = 1080f;
	float _height = 1480f;

	BaseUnit _baseUnit;
	
	public Image image {
		set {_image = value;}
		get {return _image;}
	}

	public BaseUnit baseUnit {
		set {_baseUnit = value;}
		get {return _baseUnit;}
	}

	void Start() {
		RectTransform rectTransform = GetComponent<RectTransform>();
		int ox = (int)(rectTransform.anchoredPosition.x + Screen.width / 2f);
		int oy = (int)(rectTransform.anchoredPosition.y + Screen.height / 2f);
		_origin = new Vector2Int(ox, oy);

		_width = rectTransform.sizeDelta.x;
		_height = rectTransform.sizeDelta.y;
	}

	public void OnPointerClick(PointerEventData eventData) {
		DetectQuadrant(new Vector2Int((int)Input.mousePosition.x, (int)Input.mousePosition.y));
	}

	void DetectQuadrant(Vector2Int location) {
		int x = location.x - _origin.x;
		int y = location.y - _origin.y;
		float slope = _height / _width;
		if(Mathf.Abs(x * slope) >= Mathf.Abs(y) && x < 0) {
			// Left
			Move(-1, 0);
		} else if(Mathf.Abs(x * slope) <= Mathf.Abs(y) && y > 0) {
			// Top
			Move(0, 1);
		} else if(Mathf.Abs(x * slope) <= Mathf.Abs(y) && y < 0) {
			// Down
			Move(0, -1);
		} else {
			// Right
			Move(1, 0);	
		}
	}

	void Move(int dx, int dy) {
		if(_baseUnit != null) {
			_baseUnit.Move(dx, dy);
		}
	}
}
