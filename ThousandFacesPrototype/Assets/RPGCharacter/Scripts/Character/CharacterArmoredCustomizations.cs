using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LylekGames.RPGCharacter;

namespace LylekGames.RPGCharacter {
	[ExecuteInEditMode]
	public class CharacterArmoredCustomizations : MonoBehaviour {

		public CombineSkinMeshesTextureAtlas myCombine;
		public CharacterType characterType;
		public enum CharacterType { //an enumeration of our possible character types
			Human,
			Dwarf,
			Elf,
		}
		public GenderType gender;
		public enum GenderType { //an enumeration of our possible character types
			Male,
			Female,
		}
		[Tooltip("The path of the RPGCharacter folder.")]
		public string RPGCharacterPath =  "Assets/";
		public string resourcesPath;
		[HideInInspector]
		public string characterMaterial;

		[SerializeField]
		private List<SkinnedMeshRenderer> allMeshes = new List<SkinnedMeshRenderer>();

		[Header("ARMORS")]
		[Range(0, 10)]
		public int helmetType = 0;
		[Range(0, 14)]
		public int chestType = 0;
		[Range(0, 12)]
		public int leggingsType = 0;
		[Range(0, 9)]
		public int glovesType = 0;
		[Range(0, 10)]
		public int bootsType = 0;
		[Range(0, 3)]
		public int cloakType = 0;

		[Header("BODY")]
		[Range(0, 3)]
		public int hairStyle = 0;
		[Range(0, 3)]
		public int beardStyle = 0;
		[Range(0, 13)]
		public int hairColor = 0;
		[Range(0, 3)]
		public int skinColor = 0;

		public CharacterResources characterResources;
		[HideInInspector]
		public bool initialized = false;
		[HideInInspector]
		public bool combined = false;

		public void OnValidate() {
			if (initialized) {
				if (!combined) {
					UpdateCharacter ();
				}
			}
		}

