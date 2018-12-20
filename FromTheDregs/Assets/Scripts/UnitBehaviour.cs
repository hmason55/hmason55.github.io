using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UnitBehaviour : MonoBehaviour {
	
	BaseUnit _baseUnit;

	[SerializeField] Image _unitImage;
	[SerializeField] Image _shadowImage;

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

	public Image unitImage {
		get {return _unitImage;}
	}

	public Image shadowImage {
		get {return _shadowImage;}
	}

	public bool renderFlag {
		get {return _renderFlag;}
		set {_renderFlag = value;}
	}

	public string u;
	public Vector2Int p;

	Coroutine _inputCooldownCoroutine;

	void Awake() {
		_rectTransform = GetComponent<RectTransform>();
	}

	public void UpdateSprite() {
		if(_baseUnit != null) {

			_unitImage.enabled = true;
			_unitImage.sprite = _baseUnit.sprite;

			_shadowImage.enabled = true;
			_shadowImage.sprite = _baseUnit.shadowSprite;

			if(_baseUnit.inCombat) {
				_unitImage.color = Color.red;
			} else {
				_unitImage.color = Color.white;
			}

			u = _baseUnit.spritePreset.ToString();
		} else {
			u = null;
		}
	}

	public void Transfer(Tile t, BaseUnit b) {
		float x = t.position.x * DungeonManager.TileWidth;
		float y = t.position.y * DungeonManager.TileWidth;
		_rectTransform.anchoredPosition = new Vector2(x, y);

		_renderFlag = true;
		_tile = t;
		_tile.unit = this;

		p = _tile.position;
	
		if(b != null) {
			_tile.baseUnit = b;
			_baseUnit = b;
			_baseUnit.tile = t;

			_unitImage.enabled = true;
			_unitImage.sprite = _baseUnit.sprite;

			_shadowImage.enabled = true;
			_shadowImage.sprite = _baseUnit.shadowSprite;
		}
	}

	public void Clear(bool preserveRenderState = false) {
		
		_rectTransform.anchoredPosition = new Vector2(-256f, -256f);
		_renderFlag = false;

		_baseUnit = null;
		_tile.unit = null;
		_tile = null;
		
		_unitImage.sprite = null;
		_unitImage.enabled = false;

		_shadowImage.sprite = null;
		_shadowImage.enabled = false;
	}

	public void Kill() {
		SpawnDeathParticles();
		_tile.baseUnit = null;
		_baseUnit = null;
		_unitImage.sprite = null;
		_unitImage.enabled = false;

		_shadowImage.sprite = null;
		_shadowImage.enabled = false;
	}

	public void SetInputCooldown(BaseUnit bUnit, float delay = 0.5f, bool recast = false) {
		if(_inputCooldownCoroutine != null || _baseUnit == null) {return;}
		StartCoroutine(ESetInputCooldown(bUnit, delay, recast));
	}

	public void SpawnSpellProjectiles(Spell spell) {
		StartCoroutine(ESpawnSpellProjectiles(spell));
	}

	public void SpawnSpellEffect(Spell spell) {
		StartCoroutine(ESpawnSpellEffect(spell));
	}

	IEnumerator ESetInputCooldown(BaseUnit bUnit, float delay = 0.5f, bool recast = false) {
		bUnit.isCasting = true;
		yield return new WaitForSeconds(delay);
		Debug.Log(bUnit);
		if(bUnit == null) {yield break;}
		bUnit.isCasting = false;

		// Re-enable hotbar after cooldown is done.
		if(bUnit.playerControlled) {
			if(bUnit == _tile.combatManager.turnQueue.queue[0].baseUnit) {
				Hotbar hotbar = GameObject.FindObjectOfType<Hotbar>();
				if(hotbar != null) {
					hotbar.EnableHotkeys();
					if(recast) {
						hotbar.Recast(_tile.position);
					}
				}
			}
		}
	}

	IEnumerator ESpawnSpellProjectiles(Spell spell) {
		for(int i = 0; i < spell.projCount; i++) {
			yield return new WaitForSeconds(spell.projPreSpawnDelay);
			spell.SpawnProjectileParticles(spell.caster.tile.position, spell.effectOrigin, 0f);
			yield return new WaitForSeconds(spell.projPostSpawnDelay);
		}
	}

	IEnumerator ESpawnSpellEffect(Spell spell) {
		yield return new WaitForSeconds(spell.effectPreSpawnDelay);
		spell.SpawnEffectParticles(spell.effectOrigin, 0f);
		spell.PlayEffectSound(spell.effectOrigin);
		yield return new WaitForSeconds(spell.effectDamageDelay);
		spell.DealDamage();
	}

	public void SpawnDeathParticles() {
		GameObject deathParticlesGO = GameObject.Instantiate(Resources.Load<GameObject>(baseUnit.deathParticlesPath));
		CameraController dungeon = GameObject.FindObjectOfType<CameraController>();
		deathParticlesGO.transform.SetParent(dungeon.transform);
		deathParticlesGO.transform.SetAsLastSibling();

		RectTransform unitRT = _baseUnit.tile.dungeonManager.tiles[_baseUnit.tile.position.x, _baseUnit.tile.position.y].unit.GetComponent<RectTransform>();
		RectTransform particleRT = deathParticlesGO.GetComponent<RectTransform>();
		particleRT.anchoredPosition = new Vector2(unitRT.anchoredPosition.x + DungeonManager.TileWidth/2, unitRT.anchoredPosition.y + DungeonManager.TileHeight/2);
		particleRT.localScale = new Vector3(0.25f, 0.25f, 0.25f);

		ParticleSystem ps = deathParticlesGO.GetComponent<ParticleSystem>();
		GameObject.Destroy(deathParticlesGO, ps.main.startLifetime.constantMax);
	}
}
