using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PicManager : MonoBehaviour
{
    public GameObject CanvasBack;
    GameObject AliveSetting;
    public int[] PicNumber = new int[7];　//10:未投票　1~7まで 7人分
    public bool TyouhukuFlag = false;
    public AudioClip BGM;
    AudioSource audioSource;
    public Sprite[] JobIcons = new Sprite[9];

    private static Hashtable hashtable = new Hashtable();

    // Start is called before the first frame update
    void Start()
    {
        AliveSetting = GameObject.Find("AliveSetting"); 
        audioSource = AliveSetting.GetComponent<AudioSource>();
        audioSource.clip = BGM;
        audioSource.Play();
        IgniteBord();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (PicClickFlagOne)
        {
            CanvasBack.transform.GetChild(1).gameObject.SetActive(false);
            CanvasBack.transform.GetChild(2).gameObject.SetActive(false);
            CanvasBack.transform.GetChild(3).gameObject.SetActive(true);
            if (PicClickFlagTwo)
            {
                CanvasBack.transform.GetChild(3).gameObject.SetActive(false);
                CanvasBack.transform.GetChild(4).gameObject.SetActive(true);
            }
        }*/
    }

    void IgniteBord()
    {
        AliveSetting.GetComponent<AliveSettingScript>().EscapeCounter = 0; //メインフィールドで使ったEscapeCounterの初期化

        if (AliveSetting.GetComponent<AliveSettingScript>().AliveFlag[PhotonNetwork.LocalPlayer.ActorNumber - 1])
        {
            //役職名入れ
            GameObject TempUnit = CanvasBack.transform.GetChild(0).gameObject;
            TempUnit.transform.GetChild(0).gameObject.GetComponent<Text>().text = AliveSetting.GetComponent<AliveSettingScript>().Post_Distribution[PhotonNetwork.LocalPlayer.ActorNumber - 1];
            switch (AliveSetting.GetComponent<AliveSettingScript>().Post_Distribution[PhotonNetwork.LocalPlayer.ActorNumber - 1])
            {
                case "占い師":
                    TempUnit.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = JobIcons[0];
                    break;
                case "霊媒師":
                    TempUnit.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = JobIcons[1];
                    break;
                case "騎士":
                    TempUnit.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = JobIcons[2];
                    break;
                case "共有者":
                    TempUnit.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = JobIcons[3];
                    break;
                case "神官":
                    TempUnit.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = JobIcons[4];
                    break;
                case "村人":
                    TempUnit.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = JobIcons[5];
                    break;
                case "妖狐":
                    TempUnit.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = JobIcons[6];
                    break;
                case "狂人":
                    TempUnit.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = JobIcons[7];
                    break;
                case "人狼":
                    TempUnit.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = JobIcons[8];
                    break;
            }

            GameObject TargetObj;
            int LoopNum;
            if (PhotonNetwork.CurrentRoom.MaxPlayers <= 4)
            {
                TargetObj = CanvasBack.transform.GetChild(1).gameObject;
                LoopNum = 4;
            }
            else
            {
                TargetObj = CanvasBack.transform.GetChild(2).gameObject;
                LoopNum = 7;
            }
            TargetObj.SetActive(true);
            for (int i = 0; i < LoopNum; i++)
            {
                if (i >= PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    TargetObj.transform.GetChild(i).gameObject.SetActive(false);
                }
                else
                {
                    //プレイヤー名を入れるとこ
                    TargetObj.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName;
                    if (AliveSetting.GetComponent<AliveSettingScript>().AliveFlag[i] == false)
                    {
                        TargetObj.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            CanvasBack.transform.GetChild(6).gameObject.SetActive(true);
        }
    }

    public void ExactingGrid() //投票一定時間後の集計
    {
        int[] PicCount = new int[7];
        int TopCount = 0;
        int TopUser = 0;
        for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)//投票データを整理
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties["PostPicNumber[" + i + "]"] is int TempPicNumber)
            {
                PicNumber[i] = TempPicNumber;
                Debug.LogWarning("PicNumver["+ i + "] = " + TempPicNumber);
            }
            else
            {
                PicNumber[i] = 10;//未投票
                Debug.LogWarning("未投票がいた");
            }
        }

        for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++) //集計
        {
            if (PicNumber[i] != 10) //未投票でないなら
            {
                PicCount[PicNumber[i]]++;
            }
        }

        for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++) //順位付け
        {
            if (TopCount < PicCount[i])
            {
                TopCount = PicCount[i];
                TopUser = i;
            }
        }

        int DoutyakuCount = 0;
        for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++) //同着のTopはいないか確認
        {
            if (TopCount == PicCount[i])
            {
                DoutyakuCount++;
                if (DoutyakuCount >= 2) //自身も判定するため2つ以上出たら重複
                {
                    Debug.LogWarning("同着数："+ DoutyakuCount);
                    TyouhukuFlag = true;
                }
            }
        }

        GameSetEnter(TopCount,TopUser);
        int LocalNumMinus = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        hashtable["PostPicNumber[" + LocalNumMinus + "]"] = 10; //リセット
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        hashtable.Clear();
        if (TopUser >= 0)
        {
            CanvasBack.transform.GetChild(4).gameObject.transform.GetChild(0).GetComponent<Text>().text = PhotonNetwork.PlayerList[TopUser].NickName;
        }
    }

    void GameSetEnter(int TopCount,int TopUser)
    {
        Debug.LogWarning("GameSet");
        bool PeaceFlag = true;
        bool KillerAliveFlag = false;
        for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
        {
            if (AliveSetting.GetComponent<AliveSettingScript>().Post_Distribution[i] == "人狼")
            {
                KillerAliveFlag = true;
            }
        }
        if (TopCount == 1) //最大の投票数が1の場合
        {
            Debug.LogWarning("1");
            for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
            {
                if (PicNumber[i] == 10 && AliveSetting.GetComponent<AliveSettingScript>().AliveFlag[i] == true) //生きているのに未投票がいる
                {
                    CanvasBack.transform.GetChild(5).gameObject.SetActive(true); //平和村の表示
                    PeaceFlag = false;
                }
            }
            if (PeaceFlag)
            {
                Debug.LogWarning("2");
                if (KillerAliveFlag == false)
                {
                    for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
                    {
                        if (AliveSetting.GetComponent<AliveSettingScript>().Post_Distribution[i] == "妖狐")
                        {
                            PhotonNetwork.LeaveRoom();
                            PhotonNetwork.Disconnect();
                            FadeManager.Instance.LoadScene("YoukoWin", 0.5f);
                            //PhotonNetwork.LoadLevel("YoukoWin"); //妖狐が混じってれば妖狐の勝利
                        }
                    }
                    Debug.LogWarning("3");
                    //ゲームクリア画面(生存者側)に遷移
                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.Disconnect();
                    FadeManager.Instance.LoadScene("MurabitoWin", 0.5f);
                    //PhotonNetwork.LoadLevel("MurabitoWin");
                }
                else
                {
                    Debug.LogWarning("4");
                    ShingekiShinkan();
                    CanvasBack.transform.GetChild(5).gameObject.SetActive(true); //平和村の表示
                }
            }
        }
        else
        {
            Debug.LogWarning("5");
            if (!TyouhukuFlag)
            {
                Debug.LogWarning("6");
                AliveSetting.GetComponent<AliveSettingScript>().AliveFlag[TopUser] = false; //釣り処理
                AliveSetting.GetComponent<AliveSettingScript>().AlivePlayerCount -= 1; //釣り処理

                /*if (AliveSetting.GetComponent<AliveSettingScript>().AlivePlayerCount == 2)
                {
                    int AliveOne = 10;//誰にも当てはまらない初期値
                    int AliveTwo = 10;//誰にも当てはまらない初期値
                    string CatOne = "default"; 
                    string CatTwo = "default";
                    for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
                    {
                        if (AliveSetting.GetComponent<AliveSettingScript>().AliveFlag[i] == true)
                        {
                            if (AliveOne != 10)
                            {
                                AliveTwo = i;
                                CatTwo = AliveSetting.GetComponent<AliveSettingScript>().Post_Distribution[i];
                            }
                            else
                            {
                                AliveOne = i;
                                CatOne = AliveSetting.GetComponent<AliveSettingScript>().Post_Distribution[i];
                            }
                        }
                    }
                    if (CatOne == "妖狐" || CatTwo == "妖狐")
                    {
                        PhotonNetwork.LoadLevel("YoukoWin"); //妖狐勝利
                    }
                    else
                    {
                        PhotonNetwork.LoadLevel("KillerWin"); //人狼勝利
                    }
                }*/
                AliveSetting.GetComponent<ResultChecker>().ResultChecking();
                CanvasBack.transform.GetChild(4).gameObject.SetActive(true); //吊った情報表示
                CanvasBack.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = PhotonNetwork.PlayerList[TopUser].NickName; //吊った情報表示
                ShingekiShinkan();
            }
            else
            {
                Debug.LogWarning("7");
                ShingekiShinkan();
                CanvasBack.transform.GetChild(5).gameObject.SetActive(true); //平和村の表示
            }
        }
    }

    void ShingekiShinkan()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties["ShinkanSupportNum"] is int TempNum)
        {
            if (TempNum != 10) //未実施の場合
            {
                if (AliveSetting.GetComponent<AliveSettingScript>().AliveFlag[TempNum] == false)
                {
                    AliveSetting.GetComponent<AliveSettingScript>().AliveFlag[TempNum] = true;
                    AliveSetting.GetComponent<AliveSettingScript>().AlivePlayerCount += 1;
                    hashtable["ShinkanSupportNum"] = 10; //実施済みに変更
                    PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
                    hashtable.Clear();
                }
            }
        }
    }

}
