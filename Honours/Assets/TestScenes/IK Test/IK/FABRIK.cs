using UnityEngine;
using System.Collections;

/*
 *Adapted by Jack Baptie from the Forward And Backwards Reaching Inverse Kinematics (FABRIK) paper by Andreas Aristidou
 *FABRIK Solver paper from - www.andreasaristidou.com/publications/Tracking_and_Modelling_Motion_for_Biomechanical_Analysis.pdf 
*/

public class FABRIK : MonoBehaviour 
{
	Vector3[] Fwd, Bwd, noo,  doo; 
	Color debug;
	bool cones = false;
	public IKChain myChain;
	// 15 iterations is average solve time
 	const int Max_iterations = 20;		
	const float Solve_accuracy = 0.01f; 
	float[] foo;
	public int noodle;

	void Start()
	{
		myChain.Init();
		Fwd = new Vector3[myChain.joints.Length];
		Bwd = new Vector3[myChain.joints.Length];
		noo = new Vector3[myChain.joints.Length];
		doo = new Vector3[myChain.joints.Length];
		foo = new float[myChain.joints.Length];
		debug = Color.white;
		cones = true;
	}
	
	void Update()
	{
		Solve(myChain);
		//myChain.DebugDraw(Color.cyan);

		//if (myChain.joints [0].name == "L1Hip") {
			noo [0] = myChain.joints [0].position;
			noo [1] = myChain.joints [1].position;
			noo [2] = myChain.joints [2].position;
			doo [0] = (myChain.joints [0].pos - myChain.joints [0].position);
			doo [1] = (myChain.joints [1].pos - myChain.joints [1].position);
			doo [2] = (myChain.joints [2].pos - myChain.joints [2].position);
			doo [0].Normalize ();
			doo [1].Normalize ();
			doo [2].Normalize ();
			foo [0] = myChain.joints [0].aperture;
			foo [1] = myChain.joints [1].aperture;
			foo [2] = myChain.joints [2].aperture;
		//}
	}

