using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Threading.Tasks;

public class BackTitle : MonoBehaviour
{
    public AudioClip ClickSE;
    AudioSource audioSource;
    bool OneClickFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickGotToMainTitle()
    {
        /*
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        */
        FadeManager.Instance.LoadScene("RoomSetting", 1.0f);
    } 
    public void OnClickCreateRoom()
    {
        FadeManager.Instance.LoadScene("CreateRoom", 1.0f);
    }
    public void OnClickTutorial()
    {
        FadeManager.Instance.LoadScene("Tutorial", 1.0f);
    }
    
    public async void MainTitle()
    {
        if (!OneClickFlag)
        {
            OneClickFlag = true;
            audioSource.PlayOneShot(ClickSE);
            await Task.Delay(1000);
            FadeManager.Instance.LoadScene("RoomSetting", 1.0f);
        }
        //SceneManager.LoadScene("RoomSetting");
    }
}
