using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LylekGames.RPGCharacter;

namespace LylekGames.RPGCharacter {
	public class CharacterManager : MonoBehaviour {

		public static CharacterManager characterManager;
		public CharacterProperties characterProperties;
		public GameObject character;

		public void Awake() {
			characterManager = this;
			if (character) {
				characterProperties = character.GetComponent<CharacterProperties> ();
				UpdateCharacter ();
			} else {
				Debug.LogWarning ("No character found. Creating generic character...");
				CreateGenericCharacter ();
			}
		}
		public void CreateGenericCharacter() {
			CharacterSpecs characterSpecs = new CharacterSpecs ();
			GameObject newCharacter = Resources.Load<GameObject>(characterSpecs.resourcesPath + "RPGCharacter_skin") as GameObject;
			character = Instantiate(newCharacter, transform.position, transform.rotation) as GameObject;
			character.name = "Player";
			character.transform.parent = transform;
			characterProperties = character.GetComponent<CharacterProperties> ();
		}
		public void UpdateCharacter() {
			GameObject newCharacter = Resources.Load<GameObject>(characterProperties.characterSpecs.resourcesPath + "RPGCharacter_skin") as GameObject;
			Destroy(character.gameObject);
			character = Instantiate(newCharacter, transform.position, transform.rotation) as GameObject;
			character.name = "Player";
			character.transform.parent = transform;
			characterProperties = character.GetComponent<CharacterProperties> ();
		}
	}
}
