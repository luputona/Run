//
// Mecanimのアニメーションデータが、原点で移動しない場合の Rigidbody付きコントローラ
// サンプル
// 2014/03/13 N.Kobyasahi
//
using UnityEngine;
using System.Collections;

namespace UnityChan
{
// 필요한 구성 요소의 나열
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Rigidbody))]

	public class UnityChanControlScriptWithRgidBody : MonoBehaviour
	{

		public float animSpeed = 1.5f;				// 애니메이션 재생 속도 설정
		public float lookSmoother = 3.0f;			// a smoothing setting for camera motion
		public bool useCurves = true;				// Mecanim으로 커브 조정을 사용하거나 설정
		// 이 스위치가 켜져 있지 않으면 곡선은 사용되지 않는다
		public float useCurvesHeight = 0.5f;		// 커브 보정의 유효 높이 (지면을 빠져나오기 쉬운 때에는 크게한다)

		// 다음 캐릭터 컨트롤러 매개 변수
		// 전진 속도
		public float forwardSpeed = 7.0f;
		// 후진 속도
		public float backwardSpeed = 2.0f;
		// 선회 속도
		public float rotateSpeed = 2.0f;
		// 점프 위력
		public float jumpPower = 3.0f; 
		// 캐릭터 컨트롤러 (캡슐 콜리더) 참조
		private CapsuleCollider col;
		private Rigidbody rb;
		// 캐릭터 컨트롤러 (캡슐 콜리더)의 이동량
		private Vector3 velocity;
		// CapsuleCollider으로 설정되어 콜리더의 Heiht, Center의 초기 값을 담을 변수
		private float orgColHight;
		private Vector3 orgVectColCenter;
		private Animator anim;							// 캐릭터에 부착 된 애니메이터에 대한 참조
		private AnimatorStateInfo currentBaseState;			// base layer에서 사용되는 애니메이터의 현재 상태의 참조

		private GameObject cameraObject;	// 메인 카메라에 대한 참조
		
		// 애니메이터 각 상태에 대한 참조
		static int idleState = Animator.StringToHash ("Base Layer.Idle");
		static int locoState = Animator.StringToHash ("Base Layer.Locomotion");
		static int jumpState = Animator.StringToHash ("Base Layer.Jump");
		static int restState = Animator.StringToHash ("Base Layer.Rest");

		// 初期化
		void Start ()
		{
			// Animator구성 요소를 가져오기
			anim = GetComponent<Animator> ();
			// CapsuleCollider구성 요소 가져 오기 (캡슐 형 충돌)
			col = GetComponent<CapsuleCollider> ();
			rb = GetComponent<Rigidbody> ();
			//메인 카메라를 가져오기
			cameraObject = GameObject.FindWithTag ("MainCamera");
			// CapsuleCollider 구성 요소의 Height, Center의 초기 값을 저장
			orgColHight = col.height;
			orgVectColCenter = col.center;
		}
	
	
		// 다음 주 처리. 리지드 바디와 관련시키기 때문에 FixedUpdate에서 처리를한다.
		void FixedUpdate ()
		{
			float h = Input.GetAxis ("Horizontal");				// 입력 장치의 수평을 h에서 정의
			float v = Input.GetAxis ("Vertical");				// 입력 장치의 수직 축을 v 정의
			anim.SetFloat ("Speed", v);							// Animator 측에서 설정하고있는 "Speed"매개 변수에 v를 전달
			anim.SetFloat ("Direction", h); 						// Animator 측에서 설정 한 "Direction"매개 변수에 h를 전달
			anim.speed = animSpeed;								// Animator 모션 재생 속도에 animSpeed을 설정
			currentBaseState = anim.GetCurrentAnimatorStateInfo (0);	// 참조 용 상태 변수에 Base Layer (0)의 현재 상태를 설정
			rb.useGravity = true;//점프 중에 중력을 자르기 때문에, 그 이외는 중력의 영향을 받도록한다		
		
			// 다음 캐릭터의 이동 처리
			velocity = new Vector3 (0, 0, v);		// 상하 키 입력에서 Z 축 방향의 이동량을 취득
			// 캐릭터의 로컬 공간에서의 방향으로 변환
			velocity = transform.TransformDirection (velocity);
			//다음 v 임계 값은 Mecanim 측의 전환과 함께 조정
			if (v > 0.1) {
				velocity *= forwardSpeed;		// 移動速度を掛ける이동 속도를 걸다
			} else if (v < -0.1) {
				velocity *= backwardSpeed;	// 移動速度を掛ける이동 속도를 걸다
			}
		
			if (Input.GetButtonDown ("Jump")) {	// 스페이스 키를 입력하면

				//애니메이션의 상태가 Locomotion의 작동 중에만 점프 가능
				if (currentBaseState.nameHash == locoState) {
					//상태 전환중이 아니라면 점프 불가능
					if (!anim.IsInTransition (0)) {
						rb.AddForce (Vector3.up * jumpPower, ForceMode.VelocityChange);
						anim.SetBool ("Jump", true);		// Animator에 점프로 전환 플래그 쓰기
					}
				}
			}		
			// 상하 키 입력으로 캐릭터를 이동시킨다
			transform.localPosition += velocity * Time.fixedDeltaTime;

			// 좌우의 키 입력으로 문자를 Y 축으로 회전시키는
			transform.Rotate (0, h * rotateSpeed, 0);		

			// 이하 Animator의 각 상태에서의 처리
			// Locomotion 중
			// 현재의 기반 레이어가 locoState 때
			if (currentBaseState.nameHash == locoState) {
				//커브일때で콜라이더 조정을 할 때는 、안전을 위해서 리셋한다
				if (useCurves) {
					resetCollider ();
				}
			}
			// JUMP중인 처리
			// 현재의 기반 레이어가 jumpState 때
		else if (currentBaseState.nameHash == jumpState) {
				cameraObject.SendMessage ("setCameraPositionJumpView");	// ジャンプ中のカメラに変更
				// 상태가 전환되고 있지 않은 경우
				if (!anim.IsInTransition (0)) {
				
					// 다음 커브 조정을하는 경우의 처리
					if (useCurves) {
						// 다음 JUMP00 애니메이션에 붙어있는 곡선 JumpHeight과 GravityControl
						// JumpHeight : JUMP00에서 점프의 높이 (0-1)
						// GravityControl : 1⇒ 점프 중 (중력 해제) 0⇒ 중력 활성화
						float jumpHeight = anim.GetFloat ("JumpHeight");
						float gravityControl = anim.GetFloat ("GravityControl"); 
						if (gravityControl > 0)
							rb.useGravity = false;	//점프 중 중력의 영향을 자른다
										
						// 레이 캐스팅을 캐릭터의 센터에서 떨어 뜨린다
						Ray ray = new Ray (transform.position + Vector3.up, -Vector3.up);
						RaycastHit hitInfo = new RaycastHit ();
						// 높이가 useCurvesHeight 이상 때만 콜리더의 높이와 중심을 JUMP00 애니메이션에 붙어있는 커브로 조정한다
						if (Physics.Raycast (ray, out hitInfo)) {
							if (hitInfo.distance > useCurvesHeight) {
								col.height = orgColHight - jumpHeight;			// 조정 된 콜리더의 높이
								float adjCenterY = orgVectColCenter.y + jumpHeight;
								col.center = new Vector3 (0, adjCenterY, 0);	// 조정 된 콜리더의 센터
							} else {
								// 임계 값보다 낮은 경우에는 초기 값으로 복원 (만약을 위해)			
								resetCollider ();
							}
						}
					}
					// 점프값을 재설정 (루프하지 않도록한다)				
					anim.SetBool ("Jump", false);
				}
			}
			// IDLE시의 처리
			// 현재의 기반 레이어가 idleState 때
		else if (currentBaseState.nameHash == idleState) {
				//곡선 콜리더 조정을하고있을 때는 안전을 위해 재설정
				if (useCurves) {
					resetCollider ();
				}
				// 스페이스 키를 입력하면 Rest 상태로
				if (Input.GetButtonDown ("Jump")) {
					anim.SetBool ("Rest", true);
				}
			}
			// REST중인 처리
			// 현재의 기반 레이어가 restState 때
		else if (currentBaseState.nameHash == restState) {
				//cameraObject.SendMessage ( "setCameraPositionFrontView"); // 카메라를 정면으로 전환
				// 상태가 전환되고 있지 않은 경우 Rest bool 값을 재설정 (루프하지 않도록한다)
				if (!anim.IsInTransition (0)) {
					anim.SetBool ("Rest", false);
				}
			}
		}

		void OnGUI ()
		{
			GUI.Box (new Rect (Screen.width - 260, 10, 250, 150), "Interaction");
			GUI.Label (new Rect (Screen.width - 245, 30, 250, 30), "Up/Down Arrow : Go Forwald/Go Back");
			GUI.Label (new Rect (Screen.width - 245, 50, 250, 30), "Left/Right Arrow : Turn Left/Turn Right");
			GUI.Label (new Rect (Screen.width - 245, 70, 250, 30), "Hit Space key while Running : Jump");
			GUI.Label (new Rect (Screen.width - 245, 90, 250, 30), "Hit Spase key while Stopping : Rest");
			GUI.Label (new Rect (Screen.width - 245, 110, 250, 30), "Left Control : Front Camera");
			GUI.Label (new Rect (Screen.width - 245, 130, 250, 30), "Alt : LookAt Camera");
		}


		// 캐릭터의 콜리더 크기 재설정 함수
		void resetCollider ()
		{
			// 구성 요소의 Height, Center의 초기 값을 리턴
			col.height = orgColHight;
			col.center = orgVectColCenter;
		}
	}
}