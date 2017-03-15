using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placementCone : MonoBehaviour {

	Vector3 Direction;
	Vector2 ConeDirection;

	public bool bloop = false;

	// Use this for initialization
	void Start () {
		Direction = GetComponent<IKRigJoint> ().pos;

		if (bloop) {
			ConeDirection = new Vector2 (transform.position.x - Direction.x, transform.position.z - Direction.z);
			ConeDirection.Normalize ();
			ConeDirection *= 3.0f;

			for (int i = 0; i < 10; i++) {
				for (int j = 0; j <= i; j++) {
					Debug.DrawRay (Vector3.Lerp(transform.position,(new Vector3 (ConeDirection.x,0.0f, ConeDirection.y)),i*0.1f), 
						(new Vector3 (ConeDirection.x * Mathf.Cos (90.0f), 0.0f, ConeDirection.y * Mathf.Sin (90.0f)).normalized) * -0.1f * j, Color.yellow, 100.0f);
					Debug.DrawRay (Vector3.Lerp(transform.position,(new Vector3 (ConeDirection.x,0.0f, ConeDirection.y)),i*0.1f), 
						(new Vector3 (ConeDirection.x * Mathf.Cos (-90.0f), 0.0f, ConeDirection.y * Mathf.Sin (-90.0f)).normalized) * -0.1f * j, Color.red, 100.0f);
				}
			
			}
			Debug.DrawRay (transform.position, (new Vector3 (ConeDirection.x, 0.0f, ConeDirection.y)) * -1, Color.cyan, 100.0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
