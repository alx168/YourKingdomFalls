using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCMove : TacticsMove
{
    GameObject target;
    // Start is called before the first frame update
    public Animator anim;
    public int health;
    public Image healthBar;

    public GameObject unit;
    public int maxHealth;
    GameObject enemies;
    //GameObject.FindWithTag("Player");
    void changeHealth()
    {
        healthBar.fillAmount = health / maxHealth;
    }

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindWithTag("Player");
        if( enemies == null)
        {
            stillPlaying = false;

        }
        if (stillPlaying == true)
        {
            if (!turn)
            {
                changeHealth();
                if (health <= 0)
                {
                    //Debug.Log("My nigga Died");
                    npcCount = npcCount- 1;
                    if (npcCount <= 0)
                    {
                        stillPlaying = false;
                    }
                    RemoveUnit();
                    Destroy(unit);
                    

                }
                anim.SetBool("Attacking", false);
                //anim.SetBool("Attacking", false);
                alreadyMoved = false;
                return;
            }
            if (!moving && doneAttacking && alreadyMoved == false)
            {
                //FindUnitAttacker(health, anim);


                FindNearestTarget();
                CalculatePath();
                FindSelectableTiles();
                actualTargetTile.target = true;
                //FindUnitAttacker(health, anim);

            }
            else if (!moving && doneAttacking == false)
            {
                anim.SetBool("Walking", false);
                FindUnitToAttack(health, anim, transform);
                //if (health <= 0)
                //{
                   //TurnMan.RemoveUnit(this);
                   //Destroy(unit);
                    
                //}
                TurnMan.EndTurn();

            }
            else if (moving && doneAttacking && alreadyMoved == false)
            {
                // todo move
                //FindUnitAttacker(health, anim);
                anim.SetBool("Walking", true);
                Move();
                //FindUnitAttacker(health, anim);
            }
        }
    }

    

    void CalculatePath()
    {
        Tile targetTile = GetTargetTile(target);
        FindPath(targetTile);
    }

    

    void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach(GameObject obj in targets)
        {
            float d = Vector3.Distance(transform.position, obj.transform.position);
            
            if( d < distance)
            {
                distance = d;
                nearest = obj;
            }
        }

        target = nearest;
    }
}

  