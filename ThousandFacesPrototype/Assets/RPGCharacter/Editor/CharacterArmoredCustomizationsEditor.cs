using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LylekGames.RPGCharacter {
	[CustomEditor(typeof(CharacterArmoredCustomizations))]
	public class CharacterArmoredCustomizationsEditor : Editor {
		CharacterArmoredCustomizations myArmoredCustomizations;

		public void OnEnable() {
			if (Application.isPlaying == false) {
				myArmoredCustomizations = (CharacterArmoredCustomizations)target;
			}
		}

		public override void OnInspectorGUI() {
			myArmoredCustomizations = (CharacterArmoredCustomizations)target;
			if (myArmoredCustomizations.initialized) {
				DrawDefaultInspector ();
			} else {
				myArmoredCustomizations.RPGCharacterPath = EditorGUILayout.TextField (new GUIContent("RPGCharacter Path", "The path of the RPGCharacter folder."), myArmoredCustomizations.RPGCharacterPath);
				myArmoredCustomizations.characterType = (CharacterArmoredCustomizations.CharacterType)EditorGUILayout.EnumPopup (myArmoredCustomizations.characterType);
				myArmoredCustomizations.gender = (CharacterArmoredCustomizations.GenderType)EditorGUILayout.EnumPopup (myArmoredCustomizations.gender);

				if (GUILayout.Button ("Update Character")) {
					//Check to see if we can get the materials from the provided path
					Material testMat;
					testMat = (Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Hair/Hair_a.mat", typeof(Material));
					if (testMat != null) {
						UpdateCharacter ();
					} else {
						//The path was not found, let's see if the user forgot an "/" at the end and try again
						testMat = (Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "/" + "RPGCharacter/Materials/Hair/Hair_a.mat", typeof(Material));
						if (testMat != null) {
							//The user simply forgot an "/" on the end, add it to the string and continue
							myArmoredCustomizations.RPGCharacterPath = myArmoredCustomizations.RPGCharacterPath + "/";
							UpdateCharacter ();
						} else {
							Debug.LogError ("UH-OH! It looks like the Resource Path is incorrect: " + myArmoredCustomizations.RPGCharacterPath);
						}
					}
				}
			}
			if (myArmoredCustomizations.myCombine) {
				if (myArmoredCustomizations.combined == false) {
					if (GUILayout.Button ("Randomize Appearance")) {
						myArmoredCustomizations.Randomize ();
					}
					if (GUILayout.Button ("Combine Meshes")) {
						myArmoredCustomizations.myCombine.BeginCombineMeshes ();
						myArmoredCustomizations.combined = true;
					}
					if (GUILayout.Button ("Permanently Combine Meshes")) {
						myArmoredCustomizations.myCombine.BeginCombineMeshes ();
						myArmoredCustomizations.RemoveHiddenMeshes ();
					}
				} else {
					if (GUILayout.Button ("Disassemble Mesh")) {
						myArmoredCustomizations.myCombine.DisassembleMesh ();
						myArmoredCustomizations.combined = false;
					}
				}
			}
		}
		public void UpdateCharacter() {
			myArmoredCustomizations.Initialize ();
			GetResourceMaterials ();
			myArmoredCustomizations.UpdateCharacter ();
		}
		public void GetResourceMaterials() {
			if (myArmoredCustomizations.characterResources.hairMaterials.Count <= 0) {
				myArmoredCustomizations.characterResources.hairMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Hair/Hair_a.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.hairMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Hair/Hair_b.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.hairMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Hair/Hair_c.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.hairMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Hair/Hair_d.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.hairMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Hair/Hair_e.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.hairMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Hair/Hair_f.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.hairMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Hair/Hair_g.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.hairMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Hair/Hair_h.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.hairMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Hair/Hair_i.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.hairMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Hair/Hair_j.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.hairMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Hair/Hair_k.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.hairMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Hair/Hair_l.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.hairMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Hair/Hair_m.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.hairMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Hair/Hair_n.mat", typeof(Material)));
			}

			if (myArmoredCustomizations.characterResources.skinHeadMaterials.Count <= 0) {
				myArmoredCustomizations.characterResources.skinHeadMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Characters/"
				+ myArmoredCustomizations.resourcesPath + myArmoredCustomizations.characterMaterial + "_head.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.skinHeadMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Characters/"
				+ myArmoredCustomizations.resourcesPath + myArmoredCustomizations.characterMaterial + "_head 1.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.skinHeadMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Characters/"
				+ myArmoredCustomizations.resourcesPath + myArmoredCustomizations.characterMaterial + "_head 2.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.skinHeadMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Characters/"
				+ myArmoredCustomizations.resourcesPath + myArmoredCustomizations.characterMaterial + "_head 3.mat", typeof(Material)));

				myArmoredCustomizations.characterResources.skinTorsoMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Characters/"
				+ myArmoredCustomizations.resourcesPath + myArmoredCustomizations.characterMaterial + "_chest.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.skinTorsoMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Characters/"
				+ myArmoredCustomizations.resourcesPath + myArmoredCustomizations.characterMaterial + "_chest 1.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.skinTorsoMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Characters/"
				+ myArmoredCustomizations.resourcesPath + myArmoredCustomizations.characterMaterial + "_chest 2.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.skinTorsoMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Characters/"
				+ myArmoredCustomizations.resourcesPath + myArmoredCustomizations.characterMaterial + "_chest 3.mat", typeof(Material)));

				myArmoredCustomizations.characterResources.skinLegsMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Characters/"
				+ myArmoredCustomizations.resourcesPath + myArmoredCustomizations.characterMaterial + "_legs.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.skinLegsMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Characters/"
				+ myArmoredCustomizations.resourcesPath + myArmoredCustomizations.characterMaterial + "_legs 1.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.skinLegsMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Characters/"
				+ myArmoredCustomizations.resourcesPath + myArmoredCustomizations.characterMaterial + "_legs 2.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.skinLegsMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Characters/"
				+ myArmoredCustomizations.resourcesPath + myArmoredCustomizations.characterMaterial + "_legs 3.mat", typeof(Material)));

				myArmoredCustomizations.characterResources.skinHandsMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Characters/"
				+ myArmoredCustomizations.resourcesPath + myArmoredCustomizations.characterMaterial + "_hands.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.skinHandsMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Characters/"
				+ myArmoredCustomizations.resourcesPath + myArmoredCustomizations.characterMaterial + "_hands 1.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.skinHandsMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Characters/"
				+ myArmoredCustomizations.resourcesPath + myArmoredCustomizations.characterMaterial + "_hands 2.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.skinHandsMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Characters/"
				+ myArmoredCustomizations.resourcesPath + myArmoredCustomizations.characterMaterial + "_hands 3.mat", typeof(Material)));

				myArmoredCustomizations.characterResources.skinFeetMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Characters/"
				+ myArmoredCustomizations.resourcesPath + myArmoredCustomizations.characterMaterial + "_feet.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.skinFeetMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Characters/"
				+ myArmoredCustomizations.resourcesPath + myArmoredCustomizations.characterMaterial + "_feet 1.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.skinFeetMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Characters/"
				+ myArmoredCustomizations.resourcesPath + myArmoredCustomizations.characterMaterial + "_feet 2.mat", typeof(Material)));
				myArmoredCustomizations.characterResources.skinFeetMaterials.Add ((Material)AssetDatabase.LoadAssetAtPath (myArmoredCustomizations.RPGCharacterPath + "RPGCharacter/Materials/Characters/"
				+ myArmoredCustomizations.resourcesPath + myArmoredCustomizations.characterMaterial + "_feet 3.mat", typeof(Material)));
			}
		}
	}
}
