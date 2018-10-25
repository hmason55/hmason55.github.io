﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CastOptionsUI : MonoBehaviour {

	[SerializeField] Image _image;
	[SerializeField] Hotbar _hotbar;
	[SerializeField] Button _cancelCastButton;
	public Button cancelCastButton {
		get {return _cancelCastButton;}
	}

	public void ShowUI() {
		_image.enabled = true;
		_cancelCastButton.gameObject.SetActive(true);
	}

	public void HideUI() {
		_image.enabled = false;
		_cancelCastButton.gameObject.SetActive(false);
	}

	public void CancelCast() {
		_hotbar.CancelPreview();
		HideUI();
	}

}
