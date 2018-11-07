using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Unit : MonoBehaviour {
	
	BaseUnit _baseUnit;
	Sprite[] _idleAnimation;
	Sprite[] _hitAnimation;
	static int IdleAnimationLength = 4;
	static int HitAnimationLength = 1;
	int _animationFrame = -1;
	int _hitFrame = -1;
	int _hitFrameSkip = 8;

	Sprite _sprite;

	[SerializeField] Image _image;
	
	[SerializeField] Tile _tile;

	public Tile tile {
		set {_tile = value;}
		get {return _tile;}
	}


	public Sprite[] idleAnimation {
		get {return _idleAnimation;}
		set {_idleAnimation = value;}
	}

	public Sprite[] hitAnimation {
		get {return _hitAnimation;}
		set {_hitAnimation = value;}
	}

	public int animationFrame {
		get {return _animationFrame;}
		set {_animationFrame = value;}
	}

	public int hitFrame {
		get {return _hitFrame;}
		set {_hitFrame = value;}
	}

	public Image image {
		get {return _image;}
	}

	public Sprite sprite {
		set {_sprite = value;}
		get {return _sprite;}
	}

	public BaseUnit baseUnit {
		set {_baseUnit = value;}
		get {return _baseUnit;}
	}


	public void LoadSprites() {
		if(_baseUnit != null) {
			_idleAnimation = new Sprite[IdleAnimationLength];

			switch(_baseUnit.spritePreset) {
				case BaseUnit.SpritePreset.knight:
					//_idleAnimation = _tile.spriteManager.unit.idle.ToArray();
				break;

				case BaseUnit.SpritePreset.direrat:
					_idleAnimation = _tile.spriteManager.unitDireRat1.idle.ToArray();
					_hitAnimation = _tile.spriteManager.unitDireRat1.hit.ToArray();
				break;

				case BaseUnit.SpritePreset.direratsmall:
					_idleAnimation = _tile.spriteManager.unitDireRatSmall1.idle.ToArray();
					_hitAnimation = _tile.spriteManager.unitDireRatSmall1.hit.ToArray();
				break;

				case BaseUnit.SpritePreset.sandbehemoth:
					_idleAnimation = _tile.spriteManager.unitSandBehemoth1.idle.ToArray();
					_hitAnimation = _tile.spriteManager.unitSandBehemoth1.hit.ToArray();
				break;

				case BaseUnit.SpritePreset.spider:
					_idleAnimation = _tile.spriteManager.unitSpider1.idle.ToArray();
					_hitAnimation = _tile.spriteManager.unitSpider1.hit.ToArray();
				break;

				case BaseUnit.SpritePreset.spidersmall:
					_idleAnimation = _tile.spriteManager.unitSpiderSmall1.idle.ToArray();
					_hitAnimation = _tile.spriteManager.unitSpiderSmall1.hit.ToArray();
				break;

				case BaseUnit.SpritePreset.widow:
					_idleAnimation = _tile.spriteManager.unitWidow1.idle.ToArray();
					_hitAnimation = _tile.spriteManager.unitWidow1.hit.ToArray();
				break;

				case BaseUnit.SpritePreset.widowsmall:
					_idleAnimation = _tile.spriteManager.unitWidowSmall1.idle.ToArray();
					_hitAnimation = _tile.spriteManager.unitWidowSmall1.hit.ToArray();
				break;

				case BaseUnit.SpritePreset.wizard:
					_idleAnimation = _tile.spriteManager.unitHumanWizard1.idle.ToArray();
					_hitAnimation = _tile.spriteManager.unitHumanWizard1.hit.ToArray();
				break;

				case BaseUnit.SpritePreset.greenslime:
				default:
					_idleAnimation = _tile.spriteManager.unitGreenSlime1.idle.ToArray();
					_hitAnimation = _tile.spriteManager.unitGreenSlime1.hit.ToArray();
				break;
			}
			
		}
	}

	public void IncrementAnimation() {
		_animationFrame = _baseUnit.tile.animationController.animationFrame;

		if(_hitAnimation != null && _hitFrame > -1 && _hitFrame < _hitFrameSkip) {
			_sprite = _hitAnimation[0];
			_hitFrame++;
		} else if(_idleAnimation != null) {
			_hitFrame = -1;

			if(_animationFrame < 10) {
				_sprite = _idleAnimation[0];
			} else if(_animationFrame < 11) {
				_sprite = _idleAnimation[1];
			} else if(_animationFrame < 22) {
				_sprite = _idleAnimation[2];
			} else {
				_sprite = _idleAnimation[3];
			}
		}
	}

	public void BeginHitAnimation() {
		_hitFrame = 0;
		
	}

	public void EndHitAnimation() {
		_hitFrame = -1;
	}

	public void UpdateSprite() {
		GetComponent<Image>().sprite = _sprite;
	}

	public void TransferUnit(Unit nextUnit) {
		nextUnit.idleAnimation = _idleAnimation;
		nextUnit.animationFrame = _animationFrame;
		nextUnit.sprite = _sprite;
		nextUnit.UpdateSprite();
		nextUnit.GetComponent<Image>().enabled = true;
		ClearAssets();
	}

	public void ClearAssets() {
		_idleAnimation = null;
		_animationFrame = 0;
		_sprite = null;
		GetComponent<Image>().sprite = null;
		GetComponent<Image>().enabled = false;
	}

	public void Kill() {
		SpawnDeathParticles();
		_baseUnit = null;
		ClearAssets();
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

		RectTransform tileRT = baseUnit.tile.dungeonGenerator.tiles[baseUnit.tile.position.x, baseUnit.tile.position.y].GetComponent<RectTransform>();
		RectTransform particleRT = deathParticlesGO.GetComponent<RectTransform>();
		particleRT.anchoredPosition = new Vector2(tileRT.anchoredPosition.x + DungeonGenerator.TileWidth/2, tileRT.anchoredPosition.y + DungeonGenerator.TileHeight/2);

		ParticleSystem ps = deathParticlesGO.GetComponent<ParticleSystem>();
		GameObject.Destroy(deathParticlesGO, ps.main.startLifetime.constantMax);
	}
}
