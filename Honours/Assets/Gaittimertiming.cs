using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gaittimertiming : MonoBehaviour {

	public bool starting;
	public Slider gait;
	float gaitTime;
	// Use this for initialization
	void Start () {
		gaitTime = 0.0f;
		starting = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (starting == true) {
			gaitTime += Time.deltaTime;
			//noop = false;
			//}
			if (gaitTime > 2.2f) {
				gaitTime = -0.2f;
			}
		}

		gait.value = gaitTime;
	}

	public void timerstart()
	{
		starting = !starting;
	}
}
