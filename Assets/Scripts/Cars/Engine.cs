using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Engine : MonoBehaviour {

    public float RPM;
    public float maxRPM;
    public float minRPM;
    public float aceleration;
    public float wheelTorque;
    public float actualSpeed;
    public float gearSpeedLimit;
    public float steerAngle;
    public float maxSteerAngle;
    public float minSteerAngle;
    public float turboStartRPM;
    public float turboBoost;
    public float turboMaxBoost;
    public float turboFactor;
    public float respCooldown;
    public float changeMaxGearRPM;
    public float changeMinGearRPM;

    public bool ingnition;
    public bool turboUsed;
    public bool automaticTrans;

    public AudioSource aSourceEngine;
    public AudioSource aSourceGear;
    public AudioSource aSourceStart;
    public AudioSource aSourceTurbo;
    public AudioSource aSourceTurboBlow;

    public GameObject rpmPointer;
    public GameObject turboPointer;
    public GameObject[] checkPoints;
    
    public Transform[] wheelMeshes;

    public Vector3 carCenterOfMass;
    public Vector3 respawnPosition;

    public Quaternion respawnRotation;
    
    public Toggle ingnitionToggle;
    public Toggle turboToggle;
    public Toggle automaticToggle;

    public Text gearText;
    public Text kmText;
    public Text HUDLap;

    public WheelCollider[] wheelColliders;

    public int gear;
    public int engineLevel;    
    public int gearLevel;
    public int turboLevel;
    public int checkPointInt;
    public int playerLaps;
    public int trackLaps;

    
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    
	void Start ()
    {
        maxRPM = 8000;
        minRPM = 1200;
        aceleration = 100;
        gear = 1;
        maxSteerAngle = 40;
        minSteerAngle = 10;
        engineLevel = 1;
        gearLevel = 1;
        turboLevel = 1;        
        turboStartRPM = 5000;
        changeMinGearRPM = 4500;
        changeMaxGearRPM = 7800;
        respawnPosition = new Vector3(-14, 1, 32);
    }
	
	void Update ()
    {
		Break();

        Gears();

        Turbo();

        Steer();

		TrackCounter(); // USED TO COUNT NUMBER OF LAPS AND RESPAWN THE CAR

        UpdateMeshesPositions();

        turboMaxBoost = 500 * turboLevel;

        aSourceEngine.pitch = (RPM / 1600) /1.5f;

        rpmPointer.transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(-12,-251, RPM / maxRPM));

        gameObject.GetComponentInChildren<Rigidbody>().centerOfMass = carCenterOfMass;

        ingnition = ingnitionToggle.isOn;

        turboUsed = turboToggle.isOn;

        automaticTrans = automaticToggle.isOn;

        Debug.Log(gameObject.transform.rotation);

        turboFactor = (turboBoost / turboMaxBoost);

        actualSpeed = gameObject.GetComponentInChildren<Rigidbody>().velocity.magnitude * 3.6f;

        kmText.text = Mathf.Round(actualSpeed).ToString();

        HUDLap.text = "Volta:" + playerLaps + "/" + trackLaps;

    }

	void Break()
	{
		if (Input.GetAxis("Vertical") < 0)
		{
			wheelColliders[0].brakeTorque = 1000;
			wheelColliders[1].brakeTorque = 2000;
			wheelColliders[2].brakeTorque = 1000;
			wheelColliders[3].brakeTorque = 2000;
		}

		else
		{
			wheelColliders[0].brakeTorque = 0;
			wheelColliders[1].brakeTorque = 0;
			wheelColliders[2].brakeTorque = 0;
			wheelColliders[3].brakeTorque = 0;
		}
	}

    void Steer()
    {
		steerAngle =  Mathf.LerpAngle(maxSteerAngle,minSteerAngle,actualSpeed/150);

        wheelColliders[0].steerAngle = steerAngle * Input.GetAxis("Horizontal");
        wheelColliders[2].steerAngle = steerAngle * Input.GetAxis("Horizontal");

		wheelColliders[0].attachedRigidbody.AddForce(-transform.up * 50 * wheelColliders[0].attachedRigidbody.velocity.magnitude);
		wheelColliders[1].attachedRigidbody.AddForce(-transform.up * 50 * wheelColliders[1].attachedRigidbody.velocity.magnitude);
		wheelColliders[2].attachedRigidbody.AddForce(-transform.up * 50 * wheelColliders[2].attachedRigidbody.velocity.magnitude);
		wheelColliders[3].attachedRigidbody.AddForce(-transform.up * 50 * wheelColliders[3].attachedRigidbody.velocity.magnitude);

    }

    void Gears()
    {
		if (ingnition == false)
        {
            aSourceStart.Play();

            if (RPM > 0)
            {
                RPM -= aceleration;
            }

            else
            {
                RPM = 0;
            }
        }

		if (ingnition == true)
		{
			if (Input.GetKeyDown(KeyCode.Q) && gear > 0)
			{
				gear--;

				aSourceGear.Play();

				if (turboBoost > (turboMaxBoost / 1.5f))
				{
					aSourceTurboBlow.Play();
				}

				turboBoost = 0;

			}

			if (Input.GetKeyDown(KeyCode.E) && gear < 7)
			{
				gear++;

				aSourceGear.Play();

				if (turboBoost > (turboMaxBoost / 1.5f))
				{
					aSourceTurboBlow.Play();
				}

				turboBoost = 0;
			}

			switch (gear)
			{

			case 0:
				gearText.text = "R";

				wheelTorque = (350 * engineLevel) + turboBoost;

				gearSpeedLimit = 20 * gearLevel;

				if (Input.GetAxis("Vertical") > 0)
				{
					if (actualSpeed < gearSpeedLimit)
					{
						wheelColliders[0].motorTorque = -wheelTorque;
						wheelColliders[1].motorTorque = -wheelTorque;
						wheelColliders[2].motorTorque = -wheelTorque;
						wheelColliders[3].motorTorque = -wheelTorque;

						wheelColliders[0].brakeTorque = 0;
						wheelColliders[1].brakeTorque = 0;
						wheelColliders[2].brakeTorque = 0;
						wheelColliders[3].brakeTorque = 0;
					}
				}

				else
				{
					wheelColliders[0].motorTorque = 0;
					wheelColliders[1].motorTorque = 0;
					wheelColliders[2].motorTorque = 0;
					wheelColliders[3].motorTorque = 0;

					wheelColliders[0].brakeTorque = 10;
					wheelColliders[1].brakeTorque = 10;
					wheelColliders[2].brakeTorque = 10;
					wheelColliders[3].brakeTorque = 10;
				}

				RPM = Mathf.Lerp(minRPM, maxRPM, actualSpeed / gearSpeedLimit);

				break;

			case 1:
				gearText.text = "N";

				if (Input.GetKey(KeyCode.UpArrow) && RPM < maxRPM)
				{
					RPM += aceleration;
				}

				if (!Input.GetKey(KeyCode.UpArrow) && RPM > minRPM)
				{

					RPM -= aceleration + Random.Range(0, 500);
				}

				if (RPM < minRPM)
				{
					RPM += aceleration;
				}
				break;

			case 2:
				gearText.text = "1";

				wheelTorque = (350 * engineLevel) + turboBoost;

				gearSpeedLimit = 20 * gearLevel;

				if (Input.GetAxis("Vertical") > 0)
				{

					if (actualSpeed < gearSpeedLimit)
					{
						wheelColliders[0].motorTorque = wheelTorque;
						wheelColliders[1].motorTorque = wheelTorque;
						wheelColliders[2].motorTorque = wheelTorque;
						wheelColliders[3].motorTorque = wheelTorque;

						wheelColliders[0].brakeTorque = 0;
						wheelColliders[1].brakeTorque = 0;
						wheelColliders[2].brakeTorque = 0;
						wheelColliders[3].brakeTorque = 0;
					}

				}

				else
				{
					wheelColliders[0].motorTorque = 0;
					wheelColliders[1].motorTorque = 0;
					wheelColliders[2].motorTorque = 0;
					wheelColliders[3].motorTorque = 0;

					wheelColliders[0].brakeTorque = 10;
					wheelColliders[1].brakeTorque = 10;
					wheelColliders[2].brakeTorque = 10;
					wheelColliders[3].brakeTorque = 10;
				}

				RPM = Mathf.Lerp(minRPM, maxRPM, actualSpeed / gearSpeedLimit);

				break;

			case 3:

				gearText.text = "2";


				wheelTorque = (300 * engineLevel) + turboBoost;

				gearSpeedLimit = 40 * gearLevel;

				if (Input.GetAxis("Vertical") > 0 && actualSpeed < gearSpeedLimit)
				{

					wheelColliders[0].motorTorque = wheelTorque;
					wheelColliders[1].motorTorque = wheelTorque;
					wheelColliders[2].motorTorque = wheelTorque;
					wheelColliders[3].motorTorque = wheelTorque;

					wheelColliders[0].brakeTorque = 0;
					wheelColliders[1].brakeTorque = 0;
					wheelColliders[2].brakeTorque = 0;
					wheelColliders[3].brakeTorque = 0;
				}

				else
				{
					wheelColliders[0].motorTorque = 0;
					wheelColliders[1].motorTorque = 0;
					wheelColliders[2].motorTorque = 0;
					wheelColliders[3].motorTorque = 0;

					wheelColliders[0].brakeTorque = 10;
					wheelColliders[1].brakeTorque = 10;
					wheelColliders[2].brakeTorque = 10;
					wheelColliders[3].brakeTorque = 10;
				}

				RPM = Mathf.Lerp(minRPM, maxRPM, actualSpeed / gearSpeedLimit);

				break;

			case 4:

				gearText.text = "3";

				wheelTorque = (250 * engineLevel) + turboBoost;

				gearSpeedLimit = 60 * gearLevel;

				if (Input.GetAxis("Vertical") > 0 && actualSpeed < gearSpeedLimit)
				{

					wheelColliders[0].motorTorque = wheelTorque;
					wheelColliders[1].motorTorque = wheelTorque;
					wheelColliders[2].motorTorque = wheelTorque;
					wheelColliders[3].motorTorque = wheelTorque;

					wheelColliders[0].brakeTorque = 0;
					wheelColliders[1].brakeTorque = 0;
					wheelColliders[2].brakeTorque = 0;
					wheelColliders[3].brakeTorque = 0;
				}

				else
				{
					wheelColliders[0].motorTorque = 0;
					wheelColliders[1].motorTorque = 0;
					wheelColliders[2].motorTorque = 0;
					wheelColliders[3].motorTorque = 0;

					wheelColliders[0].brakeTorque = 10;
					wheelColliders[1].brakeTorque = 10;
					wheelColliders[2].brakeTorque = 10;
					wheelColliders[3].brakeTorque = 10;
				}

				RPM = Mathf.Lerp(minRPM, maxRPM, actualSpeed / gearSpeedLimit);

				break;

			case 5:

				gearText.text = "4";

				wheelTorque = (200 * engineLevel) + turboBoost;

				gearSpeedLimit = 90 * gearLevel;

				if (Input.GetAxis("Vertical") > 0 && actualSpeed < gearSpeedLimit)
				{

					wheelColliders[0].motorTorque = wheelTorque;
					wheelColliders[1].motorTorque = wheelTorque;
					wheelColliders[2].motorTorque = wheelTorque;
					wheelColliders[3].motorTorque = wheelTorque;

					wheelColliders[0].brakeTorque = 0;
					wheelColliders[1].brakeTorque = 0;
					wheelColliders[2].brakeTorque = 0;
					wheelColliders[3].brakeTorque = 0;
				}

				else
				{
					wheelColliders[0].motorTorque = 0;
					wheelColliders[1].motorTorque = 0;
					wheelColliders[2].motorTorque = 0;
					wheelColliders[3].motorTorque = 0;

					wheelColliders[0].brakeTorque = 10;
					wheelColliders[1].brakeTorque = 10;
					wheelColliders[2].brakeTorque = 10;
					wheelColliders[3].brakeTorque = 10;
				}

				RPM = Mathf.Lerp(minRPM, maxRPM, actualSpeed / gearSpeedLimit);

				break;

			case 6:

				gearText.text = "5";

				wheelTorque = (150 * engineLevel) + turboBoost;

				gearSpeedLimit = 110 * gearLevel;

				if (Input.GetAxis("Vertical") > 0 && actualSpeed < gearSpeedLimit)
				{

					wheelColliders[0].motorTorque = wheelTorque;
					wheelColliders[1].motorTorque = wheelTorque;
					wheelColliders[2].motorTorque = wheelTorque;
					wheelColliders[3].motorTorque = wheelTorque;

					wheelColliders[0].brakeTorque = 0;
					wheelColliders[1].brakeTorque = 0;
					wheelColliders[2].brakeTorque = 0;
					wheelColliders[3].brakeTorque = 0;
				}

				else
				{
					wheelColliders[0].motorTorque = 0;
					wheelColliders[1].motorTorque = 0;
					wheelColliders[2].motorTorque = 0;
					wheelColliders[3].motorTorque = 0;

					wheelColliders[0].brakeTorque = 10;
					wheelColliders[1].brakeTorque = 10;
					wheelColliders[2].brakeTorque = 10;
					wheelColliders[3].brakeTorque = 10;
				}

				RPM = Mathf.Lerp(minRPM, maxRPM, actualSpeed / gearSpeedLimit);

				break;

			case 7:

				gearText.text = "6";

				wheelTorque = (100 * engineLevel) + turboBoost;

				gearSpeedLimit = 150 * gearLevel;

				if (Input.GetAxis("Vertical") > 0 && actualSpeed < gearSpeedLimit)
				{

					wheelColliders[0].motorTorque = wheelTorque;
					wheelColliders[1].motorTorque = wheelTorque;
					wheelColliders[2].motorTorque = wheelTorque;
					wheelColliders[3].motorTorque = wheelTorque;

					wheelColliders[0].brakeTorque = 0;
					wheelColliders[1].brakeTorque = 0;
					wheelColliders[2].brakeTorque = 0;
					wheelColliders[3].brakeTorque = 0;
				}

				else
				{
					wheelColliders[0].motorTorque = 0;
					wheelColliders[1].motorTorque = 0;
					wheelColliders[2].motorTorque = 0;
					wheelColliders[3].motorTorque = 0;

					wheelColliders[0].brakeTorque = 10;
					wheelColliders[1].brakeTorque = 10;
					wheelColliders[2].brakeTorque = 10;
					wheelColliders[3].brakeTorque = 10;
				}

				RPM = Mathf.Lerp(minRPM, maxRPM, actualSpeed / gearSpeedLimit);

				break;
			}
  	  }
	}

    void UpdateMeshesPositions()
    {
        for (int i = 0; i < 4; i++)
        {
            Quaternion quat;
            Vector3 pos;
            wheelColliders[i].GetWorldPose(out pos, out quat);

            wheelMeshes[i].position = pos;
            wheelMeshes[i].rotation = quat;
        }
    }

    void Turbo()
    {
        if (turboUsed == true)
        {
            aSourceTurbo.pitch = turboFactor * 3;

            if (RPM > turboStartRPM && Input.GetAxis("Vertical") > 0)
            {
                turboPointer.transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(30, -251, turboFactor));

                if(turboBoost < turboMaxBoost)
                {
            	    turboBoost +=10 * turboLevel;
                }
                
            }

			else
            {
                turboPointer.transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(30, -251, 0));
                turboBoost = 0;
                if(turboBoost > (turboMaxBoost / 1.5f))
                {
                    aSourceTurboBlow.Play();
                }

            }
        }

        else
        {
            turboPointer.transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(30, -251, 0));
            turboBoost = 0;
        }
    }

    void TrackCounter()
    {
        if (checkPointInt == 0)
        {
            checkPoints[0].SetActive(false);
        }

        else
        {
            checkPoints[0].SetActive(true);
        }

        if (checkPointInt > 4)
        {
            playerLaps++;
            checkPointInt = 0;
            checkPoints[0].tag = "Checkpoint";
            checkPoints[1].tag = "Checkpoint";
            checkPoints[2].tag = "Checkpoint";
            checkPoints[3].tag = "Checkpoint";
            checkPoints[4].tag = "Checkpoint";
        }

        if (Input.GetKey(KeyCode.R))
        {
            respCooldown += Time.deltaTime;

            if (respCooldown > 2)
            {
                respCooldown = 0;
                gameObject.transform.position = respawnPosition;
                gameObject.transform.rotation = respawnRotation;
            }

            switch (checkPointInt)
            {
                case 0:
                    respawnPosition = new Vector3(-14, 1, 32);
                    respawnRotation = new Quaternion(0, 0.7f, 0, 0.7f);
                    break;

                case 1:
                    respawnPosition = new Vector3(180, -1, 34);
                    respawnRotation = new Quaternion(0, 0.7f, 0, 0.7f);
                    break;

                case 2:
                    respawnPosition = new Vector3(142, 1, -160);
                    respawnRotation = new Quaternion(0, 0.7f, 0, -0.7f);
                    break;

                case 3:
                    respawnPosition = new Vector3(29, 1, -111);
                    respawnRotation = new Quaternion(0, -0.2f, 0, -1.0f);
                    break;

                case 4:
                    respawnPosition = new Vector3(-135, -6, -68);
                    respawnRotation = new Quaternion(-0.1f, 0.8f, 0, -0.6f);
                    break;
            }

        }

       

    }

    void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.tag == "Checkpoint")
        {
            c.gameObject.tag = "Untagged";

            checkPointInt++;
        }

        
    }
}