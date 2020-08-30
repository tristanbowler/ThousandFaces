using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LylekGames.RPGCharacter;

namespace LylekGames.RPGCharacter {
	public class ChangeEyeColor : MonoBehaviour {

		public Material eyeColor;

		public RandomCustomizations randomCustom;

		public void ApplyEyeColor() {
			CharacterManager.characterManager.characterProperties.characterSkinMeshes.eyeColor = eyeColor;
			CharacterManager.characterManager.characterProperties.characterSkinMeshes.eyes.GetComponent<Renderer>().sharedMaterial = eyeColor;

			randomCustom.curEyeColor = this;
		}
	}
}
