using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFB_DragonDemo : MonoBehaviour {

	Animator animator;
	public GameObject[] Heads;
	public GameObject[] Backs;
	public GameObject[] Tails;

	void Start(){
		animator = GetComponent<Animator> ();
	}
	
	public void SuperRandom()
	{
		for (int i = 0; i < Heads.Length; i++)
		{
			Heads[i].SetActive(false);
		}
		for (int i = 0; i < Backs.Length; i++)
		{
			Backs[i].SetActive(false);
		}
		for (int i = 0; i < Tails.Length; i++)
		{
			Tails[i].SetActive(false);
		}
		
		Heads[Random.Range(0, Heads.Length)].SetActive(true);
		Backs[Random.Range(0, Backs.Length)].SetActive(true);
		Tails[Random.Range(0, Tails.Length)].SetActive(true);
	}

	public void UpdateLocomotion(float value){
		animator.SetFloat ("locomotion", value);
	}
}
