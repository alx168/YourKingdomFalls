using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : TacticsMove
{
    GameObject target;
    // Start is called before the first frame update
    public Animator anim;
    public AudioSource walking;
    void Start()
    {
        Init();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!turn)
        {
            walking.Stop();
            anim.SetBool("Walking", false);
            return;
        }
        if (!moving)
        {
            FindNearestTarget();
            CalculatePath();
            FindSelectableTiles();
            actualTargetTile.target = true;

        }
        else
        {
            // todo move
            walking.Play();
        	anim.SetBool("Walking", true);
            Move();
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

  