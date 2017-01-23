using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidMovement : MonoBehaviour {

	public float 			m_BoidSpeed = 5.0f;
	public float 			m_BoidSpinSpeed = 3.0f;

	public bool 			m_DebugDrawBoidMouvementDebug = false;

	private BoidSpawner 	m_FlightArea;
	private Vector3 		m_TargetFlightDestination;
	private float 			m_MinDistanceToDestinationRadius = 1.0f;

	private Vector3 		m_TargetFlightDirection;
	private Vector3 		m_SteeringFlightDirection;
	private Vector3 		m_WantedFlightDirection;


	// Use this for initialization
	void Start ()
	{
		if (m_FlightArea == null)
		{
			Debug.LogWarning ("BoidMovement::Start, Flight Area shoudn't be null");
			return;
		}

		m_FlightArea.GenerateTargetFlightDestination ();
		SetTargetFlightDestination(m_FlightArea.TargetFlightDestination);
	}

	
	// Update is called once per frame
	void Update ()
	{

		CheckTargetFlightDestination ();

		m_TargetFlightDirection = m_TargetFlightDestination - transform.position;
		m_TargetFlightDirection.Normalize ();

		m_SteeringFlightDirection = GetSteeringBehaviorVector ();
		m_SteeringFlightDirection.Normalize ();

		m_WantedFlightDirection = m_TargetFlightDirection + m_SteeringFlightDirection;
		m_WantedFlightDirection.Normalize ();

		AdjustRotation ();

		transform.position += m_BoidSpeed * Time.deltaTime * transform.forward;
	}

	private void CheckTargetFlightDestination()
	{
		Vector3 vDistanceToDestination = m_TargetFlightDestination - transform.position;

		if (vDistanceToDestination.sqrMagnitude <= m_MinDistanceToDestinationRadius * m_MinDistanceToDestinationRadius)
		{
			m_FlightArea.GenerateTargetFlightDestination ();
		}

		SetTargetFlightDestination(m_FlightArea.TargetFlightDestination);
	}

	private void AdjustRotation ()
	{
		Quaternion targetRotation = Quaternion.LookRotation (m_WantedFlightDirection);
		float factor = Time.deltaTime * m_BoidSpinSpeed;

		if (targetRotation != transform.rotation)
		{
			transform.localRotation = Quaternion.Slerp (transform.localRotation, targetRotation, factor);
		}
	}


	private Vector3 GetSteeringBehaviorVector()
	{
		BoidBehavior boidBehavior = this.GetComponent<BoidBehavior> ();

		if (boidBehavior == null)
		{
			return Vector3.zero;
		}

		return boidBehavior.SteeringVector;
	}


	//
	//
	// GETTER / SETTER
	//
	//

	private void SetTargetFlightDestination(Vector3 _TargetFlightDestination)
	{
		m_TargetFlightDestination = _TargetFlightDestination;
	}

	public BoidSpawner FlightArea
	{
		get { return m_FlightArea; }
		set { m_FlightArea = value;	}
	}


	//
	//
	// DEBUG PART
	//
	//
	void OnDrawGizmosSelected()
	{
		if (!m_DebugDrawBoidMouvementDebug)
		{
			return;
		}

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (m_TargetFlightDestination, m_MinDistanceToDestinationRadius);

		Gizmos.DrawLine (transform.position, transform.position + m_TargetFlightDirection);

		Gizmos.color = Color.yellow;
		Gizmos.DrawLine (transform.position, transform.position + m_SteeringFlightDirection);

		Gizmos.color = Color.green;
		Gizmos.DrawLine (transform.position, transform.position + m_WantedFlightDirection);

		Gizmos.color = Color.red;
		Gizmos.DrawLine (transform.position, transform.position + transform.forward);
	}
}
