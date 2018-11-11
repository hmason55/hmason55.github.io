using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UnitBehaviour : MonoBehaviour {
	
	BaseUnit _baseUnit;

	Image _image;
	RectTransform _rectTransform;
	
	Tile _tile;
	bool _renderFlag = false;

	public Tile tile {
		set {_tile = value;}
		get {return _tile;}
	}

	public BaseUnit baseUnit {
		set {_baseUnit = value;}
		get {return _baseUnit;}
	}

	public Image image {
		get {return _image;}
	}

	public bool renderFlag {
		get {return _renderFlag;}
		set {_renderFlag = value;}
	}



	void Awake() {
		_image = GetComponent<Image>();
		_rectTransform = GetComponent<RectTransform>();
	}

	public void UpdateSprite() {
		if(_baseUnit != null) {
			_image.sprite = _baseUnit.sprite;
		}
	}

	public void Transfer(Tile t, BaseUnit b) {
		float x = t.position.x * DungeonManager.chunkDimension * DungeonManager.dungeonDimension;
		float y = t.position.y * DungeonManager.chunkDimension * DungeonManager.dungeonDimension;
		_rectTransform.anchoredPosition = new Vector2(x, y);

		_renderFlag = true;
		_tile = t;
		_tile.unit = this;
	
		if(b != null) {
			_tile.baseUnit = b;
			_baseUnit = b;
			_baseUnit.tile = t;
			_image.enabled = true;
			_image.sprite = _baseUnit.sprite;
		}
	}

	public void Clear() {		
		_rectTransform.anchoredPosition = new Vector2(-256f, -256f);

		_renderFlag = false;
		_baseUnit = null;
		_tile.unit = null;
		_tile = null;
		
		_image.sprite = null;
		_image.enabled = false;
	}

	public void Kill() {
		SpawnDeathParticles();
		_baseUnit = null;
		Clear();
	}

	public void SpawnSpellProjectiles(Spell spell) {
		StartCoroutine(ESpawnSpellProjectiles(spell));
	}

	IEnumerator ESpawnSpellProjectiles(Spell spell) {
		for(int i = 0; i < spell.projCount; i++) {
			yield return new WaitForSeconds(spell.projPreSpawnDelay);
			spell.SpawnProjectileParticles(spell.caster.tile.position, spell.effectOrigin, 0f);
			yield return new WaitForSeconds(spell.projPostSpawnDelay);
		}
	}

	public void SpawnDeathParticles() {
		GameObject deathParticlesGO = GameObject.Instantiate(Resources.Load<GameObject>(baseUnit.deathParticlesPath));
		CameraController dungeon = GameObject.FindObjectOfType<CameraController>();
		deathParticlesGO.transform.SetParent(dungeon.transform);
		deathParticlesGO.transform.SetAsLastSibling();

		RectTransform unitRT = _baseUnit.tile.dungeonManager.tiles[_baseUnit.tile.position.x, _baseUnit.tile.position.y].unit.GetComponent<RectTransform>();
		RectTransform particleRT = deathParticlesGO.GetComponent<RectTransform>();
		particleRT.anchoredPosition = new Vector2(unitRT.anchoredPosition.x + DungeonManager.TileWidth/2, unitRT.anchoredPosition.y + DungeonManager.TileHeight/2);

		ParticleSystem ps = deathParticlesGO.GetComponent<ParticleSystem>();
		GameObject.Destroy(deathParticlesGO, ps.main.startLifetime.constantMax);
	}
}
