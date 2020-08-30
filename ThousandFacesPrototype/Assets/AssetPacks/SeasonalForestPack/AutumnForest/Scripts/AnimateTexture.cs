using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTexture : MonoBehaviour {
	//This script is used to animate a texture atlas
	public int uvAnimationTileX = 4;
	public int uvAnimationTileY = 4;
	public float framesPerSecond = 10.0f;

	void Update () {
		int index = (int)(Time.time * framesPerSecond);
		index = index % (uvAnimationTileX * uvAnimationTileY);
		Vector2 size = new Vector2(1.0f / uvAnimationTileX, 1.0f / uvAnimationTileY);

		float uIndex = index % uvAnimationTileX;
		float vIndex = index / uvAnimationTileX;

		Vector2 offset = new Vector2(uIndex * size.x, 1.0f - size.y - vIndex * size.y);

		GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", offset);
		GetComponent<Renderer>().material.SetTextureScale ("_MainTex", size);
	}
}
