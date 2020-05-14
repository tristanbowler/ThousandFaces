using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MouseLook))]
public class EnableMouseLook : MonoBehaviour {

	public MouseLook mouseLook;

	void Update() {
		if (Input.GetMouseButtonUp(0)) {
			mouseLook.enabled = false;
		}
	}
	public void OnMouseDown() {
		mouseLook.enabled = true;
	}
}
