using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {


	public int coinMoney = 100;

	private int m_CoinMoney = 0;

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
			GameObject.FindGameObjectWithTag ("GameController").SendMessage ("AddCoin");
		}
	}

	public void SetCoin(int coinMoney)
	{
		m_CoinMoney = coinMoney;
	}
	public int GetCoin()
	{
		return m_CoinMoney;
	}
}
