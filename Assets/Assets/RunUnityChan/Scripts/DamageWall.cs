using UnityEngine;
using System.Collections;

public class DamageWall : MonoBehaviour {

	public float wallDamage = 5.0f;
	public bool checkDamage = false;
	public MapMoveManager mapMoveMgr;
	
	private float m_WallDamage = 0f;
	// Use this for initialization
	void Start () 
    {
	}	
	// Update is called once per frame
	void Update ()
    {

	}
	public void OnTriggerEnter(Collider col)
	{
		mapMoveMgr.StartCoroutine(mapMoveMgr.SlowMap());
		//Debug.Log (col.gameObject.tag);
		if(col.gameObject.tag=="Player")
		{
			GameObject.FindGameObjectWithTag ("GameController").SendMessage ("DamageHp");
			GameObject.FindGameObjectWithTag("GameController").SendMessage("SlowMap");

		}
	}
	public void SetwallDamage(float wallDamage)
	{
		m_WallDamage = wallDamage;
	}
	public float GetwallDamage()
	{
		return m_WallDamage;
	}

}






