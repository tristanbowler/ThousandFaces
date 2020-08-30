using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScroller : MonoBehaviour {
	
	public float scrollSpeed = 0.1f;
	
	// Update is called once per frame
	void Update () {
		if(GetComponent<Renderer>().material.shader.isSupported) {
			Camera.main.depthTextureMode |= DepthTextureMode.Depth;
		}

		float offset = Time.time * scrollSpeed;
		//GetComponent<Renderer>().material.SetTextureOffset ("_BumpMap", Vector2(offset / -10.0f, offset));
		GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", new Vector2(offset / 10.0f, offset));
	}
}
