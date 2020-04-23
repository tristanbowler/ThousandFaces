using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

namespace LylekGames.RPGCharacter {
	[System.Serializable]
	public class CharacterArmorMeshes {

		public GameObject headArmor;
		public GameObject torsoArmor;
		public GameObject legArmor;
		public GameObject handArmor;
		public GameObject feetArmor;
		public GameObject backArmor;

		public IEnumerator UpdateSkinArmorMaterials(SkinMaterials skinMaterials) {
			yield return null;
			List<GameObject> armors = new List<GameObject> ();
			armors.Add (headArmor);
			armors.Add (torsoArmor);
			armors.Add (legArmor);
			armors.Add (handArmor);
			armors.Add (feetArmor);

			List<Material> m = new List<Material> ();
			m.Add (skinMaterials.headMaterial);
			m.Add (skinMaterials.chestMaterial);
			m.Add (skinMaterials.legMaterial);
			m.Add (skinMaterials.handMaterial);
			m.Add (skinMaterials.feetMaterial);

			int k = 0;
			foreach(GameObject armor in armors) {
				if (armor != null) {
					Material[] mats = armor.GetComponent<Renderer> ().sharedMaterials;
					int i = 0;
					foreach (Material mat in mats) {
						//Simplify the names of the materials on our armor
						string matName = NewArmorMaterialName (mat.name);
						//Simplify the name of the material of our skin mesh
						string newMatName = NewSkinMaterialName(m [k].name);
						//if the names are the same, update the armor's skin material to match our character's skin material
						if (matName == newMatName) {
							mats [i] = m [k];
						}
						i += 1;
					}
					armor.GetComponent<Renderer> ().sharedMaterials = mats;
				}
				k += 1;
			}
		}
		public string NewArmorMaterialName(string materialName) {
			materialName = materialName.Replace (" (Instance)", "");
			materialName = materialName.Replace (" 1", "");
			materialName = materialName.Replace (" 2", "");
			materialName = materialName.Replace (" 3", "");
			return materialName;
		}
		public string NewSkinMaterialName(string materialName) {
			materialName = materialName.Replace (" (UnityEngine.Material)", "");
			materialName = materialName.Replace (" 1", "");
			materialName = materialName.Replace (" 2", "");
			materialName = materialName.Replace (" 3", "");
			return materialName;
		}
	}
}
