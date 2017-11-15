using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour {

	public GameObject[] PowerUps;

	private float secondsBeforeSpawn = 0;
	private int MAX_SPAWN_SECONDS_COUNT = 9;

	void Update(){
		SpawnPowerUp ();
	}

	void SpawnPowerUp(){
		if (GameData.FINGER_DOWN) {
			secondsBeforeSpawn += Time.deltaTime;
			if (secondsBeforeSpawn >= MAX_SPAWN_SECONDS_COUNT) {
				Instantiate (PowerUps[Random.Range(0,2)], transform.position, transform.rotation);
				secondsBeforeSpawn = 0;
			}
		}
	}
}
