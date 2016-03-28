using UnityEngine;
using System.Collections;

public class ExpWall : MonoBehaviour {

	public float exp = 100f;

	private float m_Exp = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col)
	{
		Debug.Log (col.gameObject.tag);
		if (col.gameObject.tag == "Player") {
			GameObject.FindGameObjectWithTag ("GameController").SendMessage ("ExpUpdate");
		}
	}

	public void SetExp(float exp)
	{
	 	m_Exp = exp;
	}

	public float GetExp()
	{
		return m_Exp;
	}
}
