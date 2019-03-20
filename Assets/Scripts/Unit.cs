using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	[SerializeField]
    private GameObject UnitModel;
    [SerializeField]
    private int rank;
    [SerializeField]
    private int file;
    [SerializeField]
    private float rankLength;
    [SerializeField]
    private float fileLength;

    public void generateSoldiers(GameObject prefab, int width, int height, float rankLength, float fileLength){
    	for(int i = 0; i < width; i++){
    		for(int j = 0; j < height; j++){
    			GameObject go = Instantiate(prefab, 
    				new Vector3(transform.position.x - (i * rankLength), transform.position.y, transform.position.z - (j * fileLength)), 
    					Quaternion.identity) as GameObject;
    			go.transform.parent = transform;
    		}
    	}
    }
    // Start is called before the first frame update
    void Start()
    {
        generateSoldiers(UnitModel, rank, file, rankLength, fileLength);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
