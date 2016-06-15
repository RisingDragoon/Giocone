using System;
using UnityEngine;
using System.Collections.Generic;

public class Boss : Mobile
{
    private int contTurns=0;
    private int toDo=-1;
    [HideInInspector]
    public List<Projectile> bossCircles;

    private List<Turret> bossTurrets;
    public GameObject projectile;
    public GameObject turretBoss;
    public Direction bossDirection = Direction.Down;
    private Transform[] hands;
    private int turnsOfAttack = 0;
    private Transform player;
    private float diffX = 0;
    private float diffZ = 0;
    private Direction whereToGo;
    public Vector3 verticeInBassoASx;
    public Vector3 verticeInAltoADx;
    private int numberOfTurretToCreateHor = 0;
    private int numberOfTurretToCreateVer = 0;
    private bool equalizzatore;
    private int stopped=-1;

    new void Start()
    {
        base.Start();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<Transform>();
        }
        hands = GetComponentsInChildren<Transform>();
        bossCircles = new List<Projectile>();
        bossTurrets = new List<Turret>();
        numberOfTurretToCreateHor += (int)((verticeInAltoADx.x - verticeInBassoASx.x + 1));
        numberOfTurretToCreateVer += (int)((verticeInAltoADx.z - verticeInBassoASx.z + 1));
        CreaTorretteBoss();
        RendiInattive();
    }

    public void ExecuteAction()
    {   
        diffX = transform.position.x - player.position.x;
        diffZ = transform.position.z - player.position.z;
        foreach (Turret turret in bossTurrets)
        {
            if (turret.isActiveAndEnabled)
            {
                turret.ExecuteAction();
            }
        }
        if (bossCircles != null)
        {
            for (int i = bossCircles.Count - 1; i >= 0; i--)
            {
                if (bossCircles[i] == null)
                {
                    bossCircles.RemoveAt(i);
                }
                else
                {
                    //fa muovere le palle
                    bossCircles[i].AttemptMove(bossCircles[i].whereToGo);
                }
            }
        }
        #region Verifica cosa deve fare e lo fa
        if (stopped >= 0)
        {
            Debug.Log("Stopped");
            Mani();
            stopped--;
        }
        //controlla se sta attaccando        
        else if (toDo == 0 || toDo==1)
        {
            turnsOfAttack++;
            Debug.Log("Sta attaccando con l'equalizzatore o con le mani");
            switch (toDo)
            {
                case 0:
                    ShotByHand();
                    if (turnsOfAttack == 6)
                    {
                        toDo = -1;
                        turnsOfAttack = 0;
                        stopped = 4;
                    }
                    break;
                case 1:
                    Equalizzatore();
                    if (turnsOfAttack == 10)
                    {
                        toDo = -1;
                        turnsOfAttack = 0;
                        stopped = 12;  
                        RendiInattive();                      
                    }
                    break;
            }
        }
        //fermo?
        else
        {
            //deve fare cose
            Debug.Log("Non Stopped");
            //RendiInattive();
            if (toDo==-1)
            {
                toDo = WhatToDo();
            }
            #region Fa cose
            switch (toDo)
            {
                case 0://attacco dalle mani
                    Idle();
                    ShotByHand();
                    break;
                case 1://equalizzatore
                    Idle();
                    Equalizzatore();
                    break;
                //case 2://mani in movimento
                    //Mani();
                    //break;
                case 3://muoversi
                    WhereToGo();
                    #region Movimento ogni 3 turni
                    if (contTurns == 0)
                    {
                        //si muove
                        AttemptMove(whereToGo);
                        if (bossDirection!=whereToGo)
                        {
                            Rotate(whereToGo.Invert());
                        }
                        bossDirection = whereToGo;
                    }
                    if (contTurns == 3)
                    {
                        contTurns = -1;
                    }
                    contTurns++;
                    toDo = -1;
                    #endregion
                    break;
            }
            #endregion
        }
        
        #endregion  
    }

    private void Equalizzatore()
    {
        if (bossDirection==Direction.Down || bossDirection == Direction.Up)
        {
            RendiAttive(Math.Abs(player.position.x % 2) < 0.6);
        }
        else
        {
            //float rest = Math.Abs(player.position.z%2);
            RendiAttive(Math.Abs(player.position.z % 2) < 0.6);
        }
    }

    private void CreaTorretteBoss()
    {
        Vector3 where;
        #region Down
        where.x = verticeInBassoASx.x;
        bool resto = true;
        where.y =  0.75f;
        where.z = verticeInBassoASx.z - 1;
        for (int i = 0; i < numberOfTurretToCreateHor; i++)
        {
            GameObject turretObj = Instantiate(turretBoss, where, Quaternion.identity) as GameObject;
            if (turretObj != null)
            {
                Turret turret = turretObj.GetComponent<Turret>();
                turret.turretDirection = Direction.Up;
                turret.turretType = Turret.TurretType.Stop;
                turret.shotType = Turret.ShotType.EveryTwoBeat;
                turret.viewDistance = 15;
                turret.isBossTurret = true;
                turret.pari =resto;
                resto = !resto;
                /*float resto = where.x%2;
                
                if (resto==0.5)
                {
                    resto++;
                }

                if (resto<0)
                {
                    turret.pari = Math.Abs(resto) < 0.6;
                }
                else
                {
                    turret.pari = !(Math.Abs(resto) > 0.6);
                }*/
                //turret.pari = Math.Abs(resto) < 0.6;

                bossTurrets.Add(turret);
            }
            where.x += 1;
        }
        #endregion

        #region Up
        where.x = verticeInBassoASx.x;
        where.y = 0.75f;
        where.z = verticeInAltoADx.z + 1;
        for (int i = 0; i < numberOfTurretToCreateHor; i++)
        {
            GameObject turretObj = Instantiate(turretBoss, where, Quaternion.identity) as GameObject;
            if (turretObj != null)
            {
                Turret turret = turretObj.GetComponent<Turret>();
                turret.turretDirection = Direction.Down;
                turret.turretType = Turret.TurretType.Stop;
                //turret.pari = Math.Abs(where.x % 2) < 0.4;
                turret.viewDistance = 15;
                turret.isBossTurret = true;
                turret.pari = resto;
                resto = !resto;
                bossTurrets.Add(turret);
            }
            where.x += 1;
        }
        #endregion

        #region Right
        where.z = verticeInBassoASx.z;
        where.y = 0.75f;
        where.x = verticeInAltoADx.x + 1;
        for (int i = 0; i < numberOfTurretToCreateVer; i++)
        {
            GameObject turretObj = Instantiate(turretBoss, where, Quaternion.identity) as GameObject;
            if (turretObj != null)
            {
                Turret turret = turretObj.GetComponent<Turret>();
                turret.turretDirection = Direction.Left;
                turret.turretType = Turret.TurretType.Stop;
                //turret.pari = Math.Abs(where.z % 2) < 0.4;
                turret.isBossTurret = true;
                turret.viewDistance = 15;
                turret.pari = resto;
                resto = !resto;
                bossTurrets.Add(turret);
            }
            where.z += 1;
        }
        #endregion

        #region Left
        where.z = verticeInBassoASx.z;
        where.y = 0.75f;
        where.x = verticeInBassoASx.x - 1;
        for (int i = 0; i < numberOfTurretToCreateVer; i++)
        {
            GameObject turretObj = Instantiate(turretBoss, where, Quaternion.identity) as GameObject;
            if (turretObj != null)
            {
                Turret turret = turretObj.GetComponent<Turret>();
                turret.turretDirection = Direction.Right;
                turret.turretType = Turret.TurretType.Stop;
                //turret.pari = Math.Abs(where.z % 2) < 0.4;
                turret.isBossTurret = true;
                turret.viewDistance = 15;
                turret.pari = resto;
                resto = !resto;
                bossTurrets.Add(turret);
            }
            where.z += 1;
        }
        #endregion

    }

    private void RendiInattive()
    {
        foreach (Turret turret in bossTurrets)
        {
            turret.gameObject.SetActive(false);
            turret.shotType=Turret.ShotType.EveryTwoBeat;
        }
    }

    private void RendiAttive(bool pari)
    {
        Direction turretDir = bossDirection.Invert();
        foreach (Turret turret in bossTurrets)
        {
            if (turret.turretDirection==turretDir && turret.pari == pari)
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
        switch (bossDirection)
        {
                case Direction.Down:
                    #region Down
                    if (diffZ > 4 && diffZ < 8)
                    {
                        //lontano da 4 a 8 unità
                        //equalizzatore
                        Debug.Log("Equalizzatore");
                        temp = 1;
                    }
                    else if (diffZ <= 4 && diffZ>1 && Math.Abs(diffX) < 1)
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
                    if (diffZ < -4 && diffZ > -8)
                    {
                        //lontano da 4 a 8 unità
                        Debug.Log("Equalizzatore");
                    //equalizzatore
                    temp = 1;
                    }
                    else if (diffZ >= -4 && diffZ<-1 && Math.Abs(diffX) > -1)
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
                    if (diffX < -4 && diffX > -8)
                    {
                        //lontano da 4 a 8 unità
                        //equalizzatore
                        Debug.Log("Equalizzatore");
                        temp = 1;
                    }
                    else if (diffX >= -4 && diffX < -1 && Math.Abs(diffZ) < 1)
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
                    if (diffX < -4 && diffX > -8)
                    {
                        Debug.Log("Equalizzatore");
                    //lontano da 4 a 8 unità
                    //equalizzatore
                    temp = 1;
                    }
                    else if (diffX >= -4 && diffX < -1 && Math.Abs(diffZ) > 1)
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
        if (Math.Abs(diffX) > Math.Abs(diffZ))
        {
            //si muove sulla x
            if (diffX > 0)
            {
                whereToGo = Direction.Left;
            }
            else
            {
                whereToGo = Direction.Right;
            }
        }
        else
        {
            //si muove sulla z
            if (diffZ > 0)
            {
                whereToGo = Direction.Down;
            }
            else
            {
                whereToGo = Direction.Up;
            }
        }
    }

    private void ShotByHand()
    {
        foreach (Transform hand in hands)
        {
            GameObject circleObj = Instantiate(projectile, hand.position, Quaternion.identity) as GameObject;
            if (circleObj != null)
            {
                Projectile circle = circleObj.GetComponent<Projectile>();
                circle.whereToGo = bossDirection;
                bossCircles.Add(circle);
            }
        }        
    }

    private void Idle()
    {
        Animator anim = GetComponentInParent<Animator>();
        if (anim != null)
        {
            anim.SetBool("Punching", false);
            anim.SetBool("MovingHand", false);
            anim.SetBool("Bite", false);
        }
    }

    private void Mani()
    {
        Animator anim = GetComponentInParent<Animator>();
        if (anim != null)
        {
            //anim.SetBool("Punching", false);
            anim.SetBool("MovingHand", true);
            //anim.SetBool("Bite", false);
        }
    }
    
}
