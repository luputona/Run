using UnityEngine;
using System.Collections;

public class FinishiCameraPos : MonoBehaviour {

	public GameObject player;


	//private Vector3 playerPos;
	private Vector3 cameraRot;
	private Vector3 cameraPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		

	}

	public void FinishiCamPos()
	{

		//playerPos =  player.transform.position;
		cameraPos = new Vector3 (-235.4875f, 2.0f, 3.855013f);
		//transform.LookAt(new Vector3 (-235.4875f, 2.0f, 3.855013f));//-235.4875f, 2.0f, 3.855013f
		//Quaternion camposQ = Quaternion.identity;
		GetComponent<Camera>().transform.position = cameraPos;
		transform.position = Vector3.Slerp (transform.position , cameraPos ,Time.deltaTime * 10f);

		cameraRot = new Vector3 (0.0f,-90.0f,0.0f);
		Quaternion convertQ = Quaternion.identity;
		convertQ.eulerAngles = cameraRot;
		transform.rotation = Quaternion.Slerp (transform.rotation , convertQ, Time.deltaTime * 10f);

	

	}

}
