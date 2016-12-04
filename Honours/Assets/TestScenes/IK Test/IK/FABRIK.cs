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
 	const int MAX_SOLVER_ITERATIONS	= 20;		
	const float SOLVE_ACCURACY 		= 0.2f; 

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
		Transform[] joints = chain.joints;
		
		if(joints.Length < 2) return;
	
		float rootToTargetDist = Dist(joints[0].position, chain.target.position);
		float lambda = 0f;
		
		// Target unreachable
		if(rootToTargetDist > chain.length)
		{
			for (int i = 0; i < joints.Length - 1; i++)
			{
				lambda = chain.segLengths[i] / Dist(joints[i].position, chain.target.position);
				joints[i+1].position = (1 - lambda) * joints[i].position + lambda * chain.target.position;
			}
		}
		else // Target within reach
		{
			int tries = 0;
			
			Vector3 rootInitial = joints[0].position;
			
			float targetDelta = Dist(joints[joints.Length-1].position, chain.target.position);
			
			while(targetDelta > SOLVE_ACCURACY && tries < MAX_SOLVER_ITERATIONS)
			{
				// Forward reaching phase
				
				joints[joints.Length-1].position = chain.target.position;
				
				for (int i = joints.Length - 2; i > 0; i--)
				{
					lambda = chain.segLengths[i] / Dist(joints[i+1].position, joints[i].position);		
					joints[i].position = (1 - lambda) * joints[i+1].position + lambda * joints[i].position;				
				}
				
				
				// Backward reaching phase
				
				joints[0].position = rootInitial;
				
				for (int i = 0; i < joints.Length - 1; i++)
				{
					lambda = chain.segLengths[i] / Dist(joints[i+1].position, joints[i].position);
					joints[i+1].position = (1 - lambda) * joints[i].position + lambda * joints[i+1].position;				
				}

				targetDelta = Dist(joints[joints.Length-1].position, chain.target.position);
				tries++;
			}
		}
	}
	
	float Dist(Vector3 a, Vector3 b)
	{
		return (a-b).magnitude;
	}

	//Solving multiple chains reaching for the same point
	void SolveMulti(IKChain core, IKChain[] chains)
	{
		//Vector3 mean;
		
		for(int i = 0; i < chains.Length; i++)
		{
			Transform[] joints = chains[i].joints;
			float[] segLengths = chains[i].segLengths;
			
			float rootToTarget = Dist(joints[0].position, chains[i].target.position);

		
			if(rootToTarget > chains[i].length) // Target unreachable
			{
				for (int j = 0; j < joints.Length - 1; j++)
				{
					float jointToTarget = Dist(joints[j].position, chains[i].target.position);
					float lambda = segLengths[j] / jointToTarget;
					
					joints[j+1].position = (1 - lambda) * joints[j].position + lambda * chains[i].target.position;
				}
			}

			/*else // Target within reach
			{
				int tries = 0;
				
				Vector3 rootInitial = subChain[0].position;
				
				float targetDelta = SqrDist(chain[subChain.Length-1].position, endPos);
				
				while(targetDelta > SOLVE_ACCURACY && tries < MAX_SOLVER_ITERATIONS)
				{
					float segDist;
					float lambda;
					
					// Forward reaching phase
					
					subChain[chain.Length-1].position = endPos;
					
					for (i = chain.Length - 2; i > 0; i--)
					{
						segDist = SqrDist(subChain[i+1].position, subChain[i].position);
						lambda = chainDists[i] / segDist;
						
						chain[i].position = (1 - lambda) * chain[i+1].position + lambda * chain[i].position;				
					}
					
					
					// Backward reaching phase
					
					chain[0].position = rootInitial;
					
					for (i = 0; i < chain.Length - 1; i++)
					{
						segDist = SqrDist(chain[i+1].position, chain[i].position);
						lambda = chainDists[i] / segDist;
						
						chain[i+1].position = (1 - lambda) * chain[i].position + lambda * chain[i+1].position;				
					}
	
					targetDelta = SqrDist(chain[chain.Length-1].position, endPos);
					tries++;
				}
			}*/

		}
	
	}

}




