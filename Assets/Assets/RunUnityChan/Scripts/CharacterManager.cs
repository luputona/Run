using UnityEngine;
using System.Collections;

public enum E_PlayerState
{
	Idle,
	Run,
	Jump,
	Tackle,
	Umato,
}

public class CharacterManager : MonoBehaviour
{
	public MapMoveManager mapMoveMgr;

	public float characterSpeed;
	public E_PlayerState playerState;
	public float characterHp = 100.0f;
	private float m_CharacterHp = 0f;
	private AnimatorStateInfo currentState;		// 현재 상태의 상태를 저장하는 참조
	private AnimatorStateInfo previousState;	//하나 전의 상태를 저장하는 참조
	private Animator anim;
	//private bool motionCheck = false;

	private Rigidbody rb;
	private float startWait = 5.0f;
	private bool checkStart = false;

	//private Vector3 velocity;
	//private CapsuleCollider col;
	//private float orgColHight;
	//private Vector3 orgVectColCenter;
		
	void Start ()
	{
		
		//anim = GetComponent<Animator> ();
		//currentState = anim.GetCurrentAnimatorStateInfo (0);
		//previousState = currentState;
		//col = GetComponent<CapsuleCollider> ();
		rb = GetComponent<Rigidbody> ();
		//orgColHight = col.height;
		//orgVectColCenter = col.center;
		checkStart = false;
		anim.SetBool ("StartMotion",checkStart);
        InitAnim();

		StartCoroutine ("Idle");
	}
	
	void Awake ()
	{
		anim = GetComponent<Animator> ();
	}

	
	// Update is called once per frame
	void FixedUpdate ()
	{
		rb.useGravity = true;
		//anim.SetFloat ("Speed", 10f);	

		if (checkStart == true) 
        {
			Run ();
		}
		if (Input.GetKeyDown (KeyCode.Space)) 
        {
			Jump();
		}
		if (Input.GetKeyDown (KeyCode.Z)) 
        {
			Tackle();
		}
		if ( Input.GetKeyDown (KeyCode.X)) 
        {
			Umato();
		}


		//CollSize ("JumpHeight");
		float gravityControl = anim.GetFloat ("GravityControl"); 
		if (gravityControl > 0)
			rb.useGravity = false;
		
	}

	public void SetCharacterHp (float characterHp)
	{
		m_CharacterHp = characterHp;
	}

	public float GetCharacterHp ()
	{
		return m_CharacterHp;
	}	

	IEnumerator Idle()
	{
		checkStart = false;
		
		yield return new WaitForSeconds(startWait);
		checkStart = true;
		anim.SetBool ("StartMotion",checkStart);
		anim.SetBool ("Run", checkStart);
	}
	
	void Jump ()
	{
        AnimCheck(E_PlayerState.Jump);
	}
	
	void Umato ()
	{
        AnimCheck(E_PlayerState.Umato);		
	}
	
	void Run ()
	{
        AnimCheck(E_PlayerState.Run);
	}
	
	void Tackle ()
	{
        AnimCheck(E_PlayerState.Tackle);		
	}

    void InitAnim()
    {
        anim.SetBool("Tackle", false);
        anim.SetBool("Jump", false);
        anim.SetBool("Umato", false);
        anim.SetBool("StartMorion", checkStart);

    }
    
	void AnimCheck (E_PlayerState estat)
	{
		if(estat == E_PlayerState.Jump)
        {
            anim.SetBool("Jump", true);
            anim.SetBool("Tackle", false);
            anim.SetBool("Idle", false);
            anim.SetBool("Run", false);
            anim.SetBool("Umato", false);
        }
        else if(estat == E_PlayerState.Tackle)
        {
            anim.SetBool("Jump", false);
            anim.SetBool("Tackle", true);
            anim.SetBool("Idle", false);
            anim.SetBool("Run", false);
            anim.SetBool("Umato", false);
        }
        else if(estat == E_PlayerState.Idle)
        {
            anim.SetBool("Jump", false);
            anim.SetBool("Tackle", false);
            anim.SetBool("Idle", true);
            anim.SetBool("Run", false);
            anim.SetBool("Umato", false);
        }
        else if(estat == E_PlayerState.Run)
        {
            anim.SetBool("Jump", false);
            anim.SetBool("Tackle", false);
            anim.SetBool("Idle", true);
            anim.SetBool("Run", false);
            anim.SetBool("Umato", false);
        }
        else if(estat == E_PlayerState.Umato)
        {
            anim.SetBool("Jump", false);
            anim.SetBool("Tackle", false);
            anim.SetBool("Idle", false);
            anim.SetBool("Run", false);
            anim.SetBool("Umato", true);
        }

	}

	public void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.name == "Coin") {
			Destroy (col.gameObject);
		} else if (col.gameObject.name == "ExpWall") {
			Destroy (col.gameObject);
		} else if (col.gameObject.name == "DamageWall") {
			Destroy (col.gameObject);
		} else if (col.gameObject.name == "RecoveryWall") {
			Destroy (col.gameObject);
		}
	}



	//	void CollSize (string animTypeHeight)
	//	{
	//		float jumpHeight = anim.GetFloat (animTypeHeight);
	//		float gravityControl = anim.GetFloat ("GravityControl"); 
	//		if (gravityControl > 0)
	//			rb.useGravity = false;
	//		
	//		Ray ray = new Ray (transform.position + Vector3.up, -Vector3.up);
	//		RaycastHit hitInfo = new RaycastHit ();
	//		// 높이가 useCurvesHeight 이상 때만 콜리더의 높이와 중심을 JUMP00 애니메이션에 붙어있는 커브로 조정한다
	//		if (Physics.Raycast (ray, out hitInfo)) {
	//			//if (hitInfo.distance > useCurvesHeight) {
	//			col.height = orgColHight - jumpHeight;			// 조정 된 콜리더의 높이
	//			float adjCenterY = orgVectColCenter.y + jumpHeight;
	//			col.center = new Vector3 (0, adjCenterY, 0);
	//		}// 조정 된 콜리더의 센터
	//			 else {
	//			// 임계 값보다 낮은 경우에는 초기 값으로 복원 (만약을 위해)			
	//			resetCollider ();
	//		}
	//
	//	}

	//	void resetCollider ()
	//	{
	//		// 구성 요소의 Height, Center의 초기 값을 리턴
	//		col.height = orgColHight;
	//		col.center = orgVectColCenter;
	//	}
	
	//IEnumerator RandomStatus()
	//{
	
	//}




}
