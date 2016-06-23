using UnityEngine;
using System.Collections;

public class ActiveBoss : MonoBehaviour 
{
	public GameObject blockobj;
	public Vector3 WhereToBlock;

	private Boss BossScript;
	void Start()
	{
		GameObject bossObj = GameObject.FindGameObjectWithTag ("Boss");
		if (bossObj != null)
		{
			BossScript = bossObj.GetComponent<Boss>();
		}

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag =="Player") 
		{
			Instantiate (blockobj, WhereToBlock, Quaternion.identity);
			BossScript.Active = true;
		}

	}
}
