using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTrigger : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D enter){
		if (enter.gameObject.tag == "Player") {
			GameData.GAME_SCORE += 1;
		}
	}
}
