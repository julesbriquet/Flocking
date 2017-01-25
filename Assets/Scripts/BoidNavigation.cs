using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidNavigation : MonoBehaviour {

	public bool 			m_DebugDrawBoidNavigationDebug = false;

	private NodeNavigation	m_PreviousNavigationArea;
	private NodeNavigation 	m_CurrentNavigationArea;

	private Vector3 		m_TargetNavigationDestination;
	private float 			m_MinDistanceToDestinationRadius = 1.0f;

	private Vector3 		m_TargetNavigationDirection;


	// Use this for initialization
	void Start ()
	{
		if (m_CurrentNavigationArea == null)
		{
			Debug.LogWarning ("BoidNavigation::Start, Navigation Area shoudn't be null");
			return;
		}
		
		SetTargetNavigationDestination(m_CurrentNavigationArea.TargetNavigationDestination);
	}

	
	// Update is called once per frame
	void Update ()
	{
		CheckTargetNavigationDestination ();

		m_TargetNavigationDirection = m_TargetNavigationDestination - transform.position;
		m_TargetNavigationDirection.Normalize ();
	}

	private void CheckTargetNavigationDestination()
	{
		Vector3 vDistanceToDestination = m_TargetNavigationDestination - transform.position;

		if (vDistanceToDestination.sqrMagnitude <= m_MinDistanceToDestinationRadius * m_MinDistanceToDestinationRadius)
		{
			if (m_CurrentNavigationArea.TargetNavigationDestination == m_TargetNavigationDestination)
			{
				m_CurrentNavigationArea.GenerateTargetNavigationDestination ();
			}
			
			NodeNavigation nextNavigationNode = m_CurrentNavigationArea.GetRandomNextNavigationNode();
			if (nextNavigationNode != m_CurrentNavigationArea)
			{
				m_PreviousNavigationArea = m_CurrentNavigationArea;
				m_CurrentNavigationArea = nextNavigationNode;
				SetTargetNavigationDestination(m_CurrentNavigationArea.TargetNavigationDestination);
				m_PreviousNavigationArea.UnregisterObjectToNavigationNode (this.gameObject);
				m_CurrentNavigationArea.RegisterObjectToNavigationNode (this.gameObject);
			}
		}		
	}

	//
	//
	// GETTER / SETTER
	//
	//
	private void SetTargetNavigationDestination(Vector3 _TargetNavigationDestination)
	{
		m_TargetNavigationDestination = _TargetNavigationDestination;
	}

	public Vector3 TargetNavigationDirection
	{
		get { return m_TargetNavigationDirection; }
		set { m_TargetNavigationDirection = value; }
	}	

	public NodeNavigation PreviousNavigationArea
	{
		get { return m_PreviousNavigationArea; }
		set { m_PreviousNavigationArea = value; }
	}

	public NodeNavigation CurrentNavigationArea
	{
		get { return m_CurrentNavigationArea; }
		set { m_CurrentNavigationArea = value; }
	}

	public List<GameObject> GetBoidListFromNavigationNode()
	{
		List<GameObject> BoidListFromNavigationNode = new List<GameObject>();

		if (m_CurrentNavigationArea != null)
		{
			return m_CurrentNavigationArea.GetEntityList ();
		}

		return BoidListFromNavigationNode;
	}
	
	//
	//
	// DEBUG PART
	//
	//
	void OnDrawGizmos()
	{
		if (!m_DebugDrawBoidNavigationDebug)
		{
			return;
		}
		
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (m_TargetNavigationDestination, m_MinDistanceToDestinationRadius);
	}
}