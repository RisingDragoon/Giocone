using UnityEngine;
using System.Collections;

public class Projectile : Mobile
{
    public int lifeTime;
    private int contBeat = 0;

    void OnTriggerEnter( Collider other )
    {
        if ( other.gameObject.layer == LayerMask.NameToLayer( "Blocking" ) )
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