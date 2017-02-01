using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSpawner : MonoBehaviour {

	public int 					NumberOfBoidToSpawn = 10;
	public GameObject 			GameObjectToSpawn;


	// Use this for initialization
	void Start () {

		SphereCollider SpawnArea = this.GetComponent<SphereCollider>();

		for ( int i = 0; i < NumberOfBoidToSpawn; ++i ) 
		{
			GenerateBoid(SpawnArea);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	private void GenerateBoid(SphereCollider _SpawnArea)
	{
		if (_SpawnArea == null)
		{
			Debug.LogWarning("Sphere Area should be attached to BoidSpawner");
			return;
		}

		float randXPos = Random.Range(this.transform.position.x - (_SpawnArea.radius / 2), this.transform.position.x + (_SpawnArea.radius / 2));
		float randYPos = Random.Range(this.transform.position.y - (_SpawnArea.radius / 2), this.transform.position.y + (_SpawnArea.radius / 2));
		float randZPos = Random.Range(this.transform.position.z - (_SpawnArea.radius / 2), this.transform.position.z + (_SpawnArea.radius / 2));

		Vector3 randPosition = new Vector3(randXPos, randYPos, randZPos);

		Quaternion randRotation = Quaternion.Euler(Random.Range(-80, 80), Random.Range(0, 360), 0);

		GameObject boidGameObject = (GameObject)MonoBehaviour.Instantiate(GameObjectToSpawn, randPosition, randRotation);


		// Notify NavigationNode to add this GameObject to the list
		NodeNavigation navigationNode = this.GetComponent<NodeNavigation>();
		if (navigationNode != null)
		{
			navigationNode.RegisterObjectToNavigationNode(boidGameObject);

			BoidNavigation boidNavigation = boidGameObject.GetComponent<BoidNavigation>();
			if (boidNavigation != null)
			{
				boidNavigation.CurrentNavigationArea = navigationNode;
			}
		}
	}

	#region Debug

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(transform.position, 0.2f);
	}

	#endregion
}