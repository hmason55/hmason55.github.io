using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Chunk {

	Sprite _template;

	public Sprite template {
		get {return _template;}
	}

	public enum ChunkType {
		c0001,
		c0010,
		c0011,
		c0100,
		c0101,
		c0110,
		c0111,
		c1000,
		c1001,
		c1010,
		c1011,
		c1100,
		c1101,
		c1110,
		c1111,
	}

	public Chunk(string preset) {
		SpriteManager spriteManager = GameObject.FindObjectOfType<SpriteManager>();
		ChunkType chunkType = (ChunkType)System.Enum.Parse(typeof(ChunkType), "c" + preset);

		List<Sprite> sprites = new List<Sprite>(); 

		switch(chunkType) {
			case ChunkType.c0001:
				sprites = spriteManager.chunk.c0001;
			break;

			case ChunkType.c0010:
				sprites = spriteManager.chunk.c0010;
			break;

			case ChunkType.c0011:
				sprites = spriteManager.chunk.c0011;
			break;

			case ChunkType.c0100:
				sprites = spriteManager.chunk.c0100;
			break;

			case ChunkType.c0101:
				sprites = spriteManager.chunk.c0101;
			break;

			case ChunkType.c0110:
				sprites = spriteManager.chunk.c0110;
			break;

			case ChunkType.c0111:
				sprites = spriteManager.chunk.c0111;
			break;

			case ChunkType.c1000:
				sprites = spriteManager.chunk.c1000;
			break;

			case ChunkType.c1001:
				sprites = spriteManager.chunk.c1001;
			break;

			case ChunkType.c1010:
				sprites = spriteManager.chunk.c1010;
			break;

			case ChunkType.c1011:
				sprites = spriteManager.chunk.c1011;
			break;

			case ChunkType.c1100:
				sprites = spriteManager.chunk.c1100;
			break;

			case ChunkType.c1101:
				sprites = spriteManager.chunk.c1101;
			break;

			case ChunkType.c1110:
				sprites = spriteManager.chunk.c1110;
			break;

			case ChunkType.c1111:
			default:
				sprites = spriteManager.chunk.c1111;
			break;
		}

		//int variation = Random.Range(1, sprites.Count-1);
		int variation = 1;
		_template = sprites[variation];
		//_template = Resources.Load<Sprite>("Chunks/" + preset + "/" + preset + "_" + variation);
	}


}
