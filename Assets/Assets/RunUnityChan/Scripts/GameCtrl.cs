using UnityEngine;
using System.Collections;


public enum E_GameState
{
	Start,
	Pause,
	Finish,
}
public class GameCtrl : MonoBehaviour {

	//public float mapMoveSpeed = 5f;

	public DamageWall damageWall;
	public CharacterManager charMgr;
	public RecoveryWall recoveryWall;
	public Coin c_coin;
	public MapMoveManager mapMoveMgr;
	public ExpWall expWall;




	private float updateHp ;
	private float getDamage ;
	private float getRecovery;

	private int getCoin;
	private float getExp;

	private float totalExp;
	private int totalCoin;

	public UIManager uiManager = null;

	float target_Offset; 

	void Awake()
	{
		//GameObject go = GameObject.FindGameObjectWithTag ("UI");
		//uiManager = go.GetComponent<UIManager>();
	}
	// Use this for initialization
	void Start () {
		charMgr.SetCharacterHp (charMgr.characterHp);
		damageWall.SetwallDamage (damageWall.wallDamage);
		recoveryWall.SetWallRecovery (recoveryWall.recoveryHp);
		c_coin.SetCoin (c_coin.coinMoney);
		expWall.SetExp (expWall.exp);
		updateHp = charMgr.characterHp;

		uiManager.SetCoin (totalCoin);
		uiManager.SetExp (totalExp);
		uiManager.SetHp (updateHp);


		Debug.Log (updateHp);

		getRecovery = recoveryWall.GetWallRecovery ();
		getDamage = damageWall.GetwallDamage ();
		updateHp = charMgr.GetCharacterHp ();
		getCoin = c_coin.GetCoin ();
		getExp = expWall.GetExp ();

		//Debug.Log (charMgr.GetCharacterHp ());
	}
	void Update () {
		//
		Debug.Log (totalCoin);

		//MoveMap ();
		//target_Offset -= Time.deltaTime * mapMoveSpeed;
		//renderer.material.mainTextureOffset = new Vector3 (target_Offset, 0);	
	}
	void DamageHp()
	{

		//updateHp = GameObject.FindGameObjectWithTag ("DamageWall").SendMessage ("");
		updateHp -= getDamage;
		uiManager.SetHp (updateHp);
	}
	void RecoverHp()
	{
		updateHp += getRecovery;
		uiManager.SetHp (updateHp);
	}

	void AddCoin()
	{
		totalCoin += getCoin; 
		uiManager.SetCoin (totalCoin);
	}

	void ExpUpdate()
	{
		totalExp += getExp;
		uiManager.SetExp (totalExp);
	}

}









