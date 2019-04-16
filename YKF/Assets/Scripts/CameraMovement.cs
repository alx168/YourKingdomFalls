using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	[SerializeField]
    private float speedInput;
    private float doubleSpeed;
    private float speed;
    Transform t;

    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Transform>();
        doubleSpeed = speedInput * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift)){
        	speed = doubleSpeed;
        }else{
        	speed = speedInput;
        }
        if(Input.GetKey("d")){
        	t.Translate( new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if(Input.GetKey("a")){
        	t.Translate( new Vector3(-speed * Time.deltaTime, 0, 0));
        }
        if(Input.GetKey("w")){
        	t.Translate( new Vector3(0, 0, speed * Time.deltaTime));
        }
        if(Input.GetKey("s")){
        	t.Translate( new Vector3(0, 0, -speed * Time.deltaTime));
        }
        if(Input.GetMouseButton(1)){
     		t.LookAt(t);
         	t.RotateAround(t.position, Vector3.up, Input.GetAxis("Mouse X")*speed);
        }
        if(Input.GetKey("e") ){
        	t.Translate( new Vector3(0, -speed * Time.deltaTime, 0));
        }
        if(Input.GetKey("q") ){
        	t.Translate( new Vector3(0, speed * Time.deltaTime, 0));
        }
    }
}