	void Solve(IKChain chain)
	{



		if(chain.joints.Length < 2) return;
	
		float rootToTargetDist = Vector3.Distance(chain.joints[0].position, chain.target.position);
		float lambda = 0f;

		for (int i = 1; i<chain.joints.Length;i++)
		{
			chain.joints [i].Straighten (chain.joints [i-1].position);
		}

		// Target unreachable
		if(rootToTargetDist > chain.length)
		{
			for (int i = 0; i < chain.joints.Length - 1; i++)
			{
				lambda = chain.segmentLengths[i] / Vector3.Distance(chain.joints[i].position, chain.target.position);
				chain.joints[i+1].position = (1 - lambda) * chain.joints[i].position + lambda * chain.target.position;
			}
		}
		else // Target within reach
		{
			int tries = 0;
			
			Vector3 rootInitial = chain.joints[0].position;
			Vector3 tempos;

			float targetDelta = Vector3.Distance(chain.joints[chain.joints.Length-1].position, chain.target.position);
			
			while(targetDelta > Solve_accuracy && tries < Max_iterations)
			{
				// Forward reaching phase
				
				chain.joints[chain.joints.Length-1].position = chain.target.position;
				
				for (int i = chain.joints.Length - 2; i > 0; i--)
				{

					//returning from the target
					lambda = chain.segmentLengths[i] / Vector3.Distance(chain.joints[i+1].position, chain.joints[i].position);
					tempos = (1 - lambda) * chain.joints[i+1].position + lambda * chain.joints[i].position;

					Fwd [i] = chain.joints [i].position;
					if (isLyingInCone (tempos, chain.joints [i-1].position, chain.joints [i].pos, (Mathf.Deg2Rad*chain.joints [i].aperture))) 
					{
						chain.joints [i].position = tempos;	
						Fwd [i] = chain.joints [i].position;
					}else {
						chain.joints [i].position = Fwd [i];
						if (noodle == 1) {
							GameObject doo = (GameObject)Instantiate (GameObject.CreatePrimitive (PrimitiveType.Cube));
							doo.transform.position = chain.joints [i-1].position;
							doo.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
							//GameObject voo = (GameObject)Instantiate (GameObject.CreatePrimitive (PrimitiveType.Cube));
							//voo.transform.position = chain.joints [i].position;
							//voo.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
						}
					}	
					Vector3.Lerp (chain.joints [i + 1].position, chain.joints [i + 1].pos, 1.0f);
				}
				
				
				// Backward reaching phase
				
				chain.joints[0].position = rootInitial;

				for (int i = 0; i < chain.joints.Length -1; i++) {
					//returning from the base

					lambda = chain.segmentLengths [i] / Vector3.Distance (chain.joints [i + 1].position, chain.joints [i].position);

					tempos = (1 - lambda) * chain.joints [i].position + lambda * chain.joints [i + 1].position;	

					noo [i] = chain.joints [i].position;
					//GameObject doott = (GameObject)Instantiate (GameObject.CreatePrimitive (PrimitiveType.Cube));

					doo [i] = chain.joints [i].position+(chain.joints [i].position - chain.joints [i].pos)*2;

					//doott.transform.position = doo [i];
					//Debug.Log(doo[i]);
					Bwd [i] = chain.joints [i + 1].position;
					if (isLyingInCone (tempos, chain.joints [i].position, chain.joints [i+1].pos, (Mathf.Deg2Rad*chain.joints [i].aperture))) {
						
						chain.joints [i + 1].position =tempos;				
						Bwd [i] = chain.joints [i + 1].position;
						debug = Color.blue;
					} else {
						chain.joints [i + 1].position = Bwd [i];
						//chain.joints [i + 1].position =tempos;
						//chain.joints [i + 1].position =tempos;
						if (noodle == 1) {
							/*GameObject doo = (GameObject)Instantiate (GameObject.CreatePrimitive (PrimitiveType.Cube));
							doo.transform.position = tempos;
							doo.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
							GameObject voo = (GameObject)Instantiate (GameObject.CreatePrimitive (PrimitiveType.Cube));
							voo.transform.position = chain.joints [i].position;
							voo.transform.localScale = new Vector3(0.1f,0.1f,0.1f);*/
						}
						debug = Color.red;
					}
					Vector3.Lerp (chain.joints [i + 1].position, chain.joints [i + 1].pos, 1.0f);
				}

				//recalculate the distance from the target
				targetDelta = Vector3.Distance(chain.joints[chain.joints.Length-1].position, chain.target.position);
				tries++;
			}


		}
	}
		
	bool isLyingInCone(Vector3 point, Vector3 apex, Vector3 centre, 
		float aperture){

		// This is for our convenience
		float halfAperture = aperture;

		// Vector pointing to X point from apex
		Vector3 apexToXVect = (apex-point);

		// Vector pointing from apex to circle-center point.
		Vector3 axisVect = (apex-centre);

		// X is lying in cone only if it's lying in 
		// infinite version of its cone -- that is, 
		// not limited by "round basement".
		// We'll use dotProd() to 
		// determine angle between apexToXVect and axis.
		bool isInInfiniteCone = Vector3.Dot(apexToXVect,axisVect)
			/apexToXVect.magnitude/axisVect.magnitude
			>
			// We can safely compare cos() of angles 
			// between vectors instead of bare angles.
			Mathf.Cos(halfAperture);


		if(!isInInfiniteCone) return false;

		// X is contained in cone only if projection of apexToXVect to axis
		// is shorter than axis. 
		// We'll use dotProd() to figure projection length.
		bool isUnderRoundCap = //Vector3.Dot(apexToXVect,axisVect)/
			apexToXVect.magnitude
			<
			axisVect.magnitude;
		return isUnderRoundCap;
	}

	void OnDrawGizmos()
	{
		if (cones==true && noodle==1)
		{
			DebugExtension.DrawCone(noo[0] ,doo[0]*2, Color.white,foo[0]);
			DebugExtension.DrawCone(noo[1] ,doo[1]*2, Color.cyan,foo[1]);
			DebugExtension.DrawCone(noo[2] ,doo[2]*2, Color.gray,foo[2]);
		}
	}
}