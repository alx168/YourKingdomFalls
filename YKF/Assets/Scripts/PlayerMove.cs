using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMove : TacticsMove
{

    public Material hover;
    public Animator anim;

    public AudioSource walking;
    //Play the music
    bool m_Play = true;
    //Detect when you use the toggle, ensures music isn’t played multiple times
    bool m_ToggleChange;


    // Start is called before the first frame update
    void Start()
    {
        Init();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!turn)
        {
            if(m_Play == false){
            walking.Stop();
           }
           m_Play = true;
            anim.SetBool("Walking", false);
            return;
        }
        if (!moving)
        {
            FindSelectableTiles();
            
            CheckMouse();
        }
        else
        {
            // todo move
           if(m_Play == true){
            walking.Play();
           }
           m_Play = false;
            anim.SetBool("Walking", true);
            Move();
        }
    }

    void CheckMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;


        //if (Physics.Raycast(ray, out hit))
        //{
        //    if (hit.collider.tag == "Tile")
        //    {
        //        Tile t = hit.collider.GetComponent<Tile>();

        //        if (t.selectable)
        //        {
        //           // t.GetComponent<Renderer>().material = hover;
        //            //path.Push(t);


        //        }

        //    }
        //}
        

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

    //void OnMouseOver()
    //{
    //    if (turn)
    //    {
    //        hoveringTile.GetComponent<Renderer>().material = hover;
    //    }
    //}

    //void OnMouseExit()
    //{
    //    if (!turn)
    //    {
    //        hoveringTile.GetComponent<Renderer>().material = original;
    //    }
    //}
}
