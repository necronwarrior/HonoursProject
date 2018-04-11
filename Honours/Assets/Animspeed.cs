using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animspeed : MonoBehaviour {

	public Animation anim;
	public float speeed;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation> ();
		anim ["Take 001"].speed = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void StartWalk()
	{
		anim ["Take 001"].speed = speeed;
		GetComponent<NavMeshAgent> ().speed = speeed*2.0f;
	}
}
