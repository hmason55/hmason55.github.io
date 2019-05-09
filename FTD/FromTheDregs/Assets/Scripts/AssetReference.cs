using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetReference : MonoBehaviour {

    [SerializeField]
    AssetLibrary assetLibrary;
    public static SpriteLibrary sprites;
    
    void Awake() {
        sprites = assetLibrary.sprites;
    }
}
