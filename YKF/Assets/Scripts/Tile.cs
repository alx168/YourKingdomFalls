using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
<<<<<<< Updated upstream
    public bool walkable = true;
    public bool current = false;
=======
    public bool walkable;
    public bool currentTile = false;
>>>>>>> Stashed changes
    public bool target = false;
    public bool selectable = false;

    public List<Tile> adjacenyList = new List<Tile>();


    //BFS vars
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    public float f = 0;
    public float g = 0;
    public float h = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTile)
        {
            GetComponent<Renderer>().material.color = Color.magenta;
        }
        else if (target)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (selectable)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void Reset()
    {
        adjacenyList.Clear();

<<<<<<< Updated upstream
        walkable = true;
        current = false;
=======
       // walkable = true;
        currentTile = false;
>>>>>>> Stashed changes
        target = false;
        selectable = false;

        visited = false;
        parent = null;
        distance = 0;
        f = 0;
        g = 0;
        h = 0;

    }

    public void FindNeighbors(float jumpHeight, Tile target)
    {
        Reset();

        CheckTile(Vector3.forward, jumpHeight, target);
        CheckTile(-Vector3.forward, jumpHeight, target);
        CheckTile(Vector3.right, jumpHeight, target);
        CheckTile(-Vector3.right, jumpHeight, target);

    }

    public void CheckTile(Vector3 direction, float jumpHeight, Tile target)
    {
        Vector3 halfExtents = new Vector3(0.25f,(1 + jumpHeight) / 2.0f ,0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach(Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if(tile != null && tile.walkable)
            {
                RaycastHit hit;

                if(!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1) || (tile == target))
                {
                    adjacenyList.Add(tile);
                }
            }
        }
    }
    
}
