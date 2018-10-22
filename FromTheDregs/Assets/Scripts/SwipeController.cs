using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class SwipeController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	float minimumDistance;
	Vector2 startPosition;
	Vector2 endPosition;
	bool dragging = false;

	BaseUnit _baseUnit;


	public BaseUnit baseUnit {
		set {_baseUnit = value;}
		get {return _baseUnit;}
	}

	public void OnBeginDrag(PointerEventData eventData) {
		dragging = true;
		startPosition = Input.mousePosition;
		//Debug.Log(startPosition);
	}

	public void OnDrag(PointerEventData eventData) {
		dragging = true;
	}
	
	public void OnEndDrag(PointerEventData eventData) {
		dragging = false;
		endPosition = Input.mousePosition;
		//EvaluateDragPattern();
	}

	void EvaluateDragPattern() {
		Vector2 direction = endPosition - startPosition;
		ResetNodes();

		if(direction.x == 0 && direction.y == 0) {
			return;
		}

		if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
			// Left or right
			if(direction.x > 0) {
				Move(-1, 0);	// Left
			} else {
				Move(1, 0);		// Right
			}
		} else {
			// Up or down
			if(direction.y > 0) {
				Move(0, -1);	// Down
			} else {
				Move(0, 1);		// Up
			}
		}

		//Debug.Log(Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2)));
	}

	void ResetNodes() {
		startPosition = Vector2.zero;
		endPosition = Vector2.zero;
	}

	void Move(int dx, int dy) {
		if(_baseUnit != null) {
			_baseUnit.Move(dx, dy);
		}
	}
}
