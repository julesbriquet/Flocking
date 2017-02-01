using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeNavigation : MonoBehaviour {

	public List<NodeNavigation>		NextNavigationNodeList;
	
	public List<GameObject> 		NavigatingGameObjectInNodeList { get; private set; }
	public SphereCollider 			SphereArea { get; set; }
	public Vector3 					TargetNavigationDestination { get; set; }

	// Called before start
	void Awake()
	{
		NavigatingGameObjectInNodeList = new List<GameObject>();
	}

	// Use this for initialization
	void Start () 
	{
		SphereArea = this.GetComponent<SphereCollider>();
		
		this.GenerateTargetNavigationDestination();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	
	public void RegisterObjectToNavigationNode(GameObject _navigatingObject)
	{
		NavigatingGameObjectInNodeList.Add(_navigatingObject);
	}
	
	public bool UnregisterObjectToNavigationNode(GameObject _navigatingObject)
	{
		return NavigatingGameObjectInNodeList.Remove(_navigatingObject);
	}
	
	public void GenerateTargetNavigationDestination()
	{
		TargetNavigationDestination = GetRandomPointInsideNavigationArea();
	}
	
	private Vector3 GetRandomPointInsideNavigationArea()
	{
		return Random.insideUnitSphere * Random.Range(0, SphereArea.radius) + SphereArea.transform.position;
	}

	public NodeNavigation GetRandomNextNavigationNode()
	{
		if (NextNavigationNodeList.Count == 0)
		{
			return this;
		}
		
		int randomIndex = Random.Range(0, NextNavigationNodeList.Count);
		return NextNavigationNodeList[randomIndex];
	}

	#region Debug

	void OnDrawGizmos()
	{
		SphereCollider sphereArea = this.GetComponent<SphereCollider>();

		if (sphereArea == null)
		{
			Debug.LogWarning("Sphere Area should be attached to BoidSpawner");
			return;
		}

		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, sphereArea.radius);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(TargetNavigationDestination, 0.5f);

		foreach (NodeNavigation navigationNode in NextNavigationNodeList)
		{
			Gizmos.DrawLine(transform.position, navigationNode.transform.position);
		}
	}

	#endregion

}