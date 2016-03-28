using UnityEngine;
using System.Collections;

public class WallLoop : MonoBehaviour {

	public float speed = 3.0f;
	public GameObject loopBlock;
	public GameObject block2;
	public GameObject block3;


	// Use this for initialization
	void Move () {
		//block2.transform.Translate (Vector3.left * speed * Time.deltaTime);
		//block3.transform.Translate (Vector3.left * speed * Time.deltaTime);

		if(block3.GetComponent<Rigidbody>().GetComponent<Collider>().isTrigger)
		{
			Destroy(block2);

			block2 = block3;
			Make ();
		}
	}
	void Make()
	{
		//int test = Random.Range (0, loopBlock.Length);

		block3 = Instantiate (loopBlock, new Vector3(-220,1,2),transform.rotation)as GameObject;
	}

	void ColliderEnter (Collider coll)
	{

	}
	// Update is called once per frame
	void Update () {
		//Move ();

	}
}