		public void Initialize() {
			if (!myCombine) {
				if (GetComponent<CombineSkinMeshesTextureAtlas> ()) {
					myCombine = GetComponent<CombineSkinMeshesTextureAtlas> ();
				}
			}
			UpdateResourcesPath ();
			GetCharacterMeshes ();
			GetArmorMeshes ();
			initialized = true;
		}
		public void Randomize() {
			helmetType = Random.Range (0, characterResources.helmetMeshes.Count);
			chestType = Random.Range (0, characterResources.chestMeshes.Count);
			leggingsType = Random.Range (0, characterResources.legMeshes.Count);
			glovesType = Random.Range (0, characterResources.glovesMeshes.Count);
			bootsType = Random.Range (0, characterResources.bootMeshes.Count);
			cloakType = Random.Range (0, characterResources.cloakMeshes.Count);
			hairStyle = Random.Range (0, characterResources.hairMeshes.Count);
			beardStyle = Random.Range (0, characterResources.beardMeshes.Count);
			hairColor = Random.Range (0, characterResources.hairMaterials.Count);
			skinColor = Random.Range (0, characterResources.skinHeadMaterials.Count);
			UpdateCharacter ();
		}
		public void UpdateCharacter() {
			characterResources.head.SetActive (true);
			characterResources.eyes.SetActive (true);
			//HELMET ARMOR
			if (helmetType > 0) {
				if (characterResources.hair != null) {
					characterResources.hair.gameObject.SetActive (false);
				}
				if (characterResources.helmet != null) {
					characterResources.helmet.gameObject.SetActive (false);
				}
				if (characterResources.helmetMeshes.Count > helmetType - 1) {
					characterResources.helmet = characterResources.helmetMeshes [helmetType - 1];
					characterResources.helmet.SetActive (true);
				}
			} else {
				if (characterResources.helmet != null) {
					characterResources.helmet.gameObject.SetActive (false);
					characterResources.helmet = null;
				}
			}
			//CHEST ARMOR
			if (chestType > 0) {
				if (characterResources.chest != null) {
					characterResources.chest.gameObject.SetActive (false);
				}
				if (characterResources.chestMeshes.Count > chestType - 1) {
					characterResources.chest = characterResources.chestMeshes [chestType - 1];
					characterResources.chest.SetActive (true);
					characterResources.torso.SetActive (false);
					Material[] mats = characterResources.chest.GetComponent<Renderer> ().sharedMaterials;
					int i = 0;
					foreach (Material mat in mats) {
						//Simplify the names of the materials on our armor
						string matName = mat.name.Replace (" (Instance)", "");
						matName = matName.Replace (" 1", "");
						matName = matName.Replace (" 2", "");
						matName = matName.Replace (" 3", "");
						//Simplify the name of the material of our skin mesh
						string newMatName = characterResources.skinTorsoMaterials [skinColor].name.Replace (" (UnityEngine.Material)", "");
						newMatName = newMatName.Replace (" 1", "");
						newMatName = newMatName.Replace (" 2", "");
						newMatName = newMatName.Replace (" 3", "");
						//if the names are the same, update the armor's skin material to match our character's skin material
						if (matName == newMatName) {
							mats [i] = characterResources.skinTorsoMaterials [skinColor];
						}
						i += 1;
					}
					characterResources.chest.GetComponent<Renderer> ().sharedMaterials = mats;
				} else {
					characterResources.torso.SetActive (true);
				}
			} else {
				if (characterResources.chest != null) {
					characterResources.chest.gameObject.SetActive (false);
					characterResources.chest = null;
				}
				characterResources.torso.SetActive (true);
			}
			//LEG ARMOR
			if (leggingsType > 0) {
				if (characterResources.leggings != null) {
					characterResources.leggings.gameObject.SetActive (false);
				}
				if (characterResources.legMeshes.Count > leggingsType - 1) {
					characterResources.leggings = characterResources.legMeshes [leggingsType - 1];
					characterResources.leggings.SetActive (true);
					characterResources.legs.SetActive (false);
				} else {
					characterResources.legs.SetActive (true);
				}
			} else {
				if (characterResources.leggings != null) {
					characterResources.leggings.gameObject.SetActive (false);
					characterResources.leggings = null;
				}
				characterResources.legs.SetActive (true);
			}
			//GLOVE ARMOR
			if (glovesType > 0) {
				if (characterResources.gloves != null) {
					characterResources.gloves.gameObject.SetActive (false);
				}
				if (characterResources.glovesMeshes.Count > glovesType - 1) {
					characterResources.gloves = characterResources.glovesMeshes [glovesType - 1];
					characterResources.gloves.SetActive (true);
					characterResources.hands.SetActive (false);
					Material[] mats = characterResources.gloves.GetComponent<Renderer> ().sharedMaterials;
					int i = 0;
					foreach (Material mat in mats) {
						//Simplify the names of the materials on our armor
						string matName = mat.name.Replace (" (Instance)", "");
						matName = matName.Replace (" 1", "");
						matName = matName.Replace (" 2", "");
						matName = matName.Replace (" 3", "");
						//Simplify the name of the material of our skin mesh
						string newMatName = characterResources.skinHandsMaterials [skinColor].name.Replace (" (UnityEngine.Material)", "");
						newMatName = newMatName.Replace (" 1", "");
						newMatName = newMatName.Replace (" 2", "");
						newMatName = newMatName.Replace (" 3", "");
						//if the names are the same, update the armor's skin material to match our character's skin material
						if (matName == newMatName) {
							mats [i] = characterResources.skinHandsMaterials [skinColor];
						}
						i += 1;
					}
					characterResources.gloves.GetComponent<Renderer> ().sharedMaterials = mats;
				} else {
					characterResources.hands.SetActive (true);
				}
			} else {
				if (characterResources.gloves != null) {
					characterResources.gloves.gameObject.SetActive (false);
					characterResources.gloves = null;
				}
				characterResources.hands.SetActive (true);
			}
			//BOOTS ARMOR
			if (bootsType > 0) {
				if (characterResources.boots != null) {
					characterResources.boots.gameObject.SetActive (false);
				}
				if (characterResources.bootMeshes.Count > bootsType - 1) {
					characterResources.boots = characterResources.bootMeshes [bootsType - 1];
					characterResources.boots.SetActive (true);
					characterResources.feet.SetActive (false);
				} else {
					characterResources.feet.SetActive (true);
				}
			} else {
				if (characterResources.boots != null) {
					characterResources.boots.gameObject.SetActive (false);
					characterResources.boots = null;
				}
				characterResources.feet.SetActive (true);
			}
			//BACK ARMOR
			if (cloakType > 0) {
				if (characterResources.cloak != null) {
					characterResources.cloak.gameObject.SetActive (false);
				}
				if (characterResources.cloakMeshes.Count > cloakType - 1) {
					characterResources.cloak = characterResources.cloakMeshes [cloakType - 1];
					characterResources.cloak.SetActive (true);
				}
			} else {
				if (characterResources.cloak != null) {
					characterResources.cloak.gameObject.SetActive (false);
					characterResources.cloak = null;
				}
			}
			//HAIR STYLE
			if (hairStyle > 0) {
				if (characterResources.hair != null) {
					characterResources.hair.gameObject.SetActive (false);
				}
				if (characterResources.hairMeshes.Count > hairStyle - 1) {
					characterResources.hair = characterResources.hairMeshes [hairStyle - 1];
					if (helmetType <= 0) {
						characterResources.hair.SetActive (true);
					}
				}
			} else {
				if (characterResources.hair != null) {
					characterResources.hair.gameObject.SetActive (false);
					characterResources.hair = null;
				}
			}
			//BEARD STYLE
			if (characterResources.beardMeshes.Count > 0) {
				if (beardStyle > 0) {
					if (characterResources.beard != null) {
						characterResources.beard.gameObject.SetActive (false);
					}
					if (characterResources.beardMeshes.Count > beardStyle - 1) {
						characterResources.beard = characterResources.beardMeshes [beardStyle - 1];
						if (helmetType <= 0) {
							characterResources.beard.SetActive (true);
						}
					}
				} else {
					if (characterResources.beard != null) {
						characterResources.beard.gameObject.SetActive (false);
						characterResources.beard = null;
					}
				}
			}
			if (characterResources.hair != null) {
				characterResources.hair.GetComponent<SkinnedMeshRenderer> ().sharedMaterial = characterResources.hairMaterials [hairColor];
			}
			if (characterResources.beard != null) {
				characterResources.beard.GetComponent<SkinnedMeshRenderer> ().sharedMaterial = characterResources.hairMaterials [hairColor];
			}
			characterResources.head.GetComponent<SkinnedMeshRenderer> ().sharedMaterial = characterResources.skinHeadMaterials [skinColor];
			characterResources.torso.GetComponent<SkinnedMeshRenderer> ().sharedMaterial = characterResources.skinTorsoMaterials [skinColor];
			characterResources.legs.GetComponent<SkinnedMeshRenderer> ().sharedMaterial = characterResources.skinLegsMaterials [skinColor];
			characterResources.hands.GetComponent<SkinnedMeshRenderer> ().sharedMaterial = characterResources.skinHandsMaterials [skinColor];
			characterResources.feet.GetComponent<SkinnedMeshRenderer> ().sharedMaterial = characterResources.skinFeetMaterials [skinColor];
		}
		public void UpdateResourcesPath() {
			switch (gender) {
			case GenderType.Male:
				switch (characterType) {
				case CharacterType.Human:
					resourcesPath = "Human_male/";
					characterMaterial = "Character_human_male";
					break;
				case CharacterType.Dwarf:
					resourcesPath = "Dwarf_male/";
					characterMaterial = "Character_dwarf_male";
					break;
				case CharacterType.Elf:
					resourcesPath = "Elf_male/";
					characterMaterial = "Character_elf_male";
					break;
				default:
					resourcesPath = "Human_male/";
					characterMaterial = "Character_human_male";
					break;
				}
				break;
			case GenderType.Female:
				switch (characterType) {
				case CharacterType.Human:
					resourcesPath = "Human_female/";
					characterMaterial = "Character_human_female";
					break;
				case CharacterType.Dwarf:
					resourcesPath = "Dwarf_female/";
					characterMaterial = "Character_dwarf_female";
					break;
				case CharacterType.Elf:
					resourcesPath = "Elf_female/";
					characterMaterial = "Character_elf_female";
					break;
				default:
					resourcesPath = "Human_female/";
					characterMaterial = "Character_human_female";
					break;
				}
				break;
			default:
				resourcesPath = "Human_male/";
				characterMaterial = "Character_human_male";
				break;
			}
		}
		public void GetCharacterMeshes() {
			Component[] myChildren;
			myChildren = transform.GetComponentsInChildren<Transform>();

			foreach (Transform child in myChildren) {
				switch (child.gameObject.name) {
				case "Character_head":
					characterResources.head = child.gameObject;
					break;
				case "Character_eyes":
					characterResources.eyes = child.gameObject;
					break;
				case "Character_torso":
					characterResources.torso = child.gameObject;
					break;
				case "Character_legs":
					characterResources.legs = child.gameObject;
					break;
				case "Character_hands":
					characterResources.hands = child.gameObject;
					break;
				case "Character_feet":
					characterResources.feet = child.gameObject;
					break;
				default:
					break;
				}
			}
		}
		public void GetArmorMeshes() {
			Component[] myChildren;
			myChildren = transform.GetComponentsInChildren<SkinnedMeshRenderer>();

			if (characterResources.helmetMeshes.Count <= 0) {
				foreach (SkinnedMeshRenderer child in myChildren) {
					if (child.gameObject.name.StartsWith ("Helmet")) {
						characterResources.helmetMeshes.Add (child.gameObject);
					} else if (child.gameObject.name.StartsWith ("Chest")) {
						characterResources.chestMeshes.Add (child.gameObject);
					} else if (child.gameObject.name.StartsWith ("Legs")) {
						characterResources.legMeshes.Add (child.gameObject);
					} else if (child.gameObject.name.StartsWith ("Gloves")) {
						characterResources.glovesMeshes.Add (child.gameObject);
					} else if (child.gameObject.name.StartsWith ("Boots")) {
						characterResources.bootMeshes.Add (child.gameObject);
					} else if (child.gameObject.name.StartsWith ("Cloak")) {
						characterResources.cloakMeshes.Add (child.gameObject);
					} else if (child.gameObject.name.StartsWith ("Hair")) {
						characterResources.hairMeshes.Add (child.gameObject);
					} else if (child.gameObject.name.StartsWith ("Beard")) {
						characterResources.beardMeshes.Add (child.gameObject);
					}
					allMeshes.Add (child);
					child.gameObject.SetActive (false);
				}
			}
		}

