using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LylekGames.RPGCharacter;

namespace LylekGames.RPGCharacter {
	[System.Serializable]
	public class CharacterBoneCollision {

		public CapsuleCollider[] lowerBodyColliders;
		public CapsuleCollider[] spineColliders;
		public CapsuleCollider[] armColliders;
		public CapsuleCollider head;

		public CharacterBoneCollision() {
			lowerBodyColliders = new CapsuleCollider[8];
			armColliders = new CapsuleCollider[6];
			spineColliders = new CapsuleCollider[2];
		}

		//Returns an array of all capsule colliders (upper and lower body)
		public CapsuleCollider[] CharacterColliders() {
			List<CapsuleCollider> allCharacterColliders = new List<CapsuleCollider>();
			foreach(CapsuleCollider collider in lowerBodyColliders) {
				allCharacterColliders.Add (collider);
			}
			foreach (CapsuleCollider collider in armColliders) {
				allCharacterColliders.Add (collider);
			}
			foreach (CapsuleCollider collider in spineColliders) {
				allCharacterColliders.Add (collider);
			}
			allCharacterColliders.Add (head);
			return allCharacterColliders.ToArray();
		}
		//Get all colliders
		public void FindCharacterColliders(GameObject character) {
			//This function will retrieve our character colliders, from our character gameObject
			Component[] myChildren;
			myChildren = character.transform.GetComponentsInChildren<Transform>();

			//assign our character components / body parts
			foreach(Transform child in myChildren) {
				switch (child.gameObject.name) {
				//UpperBody
				case "Spine1":
					spineColliders[0] = child.GetComponent<CapsuleCollider>();
					break;
				case "Spine3":
					spineColliders[1] = child.GetComponent<CapsuleCollider>();
					break;
				case "Head":
					head = child.GetComponent<CapsuleCollider>();
					break;
				case "UpperArm_L":
					armColliders[0] = child.GetComponent<CapsuleCollider>();
					break;
				case "UpperArm_R":
					armColliders[1] = child.GetComponent<CapsuleCollider>();
					break;
				case "LowerArm_L":
					armColliders[2] = child.GetComponent<CapsuleCollider>();
					break;
				case "LowerArm_R":
					armColliders[3] = child.GetComponent<CapsuleCollider>();
					break;
				case "Hand_L":
					armColliders[4] = child.GetComponent<CapsuleCollider>();
					break;
				case "Hand_R":
					armColliders[5] = child.GetComponent<CapsuleCollider>();
					break;
				//LowerBody
				case "Hip_L":
					lowerBodyColliders[0] = child.GetComponent<CapsuleCollider>();
					break;
				case "Hip_R":
					lowerBodyColliders[1] = child.GetComponent<CapsuleCollider>();
					break;
				case "UpperLeg_L":
					lowerBodyColliders[2] = child.GetComponent<CapsuleCollider>();
					break;
				case "UpperLeg_R":
					lowerBodyColliders[3] = child.GetComponent<CapsuleCollider>();
					break;
				case "LowerLeg_L":
					lowerBodyColliders[4] = child.GetComponent<CapsuleCollider>();
					break;
				case "LowerLeg_R":
					lowerBodyColliders[5] = child.GetComponent<CapsuleCollider>();
					break;
				case "Foot_L":
					lowerBodyColliders[6] = child.GetComponent<CapsuleCollider>();
					break;
				case "Foot_R":
					lowerBodyColliders[7] = child.GetComponent<CapsuleCollider>();
					break;
				default:
					break;
				}
			}
		}

	}
}
