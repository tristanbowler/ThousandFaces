using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LylekGames.RPGCharacter;

namespace LylekGames.RPGCharacter {
	public class ChangeArmor : MonoBehaviour {

		public EquippmentType.ArmorType armorType;
		public GameObject[] myArmors;
		[HideInInspector]
		public int sliderValue;
		private Slider uiSlider;

		void Awake() {
			if (GetComponent<Slider> ()) {
				uiSlider = GetComponent<Slider> ();
				uiSlider.maxValue = myArmors.Length - 1;
			}
		}
		public void Start() {
			RecalculateArmors ();
		}
		public void RecalculateArmors() {
			//Recalculate all slider bar armors, and other meshes, from our new character resource path.
			int i = 0;
			foreach(GameObject armor in myArmors) {
				string newPath = CharacterManager.characterManager.characterProperties.characterSpecs.resourcesPath + armor.name;
				if(Resources.Load<Transform>(newPath)) {
					myArmors[i] = Resources.Load<GameObject>(newPath);
				}
				else {
					if(armorType == EquippmentType.ArmorType.Beard && CharacterManager.characterManager.characterProperties.characterSpecs.gender == CharacterSpecs.GenderType.Female) {
						//if character is female, ignore if beard mesh is not found, and destroy existing beard mesh
						if(CharacterManager.characterManager.characterProperties.characterSkinMeshes.beard) {
							Destroy(CharacterManager.characterManager.characterProperties.characterSkinMeshes.beard.gameObject);
						}
						CharacterManager.characterManager.characterProperties.characterSkinMeshes.beard = null;
					}
					else {
						//Resources could not be found!
						//Debug.LogError("Resources/" + newPath + " does not exist! Character will use previous character's mesh instead, which may not fit properly.");
					}
				}
				i += 1;
			}
			if (uiSlider.value >= 0) {
				CharacterManager.characterManager.characterProperties.EquippedNewArmor (myArmors [(int)uiSlider.value], armorType);
			} else if (uiSlider.value == 0) {
				CharacterManager.characterManager.characterProperties.RemoveArmorType (armorType);
			}
		}
		public void EquippedNewArmor() {
			if (uiSlider.value >= 0) {
				CharacterManager.characterManager.characterProperties.EquippedNewArmor (myArmors [(int)uiSlider.value], armorType);
			} else {
				CharacterManager.characterManager.characterProperties.equippmentSwap.DestroyArmor (armorType);
			}
		}
		//THIS FUNCTION IS CALLED FROM OUR RANDOMIZE-ALL BUTTON, GIVING OUR SLIDER BAR A RANDOM VALUE
		public void RandomizeValue() {
			if(armorType == EquippmentType.ArmorType.Beard && CharacterManager.characterManager.characterProperties.characterSpecs.gender == CharacterSpecs.GenderType.Female) {

			}
			else {
				int randomValue = new int ();
				randomValue = Random.Range (-1, (int)uiSlider.maxValue + 1);
				uiSlider.value = randomValue;
			}
		}
	}
}
