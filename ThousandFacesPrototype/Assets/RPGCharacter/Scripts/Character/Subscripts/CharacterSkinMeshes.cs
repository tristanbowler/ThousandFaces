using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LylekGames.RPGCharacter;

namespace LylekGames.RPGCharacter {
	[System.Serializable]
	public class CharacterSkinMeshes {

		[Header("Meshes")]
		public GameObject head;
		public GameObject eyes;
		public GameObject torso;
		public GameObject legs;
		public GameObject hands;
		public GameObject feet;
		public GameObject hair;
		public GameObject beard;

		[Header("Materials")]
		public Material eyeColor;
		public Material hairColor;
		public SkinMaterials mySkinMaterials;

		public void UpdateSkinMaterials(SkinMaterials skinMaterials) {
			mySkinMaterials = skinMaterials;
			head.GetComponent<Renderer>().sharedMaterial = mySkinMaterials.headMaterial;
			torso.GetComponent<Renderer>().sharedMaterial = mySkinMaterials.chestMaterial;
			legs.GetComponent<Renderer>().sharedMaterial = mySkinMaterials.legMaterial;
			hands.GetComponent<Renderer>().sharedMaterial = mySkinMaterials.handMaterial;
			feet.GetComponent<Renderer>().sharedMaterial = mySkinMaterials.feetMaterial;
		}
		public void UpdateEyeMaterial(Material eyeMaterial) {
			eyeColor = eyeMaterial;
			eyes.GetComponent<Renderer> ().sharedMaterial = eyeColor;
		}
		public void UpdateHairMaterial(Material hairMaterial) {
			hairColor = hairMaterial;
			if (hair) {
				hair.GetComponent<Renderer> ().sharedMaterial = hairColor;
			}
			if (beard) {
				beard.GetComponent<Renderer> ().sharedMaterial = hairColor;
			}
		}

		public void FindSkinMeshes(GameObject character) {
			Component[] myChildren;
			myChildren = character.transform.GetComponentsInChildren<Transform>();

			foreach(Transform child in myChildren) {
				switch (child.gameObject.name) {
				case "Character_head":
					head = child.gameObject;
					break;
				case "Character_eyes":
					eyes = child.gameObject;
					break;
				case "Character_torso":
					torso = child.gameObject;
					break;
				case "Character_legs":
					legs = child.gameObject;
					break;
				case "Character_hands":
					hands = child.gameObject;
					break;
				case "Character_feet":
					feet = child.gameObject;
					break;
				default:
					break;
				}
			}
		}
		public void UpdateMaterialProperties() {
			if(!eyeColor && eyes) {
				eyeColor = eyes.GetComponent<Renderer>().sharedMaterial;
			}
			if (!hairColor && hair) {
				hairColor = hair.GetComponent<Renderer> ().sharedMaterial;
			}
		}
	}
}
