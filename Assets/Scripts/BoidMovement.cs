using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidMovement : MonoBehaviour {

	public float 			BoidSpeed = 5.0f;
	public float			BoidSpinSpeed = 3.0f;

	public bool 			DebugDrawBoidMouvementDebug = false;

	private Vector3 		NavigationDirection;
	private Vector3 		SteeringDirection;
	private Vector3 		WantedDirection;


	// Use this for initialization
	void Start()
	{
	}


	// Update is called once per frame
	void Update()
	{
		NavigationDirection = GetNavigationVector();
		NavigationDirection.Normalize();

		SteeringDirection = GetSteeringBehaviorVector();
		SteeringDirection.Normalize();

		WantedDirection = NavigationDirection * 1.5f + SteeringDirection;
		WantedDirection.Normalize();

		AdjustRotation();

		transform.position += BoidSpeed * Time.deltaTime * transform.forward;
	}

	private void AdjustRotation()
	{
		Quaternion targetRotation = Quaternion.LookRotation(WantedDirection);
		float factor = Time.deltaTime * BoidSpinSpeed;

		if (targetRotation != transform.rotation)
		{
			transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, factor);
		}
	}


	private Vector3 GetSteeringBehaviorVector()
	{
		BoidBehavior boidBehavior = this.GetComponent<BoidBehavior>();

		if (boidBehavior == null)
		{
			return Vector3.zero;
		}

		return boidBehavior.SteeringVector;
	}

	private Vector3 GetNavigationVector()
	{
		BoidNavigation boidNavigation = this.GetComponent<BoidNavigation>();

		if (boidNavigation == null)
		{
			return Vector3.zero;
		}

		return boidNavigation.TargetNavigationDirection;
	}

	#region Debug

	void OnDrawGizmos()
	{
		if (!DebugDrawBoidMouvementDebug)
		{
			return;
		}
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, transform.position + NavigationDirection);

		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, transform.position + SteeringDirection);

		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, transform.position + WantedDirection);

		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + transform.forward);
	}

	#endregion
}