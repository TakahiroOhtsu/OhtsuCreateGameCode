using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;

public class SwitchController : MonoBehaviour
{
    private static Hashtable hashtable = new Hashtable();
    public GameObject[] Switchs = new GameObject[10];
    public GameObject[] JudgeDai = new GameObject[10];
    int[] ChoiceNum = new int[7]; //0~9:設定値 10:未設定
    int[] ChoiceNum2 = new int[7]; //0~9:設定値 10:未設定
    GameObject AliveSetting = null;
    public PhotonView myPV;
    int PlayerCount;
    bool OtherPlayerCreateSwitchFlag = false;
    bool MultiSetFlag = false;
    bool CreateReadyFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MatchingRoom")
        {
            PlayerCount = PhotonNetwork.CurrentRoom.MaxPlayers;
        }
        else
        {
            AliveSetting = GameObject.Find("AliveSetting");
            PlayerCount = AliveSetting.GetComponent<AliveSettingScript>().AlivePlayerCount;
        }
        //if (SceneManager.GetActiveScene().name == "MatchingRoom") {
        if (PhotonNetwork.IsMasterClient)
            {
                OtherPlayerCreateSwitchFlag = true;
                MultiSetFlag = true;
                AliveSetting = GameObject.Find("AliveSetting");
                CreateSwitch();
                CreateJudgeDai();
                ConnectSwitch2JudgeDai();
                hashtable["CreateReadyTemp"] = true; 
                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
                hashtable.Clear();
        }
        //}
    }

    // Update is called once per frame
    void Update()
    {

        //if (SceneManager.GetActiveScene().name == "MainField")
        //{
        //if (MultiSetFlag == false)
        //{
        if (!CreateReadyFlag)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties["CreateReadyTemp"] is bool CreateReadyTemp)
            {
                CreateReadyFlag = CreateReadyTemp;
            }
        }
        else
        {
            UpdateCreateSwitchs();
        }
        //}
        //}
    }

    void CreateSwitch()
    {
        int LoopCount = PlayerCount - 1;
        for (int i = 0; i < 10; i++)
        {
            Switchs[i].SetActive(false);
            if (i < 7)
            {
                ChoiceNum[i] = 10;
            }
            
        }
        int Keisoku = 0;
        do
        {
            int RandomRange = UnityEngine.Random.Range((int) 0, (int) 9);
            bool CheckFlag = false;

            for (int i = 0; i < 7; i++) //ダブってないよね核人
            {
                if (ChoiceNum[i] != 10)
                {
                    if (ChoiceNum[i] == RandomRange)
                    {
                        CheckFlag = true;
                    }
                }
            }
            if (CheckFlag == false)
            {
                ChoiceNum[LoopCount] = RandomRange;
                LoopCount--;
            }
            Keisoku++;
        }while (ChoiceNum[0] == 10);

        Debug.LogWarning(ChoiceNum[0]);

        
        for (int i = 0; i < 7; i++)
        {
            if (ChoiceNum[i] != 10)
            {
                Switchs[ChoiceNum[i]].SetActive(true);
            }
        }

        for (int i = 0; i < 10; i++)
        {
            if (Switchs[i].activeSelf == false)
            {
                Destroy(Switchs[i]);
            }
        }
    }
    void CreateJudgeDai()
    {
        int LoopCount = PlayerCount - 1;

        for (int i = 0; i < 10; i++)
        {
            JudgeDai[i].SetActive(false);
            if (i < 7)
            {
                ChoiceNum2[i] = 10;
            }
        }

        do
        {
            int RandomRange = UnityEngine.Random.Range((int)0, (int)9);
            bool CheckFlag = false;

            for (int i = 0; i < 7; i++)
            {
                if (ChoiceNum2[i] != 10)
                {
                    if (ChoiceNum2[i] == RandomRange)
                    {
                        CheckFlag = true;
                    }
                }
            }
            if (CheckFlag == false)
            {
                ChoiceNum2[LoopCount] = RandomRange;
                LoopCount--;
            }
        }
        while (ChoiceNum2[0] == 10);

        
        for (int i = 0; i < 7; i++)
        {
            if (ChoiceNum2[i] != 10)
            {
                JudgeDai[ChoiceNum2[i]].SetActive(true);
            }
        }

        for (int i = 0; i < 10; i++)
        {
            if (JudgeDai[i].activeSelf == false)
            {
                Destroy(JudgeDai[i]);
            }
        }

    }
    void ConnectSwitch2JudgeDai()
    {
        int Counter = PlayerCount - 1;//0基準にするため
        
        for (int i = 0; i <= Counter; i++ )
        {
            Switchs[ChoiceNum[i]].GetComponent<SwitchScript>().CreateJudgedai = JudgeDai[ChoiceNum2[i]];
            hashtable["ChoiceNum[" + i + "]"] = ChoiceNum[i];
            hashtable["ChoiceNum2[" + i + "]"] = ChoiceNum2[i];
        }
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        hashtable.Clear();
    }

    void UpdateCreateSwitchs()
    {
        //Debug.LogWarning("OtherPlayerCreateSwitchFlag:" + OtherPlayerCreateSwitchFlag);
        if (OtherPlayerCreateSwitchFlag == false) 
        {
            Debug.LogWarning("ぬ:");
            bool GetSwitchDataFlag = GetSwitchObject();
            Debug.LogWarning("GetSwitchDataFlag:" +GetSwitchDataFlag);

            if (GetSwitchDataFlag)
            {
                for (int i = 0; i < 10; i++)
                {
                    Switchs[i].SetActive(false);
                    JudgeDai[i].SetActive(false);
                }
                for (int i = 0; i < PlayerCount; i++)
                {
                    if (ChoiceNum[i] != 10)
                    {
                        Switchs[ChoiceNum[i]].SetActive(true);
                    }
                    if (ChoiceNum2[i] != 10)
                    {
                        JudgeDai[ChoiceNum2[i]].SetActive(true);
                    }
                    Switchs[ChoiceNum[i]].GetComponent<SwitchScript>().CreateJudgedai = JudgeDai[ChoiceNum2[i]];
                }

                for (int i = 0; i < 10; i++)
                {
                    if (Switchs[i].activeSelf == false)
                    {
                        Destroy(Switchs[i]);
                    }
                    if (JudgeDai[i].activeSelf == false)
                    {
                        Destroy(JudgeDai[i]);
                    }
                }
                OtherPlayerCreateSwitchFlag = true;
            }
        }
    }
    bool GetSwitchObject()
    {
        int Counter = PlayerCount - 1;//0基準にするため
        bool GetSwitchObjectFlag = true;

        for (int i = 0; i <= Counter; i++)
        {
            Debug.LogWarning("ぬめりあん");
            if (GetSwitchObjectFlag)
            {
                Debug.LogWarning("ぬ");
                if (PhotonNetwork.CurrentRoom.CustomProperties["ChoiceNum[" + i + "]"] is int RandomNum)
                {
                    Debug.LogWarning("RandomNum:" + RandomNum);
                    ChoiceNum[i] = RandomNum;
                    //GetSwitchObjectFlag = true;
                    Debug.LogWarning("ぬこ");
                }
                else
                {
                    GetSwitchObjectFlag = false;
                    Debug.LogWarning("ぬこぬ");
                }
                if (PhotonNetwork.CurrentRoom.CustomProperties["ChoiceNum2[" + i + "]"] is int RandomNum2)
                {
                    Debug.LogWarning("RandomNum2:" + RandomNum2);
                    ChoiceNum2[i] = RandomNum2;
                    //GetSwitchObjectFlag = true;
                    Debug.LogWarning("ぬこぬこ");
                }
                else
                {
                    GetSwitchObjectFlag = false;
                    Debug.LogWarning("ぬこぬこぬ");
                }
            }
        }

        return GetSwitchObjectFlag;
    }

}
