using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCurorUP : MonoBehaviour
{

    GameObject GameController;
    Game_Controller script;

    bool push = false;
    // Start is called before the first frame update
    void Start()
    {
        GameController = GameObject.Find("GameControll");
        script = GameController.GetComponent<Game_Controller>();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (push)
        {
            ClickedCuror();
        }
    }

    public void PushDown()
    {
        push = true;
    }

    public void PushUp()
    {
        push = false;
    }
    void ClickedCuror()
    {
        if (script.turnControll == 0)
        {
            if (transform.name == "UP")
            {
                if (script.FirstPosiZ <= 3)
                {
                    script.FirstPosiZ = script.FirstPosiZ + 0.1f;
                }
                script.F_MainCamera.transform.position = new Vector3(-0.5f, 4.5f, script.FirstPosiZ);
            }
            else if (transform.name == "DOWN")
            {
                if (script.FirstPosiZ >= -4)
                {
                    script.FirstPosiZ = script.FirstPosiZ - 0.1f;
                }
                script.F_MainCamera.transform.position = new Vector3(-0.5f, 4.5f, script.FirstPosiZ);
            }
        }
        else
        {
            if (transform.name == "UP")
            {
                if (script.SecondPosiZ >= -3)
                {
                    script.SecondPosiZ = script.SecondPosiZ - 0.1f;
                }
                script.S_MainCamera.transform.position = new Vector3(0.5f, 4.5f, script.SecondPosiZ);
            }
            else if (transform.name == "DOWN")
            {
                if (script.SecondPosiZ <= 4)
                {
                    script.SecondPosiZ = script.SecondPosiZ + 0.1f;
                }
                script.S_MainCamera.transform.position = new Vector3(0.5f, 4.5f, script.SecondPosiZ);
            }
        }
    }
}
