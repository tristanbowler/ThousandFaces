using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.IO;
using LylekGames.RPGCharacter;

namespace LylekGames.RPGCharacter {
[CustomEditor(typeof(AssignSkinWeights))]
	public class AssignSkinWeightsEditor : Editor {

		public void OnEnable() {
			AssignSkinWeights myASW = (AssignSkinWeights)target;
			myASW.Initialize ();
		}

		public override void OnInspectorGUI() {
			AssignSkinWeights myASW = (AssignSkinWeights)target;
			DrawDefaultInspector();
			if (myASW.armatureRoot) {
				if (GUILayout.Button ("1.) Add Excluded Bones (UpperBody)")) {
					myASW.GetExcludedBonesUpperBoddy ();
				}
				if (GUILayout.Button ("2.) Find NEW Skinned Meshes")) {
					myASW.GetNewSkinnedMeshes ();
				}
				if (GUILayout.Button ("3.) Assign Skin Meshes")) {
					myASW.AssignSkinMeshes ();
				}
			} else {
				if (GUILayout.Button ("Initialize")) {
					myASW.Initialize ();
				}
			}
		}
	}
}
