using UnityEngine;
using System.Collections;

/*
 *Adapted by Jack Baptie from the Forward And Backwards Reaching Inverse Kinematics (FABRIK) paper by Andreas Aristidou
 *FABRIK Solver paper from - www.andreasaristidou.com/publications/Tracking_and_Modelling_Motion_for_Biomechanical_Analysis.pdf 
*/

public class FABRIK : MonoBehaviour 
{
	public IKChain myChain;
	Vector3 noo,  doo;
	// 15 iterations is average solve time
 	const int Max_iterations = 20;		
	const float Solve_accuracy = 0.2f; 

	void Start()
	{
		myChain.Init();
	}
	
	void Update()
	{
		Solve(myChain);
		myChain.DebugDraw(Color.cyan);
	}

	void Solve(IKChain chain)
	{
		noo = chain.joints [1].root.position;
		doo = noo - chain.joints [0].root.position;
		if(chain.joints.Length < 2) return;
	
		float rootToTargetDist = Vector3.Distance(chain.joints[0].position, chain.target.position);
		float lambda = 0f;
		
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

				
					{
						chain.joints [i].position = tempos;	
					}		
				}
				
				
				// Backward reaching phase
				
				chain.joints[0].position = rootInitial;
				
				for (int i = 0; i < chain.joints.Length - 1; i++) {
					//returning from the base
					lambda = chain.segmentLengths [i] / Vector3.Distance (chain.joints [i + 1].position, chain.joints [i].position);

					tempos = (1 - lambda) * chain.joints [i].position + lambda * chain.joints [i + 1].position;	


					//if ()
					{
						chain.joints [i + 1].position = (1 - lambda) * chain.joints [i].position + lambda * chain.joints [i + 1].position;				
				
					}
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
		float halfAperture = aperture/2.0f;

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
		bool isUnderRoundCap = Vector3.Dot(apexToXVect,axisVect)
			/axisVect.magnitude
			<
			axisVect.magnitude;
		return isUnderRoundCap;
	}

	void OnDrawGizmos()
	{
		DebugExtension.DrawCone(noo, doo, Color.blue);
	}
}