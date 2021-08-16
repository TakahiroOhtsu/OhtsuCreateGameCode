using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PushPlayerScript : MonoBehaviour
{
    private static Hashtable hashtable = new Hashtable();
    public int PushNum;
    GameObject Manager;
    GameObject AliveSetting;
    // Start is called before the first frame update
    void Start()
    {
        AliveSetting = GameObject.Find("AliveSetting");
        if (SceneManager.GetActiveScene().name == "DistributeArea")
        {
            Manager = GameObject.Find("DistributeManager");
        }
        else if (SceneManager.GetActiveScene().name == "PicArea")
        {
            Manager = GameObject.Find("PicManager");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pushSwitchPic()
    {
        PicManager PicManagerScript = Manager.GetComponent<PicManager>();
        int LocalNumMinus = PhotonNetwork.LocalPlayer.ActorNumber - 1; //投票するプレイヤーのやつ
        hashtable["PostPicNumber[" + LocalNumMinus + "]"] = PushNum; //リセット
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        hashtable.Clear();
        Debug.LogWarning("PostPicNumber[" + LocalNumMinus + "] = " + PushNum + "投票しました");

        //PicManagerScript.PicClickFlagOne = true;
        PicManagerScript.CanvasBack.transform.GetChild(1).gameObject.SetActive(false);
        PicManagerScript.CanvasBack.transform.GetChild(2).gameObject.SetActive(false);
        PicManagerScript.CanvasBack.transform.GetChild(3).gameObject.SetActive(true);

    }

    public void PushSwitch()
    {
        DistributeManager DistributeManagerScript = Manager.GetComponent<DistributeManager>();
        
        DistributeManagerScript.PicPlayerNum = PushNum;
        if (DistributeManagerScript.ChoiceFlag == false)
        {
            Debug.LogWarning("test0");
            DistributeManagerScript.ChoiceFlag = true;
            this.gameObject.transform.parent.gameObject.SetActive(false);
            if (AliveSetting.GetComponent<AliveSettingScript>().Post_Distribution[PhotonNetwork.LocalPlayer.ActorNumber - 1] == "神官")
            {
                DistributeManagerScript.Shinkan.SetActive(true);
                DistributeManagerScript.Shinkan.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>().text = PhotonNetwork.PlayerList[PushNum].NickName;
                DistributeManagerScript.Shinkan.transform.GetChild(1).gameObject.SetActive(true); 
                hashtable["ShinkanSupportNum"] = PushNum;
                hashtable["SinkanEffectFlag"] = true;
                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
                hashtable.Clear();

            }
        }
        else//2Phaneめ
        {
            if (DistributeManagerScript.SetJobUI == false)　//名乗るジョブ選択
            {
                Debug.LogWarning("test1");
                DistributeManagerScript.SetJobUI = true;
            }
            else //占い師、霊媒師で使用
            {
                if (DistributeManagerScript.SetSecondPicUI == false)
                {
                    Debug.LogWarning("test2");
                    DistributeManagerScript.setName = this.gameObject.transform.GetChild(0).GetComponent<Text>().text;
                    Debug.LogWarning("PIC"+ DistributeManagerScript.PicPlayerNum);
                    if (DistributeManagerScript.SetJobString == "占い師")
                    {
                        DistributeManagerScript.PicPlayerNum = 0;
                    }
                    else if (DistributeManagerScript.SetJobString == "霊媒師")
                    {
                        DistributeManagerScript.PicPlayerNum = 1;
                    }

                    this.gameObject.transform.parent.gameObject.SetActive(false);
                    DistributeManagerScript.SetSecondPicUI = true;
                }
                else
                {
                    Debug.LogWarning("test3");
                    DistributeManagerScript.PicPlayerNum = 9; //枠切り替え用
                    DistributeManagerScript.lastPicNum = PushNum;

                    this.gameObject.transform.parent.gameObject.SetActive(false);
                }
            }
            this.gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
