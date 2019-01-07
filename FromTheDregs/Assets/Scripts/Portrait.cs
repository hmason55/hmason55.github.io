using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Portrait : MonoBehaviour {
	public static int size = 48;

	[SerializeField] Image faceImage;
	[SerializeField] Image mouthImage;
	[SerializeField] Image noseImage;
	[SerializeField] Image eyebrowImage;
	[SerializeField] Image hairImage;

	[SerializeField] Sprite faceTemplate;
	[SerializeField] Sprite mouthTemplate;
	[SerializeField] Sprite noseTemplate;
	[SerializeField] Sprite eyebrowTemplate;
	[SerializeField] Sprite hairTemplate;

	Color[] skinSwatch;
	Color[] hairSwatch;

	void Start() {
		Init();
		ApplySwatches();
	}

	void Init() {
		skinSwatch = ParseColor(Swatch.skinHumanMedium);
		hairSwatch = ParseColor(Swatch.hairWhite);
	}

	void ApplySwatches() {
		// Face
		ApplySwatch(faceImage, faceTemplate, skinSwatch);

		// Mouth
		ApplySwatch(mouthImage, mouthTemplate, skinSwatch);

		// Nose
		ApplySwatch(noseImage, noseTemplate, skinSwatch);

		// Eyebrows
		ApplySwatch(eyebrowImage, eyebrowTemplate, hairSwatch);

		// Hair
		ApplySwatch(hairImage, hairTemplate, hairSwatch);
				/* 
				Texture2D hairTexture = new Texture2D(size, size);

				Color[] hairColors = hairTemplate.texture.GetPixels();
				for(int i = 0; i < hairColors.Length; i++) {
					if(hairColors[i].a > 0f) {
						hairColors[i] = Swatch.SwapToColor(hairColors[i], hairSwatch);
					}	
				}
				
				hairTexture.SetPixels(hairColors);
				hairTexture.Apply();

				Sprite hairSprite = Sprite.Create(hairTexture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 48f);
				hairSprite.texture.filterMode = FilterMode.Point;
				hairImage.sprite = hairSprite;*/
		
	}

	void ApplySwatch(Image img, Sprite template, Color[] swatch) {
		if(img == null || template == null || swatch == null) {return;}

		Texture2D texture = new Texture2D(size, size);

		Color[] colors = template.texture.GetPixels();
		for(int i = 0; i < colors.Length; i++) {
			if(colors[i].a > 0f) {
				colors[i] = Swatch.SwapToColor(colors[i], swatch);
			}	
		}
		
		texture.SetPixels(colors);
		texture.Apply();

		Sprite sprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 48f);
		sprite.texture.filterMode = FilterMode.Point;
		img.sprite = sprite;
	}

	Color[] ParseColor(string[] arr) {
		Color[] palette = new Color[Swatch.size];

		for(int i = 0; i < Swatch.size; i++) {
			if(i >= arr.Length) {
				ColorUtility.TryParseHtmlString(Swatch.template[i], out palette[i]);
			} else {
				ColorUtility.TryParseHtmlString(arr[i], out palette[i]);
			}
		}
		
		return palette;
	}
	

}
