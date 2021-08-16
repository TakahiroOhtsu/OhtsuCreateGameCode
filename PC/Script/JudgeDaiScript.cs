using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class JudgeDaiScript : MonoBehaviour
{
    bool PushFlag = false;
    public PhotonView myPV;

    private static Hashtable hashtable = new Hashtable();

    AudioSource audioSource;

    GameObject AliveSetting;

    // Start is called before the first frame update
    void Start()
    {
        AliveSetting = GameObject.Find("AliveSetting");
        audioSource = this.transform.parent.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (PushFlag == false)
        {
            other.transform.GetChild(3).GetChild(1).gameObject.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && PushFlag == false)
        {
            audioSource.Stop();
            myPV.RPC("ExitErea", RpcTarget.AllViaServer);
            other.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
            other.transform.root.gameObject.transform.GetChild(0).GetComponent<Character_Controll>().MoveLock = true;
            //other.transform.root.gameObject.SetActive(false);
            int PlayerNum = PhotonNetwork.LocalPlayer.ActorNumber;
            Debug.LogWarning("てれれー："+ PlayerNum);

            myPV.RPC("AliveCount", RpcTarget.AllViaServer,PlayerNum);

            /*
            hashtable["ServiverAliveFlag_Feat:" + other.transform.root.gameObject.transform.GetChild(0).gameObject.GetComponent<PhotonView>().OwnerActorNr] = false;
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
            hashtable.Clear();*/
            //PhotonNetwork.LoadLevel("PicArea");
            GameObject TimerCanvas = GameObject.Find("TimerCanvas");
            TimerCanvas.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetChild(3).GetChild(1).gameObject.activeSelf == true)
        {
            other.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
        }

    }

    [PunRPC]
    void ExitErea()
    {
        PushFlag = true;
        GameObject Parent = this.gameObject.transform.parent.gameObject;
        //Debug.LogWarning(Parent.name);
        Parent.GetComponent<Animator>().SetTrigger("Close_Trigger");
    }

    [PunRPC]
    void AliveCount(int ReleaseNum)
    {
        /*GameObject AliveSetting = GameObject.Find("AliveSetting");
        Debug.LogWarning("よんだ");
        AliveSetting.GetComponent<AliveSettingScript>().AliveFlag[PlayerNum] = false;
        AliveSetting.GetComponent<AliveSettingScript>().AlivePlayerCount = AliveSetting.GetComponent<AliveSettingScript>().AlivePlayerCount - 1;
        */
        var PlayerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (var PlayerObject in PlayerObjects)
        {
            if (PlayerObject.GetComponent<PhotonView>().CreatorActorNr == ReleaseNum)
            {
                PlayerObject.transform.root.gameObject.SetActive(false);
            }
        }
        //AliveSetting.GetComponent<AliveSettingScript>().EscapeCounter += 1; //脱出数の増加
        AliveSetting.GetComponent<AliveSettingScript>().EscapePlusFlag = false; //脱出数の増加、それぞれで1増やすためテスト
        /*if (AliveSetting.GetComponent<AliveSettingScript>().EscapeCounter >= AliveSetting.GetComponent<AliveSettingScript>().AlivePlayerCount - 1) //生きている人のうち、残った一人以外が脱出したのなら
        {
            PhotonNetwork.LoadLevel("PicArea");
        }*/
    }
}
