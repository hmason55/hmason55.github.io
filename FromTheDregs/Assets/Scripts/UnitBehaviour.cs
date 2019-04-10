using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UnitBehaviour : MonoBehaviour {
	
	BaseUnit _baseUnit;

	[SerializeField] Image _unitPrimaryImage;
	[SerializeField] Image _unitSecondaryImage;
	[SerializeField] Image _unitArmorImage;
	[SerializeField] Image _unitSkinImage;
	[SerializeField] Image _unitImage;
	[SerializeField] Image _shadowImage;
	[SerializeField] Image _intentImage;
	[SerializeField] Text _intentText;
	[SerializeField] bool _previewUnit = false;
	[SerializeField] List<EffectBehaviour> _effectBehaviours;

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

	public Image unitPrimaryImage {
		get {return _unitPrimaryImage;}
	}

	public Image unitSecondaryImage {
		get {return _unitSecondaryImage;}
	}

	public Image unitArmorImage {
		get {return _unitArmorImage;}
	}

	public Image unitSkinImage {
		get {return _unitSkinImage;}
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

	public Vector2Int p;

	Coroutine _inputCooldownCoroutine;

	SpriteManager _spriteManager;

	void Awake() {
		_rectTransform = GetComponent<RectTransform>();
		_spriteManager = GameObject.FindObjectOfType<SpriteManager>();

		if(_previewUnit) {
			_baseUnit = new BaseUnit(false, BaseUnit.SpritePreset.warrior, true);
			_baseUnit.character = new Character();
			_baseUnit.character.skinColor = Character.SkinColor.Human_Light;
			LoadSprites();
			UpdateSprite();
		}
	}

	public void LoadSprites() {
		SpriteManager spriteManager = GameObject.FindObjectOfType<SpriteManager>();
		_baseUnit.LoadSprites(spriteManager);
	}

	public void UpdateSprite() {
		if(_baseUnit != null) {

			if(_baseUnit.useCustomSprites) {
				UseCustomSprite();
			} else {
				UseFallbackSprite();
			}

			if(_baseUnit.inCombat) {
				_unitImage.color = Color.red;
				if(!_baseUnit.playerControlled) {
					ShowIntent();
					UpdateIntent();
				}
			} else {
				_unitImage.color = Color.white;
				HideIntent();
			}
			
			UpdateEffects();
		}
	}

	void UseCustomSprite() {
		if(_shadowImage != null) 		{_shadowImage.sprite = _baseUnit.shadowSprite;}
		if(_unitSkinImage != null) 		{_unitSkinImage.sprite = _baseUnit.spriteSkin;}
		if(_unitArmorImage != null) 	{_unitArmorImage.sprite = _baseUnit.spriteArmor;}
		if(_unitSecondaryImage != null) {_unitSecondaryImage.sprite = _baseUnit.spriteSecondary;}
		if(_unitPrimaryImage != null) 	{_unitPrimaryImage.sprite = _baseUnit.spritePrimary;}
	}

	void UseFallbackSprite() {
		if(_shadowImage != null) 		{_shadowImage.sprite = _baseUnit.shadowSprite;}
		if(_unitImage != null) 			{_unitImage.sprite = _baseUnit.sprite;}	
	}

	public void ShowUnit() {
		if(_shadowImage != null) {_shadowImage.enabled = true;}

		if(_baseUnit.useCustomSprites) {
			if(_unitSkinImage != null) 		{_unitSkinImage.enabled = true;}
			if(_unitArmorImage != null) 	{_unitArmorImage.enabled = true;}
			if(_unitSecondaryImage != null) {_unitSecondaryImage.enabled = true;}
			if(_unitPrimaryImage != null) 	{_unitPrimaryImage.enabled = true;}
			if(_unitImage != null) 			{_unitImage.enabled = false;}
		} else {
			if(_unitSkinImage != null) 		{_unitSkinImage.enabled = false;}
			if(_unitArmorImage != null) 	{_unitArmorImage.enabled = false;}
			if(_unitSecondaryImage != null) {_unitSecondaryImage.enabled = false;}
			if(_unitPrimaryImage != null) 	{_unitPrimaryImage.enabled = false;}
			if(_unitImage != null) 			{_unitImage.enabled = true;}
		}

		if(!_baseUnit.playerControlled) {
			ShowIntent();
		}

		ShowEffects();
	}

	public void HideUnit() {
		if(_shadowImage != null) 		{_shadowImage.enabled = false;}
		if(_unitSkinImage != null) 		{_unitSkinImage.enabled = false;}
		if(_unitArmorImage != null) 	{_unitArmorImage.enabled = false;}
		if(_unitSecondaryImage != null) {_unitSecondaryImage.enabled = false;}
		if(_unitPrimaryImage != null) 	{_unitPrimaryImage.enabled = false;}
		if(_unitImage != null) 			{_unitImage.enabled = false;}
		HideIntent();
		HideEffects();
	}

	public void Unset() {
		if(_shadowImage != null) 		{_shadowImage.sprite = null;}
		if(_unitImage != null) 			{_unitImage.sprite = null;}	
		if(_unitSkinImage != null) 		{_unitSkinImage.sprite = null;}
		if(_unitArmorImage != null) 	{_unitArmorImage.sprite = null;}
		if(_unitSecondaryImage != null) {_unitSecondaryImage.sprite = null;}
		if(_unitPrimaryImage != null) 	{_unitPrimaryImage.sprite = null;}
		HideUnit();
	}


	public void ShowIntent() {
		if(_intentImage != null) 		{_intentImage.enabled = true;}
		if(_intentText != null) 		{_intentText.enabled = true;}
	}

	public void HideIntent() {
		if(_intentImage != null) 		{_intentImage.enabled = false;}
		if(_intentText != null) 		{_intentText.enabled = false;}
	}

	public void UpdateIntent() {
		if(_baseUnit != null) {
			if(_baseUnit.spellCharges > 0 && _baseUnit.intentSpell != null) {
				if(_intentImage != null) {
					switch(_baseUnit.intentSpell.intentType) {
						case Spell.IntentType.Attack:
							_intentImage.sprite = _spriteManager.intents.attack;
						break;

						case Spell.IntentType.Block:
							_intentImage.sprite = _spriteManager.intents.defend;
						break;
					}
					
				}

				if(_intentText != null) {
					if(_baseUnit.intentSpell != null) {
						_intentText.text = _baseUnit.intentSpell.intentValue;
					}
				}
			} else {
				HideIntent();
			}
		} else {
			HideIntent();
		}
	}

	public void ShowEffects() {
		if(_baseUnit != null) {
			for(int i = 0; i < _effectBehaviours.Count; i++) {
				if(i < _baseUnit.effects.Count) {
					_effectBehaviours[i].ShowEffect();
				} else {
					_effectBehaviours[i].HideEffect();
				}
			}
		}
	}

	public void HideEffects() {
		for(int i = 0; i < _effectBehaviours.Count; i++) {
			_effectBehaviours[i].HideEffect();
		}
	}

	public void UpdateEffects() {
		if(_baseUnit != null) {
			for(int i = 0; i < _effectBehaviours.Count; i++) {
				if(i < _baseUnit.effects.Count) {
					_effectBehaviours[i].ShowEffect();
					_effectBehaviours[i].UpdateEffect(_baseUnit, _baseUnit.effects[i], _spriteManager);
				} else {
					_effectBehaviours[i].HideEffect();
				}
			}
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

			ShowUnit();
			UpdateSprite();
			//UpdateSprite();
			//Debug.Log(b.useCustomSprites);
			///_unitSkinImage.enabled = true;
			//_unitArmorImage.enabled = true;
			//_unitSkinImage.enabled = true;

			//_unitImage.enabled = true;
			//_unitImage.sprite = _baseUnit.sprite;

			//_shadowImage.enabled = true;
			//_shadowImage.sprite = _baseUnit.shadowSprite;
		}
	}

	public void Clear(bool preserveRenderState = false) {
		
		_rectTransform.anchoredPosition = new Vector2(-256f, -256f);
		_renderFlag = false;

		_baseUnit = null;
		_tile.unit = null;
		_tile = null;
		Unset();
	}

	public void Kill() {
		SpawnDeathParticles();
		Return();

		_tile.baseUnit = null;
		_baseUnit = null;
		Unset();
	}

	public void Return() {
		if(_tile.baseUnit != null) {
			if(_tile.baseUnit.playerControlled) {
				LoadingUI loadingUI = GameObject.FindObjectOfType<LoadingUI>();
				loadingUI.FadeIn(3f);
			}
		}
	}

	public void MoveFromTo(Vector2Int from, Vector2Int to, float duration) {
		ResetPosition();
		StartCoroutine(EMoveFromTo(from, to, duration));
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

	public void ResetPosition() {
		_rectTransform.anchoredPosition = _tile.position * DungeonManager.dimension;
	}

	IEnumerator EMoveFromTo(Vector2Int from, Vector2Int to, float duration) {
		_rectTransform.anchoredPosition = from * DungeonManager.dimension;
		to *= DungeonManager.dimension;
		
		float t = 0.0f;
		float rate = 1.0f/duration;
		while(t < 1.0f) {
			t += Time.deltaTime * rate;
			_rectTransform.anchoredPosition = Vector2.Lerp(_rectTransform.anchoredPosition, to, t);
			yield return new WaitForSeconds(0f);
		}

		ResetPosition();
		yield break;
	}

	IEnumerator ESetInputCooldown(BaseUnit bUnit, float delay = 0.5f, bool recast = false) {
		bUnit.isCasting = true;
		yield return new WaitForSeconds(delay);
		if(bUnit == null) {yield break;}
		bUnit.isCasting = false;

		// Re-enable hotbar after cooldown is done.
		if(bUnit.playerControlled) {
			if(bUnit == _tile.combatManager.turnQueue.queue[0].baseUnit) {
				
				ShortcutUI shortcutUI = GameObject.FindObjectOfType<ShortcutUI>();
				if(shortcutUI != null) {
					shortcutUI.Enable();
				}
				
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

		int count = 0;
		bool playSound = false;

		// Apply effects to targets
		foreach(Effect effect in spell.targetEffects) {

			if(count == 0) {
				playSound = true;
			} else {
				playSound = false;
			}

			switch(effect.effectType) {
				case Effect.EffectType.Damage:
					spell.DealDamage(effect, playSound);
				break;

				case Effect.EffectType.Block:
					spell.ApplyStatus(effect, false);
				break;

				case Effect.EffectType.Stun:
					spell.ApplyStatus(effect, playSound);
				break;
			}

			count++;
			yield return new WaitForSeconds(spell.effectDamageDelay);
		}

		// Apply effects to caster
		foreach(Effect effect in spell.casterEffects) {
			switch(effect.effectType) {
				case Effect.EffectType.Damage:
					//spell.DealDamage(effect, );
				break;

				case Effect.EffectType.Block:
					Debug.Log("Gained Block");
					spell.caster.ReceiveStatus(spell, effect);
				break;

				case Effect.EffectType.Focus:
					spell.caster.ReceiveStatus(spell, effect);
				break;

				case Effect.EffectType.Stun:
					spell.caster.ReceiveStatus(spell, effect);
				break;
			}
		}
		
	}

	public void SpawnDeathParticles() {
		GameObject deathParticlesGO = GameObject.Instantiate(Resources.Load<GameObject>(baseUnit.deathParticlesPath));
		Transform canvas = GameObject.FindGameObjectWithTag("Effects Layer").transform;
		deathParticlesGO.transform.SetParent(canvas);
		deathParticlesGO.transform.SetAsLastSibling();

		RectTransform unitRT = _baseUnit.tile.dungeonManager.tiles[_baseUnit.tile.position.x, _baseUnit.tile.position.y].unit.GetComponent<RectTransform>();
		RectTransform particleRT = deathParticlesGO.GetComponent<RectTransform>();
		particleRT.anchoredPosition = new Vector2(unitRT.anchoredPosition.x + DungeonManager.TileWidth/2, unitRT.anchoredPosition.y + DungeonManager.TileHeight/2);
		particleRT.localScale = new Vector3(0.25f, 0.25f, 0.25f);

		ParticleSystem ps = deathParticlesGO.GetComponent<ParticleSystem>();
		GameObject.Destroy(deathParticlesGO, ps.main.startLifetime.constantMax);
	}
}
