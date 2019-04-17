using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TacticsMove
{
    // Start is called before the first frame update
     public AudioSource walking;
     bool m_Play;
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving)
        {
            walking.Stop();
            FindSelectableTiles();
            CheckMouse();
        }
        else
        {
            // todo move
            walking.Play();
            Move();
        }
    }

    void CheckMouse()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    if(t.selectable)
                    {
                        MoveToTile(t);
                    }

                }
            }
        }
    }
}
