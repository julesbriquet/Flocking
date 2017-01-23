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


	private BoidSpawner 	m_FlightArea;

	// Use this for initialization
	void Start ()
	{
		if (m_FlightArea == null)
		{
			Debug.LogWarning ("BoidMovement::Start, Flight Area shoudn't be null");
			return;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (m_FlightArea == null)
		{
			return;
		}

		List<GameObject> boidList = m_FlightArea.GetSpawnedEntityList ();

		m_SeparationVector = GetSeparationFromBoidsVector (boidList);
		m_AligmentVector = GetAlignmentFromBoidsVector (boidList);
		m_CohesionVector = GetCohesionFromBoidsVector (boidList);

		m_SeparationVector *= m_SeparationCoefficient;
		m_AligmentVector *= m_AlignmentCoefficient;
		m_CohesionVector *= m_CohesionCoefficient;

		m_SteeringVector = m_SeparationVector + m_AligmentVector + m_CohesionVector;
		m_SteeringVector.Normalize ();
	}

	private Vector3 GetSeparationFromBoidsVector(List<GameObject> _boidList)
	{
		// TODO
		return Vector3.zero;
	}

	private Vector3 GetAlignmentFromBoidsVector(List<GameObject> _boidList)
	{
		// TODO
		return Vector3.zero;
	}

	private Vector3 GetCohesionFromBoidsVector(List<GameObject> _boidList)
	{
		// TODO
		return Vector3.zero;
	}

	public BoidSpawner FlightArea
	{
		get { return m_FlightArea; }
		set { m_FlightArea = value; }
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
