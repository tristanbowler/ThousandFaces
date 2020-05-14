using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeFromBlackScreen : MonoBehaviour {

	public FadeScreen fade;
	public enum FadeScreen {
		FadeFromBlack,
		FadeToBlack,
		FadeToInvisible,
	}
	[Range(0.5f, 3.0f)]
	public float waitTime = 1.0f;
	[Range(0.1f, 1.0f)]
	public float speed = 1.0f;

	void Start() {
		Initialize ();
	}
	public void Initialize() {
		if (fade == FadeScreen.FadeFromBlack) {
			GetComponent<Image> ().color = Color.black;
			StartCoroutine (FadeFromBlack ());
		} else if (fade == FadeScreen.FadeToBlack) {
			Color newColor = new Color ();
			newColor = Color.black;
			newColor.a = 0.0f;
			GetComponent<Image> ().color = newColor;
			StartCoroutine (FadeToBlack ());
		} else if (fade == FadeScreen.FadeToInvisible) {
			Color newColor = new Color ();
			StartCoroutine (FadeToInvisible ());
		}
	}
	public IEnumerator FadeFromBlack() {
		yield return new WaitForSeconds (waitTime);
		Image myImage = GetComponent<Image>();

		float timer = 1.0f;
		while (timer > 0.0f) {
			timer -= speed * Time.deltaTime;
			Color newColor = myImage.color;
			newColor.a = timer;
			myImage.color = newColor;
			yield return null;
		}
		enabled = false;
	}
	public IEnumerator FadeToBlack() {
		yield return new WaitForSeconds (waitTime);
		Image myImage = GetComponent<Image>();

		float timer = 0.0f;
		while (timer < 1.0f) {
			timer += speed * Time.deltaTime;
			Color newColor = myImage.color;
			newColor.a = timer;
			myImage.color = newColor;
			yield return null;
		}
		enabled = false;
	}
	public IEnumerator FadeToInvisible() {
		yield return new WaitForSeconds (waitTime);
		Text myText = GetComponent<Text>();

		float timer = 1.0f;
		while (timer > 0.0f) {
			timer -= speed * Time.deltaTime;
			Color newColor = myText.color;
			newColor.a = timer;
			myText.color = newColor;
			yield return null;
		}
		enabled = false;
	}
}
