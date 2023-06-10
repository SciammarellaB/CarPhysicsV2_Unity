using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour {

    public GameObject characterHead;
    public GameObject characterHeadRay;
    
    public LayerMask detectLayer;

    public float mouseX;
    public float mouseY;

    public float actualX;
    public float actualY;

    public float verticalAx;
    public float horizontalAx;

    public float speed;

    public RaycastHit hit;

    public MenuManager mManager;
	
	void Start ()
    {
        
	}
	
	
	void Update ()
    {
        mouseX = -Input.GetAxis("Mouse Y") * 1.5f;
        mouseY = Input.GetAxis("Mouse X") * 1.5f;

        verticalAx = Input.GetAxis("Vertical");
        horizontalAx = Input.GetAxis("Horizontal");

        Character_Head();
        Movements();
        Detector();

    }

    void Character_Head()
    {
        actualX = characterHead.transform.localEulerAngles.x;
        actualY = gameObject.transform.localEulerAngles.y;

        characterHead.transform.localEulerAngles = new Vector3( actualX + mouseX , 0, 0);
    }

    void Movements()
    {
        gameObject.transform.localEulerAngles = new Vector3(0, actualY + mouseY, 0);

        gameObject.transform.Translate(horizontalAx * speed * Time.deltaTime, 0, verticalAx * speed * Time.deltaTime);

        if(verticalAx == 0 || horizontalAx == 0)
        {
            gameObject.transform.Translate(Vector3.zero);
            gameObject.transform.Rotate(Vector3.zero);
        }
    }

    void Detector()
    {
        Debug.DrawLine(characterHead.transform.position, characterHeadRay.transform.position, Color.green);

        if(Physics.Linecast(characterHead.transform.position,characterHeadRay.transform.position,out hit,detectLayer))
        {
            Debug.Log(hit.collider.gameObject);

            if(hit.collider.gameObject.name == "Notebook" && Input.GetKeyDown(KeyCode.E))
            {
                mManager.menuMode = 1;
            }

        }
    }
}
