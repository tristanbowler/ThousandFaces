using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LylekGames.RPGCharacter;

namespace LylekGames.RPGCharacter {
	public class RandomCustomizations : MonoBehaviour {

		public ChangeArmor[] changeArmors;	//assign our slider bars in editor
		public ChangeHairColor[] hairColors;	//assign our hairColor buttons in editor
		public ChangeEyeColor[] eyeColors;	//assign our eyeColor buttons in editor
		public ChangeSkinTone[] skinTones;	//assign our skinTone buttons in editor

		public ChangeHairColor curHairColor;
		public ChangeSkinTone curSkinTone;
		public ChangeEyeColor curEyeColor;

		void Start() {
			StartCoroutine(RandomizeALL());
		}
		//Randomize ALL customizations
		public IEnumerator RandomizeALL() {
			yield return new WaitForSeconds (0.01f);
			RandomizeAppearance();
			RandomizeHairColor();
			RandomizeEyeColor();
			RandomizeSkinTone();
		}
		//If you want, you may create additional randomize buttons
		//and call the functions bellow, separately.
		public void RandomizeAppearance() {
			foreach(ChangeArmor cArmor in changeArmors) {
				cArmor.RandomizeValue ();
			}
		}
		public void RandomizeHairColor() {
			//choose a random hair color option
			var randomHairColor = Random.Range(0, hairColors.Length);
			curHairColor = hairColors [randomHairColor];
			//apply it's hair color
			curHairColor.ApplyHairColor();
		}
		public void RandomizeEyeColor() {
			//choose a random eye color option
			var randomEyeColor = Random.Range(0, eyeColors.Length);
			curEyeColor = eyeColors [randomEyeColor];
			//apply it's eye color
			curEyeColor.ApplyEyeColor();
		}
		public void RandomizeSkinTone() {
			//choose a random skin color option
			var randomSkinTone = Random.Range(0, skinTones.Length);
			curSkinTone = skinTones [randomSkinTone];
			//apply it's skin color
			curSkinTone.ApplySkinTone();
		}

		//THIS FUNCTION IS CALLED VIA THE Character_slider UI Component.
		public void UpdateAllCustomizations() {
			foreach(ChangeArmor cArmor in changeArmors) {
				cArmor.RecalculateArmors ();
			}
			//Reassign color values
			curEyeColor.ApplyEyeColor();
			curHairColor.ApplyHairColor();
			curSkinTone.ApplySkinTone();
		}
	}
}
