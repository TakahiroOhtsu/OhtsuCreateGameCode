using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class DistributeManager : MonoBehaviour
{
    GameObject AliveSetting;
    AliveSettingScript AliveSettingScript;
    bool FirstReceiveFlag = false;
    public GameObject StartText;
    public GameObject TemprateUI;
    public GameObject EffectPanel4;
    public GameObject EffectPanel5;
    public GameObject Uranai_Rebaiabi;
    public GameObject Knight;
    public GameObject kyouyuusya;
    public GameObject Shinkan;
    public GameObject murabito;
    public GameObject Kyoujin;
    public GameObject Youko;
    public GameObject jinro;
    public GameObject Reject;
    public bool ChoiceFlag = false;
    public int PicPlayerNum = 10;
    public bool SecondTimingFlag = false;
    bool firstUIDisableFlag = false;
    public bool SetJobUI = false;
    public bool SetSecondPicUI = false;
    public int lastPicNum = 0;
    public string setName = "PlayerNameHoge";
    bool CreateSecondChoice = false;
    public string SetJobString;
    public Sprite[] JobIcons = new Sprite[9];
    public AudioClip BGM;
    AudioSource audioSource;
    private static Hashtable hashtable = new Hashtable();

    // Start is called before the first frame update
    void Start()
    {
        Thread.Sleep(1000);
        AliveSetting = GameObject.Find("AliveSetting");
        AliveSettingScript = AliveSetting.GetComponent<AliveSettingScript>();
        audioSource = AliveSetting.GetComponent<AudioSource>();
        if (audioSource.clip != BGM || audioSource.clip == null)
        {
            audioSource.clip = BGM;
            audioSource.Play();
        }
        if (PhotonNetwork.IsMasterClient && AliveSettingScript.Post_Distribution[0] == "") //空ならまだ入れていないので
        {
            Post_Disrributed();
        }
        SettingPlayerListUI();

    }

    // Update is called once per frame
    void Update()
    {
        if (SecondTimingFlag == false)
        {
            Receive_Distribute();
        }
        else
        {
            //SecondUIPlan();
        }
    }

    void Post_Disrributed()
    {
        bool jinroFlag = false;
        int jinroNum = 10;
        bool KyoujinFlag = false;
        for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)//配布用ループ
        {
            AliveSettingScript.Post_Distribution[i] = Distribute_Post(AliveSettingScript.Post_Distribution, AliveSettingScript.List_Distribution);
            if (AliveSettingScript.Post_Distribution[i] == "人狼")
            {
                jinroFlag = true;
                jinroNum = i;
            }
        }
        if (jinroFlag == true && PhotonNetwork.CurrentRoom.MaxPlayers >= 5)//人狼がいて、人狼サイドと村人サイドが1：1にならないように。
        {
            for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)  //人狼がいるなら狂人を配ろうの会
            {
                if (KyoujinFlag == false)
                {
                    if (i != jinroNum)
                    {
                        int RandomRange = UnityEngine.Random.Range((int)0, (int)8); //再抽選なので確率的に人狼を抜いたまんま
                        if (RandomRange == 0) //1/7の確立で抽選に当たったなら
                        {
                            AliveSettingScript.Post_Distribution[i] = "狂人";
                            KyoujinFlag = true;
                        }
                    }
                }
            }
        }
        for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++) //登録用ループ
        {
            Debug.LogWarning("プレイヤー："+ i + 1 +"/役職："+ AliveSettingScript.Post_Distribution[i]); 
            hashtable["Post_Distribution[" + i + "]"] = AliveSettingScript.Post_Distribution[i];
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        hashtable.Clear();
    }
    string Distribute_Post(string[] Distributed_Post,string[] Dsitribute_List)
    {
        bool ReadyFlag = false;
        string returnString = "start";
        bool TogatherDoubleCheckFlag = false;

        do
        {
            int RandomRange = UnityEngine.Random.Range((int)0, (int)9); //役職数
            bool SettingFlag = false;

            for (int i = 0; i < 7; i++)
            {
                if (Distributed_Post[i] != null)
                {
                    if (Distributed_Post[i] == Dsitribute_List[RandomRange])
                    {
                        if (TogatherDoubleCheckFlag == false && Distributed_Post[i] == "共有者")
                        {
                            TogatherDoubleCheckFlag = true;
                        }
                        else
                        {
                            SettingFlag = true;
                        }
                    }
                }
            }

            if (SettingFlag == false)
            {
                ReadyFlag = true;
                if (Dsitribute_List[RandomRange] == "狂人")
                {
                    returnString = "村人";
                }
                else
                {
                    returnString = Dsitribute_List[RandomRange];
                }
            }
        }
        while(ReadyFlag == false);

        return returnString;
    }

    void Receive_Distribute()
    {
        if (FirstReceiveFlag == false) {
            bool ReceiveFlag = true;

            //Thread.Sleep(1000);
            for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
            {
                if (PhotonNetwork.CurrentRoom.CustomProperties["Post_Distribution[" + i + "]"] is string TempDistribute)
                {
                    if (TempDistribute != "null")
                    {
                        AliveSettingScript.Post_Distribution[i] = TempDistribute;
                    }
                    else
                    {
                        ReceiveFlag = false;
                    }
                }
                else
                {
                    ReceiveFlag = false;
                }
            }
            
            if (ReceiveFlag)
            {
                SetDisUI();
            }
        }
    }

    void SetDisUI()
    {
        Photon.Realtime.Player player = PhotonNetwork.LocalPlayer;
        TemprateUI.transform.GetChild(0).gameObject.GetComponent<Text>().text = AliveSettingScript.Post_Distribution[player.ActorNumber - 1]; //-1は補正
        
        TemprateUI.SetActive(true);
        StartText.SetActive(false);

        switch (AliveSettingScript.Post_Distribution[player.ActorNumber - 1]) //-1は補正
        {
            case "占い師":
                TemprateUI.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = JobIcons[0];
                if (ChoiceFlag == false)
                {
                    if (PhotonNetwork.CurrentRoom.MaxPlayers < 5)
                    {
                        EffectPanel4.SetActive(true);
                    }
                    else if (PhotonNetwork.CurrentRoom.MaxPlayers >= 5)
                    {
                        EffectPanel5.SetActive(true);
                    }
                }
                else
                {
                    Uranai_Rebaiabi.SetActive(true);
                    Uranai_Rebaiabi.transform.GetChild(3).gameObject.GetComponent<Text>().text = PhotonNetwork.PlayerList[PicPlayerNum].NickName;
                    switch (AliveSettingScript.Post_Distribution[PicPlayerNum])
                    {
                        case "占い師":
                            Uranai_Rebaiabi.transform.GetChild(2).gameObject.GetComponent<Text>().text = "村人側";
                            break;
                        case "霊媒師":
                            Uranai_Rebaiabi.transform.GetChild(2).gameObject.GetComponent<Text>().text = "村人側";
                            break;
                        case "騎士":
                            Uranai_Rebaiabi.transform.GetChild(2).gameObject.GetComponent<Text>().text = "村人側";
                            break;
                        case "共有者":
                            Uranai_Rebaiabi.transform.GetChild(2).gameObject.GetComponent<Text>().text = "村人側";
                            break;
                        case "神官":
                            Uranai_Rebaiabi.transform.GetChild(2).gameObject.GetComponent<Text>().text = "村人側";
                            break;
                        case "村人":
                            Uranai_Rebaiabi.transform.GetChild(2).gameObject.GetComponent<Text>().text = "村人側";
                            break;
                        case "妖狐":
                            Uranai_Rebaiabi.transform.GetChild(2).gameObject.GetComponent<Text>().text = "村人側";
                            hashtable["UranaiDeathYouko"] = PicPlayerNum; //実施済みに変更
                            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
                            hashtable.Clear();
                            break;
                        case "狂人":
                            Uranai_Rebaiabi.transform.GetChild(2).gameObject.GetComponent<Text>().text = "村人側";
                            break;
                        case "人狼":
                            Uranai_Rebaiabi.transform.GetChild(2).gameObject.GetComponent<Text>().text = "人狼";
                            break;
                    }
                    PicPlayerNum = 10;
                    FirstReceiveFlag = true;
                }
                break;
            case "霊媒師":
                TemprateUI.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = JobIcons[1];
                if (ChoiceFlag == false)
                {
                    if (AliveSetting.GetComponent<AliveSettingScript>().AlivePlayerCount != PhotonNetwork.CurrentRoom.MaxPlayers)
                    {

                        if (PhotonNetwork.CurrentRoom.MaxPlayers < 5)
                        {
                            EffectPanel4.SetActive(true);
                        }
                        else if (PhotonNetwork.CurrentRoom.MaxPlayers >= 5)
                        {
                            EffectPanel5.SetActive(true);
                        }
                    }
                }
                else
                {
                    Uranai_Rebaiabi.SetActive(true);
                    Uranai_Rebaiabi.transform.GetChild(2).gameObject.GetComponent<Text>().text = AliveSettingScript.Post_Distribution[PicPlayerNum];
                    PicPlayerNum = 10;
                    FirstReceiveFlag = true;
                }
                break;
            case "騎士":
                TemprateUI.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = JobIcons[2];
                Knight.SetActive(true);
                FirstReceiveFlag = true;
                break;
            case "共有者":
                TemprateUI.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = JobIcons[3];
                bool CheckFlag = false;
                string name = "DefaultName";

                for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
                {
                    if (i != player.ActorNumber - 1)
                    {
                        if (AliveSettingScript.Post_Distribution[i] == "共有者")
                        {
                            Debug.LogWarning("test"+ i);
                            name = PhotonNetwork.PlayerList[i].NickName;
                            CheckFlag = true;
                        }
                    }
                }
                if (CheckFlag)
                {
                    kyouyuusya.transform.GetChild(0).gameObject.SetActive(false);
                    kyouyuusya.transform.GetChild(1).gameObject.SetActive(true);
                    kyouyuusya.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = name + "です";
                }
                kyouyuusya.SetActive(true);
                FirstReceiveFlag = true;
                break;
            case "神官":
                TemprateUI.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = JobIcons[4];
                if (PhotonNetwork.CurrentRoom.CustomProperties["SinkanEffectFlag"] is bool TempShinkanFlag)
                {
                    if (TempShinkanFlag == false)
                    {
                        Shinkan.transform.GetChild(0).gameObject.SetActive(false);
                        Shinkan.transform.GetChild(1).gameObject.SetActive(true);
                        Shinkan.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = PhotonNetwork.PlayerList[PicPlayerNum].NickName;
                    }
                    else
                    {
                        Shinkan.transform.GetChild(0).gameObject.SetActive(true);
                        Shinkan.transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
                else if (ChoiceFlag == false)
                {
                    if (PhotonNetwork.CurrentRoom.MaxPlayers < 5)
                    {
                        EffectPanel4.SetActive(true);
                    }
                    else if (PhotonNetwork.CurrentRoom.MaxPlayers >= 5)
                    {
                        EffectPanel5.SetActive(true);
                    }
                }
                else
                {
                    if (PhotonNetwork.CurrentRoom.CustomProperties["SinkanEffectFlag"] is bool TempFlag)
                    {
                        /*if (TempFlag == false)
                        {
                            Shinkan.transform.GetChild(0).gameObject.SetActive(false);
                            Shinkan.transform.GetChild(1).gameObject.SetActive(true);
                            Shinkan.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = PhotonNetwork.PlayerList[PicPlayerNum].NickName;
                        }
                        else
                        {
                            Shinkan.transform.GetChild(0).gameObject.SetActive(true);
                            Shinkan.transform.GetChild(1).gameObject.SetActive(false);
                        }*/
                    }
                    else
                    {
                        hashtable["ShinkanSupportNum"] = PicPlayerNum;
                        hashtable["SinkanEffectFlag"] = true;
                        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
                        hashtable.Clear();
                    }
                }
                Shinkan.SetActive(true);
                FirstReceiveFlag = true;
                break;
            case "村人":
                TemprateUI.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = JobIcons[5];
                murabito.SetActive(true);
                FirstReceiveFlag = true;
                break;
            case "妖狐":
                TemprateUI.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = JobIcons[7];
                Youko.SetActive(true);
                FirstReceiveFlag = true;
                break;
            case "狂人":
                TemprateUI.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = JobIcons[6];
                Kyoujin.SetActive(true);
                FirstReceiveFlag = true;
                break;
            case "人狼":
                TemprateUI.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = JobIcons[8];
                jinro.SetActive(true);
                if (PhotonNetwork.CurrentRoom.MaxPlayers < 5)
                {
                    GameObject ForObject = jinro.transform.GetChild(1).gameObject;
                    for (int i = 0; i<4; i++)
                    {
                        if (i < PhotonNetwork.CurrentRoom.MaxPlayers)
                        {
                            GameObject NowObject = ForObject.transform.GetChild(i).gameObject;
                            NowObject.GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName;
                            //NowObject.transform.GetChild(0).gameObject
                            Sprite SetSprite;
                            switch(AliveSettingScript.Post_Distribution[i])
                            {
                                case "占い師":
                                    SetSprite = JobIcons[0];
                                    break;
                                case "霊媒師":
                                    SetSprite = JobIcons[1];
                                    break;
                                case "騎士":
                                    SetSprite = JobIcons[2];
                                    break;
                                case "共有者":
                                    SetSprite = JobIcons[3];
                                    break;
                                case "神官":
                                    SetSprite = JobIcons[4];
                                    break;
                                case "村人":
                                    SetSprite = JobIcons[5];
                                    break;
                                case "狂人":
                                    SetSprite = JobIcons[6];
                                    break;
                                case "妖狐":
                                    SetSprite = JobIcons[7];
                                    break;
                                default:
                                    SetSprite = JobIcons[8];
                                    break;
                            }
                            NowObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = SetSprite; 
                        }
                        else
                        {
                            ForObject.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
                else if (PhotonNetwork.CurrentRoom.MaxPlayers >= 5)
                {
                    jinro.transform.GetChild(1).gameObject.SetActive(false);
                    GameObject FiveObject = jinro.transform.GetChild(2).gameObject;
                    FiveObject.SetActive(true);
                    for (int i = 0; i < 7; i++)
                    {
                        if (i < PhotonNetwork.CurrentRoom.MaxPlayers)
                        {
                            GameObject NowObject = FiveObject.transform.GetChild(i).gameObject;
                            NowObject.GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName;
                            Sprite SetSprite;
                            switch (AliveSettingScript.Post_Distribution[i])
                            {
                                case "占い師":
                                    SetSprite = JobIcons[0];
                                    break;
                                case "霊媒師":
                                    SetSprite = JobIcons[1];
                                    break;
                                case "騎士":
                                    SetSprite = JobIcons[2];
                                    break;
                                case "共有者":
                                    SetSprite = JobIcons[3];
                                    break;
                                case "神官":
                                    SetSprite = JobIcons[4];
                                    break;
                                case "村人":
                                    SetSprite = JobIcons[5];
                                    break;
                                case "妖狐":
                                    SetSprite = JobIcons[6];
                                    break;
                                case "狂人":
                                    SetSprite = JobIcons[7];
                                    break;
                                default:
                                    SetSprite = JobIcons[8];
                                    break;
                            }
                            NowObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = SetSprite;
                        }
                        else
                        {
                            FiveObject.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
                FirstReceiveFlag = true;
                break;
        }
        if (!AliveSetting.GetComponent<AliveSettingScript>().AliveFlag[PhotonNetwork.LocalPlayer.ActorNumber - 1])
        {
            EffectPanel4.SetActive(false);
            EffectPanel5.SetActive(false);
            Uranai_Rebaiabi.transform.parent.gameObject.SetActive(false);
            Reject.SetActive(true);
        }
    }
    /*
    void SecondUIPlan()
    {
        if (firstUIDisableFlag)
        {
            if (SetJobUI)
            {
                //Debug.LogWarning("test4");
                GameObject JobPanel = PanelFor2.transform.GetChild(3).gameObject;
                switch (PicPlayerNum)
                {
                    case 0: //占い師
                        SetJobString = "占い師";
                        if (SetSecondPicUI)
                        {
                            Debug.LogWarning("test5");
                            PanelFor2.transform.GetChild(1).gameObject.SetActive(true);
                            PanelFor2.transform.GetChild(4).gameObject.SetActive(true);
                        }
                        else
                        {
                            //Debug.LogWarning("test6");
                            if (CreateSecondChoice == false)
                            {
                                Debug.LogWarning("test9");
                                PanelFor2.transform.GetChild(0).gameObject.SetActive(false);//名乗るやつ
                                if (PhotonNetwork.CurrentRoom.MaxPlayers >= 5) //5人以上
                                {
                                    Debug.LogWarning("test7");
                                    EffectPanel5.SetActive(true);
                                }
                                else //4人以下
                                {
                                    Debug.LogWarning("test8");
                                    EffectPanel4.SetActive(true);
                                }
                                CreateSecondChoice = true;
                            }
                        }
                        break;
                    case 1: //霊媒師
                        SetJobString = "霊媒師";
                        if (SetSecondPicUI)
                        {
                            Debug.LogWarning("test5");
                            PanelFor2.transform.GetChild(1).gameObject.SetActive(true);
                            PanelFor2.transform.GetChild(2).gameObject.SetActive(true);
                        }
                        else
                        {
                            //Debug.LogWarning("test6");
                            if (CreateSecondChoice == false)
                            {
                                Debug.LogWarning("test9");
                                PanelFor2.transform.GetChild(0).gameObject.SetActive(false);//名乗るやつ
                                if (PhotonNetwork.CurrentRoom.MaxPlayers >= 5) //5人以上
                                {
                                    Debug.LogWarning("test7");
                                    EffectPanel5.SetActive(true);
                                }
                                else //4人以下
                                {
                                    Debug.LogWarning("test8");
                                    EffectPanel4.SetActive(true);
                                }
                                CreateSecondChoice = true;
                            }
                        }
                        break;
                    case 2: //騎士
                        break;
                    case 3: //共有者
                        break;
                    case 4: //神官
                        break;
                    case 5: //村人
                        break;
                    case 6: //狂人
                        break;
                    case 7: //妖狐
                        break;
                    case 8: //人狼
                        break;
                    case 9: //占い師、霊媒師のラスト選択場面
                        PanelFor2.transform.GetChild(1).gameObject.SetActive(false);
                        JobPanel.transform.GetChild(0).gameObject.SetActive(true);
                        JobPanel.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.GetComponent<Text>().text = setName;

                        //ここ占い師判定

                        if (SetJobString == "占い師")
                        {
                            JobPanel.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            if (lastPicNum == 5) //村人
                            {
                                JobPanel.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = "村人側";
                            }
                            else if (lastPicNum == 8) //人狼
                            {
                                JobPanel.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = "人狼";
                            }
                            else
                            {

                            }
                        }
                        else if(SetJobString == "霊媒師")
                        {
                            JobPanel.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                            switch (lastPicNum)
                            {
                                case 0: //占い師
                                    JobPanel.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = "占い師";
                                    break;
                                case 1: //霊媒師
                                    JobPanel.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = "霊媒師";
                                    break;
                                case 2: //騎士
                                    JobPanel.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = "騎士";
                                    break;
                                case 3: //共有者
                                    JobPanel.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = "共有者";
                                    break;
                                case 4: //神官
                                    JobPanel.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = "神官";
                                    break;
                                case 5: //村人
                                    JobPanel.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = "村人";
                                    break;
                                case 6: //狂人
                                    JobPanel.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = "狂人";
                                    break;
                                case 7: //妖狐
                                    JobPanel.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = "妖狐";
                                    break;
                                case 8: //人狼
                                    JobPanel.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = "人狼";
                                    break;
                            }
                        }
                        
                        break;
                    default: //その他
                        break;
                }
            }
        }
        else
        {
            EffectPanel4.SetActive(false);
            EffectPanel5.SetActive(false);
            Uranai_Rebaiabi.SetActive(false);
            Knight.SetActive(false);
            kyouyuusya.SetActive(false);
            Shinkan.SetActive(false);
            murabito.SetActive(false);
            Kyoujin.SetActive(false);
            Youko.SetActive(false);
            jinro.SetActive(false);
            PanelFor2.transform.GetChild(0).gameObject.SetActive(true);//名乗るやつ
            PanelFor2.transform.GetChild(2).gameObject.SetActive(true);//名乗るやつ

            firstUIDisableFlag = true;
        }
    }
    */
    void SettingPlayerListUI()
    {
        if (PhotonNetwork.CurrentRoom.MaxPlayers < 5)
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject NowObject = EffectPanel4.transform.GetChild(i).gameObject;
                if (i < PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    NowObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName;
                    if (AliveSetting.GetComponent<AliveSettingScript>().Post_Distribution[PhotonNetwork.LocalPlayer.ActorNumber - 1] == "占い師" &&
                        AliveSetting.GetComponent<AliveSettingScript>().AliveFlag[i] == false)//自分の役職が占い師なら死んだ人は占えない。
                    {
                        NowObject.SetActive(false);
                    }
                    else if (AliveSetting.GetComponent<AliveSettingScript>().Post_Distribution[PhotonNetwork.LocalPlayer.ActorNumber - 1] == "神官" &&
                        AliveSetting.GetComponent<AliveSettingScript>().AliveFlag[i] == false)//自分の役職が神官なら死んだ人を効果の対象に出来ない。
                    {
                        NowObject.SetActive(false);
                    }
                    else if (AliveSetting.GetComponent<AliveSettingScript>().Post_Distribution[PhotonNetwork.LocalPlayer.ActorNumber - 1] == "霊媒師" &&
                             AliveSetting.GetComponent<AliveSettingScript>().AliveFlag[i] == true)//自分が霊媒師なら生きている人を調べられない。
                    {

                        Debug.LogWarning("れいばいし！");
                        NowObject.SetActive(false);
                    }
                }
                else
                {
                    NowObject.SetActive(false);
                }
            }
        }
        else if (PhotonNetwork.CurrentRoom.MaxPlayers >= 5)
        {
            for (int i = 0; i < 7; i++)
            {
                GameObject NowObject = EffectPanel5.transform.GetChild(i).gameObject;
                if (i < PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    NowObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName;
                    if (AliveSetting.GetComponent<AliveSettingScript>().Post_Distribution[PhotonNetwork.LocalPlayer.ActorNumber - 1] == "占い師" &&
                        AliveSetting.GetComponent<AliveSettingScript>().AliveFlag[i] == false)//自分の役職が占い師なら死んだ人は占えない。
                    {
                        NowObject.SetActive(false);
                    }
                    else if (AliveSetting.GetComponent<AliveSettingScript>().Post_Distribution[PhotonNetwork.LocalPlayer.ActorNumber - 1] == "神官" &&
                        AliveSetting.GetComponent<AliveSettingScript>().AliveFlag[i] == false)//自分の役職が神官なら死んだ人を効果の対象に出来ない。
                    {
                        NowObject.SetActive(false);
                    }
                    else if (AliveSetting.GetComponent<AliveSettingScript>().Post_Distribution[PhotonNetwork.LocalPlayer.ActorNumber - 1] == "霊媒師" &&
                             AliveSetting.GetComponent<AliveSettingScript>().AliveFlag[i] == true)//自分が霊媒師なら生きている人を調べられない。
                    {
                        NowObject.SetActive(false);
                    }
                }
                else
                {
                    NowObject.SetActive(false);
                }
            }
        }
    }
}
