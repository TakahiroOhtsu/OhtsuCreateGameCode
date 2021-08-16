using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;


public class TimerScript : MonoBehaviour
{

    private static Hashtable hashtable = new Hashtable();
    public TextMeshProUGUI timeLabel;
    int timestamp = 0;
    public float TimeLimit = 60; //秒
    float TimeLimited = 60;
    bool SecondFlag = false;
    public PhotonView myPV;
    bool PicFlag = false;
    bool PicResultFlag = false;
    GameObject AliveSetting;

    private void Awake()
    {
        //timeLabel = GetComponent<TextMeshProUGUI>();
        timestamp = PhotonNetwork.ServerTimestamp;
    }
    // Start is called before the first frame update
    void Start()
    {
        AliveSetting = GameObject.Find("AliveSetting");
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "DistributeArea")
        {
            ThisDistributeArea();
        }
        else if (SceneManager.GetActiveScene().name == "MainField")
        {
            ThisMainField();
        }
        else if (SceneManager.GetActiveScene().name == "PicArea")
        {
            ThisPicArea();
        }
        else if (SceneManager.GetActiveScene().name == "MurabitoWin" || SceneManager.GetActiveScene().name == "KillerWin" || SceneManager.GetActiveScene().name == "YoukoWin")
        {
            ResultArea();
        }   
    }

    bool RoadFlag = false;
    [PunRPC]
    void RPCNextRoomRead()
    {
        if (RoadFlag == false)
        {
            RoadFlag = true;

            FadeManager.Instance.PLoadScene("MainField", 0.5f);
            //PhotonNetwork.LoadLevel("MainField");
        }
    }    
    [PunRPC]
    void PicToDisRead()
    {
        hashtable["SyukeiFlag"] = SyukeiFlag;
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        hashtable.Clear();
        FadeManager.Instance.PLoadScene("DistributeArea", 0.5f);
        //PhotonNetwork.LoadLevel("DistributeArea");
    }
    [PunRPC]
    void RoadTimeRTC()
    {
        float elapsedTime = Mathf.Max(unchecked(PhotonNetwork.ServerTimestamp - timestamp) / 1000f);
        TimeLimited = TimeLimit - elapsedTime;
        timeLabel.text = TimeLimited.ToString("f2"); // 小数点以下2桁表示
    }

    void ThisDistributeArea()
    {
        if (TimeLimited > 0)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                myPV.RPC("RoadTimeRTC", RpcTarget.AllViaServer);
            }
        }
        else
        {
            if (SecondFlag == false)
            {
                this.gameObject.GetComponent<DistributeManager>().SecondTimingFlag = true;
                this.gameObject.GetComponent<DistributeManager>().ChoiceFlag = true;
                SecondFlag = true;
                if (PhotonNetwork.CurrentRoom.CustomProperties["UranaiDeathYouko"] is int UranaiDeathYouko)
                {
                    if (UranaiDeathYouko != 10)//初期値でないなら（誰か妖狐で占われたのなら
                    {
                        AliveSetting.GetComponent<AliveSettingScript>().AlivePlayerCount -= 1;
                        AliveSetting.GetComponent<AliveSettingScript>().AliveFlag[UranaiDeathYouko] = false;
                        AliveSetting.GetComponent<ResultChecker>().ResultChecking();
                        hashtable["UranaiDeathYouko"] = 10; //実施済みに変更
                        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
                        hashtable.Clear();
                    }
                }
                //    PhotonNetwork.LoadLevel("MainField");
                if (PhotonNetwork.IsMasterClient)
                {
                    myPV.RPC("RPCNextRoomRead", RpcTarget.AllViaServer);
                }
            }
        }
    }

    void ThisMainField()
    {
        if (TimeLimited > 0)
        {
            /*
            if (PhotonNetwork.IsMasterClient)
            {
                myPV.RPC("RoadTimeRTC", RpcTarget.AllViaServer);
            }
            */
            RoadTimeRTC();
        }
        else
        {
            GameObject GameManager = GameObject.Find("GameManager");
            GameManager.GetComponent<CGameManagerScript>().SecondPhaseFlag = true; 
            hashtable["CreateReadyTemp"] = false;
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
            hashtable.Clear();
            //Debug.LogWarning("ほげ"+PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }

    bool SyukeiFlag = false;
    async void ThisPicArea()
    {
        if (TimeLimited > 0)
        {
            RoadTimeRTC();
        }
        else
        {
            if (PicFlag == false)
            {

                GameObject PicManager = GameObject.Find("PicManager");
                PicManager.GetComponent<PicManager>().CanvasBack.transform.GetChild(1).gameObject.SetActive(false);
                PicManager.GetComponent<PicManager>().CanvasBack.transform.GetChild(2).gameObject.SetActive(false);
                PicManager.GetComponent<PicManager>().CanvasBack.transform.GetChild(3).gameObject.SetActive(false);
                PicFlag = true;
            }
            else
            {
                if (SyukeiFlag == false)
                {
                    GameObject PicManager = GameObject.Find("PicManager");
                    if (PhotonNetwork.IsMasterClient)
                    {
                        //PicManager.GetComponent<PicManager>().ExactingGrid();
                        myPV.RPC("PlayMasterExactGrid", RpcTarget.AllViaServer);   //test
                        SyukeiFlag = true; 
                        hashtable["SyukeiFlag"] = SyukeiFlag;
                        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
                        hashtable.Clear();
                    }
                    else
                    {
                        if (PhotonNetwork.CurrentRoom.CustomProperties["SyukeiFlag"] is bool tempSyukeiFlag)
                        {
                            SyukeiFlag = tempSyukeiFlag;
                        }
                    }
                }
                if (PicResultFlag == false)
                {
                    PicResultFlag = true;
                    for (int i = 15; i >= 0; i--)
                    {
                        await Task.Delay(1000);
                        timeLabel.text = i.ToString();
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        myPV.RPC("PicToDisRead", RpcTarget.AllViaServer); 
                    }

                }
            }
        }
    }
    void ResultArea()
    {
        if (TimeLimited > 0)
        {
            float elapsedTime = Mathf.Max(unchecked(PhotonNetwork.ServerTimestamp - timestamp) / 1000f);
            TimeLimited = TimeLimit - elapsedTime;
        }
        else
        {
            GameObject ResultManager = GameObject.Find("Manager");
            ResultManager.GetComponent<ResultManager>().CanvasObject.SetActive(true);
        }
    }

    [PunRPC]
    void PlayMasterExactGrid()
    {
        GameObject PicManager = GameObject.Find("PicManager");
        PicManager.GetComponent<PicManager>().ExactingGrid();
    }
}
