using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour {

	public int 					m_NumberOfBoidToSpawn = 10;
	public GameObject 			m_GameObjectToSpawn;

	private List<GameObject> 	m_SpawnedGameObjectList = new List<GameObject>();
	private SphereCollider 		m_SphereArea;

	private Vector3 			m_TargetFlightDestination;


	// Use this for initialization
	void Start () {

		m_SphereArea = this.GetComponent<SphereCollider> ();

		for ( int i = 0; i < m_NumberOfBoidToSpawn; ++i ) 
		{
			GenerateBoid ();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	private void GenerateBoid()
	{
		if (m_SphereArea == null)
		{
			Debug.LogWarning ("Sphere Area should be attached to BoidSpawner");
			return;
		}

		float randXPos = Random.Range (this.transform.position.x - (m_SphereArea.radius / 2), this.transform.position.x + (m_SphereArea.radius / 2));
		float randYPos = Random.Range (this.transform.position.y - (m_SphereArea.radius / 2), this.transform.position.y + (m_SphereArea.radius / 2));
		float randZPos = Random.Range (this.transform.position.z - (m_SphereArea.radius / 2), this.transform.position.z + (m_SphereArea.radius / 2));

		Vector3 randPosition = new Vector3 (randXPos, randYPos, randZPos);

		Quaternion randRotation = Quaternion.Euler(Random.Range(-80, 80), Random.Range(0, 360), 0);

		GameObject boidGameObject = (GameObject)MonoBehaviour.Instantiate (m_GameObjectToSpawn, randPosition, randRotation);


		// ENCAPSULATE THIS
		BoidMovement boidMovement = boidGameObject.GetComponent<BoidMovement> ();
		if (boidMovement != null)
		{
			boidMovement.FlightArea = this;
		}

		// This Too
		BoidBehavior boidBehavior = boidGameObject.GetComponent<BoidBehavior> ();
		if (boidBehavior != null)
		{
			boidBehavior.FlightArea = this;
		}

		m_SpawnedGameObjectList.Add (boidGameObject);
	}

	private Vector3 GetRandomPointInsideFlightArea()
	{
		return Random.insideUnitSphere * Random.Range(0, m_SphereArea.radius) + m_SphereArea.transform.position;
	}

	public void GenerateTargetFlightDestination()
	{
		m_TargetFlightDestination = GetRandomPointInsideFlightArea ();
	}


	public List<GameObject> GetSpawnedEntityList()
	{
		return m_SpawnedGameObjectList;
	}

	public SphereCollider SphereArea
	{
		get { return m_SphereArea; }
		set { m_SphereArea = value; }
	}

	public Vector3 TargetFlightDestination
	{
		get { return m_TargetFlightDestination; }
		set { m_TargetFlightDestination = value; }
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
	}
}
