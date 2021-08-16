using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelectScript : MonoBehaviour
{
    public AudioClip click_SE;
    AudioSource Source_SE;

    // Start is called before the first frame update
    void Start()
    {
        Source_SE = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    async public void PressMode()
    {
        Source_SE.PlayOneShot(click_SE);
        await Task.Delay(100);
        if (transform.name == "Deck Create")
        {
            SceneManager.LoadScene("DeckCreate");
        }
        else if (transform.name == "GameStart") {
            SceneManager.LoadScene("VS＿CategorySelect");
        }
        else if (transform.name == "Tutorial") {
            SceneManager.LoadScene("Tutorial");
        }
        else if (transform.name == "VS2P")
        {
            SceneManager.LoadScene("VS2pDeckSelect");
        }
        else if (transform.name == "VSONLINE")
        {
            SceneManager.LoadScene("VSON");
        }
        else if (transform.name == "CreateRoom")
        {
            SceneManager.LoadScene("CreateRoom");
        }
        else if (transform.name == "JoinRoom")
        {
            SceneManager.LoadScene("JoinRoom");
        }
        else if (SceneManager.GetActiveScene().name == "")
        {

        }
    }
    async public void PressBack()
    {
        Source_SE.PlayOneShot(click_SE);
        await Task.Delay(100);
        Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "VS＿CategorySelect")
        {
            SceneManager.LoadScene("Mode");
        }
        else if (SceneManager.GetActiveScene().name == "VSON")
        {
            SceneManager.LoadScene("VS＿CategorySelect");
        }
        else if (SceneManager.GetActiveScene().name == "CreateRoom")
        {
            SceneManager.LoadScene("VSON");
        }
        else if (SceneManager.GetActiveScene().name == "JoinRoom")
        {
            SceneManager.LoadScene("VSON");
        }


    }
}
