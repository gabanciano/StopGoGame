using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour {


	void OnTriggerEnter2D(Collider2D enter){
		if (enter.gameObject.tag == "Ground") {
			Destroy (enter.gameObject);
		}
		if (enter.gameObject.tag == "LightObstacle") {
			Destroy (enter.gameObject);
		}
		if (enter.gameObject.tag == "Tree") {
			Destroy (enter.gameObject);
		}
	}
}
