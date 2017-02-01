using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidBehavior : MonoBehaviour {

	public bool 			DebugDrawBoidBehavior = false;

	public float 			DistanceToConsidereNearbyBoids = 1.0f;
	public float 			DesiredSeparationFromBoids = 0.5f;

	public float 			AlignmentCoefficient = 1.0f;
	public float 			CohesionCoefficient = 1.0f;
	public float 			SeparationCoefficient = 1.0f;

	public Vector3 			SteeringVector { get; set; }

	private Vector3 		AligmentVector;
	private Vector3 		CohesionVector;
	private Vector3 		SeparationVector;


	private BoidNavigation 	NavigationComponent;

	// Use this for initialization
	void Start ()
	{
		NavigationComponent = this.GetComponent<BoidNavigation>();

		if (NavigationComponent == null)
		{
			Debug.LogWarning("BoidBehavior::Start, Navigation Component shoudn't be null");
			return;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (NavigationComponent == null)
		{
			return;
		}

		List<GameObject> boidListFromCurrentNode = NavigationComponent.CurrentNavigationArea.NavigatingGameObjectInNodeList;

		SeparationVector = GetSeparationFromBoidsVector(boidListFromCurrentNode);
		AligmentVector = GetAlignmentFromBoidsVector(boidListFromCurrentNode);
		CohesionVector = GetCohesionFromBoidsVector(boidListFromCurrentNode);

		SeparationVector *= SeparationCoefficient;
		AligmentVector *= AlignmentCoefficient;
		CohesionVector *= CohesionCoefficient;

		SteeringVector = SeparationVector + AligmentVector + CohesionVector;
		SteeringVector.Normalize();
	}

	private Vector3 GetSeparationFromBoidsVector(List<GameObject> _boidList)
	{
		Vector3 separationVector = Vector3.zero;
		uint neighborCount = 0;

		foreach ( GameObject boid in _boidList )
		{
			// Avoid boid to take himself into account
			if ( this.gameObject == boid )
			{
				continue;
			}

			float distanceFromBoid = Vector3.Distance( transform.position, boid.transform.position );
			if ( distanceFromBoid < DesiredSeparationFromBoids ) 
			{
				separationVector += boid.transform.position - this.transform.position;
				neighborCount++;
			}
		}

		if ( neighborCount != 0 )
		{
			separationVector /= neighborCount;
			separationVector *= -1;
			separationVector.Normalize();
		}

		return separationVector;
	}

	private Vector3 GetAlignmentFromBoidsVector(List<GameObject> _boidList)
	{
		Vector3 alignmentVector = Vector3.zero;
		uint neighborCount = 0;

		foreach ( GameObject boid in _boidList )
		{
			// Avoid boid to take himself into account
			if ( this.gameObject == boid )
			{
				continue;
			}
				
			float distanceFromBoid = Vector3.Distance( transform.position, boid.transform.position );
			if ( distanceFromBoid < DistanceToConsidereNearbyBoids ) 
			{
				alignmentVector += boid.transform.forward;
				neighborCount++;
			}
		}

		if ( neighborCount != 0 )
		{
			alignmentVector /= neighborCount;
			alignmentVector.Normalize();
		}

		return alignmentVector;
	}

	private Vector3 GetCohesionFromBoidsVector(List<GameObject> _boidList)
	{
		Vector3 cohesionVector = Vector3.zero;
		uint neighborCount = 0;

		foreach ( GameObject boid in _boidList )
		{
			// Avoid boid to take himself into account
			if ( this.gameObject == boid )
			{
				continue;
			}

			float distanceFromBoid = Vector3.Distance( transform.position, boid.transform.position );
			if ( distanceFromBoid < DistanceToConsidereNearbyBoids ) 
			{
				cohesionVector += boid.transform.position;
				neighborCount++;
			}
		}

		if ( neighborCount != 0 )
		{
			cohesionVector /= neighborCount;
			cohesionVector = cohesionVector - this.transform.position;
			cohesionVector.Normalize();
		}

		return cohesionVector;
	}


	#region Debug

	void OnDrawGizmosSelected()
	{
		if (!DebugDrawBoidBehavior)
		{
			return;
		}

		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, transform.position + AligmentVector);

		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, transform.position + CohesionVector);

		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(transform.position, transform.position + SeparationVector);

		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(transform.position, transform.position + SteeringVector);

		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, DistanceToConsidereNearbyBoids);

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, DesiredSeparationFromBoids);

	}

	#endregion
}
