using UnityEngine;
using System.Collections;

public class Projectile : Mobile
{
    public int lifeTime;
    public Direction whereToGo;
    private int contBeat = 0;

    void OnTriggerEnter( Collider other )
    {
        if ( other.gameObject.layer == 8 )
        {
            if ( other.tag == "Player" )
            {
                other.gameObject.GetComponent<Player>().LoseLife();
            }
            if (other.tag!="Boss")
            {
                Destroy(gameObject);
            }
            
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