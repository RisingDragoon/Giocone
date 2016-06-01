using UnityEngine;
using System.Collections;

public class Projectile : Mobile
{
    public int lifeTime;
    public Direction whereToGo;
    private int contBeat = 0;

    void OnTriggerEnter( Collider other )
    {
        Debug.Log(other.gameObject.layer.ToString());
        if ( other.gameObject.layer == 8 )
        {
            if ( other.tag == "Player" )
            {
                Debug.Log( "Il player deve fre cose" );
            }
            Destroy( gameObject );
        }
    }
    public void CountBeat()
    {
        contBeat++;
        if ( contBeat>lifeTime )
        {
            Destroy( gameObject );
        }
    }
}