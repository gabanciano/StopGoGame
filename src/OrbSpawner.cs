using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSpawner : MonoBehaviour {

	public GameObject[] Orbs;

	private float secondsBeforeSpawn = 0;
	private int MAX_SPAWN_SECONDS_COUNT = 4;

	void Update(){
		SpawnPowerUp ();
	}

	void SpawnPowerUp(){
		if (GameData.FINGER_DOWN) {
			secondsBeforeSpawn += Time.deltaTime;
			if (secondsBeforeSpawn >= MAX_SPAWN_SECONDS_COUNT) {
				Instantiate (Orbs[Random.Range(0,3)], transform.position, transform.rotation);
				secondsBeforeSpawn = 0;
			}
		}
	}
}
