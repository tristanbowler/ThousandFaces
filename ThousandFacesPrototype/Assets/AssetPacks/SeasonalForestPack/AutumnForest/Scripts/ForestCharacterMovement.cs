using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestCharacterMovement : MonoBehaviour {
	public float speed = 7; //player's movement speed
	public float gravity = 10; //amount of gravitational force applied to the player
	public int jumpSpeed = 5;
	public CharacterController controller; //player's CharacterController component
	private Vector3 moveDirection = Vector3.zero;

	// Use this for initialization
	void Start () {
		controller = transform.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		//APPLY GRAVITY
		if(moveDirection.y > gravity * -1) {
			moveDirection.y -= gravity * Time.deltaTime;
		}
		controller.Move(moveDirection * Time.deltaTime);
		var forward = transform.TransformDirection(Vector3.forward);
		var left = transform.TransformDirection(Vector3.left);

		if(controller.isGrounded) {
			//Jump
			if(Input.GetKeyDown(KeyCode.Space)) {
				moveDirection.y = jumpSpeed;
			}
			//Walk
			else if(Input.GetKey("w")) {
				if(Input.GetKey(KeyCode.LeftShift)) {
					controller.SimpleMove(forward * speed);
				}
				else {
					controller.SimpleMove(forward * speed / 2);
				}
			}
			else if(Input.GetKey("s")) {
				if(Input.GetKey(KeyCode.LeftShift)) {
					controller.SimpleMove(forward * -speed);
				}
				else {
					controller.SimpleMove(forward * -speed / 2);
				}
			}
			else if(Input.GetKey("d")) {
				if(Input.GetKey(KeyCode.LeftShift)) {
					controller.SimpleMove(left * -speed);
				}
				else {
					controller.SimpleMove(left * -speed / 2);
				}
			}
			else if(Input.GetKey("a")) {
				if(Input.GetKey(KeyCode.LeftShift)) {
					controller.SimpleMove(left * speed);
				}
				else {
					controller.SimpleMove(left * speed / 2);
				}
			}

		}
		else {
			if(Input.GetKey("w")) {
				Vector3 relative;
				relative = transform.TransformDirection(0,0,1);
				controller.Move(relative * Time.deltaTime * speed / 1.5f);
				//controller.Move(forward * 2);
			}
		}
	}
}
