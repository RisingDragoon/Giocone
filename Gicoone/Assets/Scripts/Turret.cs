using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret : Mobile
{
    public enum TurretType : byte
    {
        Stop,
        Mobile
    };
    public enum ShotType : byte
    {
        EveryBeat,
        EveryTwoBeat,
        FiveStop
    };
    public bool active = true;//da controllare nell'animator

    public Direction[] path = new Direction[1];
	public Direction turretDirection = Direction.Right;

    public TurretType turretType;//ferma o mobile
    public ShotType shotType;

    public GameObject circlePref;//ciò che deve sparare la torretta ferma
    public GameObject visibilityTurret;

    public int viewDistance;
    public bool hasAreaActivation;

    private int indexPath = 0;
    private int cont = 0;
    private bool inverse = false;
    private bool temp = false;
    private bool turn = true;
    private List<Projectile> circles;

    new void Start()
    {
		base.Start();
        circles = new List<Projectile>();
        if (hasAreaActivation)
        {
            active = false;//da controllare nell'animator
            Vector3 visibilityPos = transform.position;
            visibilityPos.y -= 1;
            GameObject area = Instantiate(visibilityTurret, visibilityPos, Quaternion.identity) as GameObject;
            area.transform.parent = transform;
            //area.transform.localScale.x = 1 + 2 * viewDistance;
        }

    }

    public void ExecuteAction()
    {
        IfRotateShot();
        if ( turretType == TurretType.Stop )
        {
            if ( circles!=null)
            {
                for ( int i = circles.Count-1; i >= 0; i-- )
                {
                    if ( circles[i] == null )
                    {
                        circles.RemoveAt(i);
                    }
                    else
                    {
                        //fa muovere le palle
                        circles[i].AttemptMove( turretDirection );
                        circles[i].CountBeat();
                    }                    
                }   
            }
            if (shotType==ShotType.EveryBeat)
            {
                Shot();
            }
            else if (shotType == ShotType.EveryTwoBeat)
            {
                if (turn)
                {
                    Shot();
                    turn = !turn;
                    return;
                }
                turn = !turn;
            }
            else
            {
                cont++;
                if (cont>5)
                {
                    if (cont==8)
                    {
                        cont = 0;
                    }
                }
                else
                {
                    Shot();                   
                }
            }
           
        }
        else
        {
            Move();
        }

    }
    public void Move()
    {
        //Debug.Log(indexPath);
       
        if ( inverse )
        {
            Direction newDirection = path[indexPath].Invert();
            
            temp = AttemptMove(newDirection);
        }
        else
        {
            temp = AttemptMove(path[indexPath]);
        }
        if (temp)
        {
            if (!inverse)//normale
            {
                indexPath++;
            }
            else//inverso
            {
                indexPath--;
            }
        }

        if (temp == inverse)
        {
            if (temp == false && !inverse)
            {
                indexPath--;
            }
            inverse = true;
        }
        else
        {
            if (temp == false && inverse == true)
            {
                inverse = false;
                indexPath++;
            }
        }
        if (indexPath == path.Length)//arriva in fondo normale
        {
            indexPath = 0;
        }
        else
        {
            if (indexPath == -1)//arriva in fondo inverso
            {
                indexPath = path.Length - 1;
            }
        }        
        turretDirection = path[indexPath];
    }

    private void Shot()
    {       
        GameObject circleObj = Instantiate( circlePref, transform.position, Quaternion.identity ) as GameObject;
        Projectile circle = circleObj.GetComponent<Projectile>();
        circle.lifeTime = viewDistance;
        circles.Add( circle );          
    }

    private void IfRotateShot()
    {
        if (turretDirection != path[indexPath])
        {
            Shot();
        }
    }
}
