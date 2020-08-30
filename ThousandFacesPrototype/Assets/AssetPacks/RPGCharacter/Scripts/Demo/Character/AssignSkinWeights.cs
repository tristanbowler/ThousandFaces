using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attached to character's armature
//This script will assign proper bone weights to meshes (such as armors), on start
[System.Serializable]
public class AssignSkinWeights : MonoBehaviour {

	[SerializeField]
	public GameObject armatureRoot; //character's armature root (top/first of skeleton hierarchy (before root/master/pelvis)) | if not assigns, assume it is this object
	[SerializeField]
	public SkinnedMeshRenderer defaultSkinWeight; //the skinned mesh renderer of an object containing the weight data we require | we use the character's torso, which came rigg to this character, on import
	[SerializeField]
	public List<GameObject> excludedBones = new List<GameObject>(); //ignore these bones, even if they have a capsule collider. We do not want them affecting our Cloth
	public List<SkinnedMeshRenderer> skinMeshes = new List<SkinnedMeshRenderer>(); //our skinned meshes to be weighted | assign in the inspector
	public bool assignCollidersToCloth = true; //assign colliders to meshes containing a Cloth component?
	public bool setFeetToIgnoreRaycasts = false; //enable this if using a system that checks character's feet distance from ground
	[SerializeField]
	public List<CapsuleCollider> boneColliders = new List<CapsuleCollider>(); //list of colliders attached to our character's bones | this will automatically be calculated on start

	public void Initialize() {
		GetArmatureRoot ();
		GetDefaultSkinWeightReference ();
		if (armatureRoot) {
			GetBoneColliders ();
		}
	}
	public void GetArmatureRoot() {
		Transform[] children;
		children = transform.GetComponentsInChildren<Transform> ();

		foreach (Transform child in children) {
			if (child.name == "Armature") {
				armatureRoot = child.gameObject;
				break;
			}
		}
		if (!armatureRoot) {
			Debug.LogWarning ("Unable to locate 'Armature'. Please assign Armature Root manually.");
		} else {
			Debug.Log ("Armature found!");
		}
	}
	public void GetDefaultSkinWeightReference() {
		SkinnedMeshRenderer[] allSkinnedMeshes;

		allSkinnedMeshes = transform.GetComponentsInChildren<SkinnedMeshRenderer>();

		foreach (SkinnedMeshRenderer skinnedMesh in allSkinnedMeshes) {
			if (skinnedMesh.rootBone) {
				defaultSkinWeight = skinnedMesh;
				break;
			}
		}
		if (!defaultSkinWeight) {
			Debug.LogWarning ("Unable to locate any SkinnedMeshRenderers with proper bone weights. Please assign Default Skin Weight manually.");
		} else {
			Debug.Log ("Default Skin Weight found!");
		}
	}
	public void GetBoneColliders() {
		boneColliders.Clear ();
		CapsuleCollider[] armatureColliders;
		armatureColliders = armatureRoot.transform.GetComponentsInChildren<CapsuleCollider>();

		//sort between the ones we want, and the ones to exclude
		foreach(CapsuleCollider boneCol in armatureColliders) {
			//SET FEET COLLIDERS TO IgnoreRaycast LAYER
			if(setFeetToIgnoreRaycasts == true) {
				if(boneCol.gameObject.name == "Foot_L" || boneCol.gameObject.name == "Foot_R") {
					boneCol.gameObject.layer = 2;
					Debug.Log("Assigned IgnoreRaycast layer to " + boneCol.gameObject.name);
				}
			}
			bool excludedFromColliders = false;
			foreach(GameObject excluded in excludedBones) {
				//if the collider is not excluded, add it to our list of colliders
				if(boneCol.gameObject == excluded) {
					excludedFromColliders = true;
				}
			}
			if(excludedFromColliders == false) {
				boneColliders.Add(boneCol);
			}
		}
	}
	public void GetNewSkinnedMeshes() {
		skinMeshes.Clear ();
		SkinnedMeshRenderer[] allSkinnedMeshes;
		allSkinnedMeshes = transform.GetComponentsInChildren<SkinnedMeshRenderer>();

		foreach (SkinnedMeshRenderer skinnedMesh in allSkinnedMeshes) {
			if (skinnedMesh.rootBone == null) {
				skinMeshes.Add (skinnedMesh);
			}
		}

		if (skinMeshes.Count == 0) {
			Debug.Log ("No new skin meshes have been found. All SkinnedMeshRenderers already have bone weights!");
		} else {
			Debug.Log ("New skin meshes have been added!");
		}
	}
	public void AssignSkinMeshes () {
		if(!armatureRoot) {
			Debug.Log("ArmatureRoot was not assigned. AssignSkinWeight will try to use THIS gameObject, until otherwise specified.");
			armatureRoot = gameObject;
		}

		foreach(SkinnedMeshRenderer mesh in skinMeshes) {
			if(assignCollidersToCloth) {
				//if mesh has Cloth, assign bones
				if(mesh.gameObject.GetComponent<Cloth>()) {
					mesh.gameObject.GetComponent<Cloth>().capsuleColliders = boneColliders.ToArray();
				}
			}
			mesh.bones = defaultSkinWeight.bones;
			mesh.rootBone = defaultSkinWeight.rootBone;
		}
		if (skinMeshes.Count > 0) {
			Debug.Log ("Skin Weights have been assigned!");
		} else {
			Debug.Log ("No skin meshes to assign.");
		}
		skinMeshes.Clear ();
	}

	public void GetExcludedBonesUpperBoddy() {
		excludedBones.Clear ();
		if (!armatureRoot) {
			GetArmatureRoot ();
		}
		CapsuleCollider[] armatureColliders;
		armatureColliders = armatureRoot.transform.GetComponentsInChildren<CapsuleCollider>();
		string[] upperBodyBoneNames = new string[] {"Spine1", "Spine3", "Head", "UpperArm_L",  "UpperArm_R", "LowerArm_L", "LowerArm_R", "Hand_L", "Hand_R"};

		foreach (CapsuleCollider boneCol in armatureColliders) {
			foreach(string boneName in upperBodyBoneNames) {
				if (boneCol.gameObject.name == boneName) {
					excludedBones.Add(boneCol.gameObject);
				}
			}
		}
		GetBoneColliders ();
		Debug.Log ("Excluded upper body bones.");
	}
}
