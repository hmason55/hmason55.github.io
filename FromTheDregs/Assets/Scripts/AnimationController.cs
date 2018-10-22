using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

	public float _animationFrame = 0f;

	public static int AnimationLength = 26;
	public float animationSpeed = 1f;

	public int animationFrame {
		get {return (int)_animationFrame;}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		_animationFrame += (animationSpeed * AnimationLength * Time.deltaTime);
		_animationFrame %= AnimationLength;
	}
}
