using UnityEngine;
using System.Collections;

public class ObstacleCreation : MonoBehaviour {

	public int NumberOfObstacles;
	public float speed = 1;
	// Use this for initialization
	void Start () {
		
		for(int i = 0; i<NumberOfObstacles; i++)
		{
			GameObject Obstacle = (GameObject)Instantiate (Resources.Load("Obstacle1"));
			Obstacle.GetComponent<ObstacleMovement> ().Init (GetComponent<Terrain>().GetPosition(),
				GetComponent<Terrain>().GetPosition()+GetComponent<Terrain>().terrainData.size, speed);

		}
	}
}
