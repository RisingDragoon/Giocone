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

    public bool active = true;//da controllare nell'animator

    public Direction[] path = new Direction[1];
	public Direction turretDirection = Direction.Right;

    public TurretType turretType;//ferma o mobile

    public GameObject circlePref;//ciò che deve sparare la torretta ferma
    public GameObject visibilityTurret;

    public int viewDistance;
    public bool hasArea;

    private int indexPath = 0;
    private bool inverse = false;
    private bool temp = false;
    private bool turn = true;
    private List<Mobile> circles;

    new void Start()
    {
		base.Start();
        circles = new List<Mobile>();
        if (hasArea)
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
        if ( turretType == TurretType.Stop )
        {
            if ( circles!=null)
            {
                for ( int i = circles.Count-1; i >= 0; i-- )
                {
                    if ( circles[i] == null )
                    {
                        //GameObject obj = circles[i].gameObject;
                        circles.RemoveAt(i);
                        //Destroy(obj);
                    }
                    else
                    {
                        circles[i].AttemptMove( turretDirection );                        
                    }                    
                }   
            }
            
            if ( turn )
            {
                Spara();
                turn = !turn;
                return;
            }
            turn = !turn;
        }
        else
        {
            Muoviti();
        }

    }
    public void Muoviti()
    {
        Debug.Log(indexPath);
       
        if ( inverse )
        {
            Direction newDirection = path[indexPath].Invert();
            temp = AttemptMove(newDirection);
            //Debug.Log(newDirection);            
        }
        else
        {
            temp = AttemptMove(path[indexPath]);
            //Debug.Log(path[indexPath]);
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
        if ( indexPath == path.Length)//arriva in fondo normale
        {
            indexPath = 0;
        }
        else
        {
            if (indexPath == -1)//arriva in fondo inverso
            {
                indexPath = path.Length-1;
            }
        }
        
        turretDirection = path[indexPath];
       
    }

    private void Spara()
    {       
        GameObject circleObj = Instantiate( circlePref, transform.position, Quaternion.identity ) as GameObject;
        Mobile circle = circleObj.GetComponent<Mobile>();
        circles.Add( circle );          
    }

}
