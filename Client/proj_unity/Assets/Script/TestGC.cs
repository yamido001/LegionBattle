using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGC : MonoBehaviour {


	// Update is called once per frame
	void Update () {
		for(int i = 0; i < 1000; ++i)
			PlayTest (1);
	}

	void PlayTest(System.ValueType valueParam)
	{
		
	}
}
