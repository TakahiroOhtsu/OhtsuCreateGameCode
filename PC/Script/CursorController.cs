using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class CursorController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainField" || SceneManager.GetActiveScene().name == "MatchingRoom")
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // Update is called once per frame
    async void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainField" || SceneManager.GetActiveScene().name == "MatchingRoom")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                this.gameObject.GetComponent<Player_Fallow_camera>().enabled = false;
            }
            if (Input.GetMouseButtonDown(0))
            {
                await Task.Delay(200);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                this.gameObject.GetComponent<Player_Fallow_camera>().enabled = true;
            }
        }
    }
}