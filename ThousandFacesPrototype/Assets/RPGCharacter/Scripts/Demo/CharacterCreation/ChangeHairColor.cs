using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LylekGames.RPGCharacter;

namespace LylekGames.RPGCharacter {
	public class ChangeHairColor : MonoBehaviour {

		public Material hairColor;

		public RandomCustomizations randomCustom;

		public void ApplyHairColor() {
			CharacterManager.characterManager.characterProperties.characterSkinMeshes.hairColor = hairColor;
			if(CharacterManager.characterManager.characterProperties.characterSkinMeshes.hair) {
				CharacterManager.characterManager.characterProperties.characterSkinMeshes.hair.GetComponent<Renderer>().sharedMaterial = CharacterManager.characterManager.characterProperties.characterSkinMeshes.hairColor;
			}
			if(CharacterManager.characterManager.characterProperties.characterSkinMeshes.beard) {
				CharacterManager.characterManager.characterProperties.characterSkinMeshes.beard.GetComponent<Renderer>().sharedMaterial = CharacterManager.characterManager.characterProperties.characterSkinMeshes.hairColor;
			}
			randomCustom.curHairColor = this;
		}
	}
}
