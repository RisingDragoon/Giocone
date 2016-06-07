using UnityEngine;
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
    
    [HideInInspector]
    public bool seePlayer = false;//da controllare nell'animator

    public Direction[] pathMoving = new Direction[1];
    public Direction[] pathRotating = new Direction[1];
	public Direction turretDirection = Direction.Right;

    public TurretType turretType;//ferma o mobile
    public ShotType shotType;

    public GameObject circlePref;//ciò che deve sparare la torretta ferma
    //public GameObject visibilityTurret;

    public int viewDistance=3;
    //public bool hasAreaActivation;

    private bool seenPlayer = false;
    private int indexPathMoving = 0;
    private int indexPathRotating = 0;
    private int cont = 0;
    private bool inverse = false;
    private bool temp = false;
    private bool turn = true;
    private List<Projectile> circles;

    new void Start()
    {
		base.Start();
        circles = new List<Projectile>();
        /*if (hasAreaActivation)
        {
            //active = false;//da controllare nell'animator
            Vector3 visibilityPos = transform.position;
            visibilityPos.y -= 1;
            GameObject area = Instantiate(visibilityTurret, visibilityPos, Quaternion.identity) as GameObject;
            area.transform.parent = transform;
            //area.transform.localScale.x = 1 + 2 * viewDistance;
        }*/
    }

    public void ExecuteAction()
    {
        if (circles != null)
        {
            for (int i = circles.Count - 1; i >= 0; i--)
            {
                if (circles[i] == null)
                {
                    circles.RemoveAt(i);
                }
                else
                {
                    //fa muovere le palle
                    circles[i].AttemptMove(circles[i].whereToGo);
                    circles[i].CountBeat();
                }
            }
        }
        switch (turretType)
        {
            case TurretType.Stop:                
                switch (shotType)
                {
                    case ShotType.EveryBeat:
                        Shot();
                        break;
                    case ShotType.EveryTwoBeat:
                        if (turn)
                        {
                            Shot();
                            turn = !turn;
                            return;
                        }
                        turn = !turn;
                        break;
                    case ShotType.FiveStop:
                        cont++;
                        if (cont > 5)
                        {
                            if (cont == 8)
                            {
                                cont = 0;
                            }
                        }
                        else
                        {
                            Shot();
                        }
                        break;
                }
                if (indexPathRotating >= pathRotating.Length)
                {
                    indexPathRotating = 0;
                }
                Rotate(pathRotating[indexPathRotating]);
                turretDirection = pathRotating[indexPathRotating];
                indexPathRotating++;
                break;
            case TurretType.Mobile:
                if (seePlayer)
                {
                    //sta vedendo il player
                    seenPlayer = true;
                    //Move();
                    //turretDirection = pathMoving[indexPathMoving];
                    Shot();
                }
                else
                {
                    //non sta vedendo il player
                    if (seenPlayer)
                    {
                        //l'ha visto ma non lo sta più vedendo
                        inverse = !inverse;
                        if (inverse)
                        {
                            indexPathMoving--;
                        }
                        else
                        {
                            indexPathMoving++;
                        }
                        ControlIndex();
                        seenPlayer = false;
                    }                    
                    Move();
                }
                break;
        }

    }
    private void Move()
    {
        //Debug.Log(indexPath);
        if (inverse)
        {
            turretDirection = pathMoving[indexPathMoving].Invert();
        }
        else
        {
            turretDirection = pathMoving[indexPathMoving];
        }
        
        if ( inverse )
        {
            Direction newDirection = pathMoving[indexPathMoving].Invert();
            
            temp = AttemptMove(newDirection);
            Rotate(newDirection);
        }
        else
        {
            temp = AttemptMove(pathMoving[indexPathMoving]);
            Rotate(pathMoving[indexPathMoving]);
        }
        if (temp)
        {
            if (!inverse)//normale
            {
                indexPathMoving++;
            }
            else//inverso
            {
                indexPathMoving--;
            }
        }
        if (temp == inverse)
        {
            if (temp == false && !inverse)
            {
                indexPathMoving--;
            }
            inverse = true;
        }
        else
        {
            if (temp == false && inverse == true)
            {
                inverse = false;
                indexPathMoving++;
            }
        }
        ControlIndex();           
        //IfRotateShot();
        
    }

    private void Shot()
    {       
        GameObject circleObj = Instantiate( circlePref, transform.position, Quaternion.identity ) as GameObject;
        if (circleObj != null)
        {
            Projectile circle = circleObj.GetComponent<Projectile>();
            circle.lifeTime = viewDistance;
            circle.whereToGo = turretDirection;
            circles.Add( circle );
        }
    }

    private void IfRotateShot()
    {
        if (turretDirection != pathMoving[indexPathMoving])
        {
            Shot();
        }
    }

    private void ControlIndex()
    {
        if (indexPathMoving == pathMoving.Length)//arriva in fondo normale
        {
            indexPathMoving = 0;
        }
        else
        {
            if (indexPathMoving == -1)//arriva in fondo inverso
            {
                indexPathMoving = pathMoving.Length - 1;
            }
        }
    }
}
