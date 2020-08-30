using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LylekGames.RPGCharacter {
	[System.Serializable]
	public class CharacterProperties : MonoBehaviour {

		public CharacterSpecs characterSpecs;
		public CharacterBoneCollision boneColliders;
		public CharacterSkinMeshes characterSkinMeshes;
		public CharacterArmorMeshes characterArmorMeshes;
		public EquippmentSwap equippmentSwap;

		public void Awake() {
			if (GetComponent<EquippmentSwap> ()) {
				equippmentSwap = GetComponent<EquippmentSwap> ();
			}
			characterSkinMeshes.FindSkinMeshes (gameObject);
			characterSkinMeshes.UpdateMaterialProperties ();
			boneColliders.FindCharacterColliders (gameObject);
			characterSpecs.GetCharacterPath ();
		}
		public void EquippedNewArmor(GameObject armor, EquippmentType.ArmorType armorType) {
			equippmentSwap.EquippedNewArmor (armor, armorType);
		}
		public void RemoveArmorType(EquippmentType.ArmorType armorType) {
			equippmentSwap.DestroyArmor (armorType);
		}
		public void HideArmorType(EquippmentType.ArmorType armorType) {
			equippmentSwap.HideArmor (armorType);
		}
		public void ShowArmorType(EquippmentType.ArmorType armorType) {
			equippmentSwap.DisplayArmor (armorType);
		}
		public void UpdateSkinMaterials(SkinMaterials skinMaterials) {
			characterSkinMeshes.UpdateSkinMaterials (skinMaterials);
			StartCoroutine(characterArmorMeshes.UpdateSkinArmorMaterials (skinMaterials));
		}
		public void UpdateEyeColor(Material eyeColor) {
			characterSkinMeshes.UpdateEyeMaterial (eyeColor);
		}
		public void UpdateHairColor(Material hairColor) {
			characterSkinMeshes.UpdateHairMaterial (hairColor);
		}
	}
}




