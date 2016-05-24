using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{
	public Vector3 offset;
	
	private GameObject player;
	
	void Start()
	{
		player = GameObject.FindGameObjectWithTag( "Player" );
	}
	
	void Update()
	{
		transform.position = player.transform.position + offset;
	}
}