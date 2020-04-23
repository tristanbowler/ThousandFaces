using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LylekGames.RPGCharacter;


namespace LylekGames.UltimateRPG {
	public class SetupPlayerMovement {

		[MenuItem("Tools/LylekGames/RPGCharacter/Setup Player Movement")]
		private static void CreateNewPlayer() {
			//MY OBJECT
			Object selectedObject = Selection.activeObject;
			GameObject myObject = (GameObject)selectedObject;

			CharacterController myController;
			if (!myObject.GetComponent<CharacterController> ()) {
				myController = myObject.AddComponent<CharacterController> ();
			} else {
				myController = myObject.GetComponent<CharacterController> ();
			}
			myController.center = new Vector3(0, 0.95f, 0);
			myController.height = 1.9f;
			myController.radius = 0.3f;

			//PLAYER INPUT
			PlayerInput myInput;
			if (!myObject.GetComponent<PlayerInput> ()) {
				myInput = myObject.AddComponent<PlayerInput> ();
			} else {
				myInput = myObject.GetComponent<PlayerInput> ();
			}
			myInput.anim = myObject.GetComponent<Animator> ();

			//CHARACTER MOVEMENT
			CharacterCharacterMovement myMovement;
			if (!myObject.GetComponent<CharacterCharacterMovement> ()) {
				 myMovement = myObject.AddComponent<CharacterCharacterMovement> ();
			}
			else {
				myMovement = myObject.GetComponent<CharacterCharacterMovement> ();
			}
			myMovement.baseSpeed = 2.0f;
			myMovement.controller = myController;
			myMovement.animator = myObject.GetComponent<Animator> ();

			myInput.characterMovement = myMovement;

			//FOCUS POINT
			GameObject myFocusPoint;
			GameObject myCamera;
			if (myInput.focusPoint == null) {
				if (myObject.transform.Find ("FocusPoint")) {
					GameObject.DestroyImmediate(myObject.transform.Find ("FocusPoint").gameObject);
				}
				myFocusPoint = Resources.Load ("FocusPoint") as GameObject;
				myFocusPoint = GameObject.Instantiate (myFocusPoint, myObject.transform.position, myObject.transform.rotation);
				myFocusPoint.transform.parent = myObject.transform;
				Vector3 newPos = myObject.transform.position;
				newPos.y = myController.height;
				myFocusPoint.transform.position = newPos;
				myFocusPoint.transform.rotation = myObject.transform.rotation;
			} else {
				myFocusPoint = myInput.focusPoint;
			}
			myFocusPoint.SetActive (true);
			myCamera = myFocusPoint.transform.GetChild (0).gameObject;
			myFocusPoint.GetComponent<LylekGames.RPGCharacter.FollowScript> ().target = myObject;
			myInput.focusPoint = myFocusPoint;
			myInput.mainCamera = myCamera;

		}
	}
}