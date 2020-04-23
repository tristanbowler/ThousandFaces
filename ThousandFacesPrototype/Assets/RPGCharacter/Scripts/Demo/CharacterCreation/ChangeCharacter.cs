using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LylekGames.RPGCharacter;

namespace LylekGames.RPGCharacter {
	public class ChangeCharacter : MonoBehaviour {

		public GameObject beardSlider;

		public void UpdateCharacter() {
			int sliderValue = (int)GetComponent<Slider>().value;
			switch (sliderValue) {
			case 0:
				CharacterManager.characterManager.characterProperties.characterSpecs.characterType = CharacterSpecs.CharacterType.Human;
				CharacterManager.characterManager.characterProperties.characterSpecs.gender = CharacterSpecs.GenderType.Male;
				beardSlider.SetActive (true);
				CharacterManager.characterManager.characterProperties.characterSpecs.GetCharacterPath ();
				break;
			case 1:
				CharacterManager.characterManager.characterProperties.characterSpecs.characterType = CharacterSpecs.CharacterType.Human;
				CharacterManager.characterManager.characterProperties.characterSpecs.gender = CharacterSpecs.GenderType.Female;
				beardSlider.SetActive (false);
				break;
			case 2:
				CharacterManager.characterManager.characterProperties.characterSpecs.characterType = CharacterSpecs.CharacterType.Dwarf;
				CharacterManager.characterManager.characterProperties.characterSpecs.gender = CharacterSpecs.GenderType.Male;
				beardSlider.SetActive(true);
				break;
			case 3:
				CharacterManager.characterManager.characterProperties.characterSpecs.characterType = CharacterSpecs.CharacterType.Dwarf;
				CharacterManager.characterManager.characterProperties.characterSpecs.gender = CharacterSpecs.GenderType.Female;
				beardSlider.SetActive(false);
				break;
			case 4:
				CharacterManager.characterManager.characterProperties.characterSpecs.characterType = CharacterSpecs.CharacterType.Elf;
				CharacterManager.characterManager.characterProperties.characterSpecs.gender = CharacterSpecs.GenderType.Male;
				beardSlider.SetActive(true);
				break;
			case 5:
				CharacterManager.characterManager.characterProperties.characterSpecs.characterType = CharacterSpecs.CharacterType.Elf;
				CharacterManager.characterManager.characterProperties.characterSpecs.gender = CharacterSpecs.GenderType.Female;
				beardSlider.SetActive (false);
				break;
			case 6:
				CharacterManager.characterManager.characterProperties.characterSpecs.characterType = CharacterSpecs.CharacterType.Goblin;
				CharacterManager.characterManager.characterProperties.characterSpecs.gender = CharacterSpecs.GenderType.Male;
				beardSlider.SetActive (true);
				break;
			case 7:
				CharacterManager.characterManager.characterProperties.characterSpecs.characterType = CharacterSpecs.CharacterType.Orc;
				CharacterManager.characterManager.characterProperties.characterSpecs.gender = CharacterSpecs.GenderType.Male;
				beardSlider.SetActive (true);
				break;
			case 8:
				CharacterManager.characterManager.characterProperties.characterSpecs.characterType = CharacterSpecs.CharacterType.DarkElf;
				CharacterManager.characterManager.characterProperties.characterSpecs.gender = CharacterSpecs.GenderType.Male;
				beardSlider.SetActive (true);
				break;
			case 9:
				CharacterManager.characterManager.characterProperties.characterSpecs.characterType = CharacterSpecs.CharacterType.Goblin;
				CharacterManager.characterManager.characterProperties.characterSpecs.gender = CharacterSpecs.GenderType.Female;
				beardSlider.SetActive (false);
				break;
			case 10:
				CharacterManager.characterManager.characterProperties.characterSpecs.characterType = CharacterSpecs.CharacterType.Orc;
				CharacterManager.characterManager.characterProperties.characterSpecs.gender = CharacterSpecs.GenderType.Female;
				beardSlider.SetActive (false);
				break;
			case 11:
				CharacterManager.characterManager.characterProperties.characterSpecs.characterType = CharacterSpecs.CharacterType.DarkElf;
				CharacterManager.characterManager.characterProperties.characterSpecs.gender = CharacterSpecs.GenderType.Female;
				beardSlider.SetActive (false);
				break;
			default:
				CharacterManager.characterManager.characterProperties.characterSpecs.characterType = CharacterSpecs.CharacterType.Human;
				CharacterManager.characterManager.characterProperties.characterSpecs.gender = CharacterSpecs.GenderType.Male;
				beardSlider.SetActive (true);
				break;
			}
			CharacterManager.characterManager.characterProperties.characterSpecs.GetCharacterPath ();
			CharacterManager.characterManager.UpdateCharacter ();
		}
	}
}
