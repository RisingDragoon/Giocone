using UnityEngine;
using System.Collections;

public class Projectile : Mobile
{
    public int lifeTime;
    public Direction whereToGo;
    private int contBeat = 0;
    public GameObject particles;

    void OnTriggerEnter( Collider other )
    {
        if ( other.gameObject.layer == 8 )
        {
            if ( other.CompareTag( "Player" ) )
            {
                other.gameObject.GetComponent<Player>().LoseLife();
                other.gameObject.GetComponent<Player>().PlayLoseLifeByProjectile();
                Instantiate(particles, transform.position-(whereToGo.ToVector()/2), Quaternion.identity);
            }

            if ( !other.CompareTag( "Boss" ) && !other.CompareTag( "Blocker" ) )
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