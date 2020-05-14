using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LylekGames.RPGCharacter {
	public class EquippmentSwap : MonoBehaviour {

		public CharacterProperties characterProperties;

		void Awake() {
			if (GetComponent<CharacterProperties> ()) {
				characterProperties = GetComponent<CharacterProperties> ();
			}
		}
		public void EquippedNewArmor(GameObject newArmor, EquippmentType.ArmorType armorType) {
			if (armorType == EquippmentType.ArmorType.Beard && characterProperties.characterSpecs.gender == CharacterSpecs.GenderType.Female) {

			} else {
				DestroyArmor (armorType);
				SpawnNewArmor (newArmor, armorType);
				DisplayArmor (armorType);
			}
		}
		public void SpawnNewArmor(GameObject newArmor, EquippmentType.ArmorType armorType) {
			//Spawn our new armor piece
			GameObject armor = Instantiate(newArmor, transform.position, transform.rotation) as GameObject;
			//set our armor as a child of our character
			armor.transform.parent = transform;

			//if our armor has a cloth component, assign capsule colliders | this will make armor, such as a robe, move with the character
			if(armor.GetComponent<Cloth>()) {
				Cloth cloth = armor.GetComponent<Cloth>();
				cloth.enabled = false;

				if (armorType == EquippmentType.ArmorType.Cloak) {
					//Assign lower body and spine colliders
					List<CapsuleCollider> characterColliders = new List<CapsuleCollider>();
					foreach(CapsuleCollider collider in characterProperties.boneColliders.lowerBodyColliders) {
						characterColliders.Add (collider);
					}
					foreach (CapsuleCollider collider in characterProperties.boneColliders.spineColliders) {
						characterColliders.Add (collider);
					}
					cloth.capsuleColliders = characterColliders.ToArray ();
					cloth.enabled = true;
				} else {
					//assign only lower body colliders
					cloth.capsuleColliders = characterProperties.boneColliders.lowerBodyColliders;
					cloth.enabled = true;
				}
			}
			//set our armor's meshes' bones as our character's bones
			//this is done by accessing a skinned mesh renderer already animated by our character's armature. For this, we use a character body part, since we never Destroy() those.
			armor.GetComponent<SkinnedMeshRenderer>().bones = characterProperties.characterSkinMeshes.torso.GetComponent<SkinnedMeshRenderer>().bones;
			//assign our new armor to our CharacterProperties
			AssignNewArmor(armor, armorType);
		}
		public void DestroyArmor(EquippmentType.ArmorType armorType) { 
			//destroy currently equipped armor
			switch (armorType) {
			case EquippmentType.ArmorType.Chest:
				characterProperties.characterSkinMeshes.torso.gameObject.SetActive(true);
				if(characterProperties.characterArmorMeshes.torsoArmor) {
					Destroy(characterProperties.characterArmorMeshes.torsoArmor.gameObject);
					characterProperties.characterArmorMeshes.torsoArmor = null;
				}
				break;
			case EquippmentType.ArmorType.Legs:
				characterProperties.characterSkinMeshes.legs.gameObject.SetActive(true);
				if(characterProperties.characterArmorMeshes.legArmor) {
					Destroy(characterProperties.characterArmorMeshes.legArmor.gameObject);
					characterProperties.characterArmorMeshes.legArmor = null;
				}
				break;
			case EquippmentType.ArmorType.Hands:
				characterProperties.characterSkinMeshes.hands.gameObject.SetActive(true);
				if(characterProperties.characterArmorMeshes.handArmor) {
					Destroy(characterProperties.characterArmorMeshes.handArmor.gameObject);
					characterProperties.characterArmorMeshes.handArmor = null;
				}
				break;
			case EquippmentType.ArmorType.Feet:
				characterProperties.characterSkinMeshes.feet.gameObject.SetActive(true);
				if(characterProperties.characterArmorMeshes.feetArmor) {
					Destroy(characterProperties.characterArmorMeshes.feetArmor.gameObject);
					characterProperties.characterArmorMeshes.feetArmor = null;
				}
				break;
			case EquippmentType.ArmorType.Helmet:
				if(characterProperties.characterSkinMeshes.hair) {
					characterProperties.characterSkinMeshes.hair.gameObject.SetActive(true);
				}
				if(characterProperties.characterArmorMeshes.headArmor) {
					Destroy(characterProperties.characterArmorMeshes.headArmor.gameObject);
					characterProperties.characterArmorMeshes.headArmor = null;
				}
				break;
			case EquippmentType.ArmorType.Hair:
				if(characterProperties.characterArmorMeshes.headArmor) {
					characterProperties.characterArmorMeshes.headArmor.gameObject.SetActive(true);
				}
				if(characterProperties.characterSkinMeshes.hair) {
					Destroy(characterProperties.characterSkinMeshes.hair.gameObject);
					characterProperties.characterSkinMeshes.hair = null;
				}
				break;
			case EquippmentType.ArmorType.Beard:
				if(characterProperties.characterSkinMeshes.beard) {
					Destroy(characterProperties.characterSkinMeshes.beard.gameObject);
					characterProperties.characterSkinMeshes.beard = null;
				}
				break;
			case EquippmentType.ArmorType.Cloak:
				if(characterProperties.characterArmorMeshes.backArmor) {
					Destroy(characterProperties.characterArmorMeshes.backArmor.gameObject);
					characterProperties.characterArmorMeshes.backArmor = null;
				}
				break;
			default:
				break;
			}
		}
		public void DisplayArmor(EquippmentType.ArmorType armorType) {
			//Enable armor mesh and disable corresponding body mesh
			switch (armorType) {
			case EquippmentType.ArmorType.Helmet:
				if(characterProperties.characterSkinMeshes.hair) {
					characterProperties.characterSkinMeshes.hair.gameObject.SetActive(false);
				}
				break;
			case EquippmentType.ArmorType.Chest:
				characterProperties.characterSkinMeshes.torso.gameObject.SetActive(false);
				if(characterProperties.characterArmorMeshes.torsoArmor) {
					characterProperties.characterArmorMeshes.torsoArmor.gameObject.SetActive(true);
				}
				break;
			case EquippmentType.ArmorType.Legs:
				characterProperties.characterSkinMeshes.legs.gameObject.SetActive(false);
				if(characterProperties.characterArmorMeshes.legArmor) {
					characterProperties.characterArmorMeshes.legArmor.gameObject.SetActive(true);
				}
				break;
			case EquippmentType.ArmorType.Hands:
				characterProperties.characterSkinMeshes.hands.gameObject.SetActive(false);
				if(characterProperties.characterArmorMeshes.handArmor) {
					characterProperties.characterArmorMeshes.handArmor.gameObject.SetActive(true);
				}
				break;
			case EquippmentType.ArmorType.Feet:
				characterProperties.characterSkinMeshes.feet.gameObject.SetActive(false);
				if(characterProperties.characterArmorMeshes.feetArmor) {
					characterProperties.characterArmorMeshes.feetArmor.gameObject.SetActive(true);
				}
				break;
			case EquippmentType.ArmorType.Hair:
				if(characterProperties.characterArmorMeshes.headArmor) {
					characterProperties.characterArmorMeshes.headArmor.gameObject.SetActive(false);
				}
				if(characterProperties.characterSkinMeshes.hair) {
					characterProperties.characterSkinMeshes.hair.gameObject.SetActive(true);
					characterProperties.characterSkinMeshes.hair.GetComponent<Renderer>().sharedMaterial = characterProperties.characterSkinMeshes.hairColor;
				}
				break;
			case EquippmentType.ArmorType.Beard:
				if(characterProperties.characterSkinMeshes.beard) {
					characterProperties.characterSkinMeshes.beard.gameObject.SetActive(true);
					characterProperties.characterSkinMeshes.beard.GetComponent<Renderer>().sharedMaterial = characterProperties.characterSkinMeshes.hairColor;
				}
				break;
			case EquippmentType.ArmorType.Cloak:
				if(characterProperties.characterArmorMeshes.backArmor) {
					characterProperties.characterArmorMeshes.backArmor.gameObject.SetActive(true);
				}
				break;
			default:
				break;
			}
		}
		public void AssignNewArmor(GameObject newArmor, EquippmentType.ArmorType armorType) {
			switch (armorType) {
			case EquippmentType.ArmorType.Helmet:
				characterProperties.characterArmorMeshes.headArmor = newArmor;
				break;
			case EquippmentType.ArmorType.Chest:
				characterProperties.characterArmorMeshes.torsoArmor = newArmor;
				break;
			case EquippmentType.ArmorType.Legs:
				characterProperties.characterArmorMeshes.legArmor = newArmor;
				break;
			case EquippmentType.ArmorType.Hands:
				characterProperties.characterArmorMeshes.handArmor = newArmor;
				break;
			case EquippmentType.ArmorType.Feet:
				characterProperties.characterArmorMeshes.feetArmor = newArmor;
				break;
			case EquippmentType.ArmorType.Hair:
				characterProperties.characterSkinMeshes.hair = newArmor;
				break;
			case EquippmentType.ArmorType.Beard:
				characterProperties.characterSkinMeshes.beard = newArmor;
				break;
			case EquippmentType.ArmorType.Cloak:
				characterProperties.characterArmorMeshes.backArmor = newArmor;
				break;
			default:
				break;
			}
			//assign skin material to armors requiring more than one material
			if (newArmor.GetComponent<Renderer> ().sharedMaterials.Length > 1) {
				characterProperties.characterArmorMeshes.UpdateSkinArmorMaterials (characterProperties.characterSkinMeshes.mySkinMaterials);
			}
		}
		public void HideArmor(EquippmentType.ArmorType armorType) {
			//Disable armor mesh and enable corresponding body mesh
			switch (armorType) {
			case EquippmentType.ArmorType.Helmet:
				if(characterProperties.characterSkinMeshes.hair) {
					characterProperties.characterSkinMeshes.hair.gameObject.SetActive(true);
				}
				if(characterProperties.characterArmorMeshes.headArmor) {
					characterProperties.characterArmorMeshes.headArmor.gameObject.SetActive(false);
				}
				break;
			case EquippmentType.ArmorType.Chest:
				characterProperties.characterSkinMeshes.torso.gameObject.SetActive(true);
				if(characterProperties.characterArmorMeshes.torsoArmor) {
					characterProperties.characterArmorMeshes.torsoArmor.gameObject.SetActive(false);
				}
				break;
			case EquippmentType.ArmorType.Legs:
				characterProperties.characterSkinMeshes.legs.gameObject.SetActive(true);
				if(characterProperties.characterArmorMeshes.legArmor) {
					characterProperties.characterArmorMeshes.legArmor.gameObject.SetActive(false);
				}
				break;
			case EquippmentType.ArmorType.Hands:
				characterProperties.characterSkinMeshes.hands.gameObject.SetActive(true);
				if(characterProperties.characterArmorMeshes.handArmor) {
					characterProperties.characterArmorMeshes.handArmor.gameObject.SetActive(false);
				}
				break;
			case EquippmentType.ArmorType.Feet:
				characterProperties.characterSkinMeshes.feet.gameObject.SetActive(true);
				if(characterProperties.characterArmorMeshes.feetArmor) {
					characterProperties.characterArmorMeshes.feetArmor.gameObject.SetActive(false);
				}
				break;
			case EquippmentType.ArmorType.Cloak:
				if(characterProperties.characterArmorMeshes.backArmor) {
					characterProperties.characterArmorMeshes.backArmor.gameObject.SetActive(false);
				}
				break;
			default:
				break;
			}
		}
	}
}
