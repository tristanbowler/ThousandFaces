using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace LylekGames.RPGCharacter {
	public class CombineSkinMeshesMobile : MonoBehaviour {

		private bool initiallized = false;
		public bool invertMatrix = true;
		[Header("Character Name")]
		public string saveName = "MyCharacterName";
		[Header("Animation Info")]
		public GameObject armature; //*required to ensure proper mesh deform when combining meshes
		public GameObject masterBone; //*required to ensure proper mesh deform when combining meshes
		public Animator anim;
		public AnimatorCullingMode animCullingMode = AnimatorCullingMode.AlwaysAnimate;
		[Header("Texture Atlasing")]
		public TextureAtlasSize textureAtlasSize = TextureAtlasSize.x1024;
		public bool isMipMap = true;
		[HideInInspector]
		public TextureFormat textureFormat = TextureFormat.RGBA32;
		[Header("Meshes")]
		[Tooltip("When combing meshes (if meshes are not manually assigned) only search for immediate children, ignoring meshes which may be stored as children of children and within the character's armature.")]
		public bool combineImmediateChildrenOnly = true;
		[Tooltip("When enabled, Skinned Meshes containing a Cloth Component such as robes or a cape will not be combined as to retain their Cloth physics.")]
		public bool excludeClothMeshes = false;
		[HideInInspector]
		[SerializeField]
		public GameObject myCombinedMesh;
		[SerializeField]
		private SkinnedMeshRenderer[] mySkinnedMeshes;
		[Header("Bones")]
		[SerializeField]
		private List<Vector3> defaultBonePositions = new List<Vector3>(); //*required to ensure proper mesh deform when combining meshes
		[SerializeField]
		private List<Quaternion> defaultBoneRotations = new List<Quaternion>(); //*required to ensure proper mesh deform when combining meshes
		[SerializeField]
		private List<Transform> myBones = new List<Transform>(); //*required to ensure proper mesh deform when combining meshes
		private List<Transform> bones = new List<Transform>();
		private List<BoneWeight> boneWeights = new List<BoneWeight>();
		private List<CombineInstance> combineInstances = new List<CombineInstance>();
		[Header("Textures")]
		public List<Texture2D> texturesDiffuse = new List<Texture2D>();
		public List<Texture2D> texturesNormals = new List<Texture2D>();
		public List<Texture2D> defaultNormsList = new List<Texture2D>(); //64, 128, 256, 512, 1024
		private Texture2D defaultNorms;
		[HideInInspector]
		public Texture2D diffuseAtlas;
		[HideInInspector]
		public Texture2D normalsAtlas;
		[Header("Buttons")]
		public bool displayHiddenButtons = false;

		//TEXTURE SIZES------------------
		public enum TextureAtlasSize {
			x64 = 64,
			x128 = 128,
			x256 = 256,
			x512 = 512,
			x1024 = 1024,
			x2048 = 2048,
		}

		//CALLED IN EDITOR, AT THE TIME THE SCRIPT IS ADDED TO GAMEOBJECT------------------
		public void Initialize() {
			if (initiallized == false) {
				//GET OUR ARMATURE
				foreach (Transform child in transform) {
					if (child.gameObject.name == "Armature") {
						armature = child.gameObject;
						break;
					}
				}
				if (!masterBone) {
					if (armature.transform.GetChild (0).gameObject.name == "Master") {
						masterBone = armature.transform.GetChild(0).gameObject;
					}
				}
				if (!armature) {
					Debug.LogWarning ("Armature not found! Please assign an Armature!");
				}
				//GET ANIMATOR
				if (GetComponent<Animator> ()) {
					anim = GetComponent<Animator> ();
				} else {
					Debug.LogWarning ("Animator not found! Please assign an Animator Component.");
				}
				//GET BONES AND DEFAULT BONE INFORMATION
				myBones.Clear();
				defaultBonePositions.Clear ();
				defaultBoneRotations.Clear ();
				myBones.Add (armature.transform);
				defaultBonePositions.Add (armature.transform.localPosition);
				defaultBoneRotations.Add (armature.transform.localRotation);
				if (masterBone) {
					myBones.Add (masterBone.transform);
					defaultBonePositions.Add (masterBone.transform.localPosition);
					defaultBoneRotations.Add (masterBone.transform.localRotation);
				}
				SkinnedMeshRenderer[] skinMeshes = GetComponentsInChildren<SkinnedMeshRenderer> ();
				Transform[] meshBone = skinMeshes [0].bones;
				foreach (Transform bone in meshBone) {
					defaultBonePositions.Add (bone.localPosition);
					defaultBoneRotations.Add (bone.localRotation);
					myBones.Add (bone);
				}
				//ACQUIRE OUR DEFAULT NORMAL MAPS
				defaultNormsList.Clear();
				defaultNormsList.Add((Texture2D)Resources.Load ("DefaultTextures/Default_NormalMap_64"));
				defaultNormsList.Add((Texture2D)Resources.Load ("DefaultTextures/Default_NormalMap_128"));
				defaultNormsList.Add((Texture2D)Resources.Load ("DefaultTextures/Default_NormalMap_256"));
				defaultNormsList.Add((Texture2D)Resources.Load ("DefaultTextures/Default_NormalMap_512"));
				defaultNormsList.Add((Texture2D)Resources.Load ("DefaultTextures/Default_NormalMap_1024"));
				defaultNormsList.Add((Texture2D)Resources.Load ("DefaultTextures/Default_NormalMap_2048"));
				initiallized = true;
			}
		}
		//RECALCULATE BONES------------------
		public void RecalculateBones() {
			if (!Application.isPlaying) {
				myBones.Clear ();
				myBones.Add (armature.transform);
				defaultBonePositions.Add (armature.transform.localPosition);
				defaultBoneRotations.Add (armature.transform.localRotation);
				if (masterBone) {
					myBones.Add (masterBone.transform);
					defaultBonePositions.Add (masterBone.transform.localPosition);
					defaultBoneRotations.Add (masterBone.transform.localRotation);
				}
				SkinnedMeshRenderer[] skinMeshes = GetComponentsInChildren<SkinnedMeshRenderer> ();
				Transform[] meshBone = skinMeshes [0].bones;
				foreach (Transform bone in meshBone) {
					defaultBonePositions.Add (bone.localPosition);
					defaultBoneRotations.Add (bone.localRotation);
					myBones.Add (bone);
				}
				defaultBonePositions.Clear ();
				defaultBoneRotations.Clear ();
				foreach (Transform bone in myBones) {
					defaultBonePositions.Add (bone.localPosition);
					defaultBoneRotations.Add (bone.localRotation);
				}
			} else {
				Debug.LogError ("Recalculating bones should only be done in the Editor while the character is in a default T-pose!");
			}
		}
		//BEGIN COMBINE SKINNED MESHES------------------
		public void BeginCombineMeshes() {
			//CLEAR BONES, WEIGHTS, AND MESH INFORMATION
			bones.Clear ();
			boneWeights.Clear ();
			combineInstances.Clear ();
			texturesDiffuse.Clear ();
			texturesNormals.Clear ();
			//REMOVE ANY PREVIOUSLY COMBINED MESH
			if (myCombinedMesh) {
				DestroyImmediate(myCombinedMesh);
			}
			//MAKE SURE OUR LIST OF SKINED MESHES ARE ACTIVE
			if (mySkinnedMeshes != null) {
				if (mySkinnedMeshes.Length > 0) {
					foreach (SkinnedMeshRenderer sMesh in mySkinnedMeshes) {
						sMesh.gameObject.SetActive (true);
					}
				}
			}
			//IF MY-SKINNED-MESHES HAVE NOT BEEN MANUALLY ASSIGNED, GET ALL ACTIVE SKINNED MESHES
			if (mySkinnedMeshes == null || mySkinnedMeshes.Length <= 0) {
				if (combineImmediateChildrenOnly) {
					List<Transform> immediateChildren = new List<Transform>();
					for (int k = 0; k < transform.childCount; k++) {
						immediateChildren.Add (transform.GetChild (k).transform);
					}
					List<SkinnedMeshRenderer> immediateSkinMeshes = new List<SkinnedMeshRenderer>();
					foreach (Transform child in immediateChildren) {
						if (child.gameObject.activeSelf) {
							if (child.GetComponent<SkinnedMeshRenderer> ()) {
								if (!child.GetComponent<Cloth> () && excludeClothMeshes == true) {
									immediateSkinMeshes.Add (child.GetComponent<SkinnedMeshRenderer> ());
								} else if(excludeClothMeshes == false) {
									immediateSkinMeshes.Add (child.GetComponent<SkinnedMeshRenderer> ());
								}
							}
						}
					}
					mySkinnedMeshes = immediateSkinMeshes.ToArray ();
				} else {
					mySkinnedMeshes = GetComponentsInChildren<SkinnedMeshRenderer> ();

					List<SkinnedMeshRenderer> skinMeshes = new List<SkinnedMeshRenderer>();
					foreach (SkinnedMeshRenderer mesh in mySkinnedMeshes) {
						if (mesh.gameObject.activeSelf) {
							if (!mesh.GetComponent<Cloth> () && excludeClothMeshes == true) {
								skinMeshes.Add (mesh);
							} else if (excludeClothMeshes == false) {
								skinMeshes.Add (mesh);
							}
						}
					}
					mySkinnedMeshes = skinMeshes.ToArray ();
				}
			}
			//CREATE A NEW GAMEOBJECT TO USE AS OUR COMBINED SKINNED MESH
			myCombinedMesh = new GameObject ("MyCombinedMesh");
			//SET ITS POSITION AND ROTATION, AND PARENT IT TO OUR PLAYER OBJECT
			myCombinedMesh.transform.position = transform.position;
			myCombinedMesh.transform.localRotation = transform.rotation;
			myCombinedMesh.transform.parent = transform;
			//GET OUR DEFAULT NORMAL MAP
			switch (textureAtlasSize) {
			case TextureAtlasSize.x64:
				defaultNorms = defaultNormsList [0];
				break;
			case TextureAtlasSize.x128:
				defaultNorms = defaultNormsList [1];
				break;
			case TextureAtlasSize.x256:
				defaultNorms = defaultNormsList [2];
				break;
			case TextureAtlasSize.x512:
				defaultNorms = defaultNormsList [3];
				break;
			case TextureAtlasSize.x1024:
				defaultNorms = defaultNormsList [4];
				break;
			case TextureAtlasSize.x2048:
				defaultNorms = defaultNormsList [5];
				break;
			default:
				defaultNorms = defaultNormsList [5];
				break;
			}
			//COMBINE SKINNED MESHES
			CombineMeshes();
		}
		//COMBINE SKINNED MESHES------------------
		public void CombineMeshes() {
			//GET SUBMESH COUNT
			int numSubs = 0;
			foreach (SkinnedMeshRenderer smr in mySkinnedMeshes) {
				numSubs += smr.sharedMesh.subMeshCount;
			}
			int[] meshIndex = new int[numSubs];
			int k = 0;
			//FOR EACH SKINNED MESH,
			foreach(SkinnedMeshRenderer smr in mySkinnedMeshes) {
				//GET BONES
				Transform[] meshBones = smr.bones;
                foreach (Transform bone in meshBones)
                {
                    bones.Add(bone);
                }
                //GET BONE WEIGHTS
                BoneWeight[] meshBoneweight = smr.sharedMesh.boneWeights;
                foreach (BoneWeight bw in meshBoneweight)
                {
                    BoneWeight bWeight = bw;
                    boneWeights.Add(bWeight);
                }
                //FOR EACH SUBMESH
                for (int j = 0; j < smr.sharedMesh.subMeshCount; j++) {
                    CombineInstance ci = new CombineInstance();
                    Material mat = smr.materials[j];
                    //GET DIFFUSE TEXTURE
                    if (mat.mainTexture)
                    {
                        Texture2D myTexture = (Texture2D)mat.mainTexture;
                        Texture2D newDiffuse = new Texture2D(myTexture.width, myTexture.height, textureFormat, isMipMap);
                        newDiffuse.SetPixels(0, 0, myTexture.width, myTexture.height, myTexture.GetPixels());

                        texturesDiffuse.Add(newDiffuse);
                    }
                    else
                    {
                        Debug.LogError(smr.name + " " + mat.name + " does not contain a diffuse texture! Atlasing will fail!");
                    }

                    //GET NORMAL MAP
                    if (mat.HasProperty("_BumpMap"))
                    {
                        Texture2D myTexture = (Texture2D)mat.GetTexture("_BumpMap");
                        Texture2D newNormals = new Texture2D(myTexture.width, myTexture.height, textureFormat, isMipMap);
                        newNormals.SetPixels(0, 0, myTexture.width, myTexture.height, myTexture.GetPixels());

                        texturesNormals.Add(newNormals);
                    }
                    else
                    {
                        Debug.LogError(smr.name + " " + mat.name + " does not contain a normal map texture! Atlasing will fail!");
                    }
                    //RESET OUR BONES TO DEFAULT POSITION
                    for (int b = 0; b < myBones.Count; b++)
                    {
                        myBones[b].transform.localPosition = defaultBonePositions[b];
                        myBones[b].transform.localRotation = defaultBoneRotations[b];
                    }
                    //GET MESH INSTANCES
                    Mesh ciMesh = Instantiate(smr.sharedMesh);
                    smr.BakeMesh(ciMesh);
                    Mesh newMesh = MeshExtension.GetSubmesh(ciMesh, j);
                    ci.mesh = newMesh;
                    ci.subMeshIndex = 0;
                    meshIndex[k] = newMesh.vertices.Length;
                    if (invertMatrix == true) {
						ci.transform = smr.transform.localToWorldMatrix * transform.worldToLocalMatrix.inverse;
					} else {
						ci.transform = smr.transform.localToWorldMatrix * transform.worldToLocalMatrix;
					}
					combineInstances.Add (ci);
					k++;
                    Destroy(ciMesh);
                }                
				//DISABLE OUR INDIVIDUAL SKINNED MESHES
				smr.gameObject.SetActive (false);
			}
			//GET BINDPOSES
			List<Matrix4x4> bindposes = new List<Matrix4x4>();
			for( int b = 0; b < bones.Count; b++ ) {
				bindposes.Add(bones[b].worldToLocalMatrix * transform.worldToLocalMatrix);
			}
			//CREATE OUR NEW SKINNED MESH RENDERER
			SkinnedMeshRenderer r = myCombinedMesh.gameObject.AddComponent<SkinnedMeshRenderer>();
			r.sharedMesh = new Mesh();
			//COMBINE OUR MESH INSTANCES
			r.sharedMesh.CombineMeshes(combineInstances.ToArray(), true, true);
			r.updateWhenOffscreen = true;
			//CREATE OUR NEW TEXTURE ATLAS
			//DIFFUSE
			diffuseAtlas = new Texture2D((int)textureAtlasSize, (int)textureAtlasSize, textureFormat, isMipMap);
			Rect[] packedDiffuse = diffuseAtlas.PackTextures(texturesDiffuse.ToArray(), 0, (int)textureAtlasSize);
			//NORMALS
			normalsAtlas = Instantiate(defaultNorms) as Texture2D;
			normalsAtlas.PackTextures(texturesNormals.ToArray(), 0, (int)textureAtlasSize);
			//MAP OUR MESH'S UVs
			Vector2[] originalUVs = r.sharedMesh.uv;
			Vector2[] atlasUVs = new Vector2[originalUVs.Length];
			int rectIndex = 0;
			int vertTracker = 0;
			for( int i = 0; i < atlasUVs.Length; i++ ) {
				if(i >= meshIndex[rectIndex] + vertTracker) {                
					vertTracker += meshIndex[rectIndex];
					rectIndex++;                
				}
				atlasUVs[i].x = Mathf.Lerp(packedDiffuse[rectIndex].x, packedDiffuse[rectIndex].xMax, originalUVs[i].x);
				atlasUVs[i].y = Mathf.Lerp(packedDiffuse[rectIndex].y, packedDiffuse[rectIndex].yMax, originalUVs[i].y);      
			}
			//CREATE OUR NEW MATERIAL
			Material combinedMat = new Material(Shader.Find("Mobile/Bumped Diffuse"));
			//ASSIGN OUR TEXTURE ATLAS
			combinedMat.mainTexture = diffuseAtlas;
			combinedMat.SetTexture("_BumpMap", normalsAtlas);
			//SET UVS OF OUR NEW MESH
			r.sharedMesh.uv = atlasUVs;
			//SET THE MATERIAL OF OUR NEW MESH
			r.sharedMaterial = combinedMat;
			//SET THE BONES OF OUR NEW MESH
			r.bones = bones.ToArray();
			//SET BONE WEIGHTS OF OUR NEW MESH 
			r.sharedMesh.boneWeights = boneWeights.ToArray();
			//SET BINDPOSES
			r.sharedMesh.bindposes = bindposes.ToArray();
			//recalculate bounds
			r.sharedMesh.RecalculateBounds();
			if (anim) {
				anim.cullingMode = animCullingMode;
			}
			Debug.Log ("SkinedMeshes combined.");
		}
		//DISABLE MESH------------------
		public void DisassembleMesh() {
			//RE-ENABLE OUR CHARACTER'S MESHES
			if(mySkinnedMeshes != null) {
				if (mySkinnedMeshes.Length > 0) {
					foreach (SkinnedMeshRenderer mesh in mySkinnedMeshes) {
						mesh.gameObject.SetActive (true);
					}
				}
			}
			//DESTROY OUR COMBINED MESH
			if(myCombinedMesh) {
				DestroyImmediate(myCombinedMesh);
				myCombinedMesh = null;
				Debug.Log ("MyCombinedMesh disassembled.");
			}
			//CLEAR
			bones.Clear ();
			boneWeights.Clear ();
			combineInstances.Clear ();
			texturesDiffuse.Clear ();
			texturesNormals.Clear ();
			System.Array.Clear (mySkinnedMeshes, 0, mySkinnedMeshes.Length);
			mySkinnedMeshes = new SkinnedMeshRenderer[0];
		}
	}
}
