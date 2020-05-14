using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LylekGames.RPGCharacter;

namespace LylekGames.RPGCharacter {
	public class ChangeSkinTone : MonoBehaviour {

		public CharacterProperties characterProperties;

		public SkinMaterials dwarfFemaleSkinTone; //head, chest, legs, hands, feet
		public SkinMaterials dwarfMaleSkinTone;
		public SkinMaterials elfFemaleSkinTone;
		public SkinMaterials elfMaleSkinTone;
		public SkinMaterials humanFemaleSkinTone;
		public SkinMaterials humanMaleSkinTone;

		public RandomCustomizations randomCustom;

		public void ApplySkinTone() {
			characterProperties = CharacterManager.characterManager.characterProperties;

			switch (CharacterManager.characterManager.characterProperties.characterSpecs.gender) {
			case CharacterSpecs.GenderType.Male:
				switch (CharacterManager.characterManager.characterProperties.characterSpecs.characterType) {
				case CharacterSpecs.CharacterType.Human:
					characterProperties.UpdateSkinMaterials (humanMaleSkinTone);
					break;
				case CharacterSpecs.CharacterType.Dwarf:
					characterProperties.UpdateSkinMaterials (dwarfMaleSkinTone);
					break;
				case CharacterSpecs.CharacterType.Elf:
					characterProperties.UpdateSkinMaterials (elfMaleSkinTone);
					break;
				default:
					characterProperties.UpdateSkinMaterials (humanMaleSkinTone);
					break;
				}
				break;
			case CharacterSpecs.GenderType.Female:
				switch (CharacterManager.characterManager.characterProperties.characterSpecs.characterType) {
				case CharacterSpecs.CharacterType.Human:
					characterProperties.UpdateSkinMaterials (humanFemaleSkinTone);
					break;
				case CharacterSpecs.CharacterType.Dwarf:
					characterProperties.UpdateSkinMaterials (dwarfFemaleSkinTone);
					break;
				case CharacterSpecs.CharacterType.Elf:
					characterProperties.UpdateSkinMaterials (elfFemaleSkinTone);
					break;
				default:
					characterProperties.UpdateSkinMaterials (humanFemaleSkinTone);
					break;
				}
				break;
			default:
				characterProperties.UpdateSkinMaterials (humanMaleSkinTone);
				break;
			}
			randomCustom.curSkinTone = this;
		}
	}
}
