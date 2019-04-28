using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	[SerializeField]
    private float speedInput;
    private float doubleSpeed;
    private float speed;
    [SerializeField]
    private float MAX_X;
    [SerializeField]
    private float MIN_X;
    [SerializeField]
    private float MIN_Y;
    [SerializeField]
    private float MAX_Y;
    [SerializeField]
    private float MIN_Z;
    [SerializeField]
    private float MAX_Z;

    Transform t;
    float pitch;
    float yaw;
    float roll;

    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Transform>();
        doubleSpeed = speedInput * 2;
        pitch = t.eulerAngles.x;
        yaw = t.eulerAngles.y;
        roll = t.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftShift)){
        	speed = doubleSpeed;
        }else{
        	speed = speedInput;
        }
        if(Input.GetKey("d")){
            movement.x++;
        }
        if(Input.GetKey("a")){
            movement.x--;
        }
        if(Input.GetKey("w")){
            movement.z++;
        }
        if(Input.GetKey("s")){
            movement.z--;
        }
        if(Input.GetKey("q"))
        {
            movement.y++;
        }
        if(Input.GetKey("e"))
        {
            movement.y--;
        }
        if(Input.GetMouseButton(1)){
            pitch += -speed * Input.GetAxis("Mouse Y");
            yaw += speed * Input.GetAxis("Mouse X");
            pitch = Mathf.Clamp(pitch, -10f, 60f);
            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }
        t.Translate(movement * speed * Time.deltaTime, Space.Self);
        transform.position = new Vector3(
            Mathf.Clamp(t.position.x, MIN_X, MAX_X),
            Mathf.Clamp(t.position.y, MIN_Y, MAX_Y),
            Mathf.Clamp(t.position.z, MIN_Z, MAX_Z)
        );
    }
}
