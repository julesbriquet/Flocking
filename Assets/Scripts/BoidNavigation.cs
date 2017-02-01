using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidNavigation : MonoBehaviour {

	public float 			MinDistanceToDestinationRadius = 1.0f;

	public bool 			DebugDrawBoidNavigationDebug = false;

	public NodeNavigation	PreviousNavigationArea { get; set; }
	public NodeNavigation 	CurrentNavigationArea { get; set; }

	public Vector3 			TargetNavigationDirection { get; set; }
	private Vector3 		TargetNavigationDestination;


	// Use this for initialization
	void Start ()
	{
		if (CurrentNavigationArea == null)
		{
			Debug.LogWarning("BoidNavigation::Start, Navigation Area shoudn't be null");
			return;
		}
		
		TargetNavigationDestination = CurrentNavigationArea.TargetNavigationDestination;
		TargetNavigationDirection = TargetNavigationDestination - transform.position;
		TargetNavigationDirection.Normalize();
	}

	
	// Update is called once per frame
	void Update ()
	{
		CheckTargetNavigationDestination();

		TargetNavigationDirection = TargetNavigationDestination - transform.position;
		TargetNavigationDirection.Normalize();
	}

	private void CheckTargetNavigationDestination()
	{
		Vector3 vDistanceToDestination = TargetNavigationDestination - transform.position;

		if (vDistanceToDestination.sqrMagnitude <= MinDistanceToDestinationRadius * MinDistanceToDestinationRadius)
		{
			if (CurrentNavigationArea.TargetNavigationDestination == TargetNavigationDestination)
			{
				CurrentNavigationArea.GenerateTargetNavigationDestination();
			}
			
			NodeNavigation nextNavigationNode = CurrentNavigationArea.GetRandomNextNavigationNode();
			if (nextNavigationNode != CurrentNavigationArea)
			{
				PreviousNavigationArea = CurrentNavigationArea;
				CurrentNavigationArea = nextNavigationNode;
				PreviousNavigationArea.UnregisterObjectToNavigationNode(this.gameObject);
				CurrentNavigationArea.RegisterObjectToNavigationNode(this.gameObject);
			}

			TargetNavigationDestination = CurrentNavigationArea.TargetNavigationDestination;
		}		
	}
		
	#region Debug

	void OnDrawGizmos()
	{
		if (!DebugDrawBoidNavigationDebug)
		{
			return;
		}
		
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(TargetNavigationDestination, MinDistanceToDestinationRadius);
	}

	#endregion
}