using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CachedBase : MonoBehaviour {

	// Do some optimisation to access transform
	[HideInInspector]
	public new Transform transform;

	void Awake ()
	{
		transform = gameObject.transform;
	}
}
