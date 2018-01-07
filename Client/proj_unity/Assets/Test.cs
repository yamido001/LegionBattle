using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LegionBattle.ServerClientCommon;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {

		for (int i = 0; i < 15; ++i) {
			int x = Random.Range (0, 100);
			int y = Random.Range (0, 100);
			Debug.LogError (x + " " + y + "  =>  " + MathUtils.MaxCommonDivisor (377, 319));
		}
	}

}
