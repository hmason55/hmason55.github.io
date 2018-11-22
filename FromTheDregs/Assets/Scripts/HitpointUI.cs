using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HitpointUI : MonoBehaviour {

	[SerializeField] Image hitpointFillImage;
	[SerializeField] Text hitpointText;

	BaseUnit _baseUnit;
	
	public BaseUnit baseUnit {
		get {return _baseUnit;}
		set {_baseUnit = value;}
	}

	public void UpdateHitpoints(int current, int max) {

		// Adjust the hitpoint fill.
		RectTransform canvasRT = GameObject.FindObjectOfType<Canvas>().GetComponent<RectTransform>();
		RectTransform rt = GetComponent<RectTransform>();
		float leftBound = rt.offsetMin.x;
		float rightBound = rt.offsetMax.x;

		float width = canvasRT.sizeDelta.x - leftBound + rightBound;
		float right = width - width*( ((float)current) / ((float)max) );

		RectTransform fillRT = hitpointFillImage.GetComponent<RectTransform>();
		fillRT.offsetMax = new Vector2(-right, fillRT.offsetMax.y);

		// Update the hitpoint text values.
		hitpointText.text = current + " / " + max;
	}

}
