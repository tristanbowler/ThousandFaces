using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LylekGames.RPGCharacter {
	[System.Serializable]
	public class CharacterSpecs {
		public CharacterType characterType;
		public enum CharacterType {
			Human,
			Dwarf,
			Elf,
			Goblin,
			Orc,
			DarkElf,
		}
		public GenderType gender;
		public enum GenderType {
			Male,
			Female,
		}
		public string resourcesPath;
		
		public CharacterSpecs(CharacterType cType = CharacterType.Human, GenderType gType = GenderType.Male) {
			characterType = cType;
			gender = gType;
			GetCharacterPath ();
		}
		public void GetCharacterPath() {
			switch (gender) {
			case GenderType.Male:
				switch (characterType) {
				case CharacterType.Human:
					resourcesPath = "Human_male/";
					break;
				case CharacterType.Dwarf:
					resourcesPath = "Dwarf_male/";
					break;
				case CharacterType.Elf:
					resourcesPath = "Elf_male/";
					break;
				case CharacterType.Goblin:
					resourcesPath = "Goblin_male/";
					break;
				case CharacterType.Orc:
					resourcesPath = "Orc_male/";
					break;
				case CharacterType.DarkElf:
					resourcesPath = "DarkElf_male/";
					break;
				default:
					resourcesPath = "Human_male/";
					break;
				}
				break;
			case GenderType.Female:
				switch (characterType) {
				case CharacterType.Human:
					resourcesPath = "Human_female/";
					break;
				case CharacterType.Dwarf:
					resourcesPath = "Dwarf_female/";
					break;
				case CharacterType.Elf:
					resourcesPath = "Elf_female/";
					break;
				case CharacterType.Goblin:
					resourcesPath = "Goblin_female/";
					break;
				case CharacterType.Orc:
					resourcesPath = "Orc_female/";
					break;
				case CharacterType.DarkElf:
					resourcesPath = "DarkElf_female/";
					break;
				default:
					resourcesPath = "Human_female/";
					break;
				}
				break;
			default:
				resourcesPath = "Human_male/";
				break;
			}
		}
	}
}
