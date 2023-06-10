using System.Collections;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public Transform car;
    public Transform camLookTarget;
    public float distance;
    public float height;
    public float rotationDamping;
    public float heightDamping;
    public float zoomRatio;
    public float defaultFOV;
    public float rotationVector;
    

	void Start()
	{
		car = GameObject.FindGameObjectWithTag("Player").transform;
		camLookTarget = GameObject.FindGameObjectWithTag("PlayerFollow").transform;
	}

    void FixedUpdate()
    {
        Vector3 localVelocity = car.InverseTransformDirection(car.GetComponent<Rigidbody>().velocity);

        if(localVelocity.z < -0.5f)
        {
            rotationVector = car.eulerAngles.y + 100;
        }

        else
        {
            rotationVector = car.eulerAngles.y;
        }

        float acceleration = car.GetComponent<Rigidbody>().velocity.magnitude;
        Camera.main.fieldOfView = defaultFOV + acceleration*zoomRatio*Time.deltaTime;
    }

    void LateUpdate()
    {
        float wantedAngle = rotationVector;
        float wantedHeight = camLookTarget.position.y + height;
        float myAngle = transform.eulerAngles.y;
        float myHeight = transform.position.y;

        myAngle = Mathf.LerpAngle(myAngle, wantedAngle, rotationDamping * Time.deltaTime);
        myHeight = Mathf.LerpAngle(myHeight, wantedHeight, heightDamping * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, myAngle, 0);

        transform.position = camLookTarget.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        Vector3 temp = transform.position;
        temp.y = myHeight;
        transform.position = temp;

        transform.LookAt(camLookTarget);
    }
	
}
