using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMove : TacticsMove
{

    
    public Animator anim;
    public Image healthBar;
    Tile hoveringTile;

    public float maxHealth;
    public float health;
    public GameObject unit;
    
    GameObject enemies;


    void changeHealth() {
        healthBar.fillAmount = health / maxHealth;
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

        enemies = GameObject.FindWithTag("NPC");
        if (enemies == null)
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
                    unitCount--;

                    RemoveUnit();
                    if (unitCount == 0)
                    {
                        stillPlaying = false;
                    }
                    

                    Destroy(unit);
                    //TurnMan.RemoveUnit(this);
                    //TurnMan.EndTurn();
                    //Destroy(this);
                }
                //anim.SetBool("Walking", false);
                anim.SetBool("Attacking", false);
                alreadyMoved = false;
                
                return;
            }
            if (!moving && doneAttacking && alreadyMoved == false)
            {

                // FindNPCAttacker(health, anim);
                
                FindSelectableTiles();
                
                CheckMouse();

                if (health <= 0)
                {
                    RemoveUnit();
                    TurnMan.EndTurn();
                    Destroy(unit);
                    //TurnMan.RemoveUnit(this);
                    //TurnMan.EndTurn();
                    //    //Destroy(this);
                }
                //FindNPCAttacker(health, anim);
            }
            else if (!moving && doneAttacking == false)
            {
                anim.SetBool("Walking", false);
                FindNPCToAttack((int) health, anim, transform);

                //TurnMan.EndTurn();
                
                if (health <= 0)
                {
                   RemoveUnit();
                    TurnMan.EndTurn();
                    Destroy(unit);
                    //TurnMan.RemoveUnit(this);
                    //TurnMan.EndTurn();
                    //    //Destroy(this);
                }



            }
            else if (moving && doneAttacking && alreadyMoved == false)
            {
                // todo move
                //FindNPCAttacker(health, anim);
                anim.SetBool("Walking", true);
                Move();
                
                if (health <= 0)
                {
                    RemoveUnit();
                    TurnMan.EndTurn();
                    Destroy(unit);
                    //TurnMan.RemoveUnit(this);
                    //TurnMan.EndTurn();
                    //    //Destroy(this);
                }
                //FindNPCAttacker(health, anim);
            }
        }
    }

    

    void CheckMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Input.GetMouseButtonUp(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    if(t.selectable)
                    {
                        MoveToTile(t);
                        //t.target = true;
                        //moving = true;
                    }

                }
            }
        }

        
    }
}
