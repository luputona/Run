using UnityEngine;
using System.Collections;

public class MapMoveManager : MonoBehaviour {

	public float mapMoveSpeed = 10.0f;
	public Collider collCharacter;
	public Collider finishiCheckColl;
	public FinishiCameraPos finishiCamPos;
	public Finish finish;
	public DamageWall damageWall;
	public Animator anim;
    public float mapMoveTemp;

	private float slowMap;	
	private float smooth = 15.0f;
	private float delayMap = 2.0f;

	private float startWait = 5.0f;
	//private bool stopCheck = false;



	// Use this for initialization
	void Awake ()
	{
	}
	void Start () {
		finish.checkMap = true;
		damageWall.checkDamage = true;
		anim.SetBool ("FinishiMotion", false);
		anim.SetBool ("StartMotion",false);
		mapMoveTemp = mapMoveSpeed;
		slowMap = mapMoveSpeed / 2f;
		StartCoroutine("StartWait");
	}
	
	// Update is called once per frame
	void Update () {

		if (finish.checkMap == false && damageWall.checkDamage == false) 
        {
			MoveMap ();
			anim.SetBool ("FinishiMotion", false);
		} 
		else if (finish.checkMap == true && damageWall.checkDamage == false) 
        {
			anim.SetBool ("FinishiMotion", true);
			finishiCamPos.FinishiCamPos ();
		}
	}

	public void MoveMap()
	{
		//damageWall.checkDamage = false;
		transform.Translate (Vector3.left * mapMoveSpeed * Time.deltaTime);

	}

	public void StopMap()
	{
		if (finish.checkMap == true) 
        {
			mapMoveSpeed -= smooth * Time.deltaTime;
			transform.Translate (Vector3.left * mapMoveSpeed * Time.deltaTime);
			if(mapMoveSpeed <= 0.1f){
				mapMoveSpeed = 0.0f;
				smooth = 0.0f;
			}
		}
	}
	public IEnumerator SlowMap()
	{
		damageWall.checkDamage = true;

		if (damageWall.checkDamage == true) 
        {
			mapMoveSpeed = slowMap * Time.deltaTime;
			transform.Translate (Vector3.left * mapMoveSpeed * Time.deltaTime);
			if (mapMoveSpeed <= slowMap) 
            {
				mapMoveSpeed = slowMap;
				slowMap = mapMoveSpeed;
			}
		} 
		damageWall.checkDamage = false;
		yield return new WaitForSeconds (delayMap); 
		mapMoveSpeed = mapMoveTemp;
	}

	IEnumerator StartWait()
	{
		yield return new WaitForSeconds (startWait);
		anim.SetBool ("StartMotion",true);
		finish.checkMap = false;
		damageWall.checkDamage = false;
	}
}





