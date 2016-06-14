using System;
using UnityEngine;
using System.Collections.Generic;

public class Boss : Mobile
{
    private int contTurns=0;
    private int toDo=-1;
    private List<Projectile> circles;
    private List<Turret> turrets;
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
    private int numberOfTurretToCreate = 0;
    private bool equalizzatore;
    private int stopped;

    new void Start()
    {
        base.Start();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<Transform>();
        }
        hands = GetComponentsInChildren<Transform>();
        circles = new List<Projectile>();
        numberOfTurretToCreate = (int)((verticeInAltoADx.x - verticeInBassoASx.x + 1));
        CreaTorretteBoss();
        RendiInattive();
    }

    public void ExecuteAction()
    {
        diffX = transform.position.x - player.position.x;
        diffZ = transform.position.z - player.position.z;

        #region Verifica cosa deve fare e lo fa
        if (toDo != -1)
        {
            //controlla se sta attaccando
            turnsOfAttack++;
            switch (toDo)
            {
                case 0:
                    if (turnsOfAttack == 6)
                    {
                        toDo = -1;
                        turnsOfAttack = 0;
                        stopped = 4;
                    }
                    break;
                case 1:
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
        
        if (stopped > 0)
        {
            stopped--;
        }
        else
        {
            toDo = WhatToDo();
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
                case 2://mani in movimento
                    Mani();
                    break;
                case 3://muoversi
                    #region Movimento ogni 3 turni
                    if (contTurns == 0)
                    {
                        //si muove
                        AttemptMove(whereToGo);
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
        RendiAttive(!(Math.Abs(player.position.x%2) < 0.4));
    }

    private void CreaTorretteBoss()
    {
        Vector3 where;
        #region Down
        where.x = verticeInBassoASx.x;
        where.y =  0.75f;
        where.z = verticeInBassoASx.z - 1;
        for (int i = 0; i < numberOfTurretToCreate; i++)
        {
            GameObject turretObj = Instantiate(turretBoss, where, Quaternion.identity) as GameObject;
            if (turretObj != null)
            {
                Turret turret = turretObj.GetComponent<Turret>();
                turret.turretDirection = Direction.Up;
                turret.turretType = Turret.TurretType.Stop;
                turret.pari = Math.Abs(@where.x % 2) < 0.4;
                turrets.Add(turret);
            }
            where.x += 1;
        }
        #endregion

        #region Up
        where.x = verticeInBassoASx.x;
        where.y = 0.75f;
        where.z = verticeInAltoADx.z + 1;
        for (int i = 0; i < numberOfTurretToCreate; i++)
        {
            GameObject turretObj = Instantiate(turretBoss, where, Quaternion.identity) as GameObject;
            if (turretObj != null)
            {
                Turret turret = turretObj.GetComponent<Turret>();
                turret.turretDirection = Direction.Down;
                turret.turretType = Turret.TurretType.Stop;
                turret.pari = Math.Abs(@where.x % 2) < 0.4;
                turrets.Add(turret);
            }
            where.x += 1;
        }
        #endregion

        #region Right
        where.z = verticeInBassoASx.z;
        where.y = 0.75f;
        where.x = verticeInAltoADx.x + 1;
        for (int i = 0; i < numberOfTurretToCreate; i++)
        {
            GameObject turretObj = Instantiate(turretBoss, where, Quaternion.identity) as GameObject;
            if (turretObj != null)
            {
                Turret turret = turretObj.GetComponent<Turret>();
                turret.turretDirection = Direction.Left;
                turret.turretType = Turret.TurretType.Stop;
                turret.pari = Math.Abs(@where.x % 2) < 0.4;
                turrets.Add(turret);
            }
            where.z += 1;
        }
        #endregion

        #region Left
        where.z = verticeInBassoASx.z;
        where.y = 0.75f;
        where.x = verticeInBassoASx.x - 1;
        for (int i = 0; i < numberOfTurretToCreate; i++)
        {
            GameObject turretObj = Instantiate(turretBoss, where, Quaternion.identity) as GameObject;
            if (turretObj != null)
            {
                Turret turret = turretObj.GetComponent<Turret>();
                turret.turretDirection = Direction.Right;
                turret.turretType = Turret.TurretType.Stop;
                turret.pari = Math.Abs(@where.x % 2) < 0.4;
                turrets.Add(turret);
            }
            where.z += 1;
        }
        #endregion

    }

    private void RendiInattive()
    {
        foreach (Turret turret in turrets)
        {
            turret.gameObject.SetActive(false);
            turret.shotType=Turret.ShotType.EveryTwoBeat;
        }
    }

    private void RendiAttive(bool pari)
    {
        Direction turretDir = bossDirection.Invert();
        foreach (Turret turret in turrets)
        {
            if (turret.turretDirection==turretDir && turret.pari == pari)
            {
                turret.gameObject.SetActive(true);
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
                    if (diffX < 2)
                        {
                            //nella colonna del boss
                            //movimento mani
                            temp = 2;
                        }
                        if (diffZ > 4 && diffZ < 8)
                        {
                            //lontano da 4 a 8 unità
                            //equalizzatore
                            temp = 1;
                        }
                        if (diffZ <= 4 && Math.Abs(diffX) < 2)
                        {
                            //lontano meno di 4 unità e nelle tre colonne del boss
                            //attacco con le mani
                            temp = 0;
                        }
                        else
                        {
                            //si dovrà vuovere
                            temp = 3;
                            WhereToGo();
                        }
                    #endregion
                break;
                case Direction.Up:
                    #region Up
                if (diffX < 2)
                    {
                        //nella colonna del boss
                        //movimento mani
                        temp = 2;
                    }
                    if (diffZ < -4 && diffZ > -8)
                    {
                        //lontano da 4 a 8 unità
                        //equalizzatore
                        temp = 1;
                    }
                    if (diffZ >= -4 && Math.Abs(diffX) > -2)
                    {
                        //lontano meno di 4 unità e nelle tre colonne del boss
                        //attacco con le mani
                        temp = 0;
                    }
                    else
                    {
                        //si dovrà vuovere
                        temp = 3;
                        WhereToGo();
                    }
                #endregion
                break;
                case Direction.Right:
                    #region Right
                if (diffZ < 1)
                    {
                        //nella colonna del boss
                        //movimento mani
                        temp = 2;
                    }
                    if (diffX > 4 && diffX < 8)
                    {
                        //lontano da 4 a 8 unità
                        //equalizzatore
                        temp = 1;
                    }
                    if (diffX <= 4 && Math.Abs(diffZ) < 2)
                    {
                        //lontano meno di 4 unità e nelle tre colonne del boss
                        //attacco con le mani
                        temp = 0;
                    }
                    else
                    {
                        //si dovrà vuovere
                        temp = 3;
                        WhereToGo();
                    }
                #endregion
                break;
                case Direction.Left:
                    #region Left
                if (diffZ < 1)
                    {
                        //nella colonna del boss
                        //movimento mani
                        temp = 2;
                    }
                    if (diffX < -4 && diffX > -8)
                    {
                        //lontano da 4 a 8 unità
                        //equalizzatore
                        temp = 1;
                    }
                    if (diffX >= -4 && Math.Abs(diffZ) > -2)
                    {
                        //lontano meno di 4 unità e nelle tre colonne del boss
                        //attacco con le mani
                        temp = 0;
                    }
                    else
                    {
                        //si dovrà vuovere
                        temp = 3;
                        WhereToGo();
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
                whereToGo = Direction.Up;
            }
            else
            {
                whereToGo = Direction.Down;
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
                circles.Add(circle);
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
