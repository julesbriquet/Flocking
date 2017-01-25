using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidBehavior : MonoBehaviour {

	public bool 			m_DebugDrawBoidBehavior = false;

	public float 			m_DistanceToConsidereNearbyBoids = 1.0f;
	public float 			m_DesiredSeparationFromBoids = 0.5f;

	public float 			m_AlignmentCoefficient = 1.0f;
	public float 			m_CohesionCoefficient = 1.0f;
	public float 			m_SeparationCoefficient = 1.0f;

	private Vector3 		m_AligmentVector;
	private Vector3 		m_CohesionVector;
	private Vector3 		m_SeparationVector;
	private Vector3 		m_SteeringVector;


	private BoidNavigation 	m_NavigationComponent;

	// Use this for initialization
	void Start ()
	{
		m_NavigationComponent = this.GetComponent<BoidNavigation> ();

		if (m_NavigationComponent == null)
		{
			Debug.LogWarning ("BoidBehavior::Start, Navigation Component shoudn't be null");
			return;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (m_NavigationComponent == null)
		{
			return;
		}

		List<GameObject> boidListFromCurrentNode = m_NavigationComponent.CurrentNavigationArea.GetEntityList();

		m_SeparationVector = GetSeparationFromBoidsVector (boidListFromCurrentNode);
		m_AligmentVector = GetAlignmentFromBoidsVector (boidListFromCurrentNode);
		m_CohesionVector = GetCohesionFromBoidsVector (boidListFromCurrentNode);

		m_SeparationVector *= m_SeparationCoefficient;
		m_AligmentVector *= m_AlignmentCoefficient;
		m_CohesionVector *= m_CohesionCoefficient;

		m_SteeringVector = m_SeparationVector + m_AligmentVector + m_CohesionVector;
		m_SteeringVector.Normalize ();
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

			float distanceFromBoid = Vector3.Distance ( transform.position, boid.transform.position );
			if ( distanceFromBoid < m_DesiredSeparationFromBoids ) 
			{
				separationVector += boid.transform.position - this.transform.position;
				neighborCount++;
			}
		}

		if ( neighborCount != 0 )
		{
			separationVector /= neighborCount;
			separationVector *= -1;
			separationVector.Normalize ();
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
				
			float distanceFromBoid = Vector3.Distance ( transform.position, boid.transform.position );
			if ( distanceFromBoid < m_DistanceToConsidereNearbyBoids ) 
			{
				alignmentVector += boid.transform.forward;
				neighborCount++;
			}
		}

		if ( neighborCount != 0 )
		{
			alignmentVector /= neighborCount;
			alignmentVector.Normalize ();
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

			float distanceFromBoid = Vector3.Distance ( transform.position, boid.transform.position );
			if ( distanceFromBoid < m_DistanceToConsidereNearbyBoids ) 
			{
				cohesionVector += boid.transform.position;
				neighborCount++;
			}
		}

		if ( neighborCount != 0 )
		{
			cohesionVector /= neighborCount;
			cohesionVector = cohesionVector - this.transform.position;
			cohesionVector.Normalize ();
		}

		return cohesionVector;
	}

	public Vector3 SteeringVector
	{
		get { return m_SteeringVector; }
		set { m_SteeringVector = value; }
	}

	void OnDrawGizmosSelected()
	{
		if (!m_DebugDrawBoidBehavior)
		{
			return;
		}

		Gizmos.color = Color.blue;
		Gizmos.DrawLine (transform.position, transform.position + m_AligmentVector);

		Gizmos.color = Color.green;
		Gizmos.DrawLine (transform.position, transform.position + m_CohesionVector);

		Gizmos.color = Color.cyan;
		Gizmos.DrawLine (transform.position, transform.position + m_SeparationVector);

		Gizmos.color = Color.magenta;
		Gizmos.DrawLine (transform.position, transform.position + m_SteeringVector);

		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere (transform.position, m_DistanceToConsidereNearbyBoids);

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, m_DesiredSeparationFromBoids);

	}
}
