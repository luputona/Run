using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	//public UILabel centerMessage = null;
	public UILabel hpLabel = null;
	public UILabel coinLabel = null;
	public UILabel expLabel = null;

	// Use this for initialization
	void Start () {
	
	}
	void Update () {
	
	}

//	public void SetMessage (string msg, Color color)
//	{
//		centerMessage.text = msg;
//		centerMessage.color = color;
//	}
	public void SetCoin(int coin)
	{
		coinLabel.text = string.Format ("COIN : {0}" , coin);
	}
	public void SetExp(float exp)
	{
		expLabel.text = string.Format ("EXP  : {0}", exp);
	}
	public void SetHp(float hp)
	{
		hpLabel.text = string.Format ("HP : {0}", hp);
	}

}
