using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

using Hashtable = ExitGames.Client.Photon.Hashtable;
public class AliveSettingScript : MonoBehaviour
{
    public int AlivePlayerCount; //生き残っているプレイヤー人数
    public bool[] AliveFlag = new bool[7];//7人の生存フラグ
    public string[] List_Distribution = new string[9]; //役職のリスト
    public string[] Post_Distribution = new string[7];　//配布された役職　プレイヤーに与えたものを記録する。
    //public string[] Pic_Distribution = new string[7];　//主張している役職
    public int EscapeCounter = 0;
    public PhotonView myPV;

    private static Hashtable hashtable = new Hashtable();
    bool EscapeResetFlag = false;
    public bool EscapePlusFlag = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainField")
        {
            if (AlivePlayerCount == EscapeCounter)
            {
                //投票シーンのロード
                EscapeResetFlag = false;
            }
            if (!EscapePlusFlag)
            {
                EscapeCounter++;
                EscapePlusFlag = true;
                if (EscapeCounter >= AlivePlayerCount - 1)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    Camera.main.gameObject.GetComponent<CursorController>().enabled = false;
                    FadeManager.Instance.PLoadScene("PicArea", 0.2f);
                    //myPV.RPC("RoadPicArea", RpcTarget.AllViaServer);
                    //PhotonNetwork.LoadLevel("PicArea");
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == "PicArea")//投票シーンなら
        {
            if (EscapeResetFlag == false)
            {
                EscapeCounter = 0;
                EscapeResetFlag = true;
            }
        }
        else if (SceneManager.GetActiveScene().name == "RoomSetting")//投票シーンなら
        {
            Destroy(gameObject);
        }
    }

    void Input_DistributionName()
    {
        List_Distribution[0] = "占い師";
        List_Distribution[1] = "霊媒師";
        List_Distribution[2] = "騎士";
        List_Distribution[3] = "共有者";
        List_Distribution[4] = "神官";
        List_Distribution[5] = "村人";
        List_Distribution[6] = "妖狐";
        List_Distribution[7] = "狂人";
        List_Distribution[8] = "人狼";
    }

    static public AliveSettingScript instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartSetting()
    {
        for (int i = 0; i < 7; i++)
        {
            if (i >= AlivePlayerCount) //6人以下の場合、参加していないプレイヤー枠をはじく
            {
                AliveFlag[i] = false;
            }
            else
            {
                AliveFlag[i] = true;
            }
        }
        Input_DistributionName();
    }

    [PunRPC]
    void RoadPicArea()
    {
        FadeManager.Instance.PLoadScene("PicArea", 0.5f);
    }
}
