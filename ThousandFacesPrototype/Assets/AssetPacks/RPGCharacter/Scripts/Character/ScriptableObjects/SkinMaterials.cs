using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LylekGames.RPGCharacter;

namespace LylekGames.RPGCharacter {
[CreateAssetMenu()]
	[System.Serializable]
	public class SkinMaterials : ScriptableObject {
		public Material headMaterial;
		public Material chestMaterial;
		public Material legMaterial;
		public Material handMaterial;
		public Material feetMaterial;
	}
}
