using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUp : MonoBehaviour
{
	public float duration = 10f;
	public Animator animator;
    

	void OnTriggerEnter (Collider other)
	{
		if (other.CompareTag("Player"))
		{
			StartCoroutine (pickup(other));
		}
	}

	IEnumerator pickup(Collider player)
	{
		//Instantiate(pickupEffect, transform.position, transform.rotation);

		animator.SetBool("cherryPower", true);

		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<Collider>().enabled = false;

		yield return new WaitForSeconds(duration);
		
		animator.SetBool("cherryPower",false);

		Destroy (gameObject);

	}
}