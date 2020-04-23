using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadDemoScenes : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			SceneManager.LoadScene ("Winter");
		}
		else if(Input.GetKeyDown(KeyCode.Alpha2)) {
			SceneManager.LoadScene ("Summer");
		}
		else if(Input.GetKeyDown(KeyCode.Alpha3)) {
			SceneManager.LoadScene ("Autumn");
		}
	}
}
