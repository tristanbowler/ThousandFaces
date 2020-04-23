using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.IO;
using LylekGames.RPGCharacter;

namespace LylekGames.RPGCharacter {
	[CustomEditor(typeof(CombineSkinMeshesMobile))]
	public class CombineSkinMeshesMobileEditor : Editor {
		CombineSkinMeshesMobile myCombine;

		public void OnEnable() {
			if (Application.isPlaying == false) {
				myCombine = (CombineSkinMeshesMobile)target;
				myCombine.Initialize();
			}
		}

		public override void OnInspectorGUI() {
			myCombine = (CombineSkinMeshesMobile)target;
			DrawDefaultInspector();
			if(GUILayout.Button("1.) Combine Meshes")) {
				myCombine.BeginCombineMeshes();
			}
			if(GUILayout.Button("2.) Save Mesh Data")) {
				SaveMeshData ();
			}
			if(GUILayout.Button("Disassemble Mesh")) {
				myCombine.DisassembleMesh();
			}
			if (myCombine.displayHiddenButtons) {
				if(GUILayout.Button("Recalculate Bones")) {
					myCombine.RecalculateBones();
				}
			}
		}
		public void SaveMeshData() {
			if(!Directory.Exists(Application.dataPath + "/RPGCharacter/Resources/Characters/")) {
				Directory.CreateDirectory(Application.dataPath + "/RPGCharacter/Resources/Characters/");
				if(!Directory.Exists(Application.dataPath + "/RPGCharacter/Resources/Characters/Textures/")) {
					Directory.CreateDirectory(Application.dataPath + "/RPGCharacter/Resources/Characters/Textures/");
				}
				if(!Directory.Exists(Application.dataPath + "/RPGCharacter/Resources/Characters/Materials/")) {
					Directory.CreateDirectory(Application.dataPath + "/RPGCharacter/Resources/Characters/Materials/");
				}
				if(!Directory.Exists(Application.dataPath + "/RPGCharacter/Resources/Characters/Meshes/")) {
					Directory.CreateDirectory(Application.dataPath + "/RPGCharacter/Resources/Characters/Meshes/");
				}
			}
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();
			//SAVE TEXTURES
			Material myMaterial = myCombine.myCombinedMesh.GetComponent<SkinnedMeshRenderer>().sharedMaterial;
			SaveTexture(myCombine.diffuseAtlas, "_diffuse");
			SaveTexture(myCombine.normalsAtlas, "_normals");
			//SAVE MATERIAL
			SaveMaterial(myMaterial);
			//SAVE MESH AND CREATE PREFAB
			SaveMesh (myCombine.myCombinedMesh);
			//RE-ADJUST THE IMPORT SETTINGS OF OUR NORMAL MAP
			Texture2D saveTexture = (Texture2D)Resources.Load("Characters/Textures/" + myCombine.saveName + "_normals");
			string textureAssetPath = AssetDatabase.GetAssetPath (saveTexture);
			TextureImporter textureImport = (TextureImporter)TextureImporter.GetAtPath (textureAssetPath);
			textureImport.textureType = TextureImporterType.Default;
			textureImport.sRGBTexture = false;
			textureImport.SaveAndReimport ();
			Debug.Log ("Data saved successfully.");
			Debug.Log ("If a dialog appears requesting to correct any normal maps, PLEASE PRESS 'IGNORE'.");
		}
		public void SaveTexture(Texture2D myTexture, string extension) {
			//CREATE A TEXTURE TO SAVE
			Texture2D newTexture = new Texture2D(myTexture.width, myTexture.height, myCombine.textureFormat, myCombine.isMipMap);
			//GET IMAGE
			newTexture.SetPixels(0, 0, myTexture.width, myTexture.height, myTexture.GetPixels());
			newTexture.Apply();
			//ENCODE TEXTURE TO PNG
			byte[] bytes = newTexture.EncodeToPNG();
			File.WriteAllBytes (Application.dataPath + "/RPGCharacter/Resources/Characters/Textures/" + myCombine.saveName + extension + ".png", bytes);
			//FORCE IMPORT OUR PNGs (so unity notices them and we can use them immediately)
			//AssetDatabase.ImportAsset(Application.dataPath + "/RPGCharacter/Resources/Characters/Textures/" + myCombine.saveName + extension + ".png", ImportAssetOptions.ForceUpdate); 
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();
			//ADJUST THE IMPORT SETTINGS OF OUR NORMAL MAP
			if (extension == "_normals") {
				Texture2D saveTexture = (Texture2D)Resources.Load("Characters/Textures/" + myCombine.saveName + "_normals");
				string textureAssetPath = AssetDatabase.GetAssetPath (saveTexture);
				TextureImporter textureImport = (TextureImporter)TextureImporter.GetAtPath (textureAssetPath);
				textureImport.textureType = TextureImporterType.NormalMap;
				textureImport.SaveAndReimport ();
			}
		}
		public void SaveMaterial(Material myMaterial) {
			//CREATE AND SAVE A NEW MATERIAL
			AssetDatabase.CreateAsset (myMaterial, "Assets/RPGCharacter/Resources/Characters/Materials/" + myCombine.saveName + ".mat");
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();
			//GET REFERENCE OF THIS NEW MATERIAL
			myMaterial = (Material)Resources.Load ("Characters/Materials/" + myCombine.saveName);
			//ASSIGN OUR SAVED TEXTURES TO OUR MATERIAL
			myMaterial.mainTexture = (Texture2D)Resources.Load("Characters/Textures/" + myCombine.saveName + "_diffuse");
			myMaterial.SetTexture ("_BumpMap", (Texture2D)Resources.Load("Characters/Textures/" + myCombine.saveName + "_normals"));
			myMaterial.SetTexture ("_MetallicGlossMap", (Texture2D)Resources.Load("Characters/Textures/" + myCombine.saveName + "_specular"));
			//ASSIGN THIS NEWLY CREATED MATERIAL TO OUR CHARACTER MESH
			myCombine.myCombinedMesh.GetComponent<SkinnedMeshRenderer> ().material = myMaterial;
		}
		public void SaveMesh(GameObject myMesh) {
			//SAVE MESH DATA
			SkinnedMeshRenderer mySkinnedMesh = myMesh.GetComponent<SkinnedMeshRenderer> ();
			AssetDatabase.CreateAsset (mySkinnedMesh.sharedMesh, "Assets/RPGCharacter/Resources/Characters/Meshes/" + myCombine.saveName + ".asset");
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();
			//SET OUR CHARACTER'S MESH DATA AS OUR NEW SAVED MESH DATA
			myCombine.myCombinedMesh.GetComponent<SkinnedMeshRenderer>().sharedMesh = (Mesh)Resources.Load("Characters/Meshes/" + myCombine.saveName);
			//------------------CREATE A PREFAB OF THIS GAMEOBJECT------------------//
			PrefabUtility.CreatePrefab("Assets/RPGCharacter/Resources/Characters/" + myCombine.saveName + ".prefab", myCombine.gameObject);
		}
	}
}
