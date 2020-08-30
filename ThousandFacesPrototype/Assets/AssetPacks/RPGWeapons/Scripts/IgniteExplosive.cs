using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgniteExplosive : MonoBehaviour {
	public Animation anim; //Animation component
	public GameObject explosionPrefab; //explosion
	public ParticleSystem[] fireEmissions; //flames on the wick
	[Range(0, 2)]
	public float speed = 1.0f; //how long until explosion

	void Start() {
		if(!anim) {
			anim = GetComponent<Animation>();
		}
	}
	//CALL THIS FUNCTION TO LIGHT FUSE
	public void Ignite() {
		//if animation is not already playing
		if(!anim.isPlaying) {
			//play ignite animation
			string animName = anim.clip.name;
			anim.Play();
			anim[animName].normalizedSpeed = speed;
			//light fuse(s)
			foreach(ParticleSystem fuse in fireEmissions) {
				fuse.Play();
			}
			//explode after animation ends
			float t = (anim.clip.length / speed);
			StartCoroutine(Explode(t));
		}
	}
	public IEnumerator Explode(float timer) {
		while(timer > 0) {
			timer -= 1.0f * Time.deltaTime;
			yield return null;
		}
		//spawn our explosion
		var explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
		//destroy explosion (second parameter represents how many seconds until the gameobject is destroyed)
		explosion.GetComponent<ParticleSystem>().Play();
		Destroy(explosion, 2);
		//destroy explosive
		Destroy(this.gameObject);
	}
}
