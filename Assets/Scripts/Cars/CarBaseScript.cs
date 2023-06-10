using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarBaseScript : MonoBehaviour {

	float actualSteer, minSteer, maxSteer; //MAKE THE CAR TURN

	public WheelCollider[] wheelColliders; //FIND WHEELCOLLIDERS

	public Transform[] wheelMeshes; //FIND WHEELMESH

	public Vector3 centerOfMass;

	public AudioSource[] carAudioS;

	public float currentSpeed; //GIVE THE CURRENT SPEED OF THE CAR
	float maxSpeed;
	float acceleration; //GIVE THE CURRENT ACCELERATION OF THE CAR
	public Text carSpeed; //SHOW THE SPEED IN GAME
	public GameObject carRPMPointer;

	public float turboBoost;
	public float maxTurboBoost;
	public float engineWithTurbo;
	public bool withTurbo;

	public float brakeForce;

	public int gear;
	public float gearTorque;
	public Text gearText;
	public bool automaticGear;




	// Use this for initialization
	void Start () 
	{
		minSteer = 30;
		maxSteer = 50;
		gear = 1;
		brakeForce = 3000;
		automaticGear = false;
		carSpeed = GameObject.FindGameObjectWithTag("CarSpeedHUD").GetComponent<Text>();
		this.gameObject.GetComponent<Rigidbody>().centerOfMass = centerOfMass;
		carRPMPointer = GameObject.FindGameObjectWithTag("CarPointerHUD");


	}

	void FixedUpdate()
	{
		currentSpeed = Mathf.Abs(gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3.6f);
		UpdateMeshesPositions();
		Steer ();

		wheelColliders[0].attachedRigidbody.AddForce(Vector3.down * 250 * (currentSpeed/250));
		wheelColliders[1].attachedRigidbody.AddForce(Vector3.down * 250 * (currentSpeed/250));
		wheelColliders[2].attachedRigidbody.AddForce(Vector3.down * 250 * (currentSpeed/250));
		wheelColliders[3].attachedRigidbody.AddForce(Vector3.down * 250 * (currentSpeed/250));
	}

	// Update is called once per frame
	void Update ()
	{
		Gears();
		Engine ();
		HUD();
		Turbo();
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

	void Steer()
	{
		actualSteer =  Mathf.LerpAngle(maxSteer,minSteer, currentSpeed/250); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!//

		wheelColliders[0].steerAngle = actualSteer * Input.GetAxis("Horizontal");
		wheelColliders[2].steerAngle = actualSteer * Input.GetAxis("Horizontal");
	}

	void Engine()
	{
		if(Input.GetAxis("Vertical") > 0)
		{
			acceleration = Input.GetAxis ("Vertical");
		}

		else
		{
			acceleration = 0;
		}

		if(Input.GetAxis("Vertical") < 0)
		{
			BreakAssist();
		}

		else
		{
			wheelColliders[0].brakeTorque = 0;
			wheelColliders[1].brakeTorque = 0;
			wheelColliders[2].brakeTorque = 0;
			wheelColliders[3].brakeTorque = 0;
		}



		if(withTurbo)
		{
			wheelColliders[0].motorTorque = gearTorque * turboBoost;
			wheelColliders[1].motorTorque = gearTorque * turboBoost;
			wheelColliders[2].motorTorque = gearTorque * turboBoost;
			wheelColliders[3].motorTorque = gearTorque * turboBoost;
		}

		else
		{
			wheelColliders[0].motorTorque = gearTorque;
			wheelColliders[1].motorTorque = gearTorque;
			wheelColliders[2].motorTorque = gearTorque;
			wheelColliders[3].motorTorque = gearTorque;
		}



		float carEngineSFactor;

		carEngineSFactor = 0.7f + currentSpeed/maxSpeed * 2.5f;

		carAudioS[0].pitch = carEngineSFactor;
	}

	void Gears()
	{
		switch (automaticGear)
		{
		case true:

			if(currentSpeed == 0 && gear == 1 && acceleration > 0 && gear <7)
			{
				gear++;
				carAudioS[1].Play();
			}

			if(currentSpeed != 0 && currentSpeed >= maxSpeed && gear < 7)
			{
				gear++;
				carAudioS[1].Play();
			}

			if(currentSpeed == 0 && gear == 2)
			{
				gear--;
				carAudioS[1].Play();
			}
							
			if(currentSpeed != 0 && gear > 2 && carAudioS[0].pitch < 1.6f)
			{
				gear--;
				carAudioS[1].Play();
			}



			switch (gear)
			{
			case 0:
				gearText.text = "R";

				if(currentSpeed < maxSpeed)
				{
					gearTorque = acceleration * -500;
				}
				else
				{
					gearTorque = 0;
				}

				maxSpeed = 50;

				break;

			case 1:
				gearText.text = "N";

				maxSpeed = 40 * gear - 30;

				break;

			case 2:
				gearText.text = "1";

				if(currentSpeed < maxSpeed)
				{
					gearTorque = acceleration * (1700 - (200 * gear));
				}
				else
				{
					gearTorque = 0;
				}

				maxSpeed = 40 * gear - 30;

				break;

			case 3:
				gearText.text = "2";

				if(currentSpeed < maxSpeed)
				{
					gearTorque = acceleration * (1700 - (200 * gear));
				}
				else
				{
					gearTorque = 0;
				}

				maxSpeed = 40 * gear - 30;

				break;
			case 4:
				gearText.text = "3";

				if(currentSpeed < maxSpeed)
				{
					gearTorque = acceleration * (1700 - (200 * gear));
				}
				else
				{
					gearTorque = 0;
				}

				maxSpeed = 40 * gear - 30;

				break;

			case 5:
				gearText.text = "4";

				if(currentSpeed < maxSpeed)
				{
					gearTorque = acceleration * (1700 - (200 * gear));
				}
				else
				{
					gearTorque = 0;
				}

				maxSpeed = 40 * gear - 30;

				break;

			case 6:
				gearText.text = "5";

				if(currentSpeed < maxSpeed)
				{
					gearTorque = acceleration * (1700 - (200 * gear));
				}
				else
				{
					gearTorque = 0;
				}

				maxSpeed = 40 * gear - 30;

				break;

			case 7:
				gearText.text = "6";

				if(currentSpeed < maxSpeed)
				{
					gearTorque = acceleration * (1700 - (200 * gear));
				}
				else
				{
					gearTorque = 0;
				}

				maxSpeed = 40 * gear - 30;

				break;
			}
			break;

		case false:

			if(Input.GetKeyDown(KeyCode.E) && gear < 7)
			{
				gear++;
				carAudioS[1].Play();
			}

			if(Input.GetKeyDown(KeyCode.Q) && gear > 0)
			{
				gear--;
				carAudioS[1].Play();
			}

				switch (gear)
				{
				case 0:
					gearText.text = "R";

					if(currentSpeed < maxSpeed)
					{
						gearTorque = acceleration * -500;
					}
					else
					{
						gearTorque = 0;
					}

					maxSpeed = 50;

					break;

				case 1:
					gearText.text = "N";

					maxSpeed = 40 * gear - 30;

					break;

				case 2:
					gearText.text = "1";

					if(currentSpeed < maxSpeed)
					{
						gearTorque = acceleration * (1700 - (200 * gear));
					}
					else
					{
						gearTorque = 0;
					}

					maxSpeed = 40 * gear - 30;

					break;

				case 3:
					gearText.text = "2";

					if(currentSpeed < maxSpeed)
					{
						gearTorque = acceleration * (1700 - (200 * gear));
					}
					else
					{
						gearTorque = 0;
					}

					maxSpeed = 40 * gear - 30;

					break;
				case 4:
					gearText.text = "3";

					if(currentSpeed < maxSpeed)
					{
						gearTorque = acceleration * (1700 - (200 * gear));
					}
					else
					{
						gearTorque = 0;
					}
				
					maxSpeed = 40 * gear - 30;

					break;

				case 5:
					gearText.text = "4";

					if(currentSpeed < maxSpeed)
					{
						gearTorque = acceleration * (1700 - (200 * gear));
					}
					else
					{
						gearTorque = 0;
					}

					maxSpeed = 40 * gear - 30;

					break;

				case 6:
					gearText.text = "5";

					if(currentSpeed < maxSpeed)
					{
						gearTorque = acceleration * (1700 - (200 * gear));
					}
					else
					{
						gearTorque = 0;
					}

					maxSpeed = 40 * gear - 30;

					break;

				case 7:
					gearText.text = "6";

					if(currentSpeed < maxSpeed)
					{
						gearTorque = acceleration * (1700 - (200 * gear));
					}
					else
					{
						gearTorque = 0;
					}
				
					maxSpeed = 40 * gear - 30;

					break;
				}
			break;
		}
	}

	void HUD()
	{
		currentSpeed = Mathf.Round(currentSpeed);

		carSpeed.text = currentSpeed.ToString();

		float pointerFactor = currentSpeed/maxSpeed;

		carRPMPointer.transform.localEulerAngles = new Vector3(0,0,Mathf.Lerp(315,100,pointerFactor));
	}

	void BreakAssist()
	{

		float teste;

		teste = currentSpeed/maxSpeed;



		if(Mathf.Abs(wheelColliders[0].rpm) > 0)
		{
			wheelColliders[0].brakeTorque = brakeForce;
		}

		else
		{
			Debug.Log("ABS:0");
			wheelColliders[0].brakeTorque = 1;
		}

		if(Mathf.Abs(wheelColliders[1].rpm) > 0)
		{
			wheelColliders[1].brakeTorque = brakeForce;
		}

		else
		{
			Debug.Log("ABS:1");
			wheelColliders[1].brakeTorque = 1;
		}

		if(Mathf.Abs(wheelColliders[2].rpm) > 0)
		{
			wheelColliders[2].brakeTorque = brakeForce;
		}

		else
		{
			Debug.Log("ABS:2");
			wheelColliders[2].brakeTorque = 1;
		}

		if(Mathf.Abs(wheelColliders[3].rpm) > 0)
		{
			wheelColliders[3].brakeTorque = brakeForce;
		}

		else
		{
			Debug.Log("ABS:3");
			wheelColliders[3].brakeTorque = 1;
		}
	}

	void Turbo()
	{
		engineWithTurbo = gearTorque * turboBoost;

		if(carAudioS[0].pitch > 2)
		{
			turboBoost = 1.3f;
		}

		else
		{
			turboBoost = 1;
		}
	}

}
