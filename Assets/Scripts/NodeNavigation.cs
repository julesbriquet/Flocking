using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeNavigation : MonoBehaviour {

	public List<NodeNavigation>		m_NextNavigationNodeList;
	
	private List<GameObject> 		m_ObjectListNavigatingInNode = new List<GameObject>();
	private SphereCollider 			m_SphereArea;
	private Vector3 				m_TargetNavigationDestination;


	// Use this for initialization
	void Start () 
	{
		m_SphereArea = this.GetComponent<SphereCollider> ();
		
		this.GenerateTargetNavigationDestination ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	
	public void RegisterObjectToNavigationNode(GameObject _navigatingObject)
	{
		m_ObjectListNavigatingInNode.Add(_navigatingObject);
	}
	
	public bool UnregisterObjectToNavigationNode(GameObject _navigatingObject)
	{
		return m_ObjectListNavigatingInNode.Remove(_navigatingObject);
	}
	
	public void GenerateTargetNavigationDestination()
	{
		m_TargetNavigationDestination = GetRandomPointInsideNavigationArea ();
	}
	
	private Vector3 GetRandomPointInsideNavigationArea()
	{
		return Random.insideUnitSphere * Random.Range(0, m_SphereArea.radius) + m_SphereArea.transform.position;
	}

	public NodeNavigation GetRandomNextNavigationNode()
	{
		if (m_NextNavigationNodeList.Count == 0)
		{
			return this;
		}
		
		int randomIndex = Random.Range(0, m_NextNavigationNodeList.Count);
		return m_NextNavigationNodeList[randomIndex];
	}

	public List<GameObject> GetEntityList()
	{
		return m_ObjectListNavigatingInNode;
	}

	public SphereCollider SphereArea
	{
		get { return m_SphereArea; }
		set { m_SphereArea = value; }
	}

	public Vector3 TargetNavigationDestination
	{
		get { return m_TargetNavigationDestination; }
		set { m_TargetNavigationDestination = value; }
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		SphereCollider sphereArea = this.GetComponent<SphereCollider> ();

		if (sphereArea == null)
		{
			Debug.LogWarning ("Sphere Area should be attached to BoidSpawner");
			return;
		}

		Gizmos.DrawWireSphere (transform.position, sphereArea.radius);

		foreach (NodeNavigation navigationNode in m_NextNavigationNodeList)
		{
			Gizmos.DrawLine (transform.position, navigationNode.transform.position);
		}
	}
}