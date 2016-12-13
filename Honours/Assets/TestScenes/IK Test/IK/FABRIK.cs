using UnityEngine;
using System.Collections;

/*
 *Adapted by Jack Baptie from the Forward And Backwards Reaching Inverse Kinematics (FABRIK) paper by Andreas Aristidou
 *FABRIK Solver paper from - www.andreasaristidou.com/publications/Tracking_and_Modelling_Motion_for_Biomechanical_Analysis.pdf 
*/

public class FABRIK : MonoBehaviour 
{
	public IKChain myChain;
	
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
		if(chain.joints.Length < 2) return;
	
		float rootToTargetDist = Dist(chain.joints[0].position, chain.target.position);
		float lambda = 0f;
		
		// Target unreachable
		if(rootToTargetDist > chain.length)
		{
			for (int i = 0; i < chain.joints.Length - 1; i++)
			{
				lambda = chain.segmentLengths[i] / Dist(chain.joints[i].position, chain.target.position);
				chain.joints[i+1].position = (1 - lambda) * chain.joints[i].position + lambda * chain.target.position;
			}
		}
		else // Target within reach
		{
			int tries = 0;
			
			Vector3 rootInitial = chain.joints[0].position;
			
			float targetDelta = Dist(chain.joints[chain.joints.Length-1].position, chain.target.position);
			
			while(targetDelta > Solve_accuracy && tries < Max_iterations)
			{
				// Forward reaching phase
				
				chain.joints[chain.joints.Length-1].position = chain.target.position;
				
				for (int i = chain.joints.Length - 2; i > 0; i--)
				{
					//returning from the target
					lambda = chain.segmentLengths[i] / Dist(chain.joints[i+1].position, chain.joints[i].position);		
					chain.joints[i].position = (1 - lambda) * chain.joints[i+1].position + lambda * chain.joints[i].position;				
				}
				
				
				// Backward reaching phase
				
				chain.joints[0].position = rootInitial;
				
				for (int i = 0; i < chain.joints.Length - 1; i++)
				{
					//returning from the base
					lambda = chain.segmentLengths[i] / Dist(chain.joints[i+1].position, chain.joints[i].position);
					chain.joints[i+1].position = (1 - lambda) * chain.joints[i].position + lambda * chain.joints[i+1].position;				
				}

				//recalculate the distance from the target
				targetDelta = Dist(chain.joints[chain.joints.Length-1].position, chain.target.position);
				tries++;
			}
		}
	}

	//for readability
	float Dist(Vector3 a, Vector3 b)
	{
		return (a-b).magnitude;
	}
}