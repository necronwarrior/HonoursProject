﻿using UnityEngine;
using System.Collections;

public class ObstacleMovement : MonoBehaviour {

	float MovementTime;
	float OriginY;
	float speed;
	bool moving;

	// Use this for initialization
	void Start (){
		MovementTime = Time.deltaTime;
	}
	// Update is called once per frame
	void Update () {
		if (moving==true) {
			if (transform.position.y > OriginY + 10f) {
				MovementTime = -1 * Time.deltaTime;
			}
			if (transform.position.y < OriginY - 10f) {
				MovementTime = Time.deltaTime;
			}
			transform.position = new Vector3 (transform.position.x,
				(transform.position.y + (MovementTime * Random.Range (1, speed))),
				transform.position.z);
		}
	}

	public void RandInit (Vector3 pos, Vector3 bounds, float Speed) {
		moving = true;
		speed = Speed;
		transform.position = new Vector3 (Random.Range (pos.x, bounds.x),
			(Random.Range (pos.y, bounds.y)/12f),
			Random.Range (pos.z, bounds.z));

		transform.localScale = new Vector3 (Random.Range (1, 10),
			Random.Range (1, 10),
			Random.Range (1, 10));

		OriginY = transform.position.y;
	}

	public void ControlledInit (Vector3 pos, float scale, bool move) {

		moving = move;
		speed = 1;
		transform.position = new Vector3 (pos.x,pos.y,pos.z);

		transform.localScale = new Vector3 (scale,scale,scale);

		OriginY = transform.position.y;
	}
}