		public void RemoveHiddenMeshes() {
			foreach (SkinnedMeshRenderer hiddenMesh in allMeshes) {
				if (!hiddenMesh.gameObject.activeSelf) {
					DestroyImmediate (hiddenMesh.gameObject);
				}
			}
			DestroyImmediate (this);
		}
	}
}

namespace LylekGames.RPGCharacter {
	[System.Serializable]
	public class CharacterResources {
		[Header("BODY MESHES")]
		public GameObject head;
		public GameObject eyes;
		public GameObject torso;
		public GameObject legs;
		public GameObject hands;
		public GameObject feet;

		[Header("EQUIPMENT")]
		public GameObject helmet;
		public GameObject chest;
		public GameObject leggings;
		public GameObject gloves;
		public GameObject boots;
		public GameObject cloak;
		public GameObject hair;
		public GameObject beard;


		[Header("ARMOR MESHES")]
		[SerializeField]
		public List<GameObject> helmetMeshes = new List<GameObject>();
		[SerializeField]
		public List<GameObject> chestMeshes = new List<GameObject>();
		[SerializeField]
		public List<GameObject> legMeshes = new List<GameObject>();
		[SerializeField]
		public List<GameObject> glovesMeshes = new List<GameObject>();
		[SerializeField]
		public List<GameObject> bootMeshes = new List<GameObject>();
		[SerializeField]
		public List<GameObject> cloakMeshes = new List<GameObject>();
		[SerializeField]
		public List<GameObject> hairMeshes = new List<GameObject>();
		[SerializeField]
		public List<GameObject> beardMeshes = new List<GameObject>();

		[Header("RESOURCE MATERIALS")]
		[SerializeField]
		public List<Material> hairMaterials = new List<Material>();
		[SerializeField]
		public List<Material> skinHeadMaterials = new List<Material>();
		[SerializeField]
		public List<Material> skinTorsoMaterials = new List<Material>();
		[SerializeField]
		public List<Material> skinLegsMaterials = new List<Material>();
		[SerializeField]
		public List<Material> skinHandsMaterials = new List<Material>();
		[SerializeField]
		public List<Material> skinFeetMaterials = new List<Material>();
	}
}
