using UnityEngine;
using System.Collections;

public class Finish : MonoBehaviour {

	public MapMoveManager mapMoveMgr;
	public bool checkMap = false;


	// Use this for initialization
	void Start () {

	}	
	// Update is called once per frame
	void Update () {

	}

	public void OnTriggerEnter(Collider col)
	{

		if (col.gameObject.tag == "Player") {
			checkMap = true;
			mapMoveMgr.StopMap ();
		} 

	}
}
