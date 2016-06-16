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
    public bool SeePlayer = false;
    [HideInInspector]
    public bool Even;
    [HideInInspector]
    public bool IsBossTurret;


    public bool UsePathRotating;
    public  Direction[] PathMoving = new Direction[1];
    public  Direction[] PathRotating = new Direction[1];
	public Direction TurretDirection = Direction.Right;
    public TurretType turretType;//ferma o mobile
    public ShotType shotType;
    public GameObject Projectile;//ciò che deve sparare la torretta

    public int ViewDistance=3;

    private bool _seenPlayer;
    private int _indexPathMoving;
    private int _indexPathRotating;
    private int _cont;
    private bool _inverse;
    private bool _temp;
    private bool _turn = true;
    private List<Projectile> _circles;
    private Boss BossScript;

    new void Start()
    {
		base.Start();
        _circles = new List<Projectile>();
        GameObject bossObj = GameObject.FindGameObjectWithTag("Boss");
        if (bossObj != null)
        {
            BossScript = bossObj.GetComponent<Boss>();
        }
    }

    public void ExecuteAction()
    {
        if (_circles != null)
        {
            for (int i = _circles.Count - 1; i >= 0; i--)
            {
                if (_circles[i] == null)
                {
                    _circles.RemoveAt(i);
                }
                else
                {
                    //fa muovere le palle
                    _circles[i].AttemptMove(_circles[i].whereToGo);
                    _circles[i].CountBeat();
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
                        if (_turn)
                        {
                            Shot();
                            _turn = !_turn;
                            return;
                        }
                        _turn = !_turn;
                        break;
                    case ShotType.FiveStop:
                        _cont++;
                        if (_cont > 5)
                        {
                            if (_cont == 8)
                            {
                                _cont = 0;
                            }
                        }
                        else
                        {
                            Shot();
                        }
                        break;
                }
                if (_indexPathRotating >= PathRotating.Length)
                {
                    _indexPathRotating = 0;
                }
                if (UsePathRotating)
                {
                    Rotate(PathRotating[_indexPathRotating]);
                    TurretDirection = PathRotating[_indexPathRotating];
                    _indexPathRotating++;
                }
                break;
            case TurretType.Mobile:
                if (SeePlayer)
                {
                    //sta vedendo il player
                    _seenPlayer = true;
                    Shot();
                }
                else
                {
                    //non sta vedendo il player
                    if (_seenPlayer)
                    {
                        //l'ha visto ma non lo sta più vedendo
                        _inverse = !_inverse;
                        if (_inverse)
                        {
                            _indexPathMoving--;
                        }
                        else
                        {
                            _indexPathMoving++;
                        }
                        ControlIndex();
                        _seenPlayer = false;
                    }
                    Move();
                }
                break;
        }
    }

    private void Move()
    {
        //Debug.Log(indexPath);
        TurretDirection = _inverse ? PathMoving[_indexPathMoving].Invert() : PathMoving[_indexPathMoving];
        if (_inverse)
        {
            Direction newDirection = PathMoving[_indexPathMoving].Invert();
            _temp = AttemptMove(newDirection);
            Rotate(newDirection);
        }
        else
        {
            _temp = AttemptMove(PathMoving[_indexPathMoving]);
            Rotate(PathMoving[_indexPathMoving]);
        }
        if (_temp)
        {
            if (!_inverse) //normale
            {
                _indexPathMoving++;
            }
            else //inverso
            {
                _indexPathMoving--;
            }
        }
        if (_temp == _inverse)
        {
            if (_temp == false && !_inverse)
            {
                _indexPathMoving--;
            }
            _inverse = true;
        }
        else
        {
            if (_temp == false && _inverse == true)
            {
                _inverse = false;
                _indexPathMoving++;
            }
        }
        ControlIndex();
    }

    private void Shot()
    {
        if (IsBossTurret)
        {
            GameObject circleObj = Instantiate(Projectile, transform.position, Quaternion.identity) as GameObject;
            if (circleObj == null) return;
            Projectile circle = circleObj.GetComponent<Projectile>();
            circle.lifeTime = ViewDistance;
            circle.whereToGo = TurretDirection;
            BossScript.BossCircles.Add(circle);
        }
        else
        {
            GameObject circleObj = Instantiate(Projectile, transform.position, Quaternion.identity) as GameObject;
            if (circleObj == null) return;
            Projectile circle = circleObj.GetComponent<Projectile>();
            circle.lifeTime = ViewDistance;
            circle.whereToGo = TurretDirection;
            _circles.Add(circle);
        }
    }

    private void ControlIndex()
    {
        if (_indexPathMoving == PathMoving.Length) //arriva in fondo normale
        {
            _indexPathMoving = 0;
        }
        else
        {
            if (_indexPathMoving == -1) //arriva in fondo inverso
            {
                _indexPathMoving = PathMoving.Length - 1;
            }
        }
    }
}
