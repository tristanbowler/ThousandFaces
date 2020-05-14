using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedTiling : MonoBehaviour {
	
	public float tileScale = 0.1f;

	// Use this for initialization
	void Start () {
		//INSTANCED TILING
		//Instance material tiling separately | Best if your scene contains multiple water planes of different scales | Increases draw calls per water plane
		GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.localScale.x * tileScale, transform.localScale.y * tileScale);

		//SHARED TILING
		//Share tiling among the same material | Best if your scene contains a single water plane, or multiple water planes of the same scale
		//GetComponent<Renderer>().sharedMaterial.mainTextureScale = new Vector2(transform.localScale.x * tileScale, transform.localScale.y * tileScale);
	}
}
