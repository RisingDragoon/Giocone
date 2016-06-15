using System;
using UnityEngine;
using System.Collections.Generic;

public class Boss : Mobile
{
    [HideInInspector]
    public List<Projectile> BossCircles;

    public GameObject Projectile;
    public GameObject TurretBoss;
    public Direction BossDirection = Direction.Down;
    public Vector3 VerticeInBassoASx;
    public Vector3 VerticeInAltoADx;

    private int _contTurns;
    private int _toDo=-1;
    private int _numberOfTurretToCreateHor;
    private int _numberOfTurretToCreateVer;
    private int _stopped = -1;
    private int _turnsOfAttack;
    private float _diffX;
    private float _diffZ;
    private List<Turret> _bossTurrets;
    private Transform[] _hands;
    private Transform _playerTransform;
    private Direction _whereToGo;

    new void Start()
    {
        base.Start();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _playerTransform = playerObj.GetComponent<Transform>();
        }
        _hands = GetComponentsInChildren<Transform>();
        BossCircles = new List<Projectile>();
        _bossTurrets = new List<Turret>();
        _numberOfTurretToCreateHor += (int)(VerticeInAltoADx.x - VerticeInBassoASx.x + 1);
        _numberOfTurretToCreateVer += (int)(VerticeInAltoADx.z - VerticeInBassoASx.z + 1);
        CreateBossTurrets();
        SetInactive();
    }

    public void ExecuteAction()
    {   
        _diffX = transform.position.x - _playerTransform.position.x;
        _diffZ = transform.position.z - _playerTransform.position.z;
        foreach (Turret turret in _bossTurrets)
        {
            if (turret.isActiveAndEnabled)
            {
                turret.ExecuteAction();
            }
        }
        if (BossCircles != null)
        {
            for (int i = BossCircles.Count - 1; i >= 0; i--)
            {
                if (BossCircles[i] == null)
                {
                    BossCircles.RemoveAt(i);
                }
                else
                {
                    //fa muovere le palle
                    BossCircles[i].AttemptMove(BossCircles[i].whereToGo);
                }
            }
        }
        #region Verifica cosa deve fare e lo fa
        if (_stopped >= 0)
        {
            Debug.Log("Stopped");
            AnimatorToMovingHands();
            _stopped--;
        }
        //controlla se sta attaccando        
        else if (_toDo == 0 || _toDo==1)
        {
            _turnsOfAttack++;
            Debug.Log("Sta attaccando con l'equalizzatore o con le mani");
            switch (_toDo)
            {
                case 0:
                    ShotByHand();
                    if (_turnsOfAttack == 6)
                    {
                        AnimatorToIdle();
                        _toDo = -1;
                        _turnsOfAttack = 0;
                        _stopped = 4;
                    }
                    break;
                case 1:
                    Equalizer();
                    if (_turnsOfAttack == 10)
                    {
                        AnimatorToIdle();
                        _toDo = -1;
                        _turnsOfAttack = 0;
                        _stopped = 12;  
                        SetInactive();                      
                    }
                    break;
            }
        }
        //fermo?
        else
        {
            //deve fare cose
            Debug.Log("Non Stopped");
            if (_toDo==-1)
            {
                _toDo = WhatToDo();
            }
            #region Fa cose
            switch (_toDo)
            {
                case 0://attacco dalle mani
                    AnimatorToIdle();
                    ShotByHand();
                    break;
                case 1://equalizzatore
                    AnimatorToIdle();
                    Equalizer();
                    break;
                case 3://muoversi
                    WhereToGo();
                    #region Movimento ogni 3 turni
                    if (_contTurns == 0)
                    {
                        //si muove
                        AttemptMove(_whereToGo);
                        if (BossDirection!=_whereToGo)
                        {
                            Rotate(_whereToGo.Invert());
                        }
                        BossDirection = _whereToGo;
                    }
                    if (_contTurns == 3)
                    {
                        _contTurns = -1;
                    }
                    _contTurns++;
                    _toDo = -1;
                    #endregion
                    break;
            }
            #endregion
        }
        
        #endregion  
    }

    private void Equalizer()
    {
        if (BossDirection==Direction.Down || BossDirection == Direction.Up)
        {
            SetActive(Math.Abs(_playerTransform.position.x % 2) < 0.6);
        }
        else
        {
            SetActive(Math.Abs(_playerTransform.position.z % 2) < 0.6);
        }
    }

    private void CreateBossTurrets()
    {
        Vector3 where;
        #region Down
        where.x = VerticeInBassoASx.x;
        bool resto = true;
        where.y =  0.75f;
        where.z = VerticeInBassoASx.z - 1;
        for (int i = 0; i < _numberOfTurretToCreateHor; i++)
        {
            GameObject turretObj = Instantiate(TurretBoss, where, Quaternion.identity) as GameObject;
            if (turretObj != null)
            {
                Turret turret = turretObj.GetComponent<Turret>();
                turret.TurretDirection = Direction.Up;
                turret.turretType = Turret.TurretType.Stop;
                turret.shotType = Turret.ShotType.EveryTwoBeat;
                turret.ViewDistance = 15;
                turret.IsBossTurret = true;
                turret.Even =resto;
                resto = !resto;
                _bossTurrets.Add(turret);
            }
            where.x += 1;
        }
        #endregion

        #region Up
        where.x = VerticeInBassoASx.x;
        where.y = 0.75f;
        where.z = VerticeInAltoADx.z + 1;
        for (int i = 0; i < _numberOfTurretToCreateHor; i++)
        {
            GameObject turretObj = Instantiate(TurretBoss, where, Quaternion.identity) as GameObject;
            if (turretObj != null)
            {
                Turret turret = turretObj.GetComponent<Turret>();
                turret.TurretDirection = Direction.Down;
                turret.turretType = Turret.TurretType.Stop;
                turret.ViewDistance = 15;
                turret.IsBossTurret = true;
                turret.Even = resto;
                resto = !resto;
                _bossTurrets.Add(turret);
            }
            where.x += 1;
        }
        #endregion

        #region Right
        where.z = VerticeInBassoASx.z;
        where.y = 0.75f;
        where.x = VerticeInAltoADx.x + 1;
        for (int i = 0; i < _numberOfTurretToCreateVer; i++)
        {
            GameObject turretObj = Instantiate(TurretBoss, where, Quaternion.identity) as GameObject;
            if (turretObj != null)
            {
                Turret turret = turretObj.GetComponent<Turret>();
                turret.TurretDirection = Direction.Left;
                turret.turretType = Turret.TurretType.Stop;
                turret.IsBossTurret = true;
                turret.ViewDistance = 15;
                turret.Even = resto;
                resto = !resto;
                _bossTurrets.Add(turret);
            }
            where.z += 1;
        }
        #endregion

        #region Left
        where.z = VerticeInBassoASx.z;
        where.y = 0.75f;
        where.x = VerticeInBassoASx.x - 1;
        for (int i = 0; i < _numberOfTurretToCreateVer; i++)
        {
            GameObject turretObj = Instantiate(TurretBoss, where, Quaternion.identity) as GameObject;
            if (turretObj != null)
            {
                Turret turret = turretObj.GetComponent<Turret>();
                turret.TurretDirection = Direction.Right;
                turret.turretType = Turret.TurretType.Stop;
                turret.IsBossTurret = true;
                turret.ViewDistance = 15;
                turret.Even = !resto;
                resto = !resto;
                _bossTurrets.Add(turret);
            }
            where.z += 1;
        }
        #endregion

    }

    private void SetInactive()
    {
        foreach (Turret turret in _bossTurrets)
        {
            turret.gameObject.SetActive(false);
            turret.shotType=Turret.ShotType.EveryTwoBeat;
        }
    }

    private void SetActive(bool pari)
    {
        Direction turretDir = BossDirection.Invert();
        foreach (Turret turret in _bossTurrets)
        {
            if (turret.TurretDirection==turretDir && turret.Even == pari)
            {
                turret.gameObject.SetActive(true);
            }
            else
            {
                turret.gameObject.SetActive(false);
            }
        }
    }

    private int WhatToDo()
    {
        int temp=0;
        switch (BossDirection)
        {
                case Direction.Down:
                    #region Down
                    if (_diffZ > 4 && _diffZ < 8)
                    {
                        //lontano da 4 a 8 unità
                        //equalizzatore
                        Debug.Log("Equalizer");
                        temp = 1;
                    }
                    else if (_diffZ <= 4 && _diffZ>1 && Math.Abs(_diffX) < 2)
                    {
                        //lontano meno di 4 unità e nelle tre colonne del boss
                        //attacco con le mani
                        Debug.Log("Attacco mani");
                        temp = 0;
                    }
                    else
                    {
                        //si dovrà vuovere
                        temp = 3;
                        Debug.Log("Movimento");
                    
                    }
                    #endregion
                break;
                case Direction.Up:
                    #region Up
                    if (_diffZ < -4 && _diffZ > -8)
                    {
                        //lontano da 4 a 8 unità
                        Debug.Log("Equalizer");
                    //equalizzatore
                    temp = 1;
                    }
                    else if (_diffZ >= -4 && _diffZ<-1 && Math.Abs(_diffX) < 2)
                    {
                        //lontano meno di 4 unità e nelle tre colonne del boss
                        //attacco con le mani
                        Debug.Log("Attacco mani");
                        temp = 0;
                    }
                    else
                    {
                        //si dovrà vuovere
                        temp = 3;
                        Debug.Log("Movimento");
                    
                    }
                #endregion
                break;
                case Direction.Right:
                    #region Right
                    if (_diffX < -4 && _diffX > -8)
                    {
                        //lontano da 4 a 8 unità
                        //equalizzatore
                        Debug.Log("Equalizer");
                        temp = 1;
                    }
                    else if (_diffX >= -4 && _diffX < -1 && Math.Abs(_diffZ) < 2)
                    {
                        //lontano meno di 4 unità e nelle tre colonne del boss
                        //attacco con le mani
                        Debug.Log("Attacco mani");
                        temp = 0;
                    }
                    else
                    {
                        //si dovrà vuovere
                        temp = 3;
                        Debug.Log("Movimento");
                    
                    }
                #endregion
                break;
                case Direction.Left:
                    #region Left
                    if (_diffX > 4 && _diffX < 8)
                    {
                        Debug.Log("Equalizer");
                    //lontano da 4 a 8 unità
                    //equalizzatore
                    temp = 1;
                    }
                    else if (_diffX <= 4 && _diffX > 1 && Math.Abs(_diffZ) < 2)
                    {
                        //lontano meno di 4 unità e nelle tre colonne del boss
                        //attacco con le mani
                        Debug.Log("Attacco mani");
                        temp = 0;
                    }
                    else
                    {
                        //si dovrà vuovere
                        temp = 3;
                        Debug.Log("Movimento");
                    
                    }
                #endregion
                break;
        }
        return temp;
    }

    private void WhereToGo()
    {
        if (Math.Abs(_diffX) > Math.Abs(_diffZ))
        {
            //si muove sulla x
            _whereToGo = _diffX > 0 ? Direction.Left : Direction.Right;
        }
        else
        {
            //si muove sulla z
            _whereToGo = _diffZ > 0 ? Direction.Down : Direction.Up;
        }
    }

    private void ShotByHand()
    {
        foreach (Transform hand in _hands)
        {
            GameObject circleObj = Instantiate(Projectile, hand.position, Quaternion.identity) as GameObject;
            if (circleObj == null) continue;
            Projectile circle = circleObj.GetComponent<Projectile>();
            circle.whereToGo = BossDirection;
            BossCircles.Add(circle);
        }        
    }

    private void AnimatorToIdle()
    {
        Animator anim = GetComponentInParent<Animator>();
        if (anim == null) return;
        anim.SetBool("Punching", false);
        anim.SetBool("MovingHand", false);
    }

    private void AnimatorToMovingHands()
    {
        Animator anim = GetComponentInParent<Animator>();
        if (anim == null) return;
        anim.SetBool("Punching", false);
        anim.SetBool("MovingHand", true);
    }
    
}
