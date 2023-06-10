using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public int menuMode;
    public GameObject raceMenu;
    	
	void Start ()
    {
		
	}
	
	
	void Update ()
    {
        Menu_HUD();

    }

    void Menu_HUD()
    {
        switch (menuMode)
        {
            // 0 = Normal view
            // 1 = Race Menu
            case 0:
                Cursor.lockState = CursorLockMode.Locked;
                raceMenu.SetActive(false);
                break;
            case 1:
                Cursor.lockState = CursorLockMode.None;
                raceMenu.SetActive(true);
                break;
                   
        }

    }

    public void Back()
    {
        menuMode = 0;
    }

    public void Practice(string s)
    {
        switch (s)
        {
            case "Rally":
                SceneManager.LoadScene(1);
                Debug.Log("First Track");
                break;

        }
    }

}
