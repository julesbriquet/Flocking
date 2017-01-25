using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidMovement : MonoBehaviour {

	public float 			m_BoidSpeed = 5.0f;
	public float			m_BoidSpinSpeed = 3.0f;

	public bool 			m_DebugDrawBoidMouvementDebug = false;

	private Vector3 		m_NavigationDirection;
	private Vector3 		m_SteeringDirection;
	private Vector3 		m_WantedDirection;


	// Use this for initialization
	void Start ()
	{
	}


	// Update is called once per frame
	void Update ()
	{
		m_NavigationDirection = GetNavigationVector ();
		m_NavigationDirection.Normalize ();

		m_SteeringDirection = GetSteeringBehaviorVector ();
		m_SteeringDirection.Normalize ();

		m_WantedDirection = m_NavigationDirection * 1.5f + m_SteeringDirection;
		m_WantedDirection.Normalize ();

		AdjustRotation ();

		transform.position += m_BoidSpeed * Time.deltaTime * transform.forward;
	}

	private void AdjustRotation ()
	{
		Quaternion targetRotation = Quaternion.LookRotation (m_WantedDirection);
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

	private Vector3 GetNavigationVector()
	{
		BoidNavigation boidNavigation = this.GetComponent<BoidNavigation> ();

		if (boidNavigation == null)
		{
			return Vector3.zero;
		}

		return boidNavigation.TargetNavigationDirection;
	}

	//
	//
	// DEBUG PART
	//
	//
	void OnDrawGizmos()
	{
		if (!m_DebugDrawBoidMouvementDebug)
		{
			return;
		}
		Gizmos.color = Color.blue;
		Gizmos.DrawLine (transform.position, transform.position + m_NavigationDirection);

		Gizmos.color = Color.yellow;
		Gizmos.DrawLine (transform.position, transform.position + m_SteeringDirection);

		Gizmos.color = Color.green;
		Gizmos.DrawLine (transform.position, transform.position + m_WantedDirection);

		Gizmos.color = Color.red;
		Gizmos.DrawLine (transform.position, transform.position + transform.forward);
	}
}