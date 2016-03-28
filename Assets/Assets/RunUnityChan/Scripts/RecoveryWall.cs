using UnityEngine;
using System.Collections;

public class RecoveryWall : MonoBehaviour {

	public float recoveryHp = 5.0f;

	private float m_WallRecovery = 0.0f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag=="Player")
		{
			GameObject.FindGameObjectWithTag ("GameController").SendMessage ("RecoverHp");

		}
	}

	public void SetWallRecovery(float wallRecovery)
	{
		m_WallRecovery = wallRecovery;
	}
	public float GetWallRecovery()
	{
		return m_WallRecovery;
	}

}
