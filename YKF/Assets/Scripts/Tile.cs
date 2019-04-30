using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool walkable = true;
    public bool current = false;
    public bool currentTile = false;
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
            GetComponent<Renderer>().material.color = new Color32(251, 211, 70, 1);
        }
        else if (target)
        {
            GetComponent<Renderer>().material.color = new Color32(59, 246,112, 1);
        }
        else if (selectable)
        {
            GetComponent<Renderer>().material.color = new Color32(9, 149, 204, 0);
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void Reset()
    {
        adjacenyList.Clear();
        walkable = true;
        current = false;
       // walkable = true;
        currentTile = false;
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

    public bool CheckForUnits(float jumpHeight, ref int direction, Transform position)
    {
        bool hasEnemy = false;
        direction = 0;
        hasEnemy = CheckTilesForUnits(Vector3.forward, jumpHeight, ref direction, position);

        
        if(hasEnemy == true)
        {
            direction = 0;
            return hasEnemy;
        }
        direction = 1;
        hasEnemy = CheckTilesForUnits(-Vector3.forward, jumpHeight, ref direction, position);

        
        if (hasEnemy == true)
        {
            direction = 1;
            return hasEnemy;
        }
        direction = 2;
        hasEnemy = CheckTilesForUnits(Vector3.right, jumpHeight, ref direction, position);


        if (hasEnemy == true)
        {
            direction = 2;
            return hasEnemy;
        }
        direction = 3;
        hasEnemy = CheckTilesForUnits(-Vector3.right, jumpHeight, ref direction, position);
        if (hasEnemy == true)
        {
            direction = 3;
            return hasEnemy;
        }

        return hasEnemy;
    }

    public bool CheckTilesForUnits(Vector3 direction, float jumpHeight, ref int dirt, Transform pos)
    {
        bool hasEnemy = false;
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);


        foreach (Collider item in colliders)
        {

            Tile tile = item.GetComponent<Tile>();
            
            

            if (tile != null )
            { 
                RaycastHit hit;
                if (dirt == 0)
                {
                    
                    if (Physics.Raycast(pos.position, Vector3.forward, out hit, 1) && hit.transform.tag == "Player")
                    {
                        //Debug.Log("got here");
                        hasEnemy = true;
                        //GameObject player = hit.transform.gameObject;
                        UnitMove player = hit.transform.gameObject.GetComponent<UnitMove>();
                        System.Random rand = new System.Random();
                        player.health -= rand.Next(1, 4);
                        
                        
                    }
                
                }
                else if (dirt == 1)
                {
                    if (Physics.Raycast(pos.position, -Vector3.forward, out hit, 1) && hit.transform.tag == "Player")
                    {
                        hasEnemy = true;
                        UnitMove player = hit.transform.gameObject.GetComponent<UnitMove>();
                        System.Random rand = new System.Random();
                        player.health -= rand.Next(1, 4);
                    }
                }
                else if (dirt == 2)
                {
                    if (Physics.Raycast(pos.position, Vector3.right, out hit, 1) && hit.transform.tag == "Player")
                    {
                        hasEnemy = true;
                        UnitMove player = hit.transform.gameObject.GetComponent<UnitMove>();
                        System.Random rand = new System.Random();
                        player.health -= rand.Next(1, 4);
                    }
                }
                else if (dirt == 3)
                {
                    if (Physics.Raycast(pos.position, -Vector3.right, out hit, 1) && hit.transform.tag == "Player")
                    {
                        hasEnemy = true;
                        UnitMove player = hit.transform.gameObject.GetComponent<UnitMove>();
                        System.Random rand = new System.Random();
                        player.health -= rand.Next(1, 4);
                    }
                }

            }
        }
        return hasEnemy;
    }

    public bool CheckForNPCEnemies(float jumpHeight, ref int direction, Transform position)
    {
        bool hasEnemy = false;
        direction = 0;
        hasEnemy = CheckEnemyNPCTile(Vector3.forward, jumpHeight, direction, position);
        if (hasEnemy == true)
        {
            direction = 0;
            return true;
        }
        direction = 1;
        hasEnemy = CheckEnemyNPCTile(-Vector3.forward, jumpHeight, direction, position);
        if (hasEnemy == true)
        {
            direction = 1;
            return true;
        }
        direction = 2;
        hasEnemy = CheckEnemyNPCTile(Vector3.right, jumpHeight, direction, position);
        if (hasEnemy == true)
        {
            Debug.Log("right");
            direction = 2;
            return true;
        }
        direction = 3;
        hasEnemy = CheckEnemyNPCTile(-Vector3.right, jumpHeight, direction, position);
        if (hasEnemy == true)
        {
            
            direction = 3;
            return true;
        }

        return false;
    }

    public bool CheckEnemyNPCTile(Vector3 direction, float jumpHeight,  int dirt, Transform pos )
    {
        bool hasEnemy = false;
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
           
            if (tile != null)
            {

                //Debug.Log("Tile exists");
                RaycastHit hit;
                if (dirt == 0)
                {
                    if (Physics.Raycast(pos.position, Vector3.forward, out hit, 1)  && hit.transform.tag == "NPC")
                    {
                        //Debug.Log("got here");
                        hasEnemy = true;
                        NPCMove npc = hit.transform.gameObject.GetComponent<NPCMove>();
                        System.Random rand = new System.Random();
                        npc.health -= rand.Next(1, 4);
                    }
                }
                else if(dirt == 1)
                {
                    if (Physics.Raycast(pos.position, -Vector3.forward, out hit, 1) && hit.transform.tag == "NPC")
                    {
                        hasEnemy = true;
                        NPCMove npc = hit.transform.gameObject.GetComponent<NPCMove>();
                        System.Random rand = new System.Random();
                        npc.health -= rand.Next(1, 4);
                    }
                }
                else if (dirt == 2)
                {
                    if (Physics.Raycast(pos.position, Vector3.right, out hit, 0.8f) && hit.transform.tag == "NPC")
                    {
                        hasEnemy = true;
                        NPCMove npc = hit.transform.gameObject.GetComponent<NPCMove>();
                        System.Random rand = new System.Random();
                        npc.health -= rand.Next(1, 4);
                    }
                }
                else if (dirt == 3)
                {
                    if (Physics.Raycast(pos.position, -Vector3.right, out hit, 1) && hit.transform.tag == "NPC")
                    {
                        hasEnemy = true;
                        NPCMove npc = hit.transform.gameObject.GetComponent<NPCMove>();
                        System.Random rand = new System.Random();
                        npc.health -= rand.Next(1, 4);
                    }
                }

            }
        }
        return hasEnemy;
    }

}
