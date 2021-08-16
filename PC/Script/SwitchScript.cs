using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SwitchScript : MonoBehaviour
{
    public GameObject CreateJudgedai;
    public PhotonView myPV;
    bool PushFlag = false;
    GameObject GameManager;
    GameObject AliveSetting;
    AudioSource audioSource;
    GameObject Canvas;
    // Start is called before the first frame update
    void Start()
    {
        Canvas = GameObject.Find("Canvas");
        GameManager = GameObject.Find("GameManager");
        AliveSetting = GameObject.Find("AliveSetting");
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.GetComponent<CGameManagerScript>().SecondPhaseFlag == true)
        {
            if (PushFlag == false)
            {
                other.transform.GetChild(3).GetChild(1).gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (GameManager.GetComponent<CGameManagerScript>().SecondPhaseFlag == true)
        {
            if (Input.GetKeyDown(KeyCode.E) && PushFlag == false)
            {
                myPV.RPC("JudgeDaiEntry", RpcTarget.AllViaServer);
                other.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
                audioSource.Stop();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (GameManager.GetComponent<CGameManagerScript>().SecondPhaseFlag == true)
        {
            if (other.transform.GetChild(3).GetChild(1).gameObject.activeSelf == true)
            {
                other.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    [PunRPC]
    void JudgeDaiEntry()
    {
        CreateJudgedai.GetComponent<Animator>().SetTrigger("Wake_Trigger") ;
        this.GetComponent<Animator>().SetTrigger("Switch_Trigger");
        PushFlag = true;
    }

}
