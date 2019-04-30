using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsMove : MonoBehaviour
{
    public bool turn = false;

    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    public Stack<Tile> path = new Stack<Tile>();
    Tile currentTile;
    


    public bool moving = false;
    
    public int move = 5;
    public float jumpHeight = 2;
    public float moveSpeed = 2;
    public float jumpVelocity = 4.5f;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    float halfHeight = 0;
    float halfZ = 0;
    float halfX = 0;

    bool fallingDown = false;
    bool jumpingUp = false;
    bool movingEdge = false;
    Vector3 jumpTarget;

    public Tile actualTargetTile;
    private bool enemy;
    public int direction;

    public bool doneAttacking = true;
    public bool alreadyMoved = false;
    public bool stillPlaying = true;

    public int unitCount;
    public int npcCount;

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;
        halfZ = GetComponent<Collider>().bounds.extents.z;
        halfX = GetComponent<Collider>().bounds.extents.x;

        TurnMan.AddUnit(this);
    }

    public void RemoveUnit()
    {
        TurnMan.RemoveUnit(this);
    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.currentTile = true;

    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;

        if(Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

    public void ComputeAdjacencyList(float jumpHeight, Tile target)
    {
        foreach(GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(jumpHeight, target);
        }
    }

    public void FindSelectableTiles()
    {
        ComputeAdjacencyList(jumpHeight, null);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;


        while (process.Count != 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            t.selectable = true;
            if (t.distance < move)
            {
                foreach (Tile tile in t.adjacenyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void MoveToTile(Tile tile)
    {
        path.Clear();
        tile.target = true;
        moving = true;

        Tile next = tile;

        while(next != null)
        {
            path.Push(next);
            next = next.parent;

        }
    }

    public void Move()
    {
        if(path.Count == 0)
        {
            //Debug.Log("nothing here");
        }
        if (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;


            //Calculate the unit's position on top of th target tile
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;
            target.x += t.GetComponent<Collider>().bounds.extents.x - halfX;
            target.z += t.GetComponent<Collider>().bounds.extents.z - halfZ;



            

            if (Vector3.Distance(transform.position, target) >= .15f)
            { 
                bool jump = transform.position.y != target.y;
                if (jump)
                {
                    Jump(target);
                }
                else
                {
                    CalculateHeading(target);
                    SetHorizontalVelocity();

                }
                

                    transform.forward = heading;
                    transform.position += velocity * Time.deltaTime;
            }
            else
            {
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            RemoveSelectableTiles();
            moving = false;
            doneAttacking = false;
            alreadyMoved = true;
            //TurnMan.EndTurn();
            //path.Clear();

        }
    }

    protected void RemoveSelectableTiles()
    {
        if(currentTile != null)
        {
            currentTile.currentTile = false;
            currentTile = null;

        }

        foreach(Tile tile in selectableTiles)
        {
            tile.Reset();
        }

        selectableTiles.Clear();
    }

    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }
    void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    void Jump(Vector3 target)
    {
        if (fallingDown)
        {
            FallDownard(target);
        }
        else if(jumpingUp)
        {
            JumpUpward(target);
        }
        else if (movingEdge)
        {
            MoveToEdge();
        }
        else
        {
            PrepareJump(target);
        }
    }

    void PrepareJump(Vector3 target)
    {
        float targetY = target.y;

        target.y = transform.position.y;

        CalculateHeading(target);

        if( transform.position.y > targetY)
        {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = true;

            jumpTarget = transform.position + (target - transform.position) / 2.0f;
        }
        else
        {
            fallingDown = false;
            jumpingUp = true;
            movingEdge = false;

            velocity = heading * moveSpeed / 3.0f;

            float difference = targetY - transform.position.y;

            velocity.y = jumpVelocity * (0.5f + difference / 2);

        }
    }

    void FallDownard(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if( transform.position.y <= target.y)
        {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = false;

            Vector3 pos = transform.position;
            pos.y = target.y;
            transform.position = pos;

            velocity = new Vector3();
        }
    }

    void JumpUpward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if(transform.position.y > target.y)
        {
            jumpingUp = false;
            fallingDown = true;
        }
    }

    void MoveToEdge()
    {
        if(Vector3.Distance(transform.position, jumpTarget) >= 0.05f)
        {
            SetHorizontalVelocity();
        }
        else
        {
            movingEdge = false;
            fallingDown = true;

            velocity /= 5.0f;
            velocity.y = 1.5f;
        }
    }

    protected Tile FindLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach( Tile t in list)
        {
            if( t.f < lowest.f)
            {
                lowest = t;
            }
        }

        list.Remove(lowest);
        
        return lowest;
    }

    protected Tile FindEndTile(Tile t)
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;

        while(next != null)
        {
            tempPath.Push(next);
            next = next.parent;
        }

        if (tempPath.Count <= move)
        {
            return t.parent;
        }

        Tile endTile = null;
        for (int i = 0; i <= move; i++)
        {
            endTile = tempPath.Pop();
        }
        return endTile;

    }

    protected void FindPath(Tile target)
    {
        ComputeAdjacencyList(jumpHeight, target);
        GetCurrentTile();

        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(currentTile);
        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h;

        while( openList.Count > 0)
        {
            Tile t = FindLowestF(openList);

            closedList.Add(t);

            if(t == target)
            {
                actualTargetTile = FindEndTile(t);
                MoveToTile(actualTargetTile);
                return;
            }

            foreach(Tile tile in t.adjacenyList)
            {
                if (closedList.Contains(tile))
                {

                }
                else if (openList.Contains(tile))
                {
                    float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                    if( tempG < tile.g)
                    {
                        tile.parent = t;

                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                }
                else
                {
                    tile.parent = t;

                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                    tile.f = tile.g + tile.h;

                    openList.Add(tile);
                }
            }

        }

    } 

    public void BeginTurn()
    {
        turn = true;
    }

    public void EndTurn()
    {
        turn = false;
    }

    public void FindUnitToAttack(int health, Animator anim, Transform position)
    {
        if (doneAttacking == false)
        {
            enemy = false;
            GetCurrentTile();
            enemy = currentTile.CheckForUnits(jumpHeight, ref direction, position);


            if (direction == 0 && enemy == true)
            {
                //Debug.Log("gothere0");
                transform.rotation = Quaternion.Euler(0, 0, 0);
                System.Random rand = new System.Random();
                health -= rand.Next(1, 4);
                anim.SetBool("Attacking", true);
                //StartCoroutine(Example(anim));
            }
            else if (direction == 1 && enemy == true)
            {
                // Debug.Log("gothere1");
                transform.rotation = Quaternion.Euler(0, 180, 0);
                System.Random rand = new System.Random();
                health -= rand.Next(1, 4);
                anim.SetBool("Attacking", true);
                //StartCoroutine(Example(anim));
            }
            else if (direction == 2 && enemy == true)
            {
                //   Debug.Log("gothere2");
                transform.rotation = Quaternion.Euler(0, 90, 0);
                System.Random rand = new System.Random();
                health -= rand.Next(1, 4);
                anim.SetBool("Attacking", true);
                //StartCoroutine(Example(anim));
            }
            else if (direction == 3 && enemy == true)
            {
                // Debug.Log("gothere3");
                transform.rotation = Quaternion.Euler(0, 270, 0);
                System.Random rand = new System.Random();
                health -= rand.Next(1, 4);
                anim.SetBool("Attacking", true);
                //StartCoroutine(Example(anim));
            }
            Debug.Log("Unit " + health);
            //if(enemy == false)
            //{
            doneAttacking = true;

            //}
            //doneAttacking = true;

            enemy = false;
            

            //Debug.Log("Npc " + health);
            //Debug.Log("Direction: " + direction);
            
            

        }
            


    }
    
    public void  FindNPCToAttack(int health, Animator anim, Transform position)
    {
        if (doneAttacking == false)
        {

            enemy = false;
            GetCurrentTile();
            enemy = currentTile.CheckForNPCEnemies( jumpHeight, ref direction, position);

            Debug.Log(enemy);
            if (direction == 0 && enemy == true)
            {
                //Debug.Log("gothere0");
                transform.rotation = Quaternion.Euler(0, 0, 0);
                
                anim.SetBool("Attacking", true);
                //StartCoroutine(Example(anim));
            }
            else if (direction == 1 && enemy == true)
            {
                //Debug.Log("gothere1");
                transform.rotation = Quaternion.Euler(0, 180, 0);
               
                anim.SetBool("Attacking", true);
                //StartCoroutine(Example(anim));
            }
            else if (direction == 2 && enemy == true)
            {
                //Debug.Log("gothere2");
                transform.rotation = Quaternion.Euler(0, 90, 0);
                
                anim.SetBool("Attacking", true);
                //StartCoroutine(Example(anim));
            }
            else if (direction == 3 && enemy == true)
            {
                //Debug.Log("gothere3");

                transform.rotation = Quaternion.Euler(0, 270, 0);
                
                
                anim.SetBool("Attacking", true);
                //StartCoroutine(Example(anim));
            }

            Debug.Log("NPC " + health);
            //Debug.Log("Direction: " + direction);
            //Debug.Log("Enemy: " + enemy);
            
            doneAttacking = true;

            
            enemy = false;
            if (health <= 0)
            {
                TurnMan.RemoveUnit(this);
                TurnMan.EndTurn();
                //Destroy(this);
            }
            
        }
            
    }

    IEnumerator Example(Animator anim)
    {
        //print(Time.time);
        
        yield return new WaitForSecondsRealtime(1);
        anim.SetBool("Attacking", false);
        
        
        doneAttacking = true;
        //doneAttacking = true;
        //print(Time.time);
    }
}
