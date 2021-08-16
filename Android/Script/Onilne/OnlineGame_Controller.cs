using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityEngine.Video;
using UnityEngine.UI;
using NCMB;

public class battleObjectData
{
    public string BattleObject1Name;
    public string BattleObject2Name;
    public string BattleObject3Name;
    public string BattleUnit1Name;
    public string BattleUnit2Name;

    public int BattleObject1Helth;
    public int BattleObject2Helth;
    public int BattleObject3Helth;

    public Vector3 BattleObject1Position;
    public Vector3 BattleObject2Position;
    public Vector3 BattleObject3Position;
    public Vector3 BattleUnit1Position;
    public Vector3 BattleUnit2Position;
    public Vector3 HomePosition;

}
public class OnlineGame_Controller : MonoBehaviour
{

    GameObject clickedGameObject;
    GameObject[] EffectObject = new GameObject[24];
    /*
     0:UP
     1:DOWN
     2:LEFT
     3:RIGHT
     4:UUP
     5:DDOWN
     6:LLEFT
     7:RRIGHT
     8:LEFTUP
     9:LEFTDOWM
     10:RIGHTUP
     11:RIGHTDOWN
         */
    GameObject tempPickFigure;

    [SerializeField] public GameObject NIKO_CHAN;
    [SerializeField] public GameObject UNITY_CHAN;
    [SerializeField] public GameObject PRONAMA_CHAN;
    [SerializeField] public GameObject RADER;
    [SerializeField] public GameObject MINE;
    [SerializeField] public GameObject HOME;

    [SerializeField] public GameObject F_MainCamera;
    [SerializeField] public GameObject S_MainCamera;

    [SerializeField] public GameObject Icon_UnityChan;　//キャラ2
    [SerializeField] public GameObject Icon_Niko;       //キャラ1
    [SerializeField] public GameObject Icon_Pronama;    //キャラ3
    [SerializeField] public GameObject Icon_Rader;      //装備1
    [SerializeField] public GameObject Icon_Mine;       //装備2
    [SerializeField] public int choiseUnit;
    [SerializeField] public int choiseUnitCategory;
    [SerializeField] public GameObject FieldParent1;
    [SerializeField] public GameObject FieldParent2;
    [SerializeField] public GameObject FieldParent3;
    [SerializeField] public GameObject FieldParent4;
    [SerializeField] public GameObject FieldParent5;
    [SerializeField] public GameObject FieldParent6;
    [SerializeField] public GameObject FieldParent7;
    [SerializeField] public GameObject FieldParent8;
    [SerializeField] public GameObject FieldParent9;
    [SerializeField] public Material NORMALPiece;
    [SerializeField] public Material ATTACKPiece;
    [SerializeField] public Material RaderPiece;
    [SerializeField] public GameObject Effect;
    [SerializeField] public Text NowPlayerTurnText;
    [SerializeField] public Text BlindNowPlayerTurnText;
    [SerializeField] public Text BlindNowPlayerTurnText2;
    [SerializeField] public GameObject BlindPicture;
    [SerializeField] public GameObject SkillIconMaster;
    [SerializeField] public GameObject SkillIconOne;
    [SerializeField] public GameObject SkillIconTwo;
    [SerializeField] public GameObject SkillIconThree;
    [SerializeField] public GameObject SkillIconOneEffect;
    [SerializeField] public GameObject SkillIconTwoEffect;
    [SerializeField] public GameObject SkillIconThreeEffect;
    [SerializeField] public Text SkillMaterText;
    [SerializeField] public GameObject AttackMovie;
    [SerializeField] public VideoClip NicoHandMovie;
    [SerializeField] public VideoClip NicoAssultMovie;
    [SerializeField] public VideoClip NicoSniperMovie;
    [SerializeField] public VideoClip UnityHandMovie;
    [SerializeField] public VideoClip UnityAssultMovie;
    [SerializeField] public VideoClip UnitySniperMovie;
    [SerializeField] public VideoClip PronamaHandMovie;
    [SerializeField] public VideoClip PronamaAssultMovie;
    [SerializeField] public VideoClip PronamaSniperMovie;
    [SerializeField] public VideoClip testMovie;
    [SerializeField] public GameObject GUICanvas;
    [SerializeField] public GameObject PieceField;
    [SerializeField] public GameObject BackField;
    [SerializeField] public GameObject CharacterParent;
    [SerializeField] public GameObject VideoPlayer;
    [SerializeField] public GameObject Winnerback;

    public GameObject[,] PlayerBattleObject = new GameObject[2, 6];
    /*0:1p駒 
     *1:2p駒
     
    0:歩兵(ニコニ立体ちゃん)
    1:突撃兵(Unityちゃん)
    2:狙撃兵(ニコ生ちゃん)[0, 0] = 2
    3:レーダー
    4:地雷
    5:本陣  
         */

    public int[] SkillMater = new int[2];
    public AudioClip bomb_SE;
    public AudioClip click_SE;
    public AudioClip first_SE;
    public AudioClip fin_SE;
    public AudioClip BattleBGM;
    AudioSource Source_SE;

    //1:ニコニ立体ちゃん　2:Unityちゃん 3:プロ生ちゃん 4:レーダー 5:地雷
    /*float[,] FirstPlayer_SetPosition = new float[6, 3];
    float[,] SecondPlayer_SetPosition = new float[6, 3];*/
    float[,] MyPlayer_SetPosition = new float[6, 3];
    SelectDeckNum MyDeckData = new SelectDeckNum();
    /*
     *0:ニコニ立体ちゃん設置フラグ 0:未設置 1:設置済み 2:戦闘フェイズ用
     *1:Unityちゃん設置フラグ
     *2:プロ生ちゃん設置フラグ
     *3:レーダー設置フラグ
     *4:地雷設置フラグ
     *5:本陣
 
        0:設置フラグ
        1:x座標
        2:z座標
    */
    Vector3 tempPosition;
    int Writeflag = 0; //盤面設置許可フラグ
    public int PhaseControll = 0; //0:準備フェイズ 1:バトルフェイズ
    public int turnControll = 0; //0:1P turn 1:2P turn

    int ComeraControllFlag = 0; //カメラを初回の一度のみ呼び出すためのフラグ
    public float FirstPosiZ = -4;
    public float SecondPosiZ = 4;
    public int BattlePhaseKey = 4; //0:まだ動いてない 1:動いてアタック待ち 2:攻撃完了
    public bool[] SkillActiveFlag = new bool[4]; //false:未発動時

   

    int MoveValue = 0;
    Vector3[] ComeraPosition = new Vector3[2];
    const int OK = 1;
    public int[,] GameSetJudge = new int[2, 3];

    int[,] HomeCreateFlag = new int[2,1];
    //Vector3 MinePoint = new Vector3(-5,0,-5);
    Vector3[] MinePoint = new Vector3[2];
    bool battleFirstflag = false;
    float WaitCount = 0;

    bool EnemyDestroyFlag = false;
    public NCMBObject OnlineRoomClass = new NCMBObject("OnlineRoomClass");
    float TimeCount = 5;

    // Start is called before the first frame update
    void Start()
    {
        MyDeckData.InitUnitData();
        if(SceneNameData.Instance.referer == "CreateRoom")
        {
            turnControll = 0;
            OnlineRoomClass = CreateRoomScript.OnlineRoomClass;
            MyDeckData = CreateRoomScript.NowPlayerDeck;
        }
        else if (SceneNameData.Instance.referer == "JoinRoom")
        {
            turnControll = 1;
            OnlineRoomClass = JoinRoomScript.OnlineRoomClass;
            MyDeckData = JoinRoomScript.NowPlayerDeck;
        }

        NowPlayerTurnText.text = (turnControll + 1) + "P";

        DialogManager.Instance.SetLabel("Yes", "No", "Close");
        Icon_UnityChan.SetActive(true);
        Icon_Pronama.SetActive(true);
        Icon_Niko.SetActive(true);
        Icon_Rader.SetActive(true);
        Icon_Mine.SetActive(true);
        Source_SE = GetComponent<AudioSource>();

        ComeraPosition[0] = F_MainCamera.transform.position;
        ComeraPosition[1] = S_MainCamera.transform.position;
        MinePoint[0] = new Vector3(-5, 0, -5); ;
        MinePoint[1] = new Vector3(-5, 0, -5); ;

        for (int i = 0; i < 2; i++)
        {
            HomeCreateFlag[i, 0] = 0;
            SkillMater[i] = 0;
        }
        for (int i = 0; i < 4; i++)
        {
            SkillActiveFlag[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.LogWarning("BFKey:"+ BattlePhaseKey);
        Debug.LogWarning("PF:"+ PhaseControll);
        if (PhaseControll == 0) {
            SettingIconScript(turnControll);
            if (turnControll == 0)
            {
                if (ComeraControllFlag == 0 && HomeCreateFlag[0,0] == 0) {
                    HomeCreateFlag[0, 0] = 1;
                    ComeraControllFlag = 1;
                    F_MainCamera.SetActive(true);
                    S_MainCamera.SetActive(false);

                    int value = Random.Range(-3, 4);
                    Vector3 HomeTemp = new Vector3(value, 0.3f, -4f);
                    HOME.SetActive(true);
                    PlayerBattleObject[0, 5] = Instantiate(HOME, HomeTemp, Quaternion.AngleAxis(-90.0f, new Vector3(1.0f, 0.0f, 0.0f)));
                    HOME.SetActive(false);
                    MyPlayer_SetPosition[5, 0] = 1;
                    MyPlayer_SetPosition[5, 1] = value;
                    MyPlayer_SetPosition[5, 2] = -4;
                }
                PreparPhase(ref MyPlayer_SetPosition);
            }
            else
            {
                if (ComeraControllFlag == 0 && HomeCreateFlag[1, 0] == 0)
                {
                    HomeCreateFlag[1, 0] = 1;
                    ComeraControllFlag = 1;
                    F_MainCamera.SetActive(false);
                    S_MainCamera.SetActive(true);

                    HOME.SetActive(true);
                    int value = Random.Range(-3, 4);
                    Vector3 HomeTemp = new Vector3(value, 0.3f, 4f);
                    PlayerBattleObject[1, 5] = Instantiate(HOME, HomeTemp, Quaternion.AngleAxis(-90.0f, new Vector3(1.0f, 0.0f, 0.0f)));
                    HOME.SetActive(false);

                    MyPlayer_SetPosition[5, 0] = 1;
                    MyPlayer_SetPosition[5, 1] = value;
                    MyPlayer_SetPosition[5, 2] = 4;
                }
                PreparPhase(ref MyPlayer_SetPosition);
            }

            if (((MyPlayer_SetPosition[0, 0] == 1) &&
                (MyPlayer_SetPosition[1, 0] == 1) &&
                (MyPlayer_SetPosition[2, 0] == 1) &&
                (MyPlayer_SetPosition[3, 0] == 1) &&
                (MyPlayer_SetPosition[4, 0] == 1)))
            {
                PhaseControll = 1;
                //BattleObjectSwitch(false);
                
                Icon_UnityChan.SetActive(false);
                Icon_Pronama.SetActive(false);
                Icon_Niko.SetActive(false);
                Icon_Rader.SetActive(false);
                Icon_Mine.SetActive(false);
                BlindPicture.SetActive(true);
                BattlePhaseKey = 4;

                if (turnControll == 0)
                {
                    OnlineRoomClass["Player1ReadyFlag"] = "true";
                }
                else
                {
                    OnlineRoomClass["Player2ReadyFlag"] = "true";
                }

                DataUpLoad();
            }
        }
        else if(PhaseControll == 1)
        {
            if (ComeraControllFlag == 0)
            {
                if (battleFirstflag == false)
                {
                    Source_SE.PlayOneShot(first_SE);
                    battleFirstflag = true;

                    BattleObjectSwitch(true);
                }
                else
                {
                    BlindPicture.SetActive(true);
                }

                ComeraControllFlag = 1;
                SkillIconMaster.SetActive(true);
                SkillIconOne.transform.parent.gameObject.SetActive(true);
                SkillIconTwo.transform.parent.gameObject.SetActive(true);
                SkillIconThree.transform.parent.gameObject.SetActive(true);
                
                HelthColorChange();
                SkillMaterText.text = SkillMater[turnControll] + "";

                
                if (turnControll == 0)
                {
                    F_MainCamera.SetActive(true);
                    S_MainCamera.SetActive(false);
                }
                else
                {
                    F_MainCamera.SetActive(false);
                    S_MainCamera.SetActive(true);

                }
                FieldCleaning();
                BattleObjectSwitch(false);
                //RaderSearch();
            }
            if (BattlePhaseKey == 5)
            {
                if (WaitCount == 0) {
                    BattleObjectSwitch(true);
                    TimeCount = 5;
                }
                WaitCount += Time.deltaTime;
                if (WaitCount >= 0.2f) 
                {
                    BattlePhaseKey = 0;
                    WaitCount = 0;
                }
            }
            if (BattlePhaseKey == 4) //オンライン準備待機状態
            {
                TimeCount -= Time.deltaTime;
                if (TimeCount <= 0)
                {
                    OnlineRoomClass.FetchAsync((NCMBException e) => {
                        if (e != null)
                        {
                            //エラー処理
                        }
                        else
                        {
                            //成功時の処理
                            if (OnlineRoomClass["Winner"].ToString() == "0")
                            {
                                BlindPicture.SetActive(false);
                                BattleEnd(0,1);
                            }
                            else if (OnlineRoomClass["Winner"].ToString() == "1")
                            {
                                BlindPicture.SetActive(false);
                                BattleEnd(1, 0);
                            }

                            if (OnlineRoomClass["NowTurnPlayer"].ToString() == turnControll.ToString() &&
                                OnlineRoomClass["Player2ReadyFlag"].ToString() == "true" &&
                                OnlineRoomClass["Player1ReadyFlag"].ToString() == "true")
                            {
                                BlindNowPlayerTurnText.text = "貴方のターンだよ";
                                BlindNowPlayerTurnText2.text = "プレイヤーの準備が出来たらタッチしてね";
                            }
                        }
                    });
                    TimeCount = 5;
                }
            }

            if (BattlePhaseKey == 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ClickObject();

                    if (clickedGameObject.tag == "Player")
                    {
                        MoveValue = clickedGameObject.GetComponent<FigureScript>().MovePower;
                        ChoiseFigure();
                    }
                }
            }
            else if (BattlePhaseKey == 1)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ClickObject();
                    int loopNum = 4;

                    int EnemyTurn = 0;
                    if (turnControll == 0) EnemyTurn = 1;

                    if (MoveValue == 2) loopNum = 12;
                    if (MoveValue == 3) loopNum = 24;
                    for (int i = 0; i < loopNum; i++) {
                        if (EffectObject[i] != null)
                        {
                            if ((clickedGameObject.tag == "Effect") &&
                                (clickedGameObject.transform.position == EffectObject[i].transform.position))
                            {
                                Vector3 TempMovePositon = clickedGameObject.transform.position;
                                Destroy(EffectObject[i]);
                                if (clickedGameObject != null &&
                                    tempPickFigure != null)
                                {
                                    Debug.Log("Mineチェックします");
                                    tempPickFigure.transform.position = TempMovePositon;
                                    MineChecker(TempMovePositon, 3);
                                    if(tempPickFigure != null) MineChecker(TempMovePositon, 4);
                                }

                                void MineChecker(Vector3 FigureMovePosition,int WepNum)
                                {
                                   
                                    if (PlayerBattleObject[EnemyTurn, WepNum] != null) {
                                        if (FigureMovePosition.x == MinePoint[WepNum - 3].x &&
                                            FigureMovePosition.z == MinePoint[WepNum - 3].z &&
                                            PlayerBattleObject[EnemyTurn, WepNum].name == "Mine(Clone)")
                                        {
                                            Debug.Log("相手の地雷に踏みました:"+WepNum);
                                            Destroy(PlayerBattleObject[EnemyTurn, WepNum]);
                                            Source_SE.PlayOneShot(bomb_SE);
                                            tempPickFigure.GetComponent<FigureScript>().helth -= 1;
                                            Debug.Log(tempPickFigure.name + "の残りライフ" + tempPickFigure.GetComponent<FigureScript>().helth);
                                            if (tempPickFigure.GetComponent<FigureScript>().helth <= 0)
                                            {
                                                Debug.Log("0だから破壊処理しますね");
                                                for (int controllFigure = 0; controllFigure < 3; controllFigure++)
                                                {
                                                    Debug.Log("今これ見てます:" + PlayerBattleObject[turnControll, controllFigure]);
                                                    if (PlayerBattleObject[turnControll,controllFigure] != null) {
                                                        if (PlayerBattleObject[turnControll, controllFigure].transform.position.x == MinePoint[WepNum - 3].x &&
                                                            PlayerBattleObject[turnControll, controllFigure].transform.position.z == MinePoint[WepNum - 3].z)
                                                        {
                                                            Debug.Log("これ破壊します：" + PlayerBattleObject[turnControll, controllFigure]);
                                                            Destroy(PlayerBattleObject[turnControll, controllFigure]);
                                                            //Source_SE.PlayOneShot(bomb_SE);
                                                            tempPickFigure = null;
                                                            GameSetJudge[turnControll, controllFigure] = OK;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (GameSetJudge[turnControll, 0] == OK &&
                                    GameSetJudge[turnControll, 1] == OK &&
                                    GameSetJudge[turnControll, 2] == OK)
                                {
                                    int NowPlayer = turnControll;
                                    turnControll = 1 - turnControll;
                                    BattleEnd(turnControll,NowPlayer);
                                }
                                else
                                {
                                    if (tempPickFigure != null) {
                                        BattlePhaseKey = 2;
                                    }
                                    else
                                    {
                                        BattlePhaseKey = 3;
                                    }
                                }
                            }
                            /*else if(clickedGameObject == tempPickFigure)
                            {
                                for (int DeleteEffect = 0; DeleteEffect < loopNum; DeleteEffect++)
                                {
                                    if (EffectObject[DeleteEffect] != null)
                                    {
                                        Destroy(EffectObject[DeleteEffect]);
                                    }
                                }
                                BattlePhaseKey = 0;
                                Debug.Log("もう一回やり直せるドン");
                            }*/
                        }
                    }
                    if ((clickedGameObject.tag == "Home") &&
                        (clickedGameObject.transform.position == PlayerBattleObject[EnemyTurn, 5].transform.position))
                    {
                        Vector3 TempMovePositon = clickedGameObject.transform.position;
                        Destroy(PlayerBattleObject[EnemyTurn, 5]);
                        PlayerBattleObject[EnemyTurn, 5] = null;
                        tempPickFigure.transform.position = TempMovePositon;
                            
                        for (int i = 0; i < loopNum; i++)
                        {
                            if (EffectObject[i] != null)
                            {
                                Destroy(EffectObject[i]);
                            }
                        }

                        BattleEnd(turnControll,EnemyTurn);
                    }
                    if (BattlePhaseKey == 2 ||
                        BattlePhaseKey == 3) {
                        for (int i = 0; i < loopNum; i++)
                        {
                            if (EffectObject[i] != null) {
                                Destroy(EffectObject[i]);
                            }
                        }
                    }
                }
            }
            else if (BattlePhaseKey == 2)
            {
                AttackPickFigure();
            }
            else if (BattlePhaseKey == 3)
            {
                BattlePhaseKey = 4;
                tempPickFigure = null;
                TurnController();
                OnlineRoomClass["NowTurnPlayer"] = (1 - turnControll).ToString();
                DataUpLoad();
                BlindNowPlayerTurnText.text = "相手のターン中です";
                BlindNowPlayerTurnText2.text = "もうしばらく待ってね";
            }
        }

    }
    void SetFigure(ref float[,] SetPosition, GameObject Name, int Num)
    {
        Name.SetActive(true);
        if ((Name.name == "Rader") || (Name.name == "Mine"))
        {
            Vector3 tempPosit = clickedGameObject.transform.position + new Vector3(0f, 0.2f, 0f);
            switch (Name.name)
            {
                case "Rader":
                    PlayerBattleObject[turnControll, Num] = Instantiate(Name, tempPosit, Quaternion.AngleAxis(-90.0f, new Vector3(1.0f, 0.0f, 0.0f)));
                    break;
                case "Mine":
                    PlayerBattleObject[turnControll, Num] = Instantiate(Name, tempPosit, Quaternion.AngleAxis(-90.0f, new Vector3(1.0f, 0.0f, 0.0f)));
                    break;
            }
        }
        else
        {
            switch (Name.name)
            {
                case "Alicia_solid":
                    if (turnControll == 0)
                    {
                        PlayerBattleObject[turnControll, Num] = Instantiate(Name, clickedGameObject.transform.position, Quaternion.AngleAxis(180f, new Vector3(0.0f, 1.0f, 0.0f)));
                    }
                    else
                    {
                        PlayerBattleObject[turnControll, Num] = Instantiate(Name, clickedGameObject.transform.position, Quaternion.identity);
                    }
                    break;
                case "unitychan":
                    if (turnControll == 0)
                    {
                        PlayerBattleObject[turnControll, Num] = Instantiate(Name, clickedGameObject.transform.position, Quaternion.AngleAxis(180f, new Vector3(0.0f, 1.0f, 0.0f)));
                    }
                    else
                    {
                        PlayerBattleObject[turnControll, Num] = Instantiate(Name, clickedGameObject.transform.position, Quaternion.identity);
                    }
                    break;
                case "PronamaChan":
                    if (turnControll == 0)
                    {
                        PlayerBattleObject[turnControll, Num] = Instantiate(Name, clickedGameObject.transform.position, Quaternion.AngleAxis(180f, new Vector3(0.0f, 1.0f, 0.0f)));
                    }
                    else
                    {
                        PlayerBattleObject[turnControll, Num] = Instantiate(Name, clickedGameObject.transform.position, Quaternion.identity);
                    }
                    break;
            }
            RoleSetting(MyDeckData);

            void RoleSetting(SelectDeckNum TurnDeck)
            {
                switch (Num)
                {
                    case 0:
                        PlayerBattleObject[turnControll, Num].GetComponent<FigureScript>().ClassNum = TurnDeck.FirstUnit.UnitCategory;
                        break;
                    case 1:
                        PlayerBattleObject[turnControll, Num].GetComponent<FigureScript>().ClassNum = TurnDeck.SecondUnit.UnitCategory;
                        break;
                    case 2:
                        PlayerBattleObject[turnControll, Num].GetComponent<FigureScript>().ClassNum = TurnDeck.ThirdUnit.UnitCategory;
                        break;
                }
                switch (PlayerBattleObject[turnControll, Num].GetComponent<FigureScript>().ClassNum)
                {
                    case 0://歩兵
                        PlayerBattleObject[turnControll, Num].GetComponent<FigureScript>().helth = 2;
                        PlayerBattleObject[turnControll, Num].GetComponent<FigureScript>().MovePower = 1;
                        PlayerBattleObject[turnControll, Num].GetComponent<FigureScript>().AttackArea = 1;
                        break;
                    case 1://突撃兵
                        PlayerBattleObject[turnControll, Num].GetComponent<FigureScript>().helth = 1;
                        PlayerBattleObject[turnControll, Num].GetComponent<FigureScript>().MovePower = 2;
                        PlayerBattleObject[turnControll, Num].GetComponent<FigureScript>().AttackArea = 1;
                        break;
                    case 2://狙撃兵
                        PlayerBattleObject[turnControll, Num].GetComponent<FigureScript>().helth = 1;
                        PlayerBattleObject[turnControll, Num].GetComponent<FigureScript>().MovePower = 1;
                        PlayerBattleObject[turnControll, Num].GetComponent<FigureScript>().AttackArea = 100;
                        break;
                }
                Debug.Log(PlayerBattleObject[turnControll, Num]);
                Debug.Log(PlayerBattleObject[turnControll, Num].GetComponent<FigureScript>().ClassNum);
                Debug.Log(PlayerBattleObject[turnControll, Num].GetComponent<FigureScript>().helth);
                Debug.Log(PlayerBattleObject[turnControll, Num].GetComponent<FigureScript>().MovePower);
                Debug.Log(PlayerBattleObject[turnControll, Num].GetComponent<FigureScript>().AttackArea);
            }
        }
        Name.SetActive(false);

        SetPosition[Num, 1] = tempPosition.x;
        SetPosition[Num, 2] = tempPosition.z;
    }

    public void TurnController()
    {
        int NowPlayer = turnControll;
        ComeraControllFlag = 0;
        SkillReset();

        if (PhaseControll == 1)
        {
            BattleEnd(NowPlayer, turnControll);
            if (SkillMater[turnControll] < 10)
            {
                SkillMater[turnControll]++;
            }
        }
        if (PhaseControll != 2)
        {
            NowPlayerTurnText.text = (turnControll + 1) + "P";
            FieldCleaning();
            BattleObjectSwitch(false);
        }

    }

    public void BattleEnd(int NowPlayer,int EnemyPlayer)
    {
        if ((GameSetJudge[EnemyPlayer, 0] == OK &&
            GameSetJudge[EnemyPlayer, 1] == OK &&
            GameSetJudge[EnemyPlayer, 2] == OK) ||
            PlayerBattleObject[EnemyPlayer, 5] == null ||
            OnlineRoomClass["Winner"].ToString() != "noResult")
        {
            Winnerback.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<Text>().text = NowPlayer + 1 + "P";
            Winnerback.SetActive(true);
            Source_SE.PlayOneShot(fin_SE);
            if (OnlineRoomClass["Winner"].ToString() != "noResult")
            {
                OnlineRoomClass.DeleteAsync((NCMBException e) => {
                    if (e != null)
                    {
                        //エラー処理
                    }
                    else
                    {
                        //成功時の処理
                    }
                });
            }
            else
            {
                OnlineRoomClass["Winner"] = NowPlayer.ToString();
                OnlineRoomClass.SaveAsync();
            }
            
            PhaseControll = 2;
        }
        else
        {
            Debug.Log("続行");
        }
    }

    void PreparPhase(ref float[,] SetPosition)
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickObject();

            tempPosition = clickedGameObject.transform.position;

            if (clickedGameObject.transform.root.gameObject.name == "FieldPieceBox")
            {
                for (int loopName = 0; loopName < 6; loopName++)
                {
                    if (tempPosition.x == SetPosition[loopName, 1])
                    {
                        if (tempPosition.z == SetPosition[loopName, 2])
                        {
                            Writeflag = 0;
                        }
                    }
                }
                if (Writeflag == 1)
                {
                    if ((turnControll == 0 && ((clickedGameObject.transform.parent.name == "1段目") ||
                        (clickedGameObject.transform.parent.name == "2段目") ||
                        (clickedGameObject.transform.parent.name == "3段目")))
                        ||
                        (turnControll == 1 && ((clickedGameObject.transform.parent.name == "7段目") ||
                        (clickedGameObject.transform.parent.name == "8段目") ||
                        (clickedGameObject.transform.parent.name == "9段目"))))
                    {
                        switch (choiseUnit)
                        {
                            case 0:
                                break;

                            case 1:　//いちわく
                                if (SetPosition[0, 0] == 0)
                                {
                                    SetFigure(ref SetPosition, SelectUnitNumber(choiseUnitCategory), 0);
                                    SetPosition[0, 0] = 1;
                                }
                                break;

                            case 2:　//にわく
                                if (SetPosition[1, 0] == 0)
                                {
                                    SetFigure(ref SetPosition, SelectUnitNumber(choiseUnitCategory), 1);
                                    SetPosition[1, 0] = 1;
                                }
                                break;

                            case 3: //さんわく
                                if (SetPosition[2, 0] == 0)
                                {
                                    SetFigure(ref SetPosition, SelectUnitNumber(choiseUnitCategory), 2);
                                    SetPosition[2, 0] = 1;
                                }
                                break;

                            case 4: //よんわく
                                if (SetPosition[3, 0] == 0)
                                {
                                    SetFigure(ref SetPosition, SelectUnitNumber(choiseUnitCategory), 3);
                                    SetPosition[3, 0] = 1;
                                }
                                break;

                            case 5: //ごわく
                                if (SetPosition[4, 0] == 0)
                                {
                                    SetFigure(ref SetPosition, SelectUnitNumber(choiseUnitCategory), 4);
                                    SetPosition[4, 0] = 1;
                                }
                                break;
                        }
                        choiseUnit = 0;
                        choiseUnitCategory = 0;
                    }
                    else
                    {
                        //EditorUtility.DisplayDialog("Notice", "手前3マス以内を選択してね!", "OK");
                        DialogManager.Instance.ShowSubmitDialog(
                            "[Attention]",
                            "手前3マス以内を選択してね!",
                            (bool result) => { Debug.Log("submited!"); }
                        );
                    }
                }
                else
                {
                    //EditorUtility.DisplayDialog("Notice", "置けないよ!", "OK");
                    DialogManager.Instance.ShowSubmitDialog(
                        "[Attention]",
                        "そこには設置できません！",
                        (bool result) => { Debug.Log("submited!"); }
                    );
                }
            }

        }
        GameObject SelectUnitNumber(int UnitNum)
        {
            switch (UnitNum)
            {
                case 1: //にこ
                    return NIKO_CHAN;
                case 2: //ゆにてぃ
                    return UNITY_CHAN;
                case 3: //ぷろなま
                    return PRONAMA_CHAN;
                case 4: //れーだー
                    return RADER;
                case 5: //じらい
                    return MINE;
                default:
                    return null;
            }
        }
    }

    void FieldCleaning()
    {
        foreach (Transform childTransform in FieldParent1.transform)
        {
            childTransform.GetComponent<Renderer>().material = NORMALPiece;
        }
        foreach (Transform childTransform in FieldParent2.transform)
        {
            childTransform.GetComponent<Renderer>().material = NORMALPiece;
        }
        foreach (Transform childTransform in FieldParent3.transform)
        {
            childTransform.GetComponent<Renderer>().material = NORMALPiece;
        }
        foreach (Transform childTransform in FieldParent4.transform)
        {
            childTransform.GetComponent<Renderer>().material = NORMALPiece;
        }
        foreach (Transform childTransform in FieldParent5.transform)
        {
            childTransform.GetComponent<Renderer>().material = NORMALPiece;
        }
        foreach (Transform childTransform in FieldParent6.transform)
        {
            childTransform.GetComponent<Renderer>().material = NORMALPiece;
        }
        foreach (Transform childTransform in FieldParent7.transform)
        {
            childTransform.GetComponent<Renderer>().material = NORMALPiece;
        }
        foreach (Transform childTransform in FieldParent8.transform)
        {
            childTransform.GetComponent<Renderer>().material = NORMALPiece;
        }
        foreach (Transform childTransform in FieldParent9.transform)
        {
            childTransform.GetComponent<Renderer>().material = NORMALPiece;
        }
    }

    public void BattleObjectSwitch(bool flag)
    {
        if (PhaseControll == 1)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (PlayerBattleObject[j, i] != null && flag == false)
                    {
                        PlayerBattleObject[j, i].SetActive(false);
                    }
                    if (PlayerBattleObject[turnControll, i] != null && flag == true)
                    {
                        PlayerBattleObject[turnControll, i].SetActive(true);
                    }
                }
            }
        }
    }

    void ClickObject()
    {
        Source_SE.PlayOneShot(click_SE);
        clickedGameObject = null;
        Writeflag = 1;
        Camera FirstCamera = F_MainCamera.GetComponent<Camera>();
        Camera SecondCamera = S_MainCamera.GetComponent<Camera>();
        Ray ray;


        if (turnControll == 0)
        {
            ray = FirstCamera.ScreenPointToRay(Input.mousePosition);
            //ray = Camera.main.ScreenPointToRay(TouchPosition);
        }
        else
        {

            ray = SecondCamera.ScreenPointToRay(Input.mousePosition);
            //ray = SecondCamera.ScreenPointToRay(TouchPosition);
        }
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            clickedGameObject = hit.collider.gameObject;
        }
    }

    void ChoiseFigure()
    {
        Vector3[] Cursor = new Vector3[24];
        Cursor[0] = new Vector3(0, 0, 1);    //Up
        Cursor[1] = new Vector3(0, 0, -1);   //Down
        Cursor[2] = new Vector3(-1, 0, 0);   //Left
        Cursor[3] = new Vector3(1, 0, 0);    //Right
        Cursor[4] = new Vector3(0, 0, 2);    //DoubleUp
        Cursor[5] = new Vector3(0, 0, -2);    //DoubleDown
        Cursor[6] = new Vector3(-2, 0, 0);    //DoubleLeft
        Cursor[7] = new Vector3(2, 0, 0);    //DoubleRight
        Cursor[8] = new Vector3(-1, 0, 1);    //LeftUp
        Cursor[9] = new Vector3(-1, 0, -1);    //LeftDown
        Cursor[10] = new Vector3(1, 0, 1);    //RightUp
        Cursor[11] = new Vector3(1, 0, -1);    //RightDown
        Cursor[12] = new Vector3(0, 0, 3);    //UUU
        Cursor[13] = new Vector3(-1, 0, 2);    //UUL
        Cursor[14] = new Vector3(-2, 0, 1);    //ULL
        Cursor[15] = new Vector3(-3, 0, 0);    //LLL
        Cursor[16] = new Vector3(-2, 0, -1);    //DLL
        Cursor[17] = new Vector3(-1, 0, -2);    //DDL
        Cursor[18] = new Vector3(0, 0, -3);    //DDD
        Cursor[19] = new Vector3(1, 0, -2);    //DDR
        Cursor[20] = new Vector3(2, 0, -1);    //DRR
        Cursor[21] = new Vector3(3, 0, 0);    //RRR
        Cursor[22] = new Vector3(2, 0, 1);    //URR
        Cursor[23] = new Vector3(1, 0, 2);    //UUR

        int[] SetEffectFlag = new int[24];
        int loopNum = 4;

        const int MatchPosition = 2;
        const int NotMatchPosition = 1;

        if (MoveValue == 2) loopNum = 12;
        if (MoveValue == 3) loopNum = 24;

        Effect.SetActive(true);
        for (int FlagValue = 0; FlagValue < loopNum; FlagValue++)
        {
            Vector3 NowCorsorSetPosition = clickedGameObject.transform.position + Cursor[FlagValue];
            NowCorsorSetPosition.y = 0.1f;
            for (int turnNum = 0; turnNum < 2; turnNum++)
            {
                for (int FigureNum = 0; FigureNum < 6; FigureNum++)
                {
                    if (PlayerBattleObject[turnNum, FigureNum] != null)
                    {
                        if ((PlayerBattleObject[turnNum, FigureNum].transform.position.x == NowCorsorSetPosition.x) &&
                        PlayerBattleObject[turnNum, FigureNum].transform.position.z == NowCorsorSetPosition.z)
                        {
                            SetEffectFlag[FlagValue] = MatchPosition;
                            if ((turnControll == 0 && turnNum == 1) ||
                                (turnControll == 1 && turnNum == 0))
                            {
                                if (PlayerBattleObject[turnNum, FigureNum].name != "Mine(Clone)") {
                                    PlayerBattleObject[turnNum, FigureNum].SetActive(true);
                                }
                                else
                                {
                                    int MineCounter = 0;
                                    if (FigureNum == 4)
                                    {
                                        MineCounter = 1;
                                    }
                                    MinePoint[MineCounter] = PlayerBattleObject[turnNum, FigureNum].transform.position;
                                    SetEffectFlag[FlagValue] = NotMatchPosition;
                                    Debug.Log("わおん" + FigureNum);
                                    Debug.Log("ねーむ" + PlayerBattleObject[turnNum, FigureNum].name);
                                }
                            }
                        }
                        else
                        {
                            if (SetEffectFlag[FlagValue] != MatchPosition)
                            {
                                SetEffectFlag[FlagValue] = NotMatchPosition;
                            }
                        }
                    }
                }
            }
            if (SetEffectFlag[FlagValue] == NotMatchPosition)
            {
                if (NowCorsorSetPosition.x > -4 &&
                    NowCorsorSetPosition.x < 4 &&
                    NowCorsorSetPosition.z > -5 &&
                    NowCorsorSetPosition.z < 5) 
                {
                    EffectObject[FlagValue] = Instantiate(Effect, NowCorsorSetPosition, Quaternion.AngleAxis(90.0f, new Vector3(1.0f, 0.0f, 0.0f)));
                    //Debug.Log("ほにゃら:"+ NowCorsorSetPosition);
                }
            }
            else if (SetEffectFlag[FlagValue] == MatchPosition)
            {
                
            }
            else
            {
                Debug.Log("!!--ERROR--!! : C#00001:" + SetEffectFlag[FlagValue]);
            }
        }
        Effect.SetActive(false);

        BattlePhaseKey = 1;
        tempPickFigure = clickedGameObject;
        AttackedMovie(tempPickFigure);
    }

    void AttackPickFigure()
    {

        Vector3[] Cursor = new Vector3[24];
        Cursor[0] = new Vector3(0, 0.1f, 1);    //Up
        Cursor[1] = new Vector3(0, 0.1f, -1);   //Down
        Cursor[2] = new Vector3(-1, 0.1f, 0);   //Left
        Cursor[3] = new Vector3(1, 0.1f, 0);    //Right
        Cursor[4] = new Vector3(0, 0.1f, 2);    //DoubleUp
        Cursor[5] = new Vector3(0, 0.1f, -2);    //DoubleDown
        Cursor[6] = new Vector3(-2, 0.1f, 0);    //DoubleLeft
        Cursor[7] = new Vector3(2, 0.1f, 0);    //DoubleRight
        Cursor[8] = new Vector3(-1, 0.1f, 1);    //LeftUp
        Cursor[9] = new Vector3(-1, 0.1f, -1);    //LeftDown
        Cursor[10] = new Vector3(1, 0.1f, 1);    //RightUp
        Cursor[11] = new Vector3(1, 0.1f, -1);    //RightDown
        Cursor[12] = new Vector3(0, 0, 3);    //UUU
        Cursor[13] = new Vector3(-1, 0, 2);    //UUL
        Cursor[14] = new Vector3(-2, 0, 1);    //ULL
        Cursor[15] = new Vector3(-3, 0, 0);    //LLL
        Cursor[16] = new Vector3(-2, 0, -1);    //DLL
        Cursor[17] = new Vector3(-1, 0, -2);    //DDL
        Cursor[18] = new Vector3(0, 0, -3);    //DDD
        Cursor[19] = new Vector3(1, 0, -2);    //DDR
        Cursor[20] = new Vector3(2, 0, -1);    //DRR
        Cursor[21] = new Vector3(3, 0, 0);    //RRR
        Cursor[22] = new Vector3(2, 0, 1);    //URR
        Cursor[23] = new Vector3(1, 0, 2);    //UUR

        int[] SetEffectFlag = new int[24];
        int loopNum = 4;
        int AttackArea = 0;


        const int MatchPosition = 2;
        const int NotMatchPosition = 1;

        GameObject[] AttackAreaPiece = new GameObject[12];
        bool FindAttackTarget = false;
        bool FigureSurviveCheck = false;

        for (int i = 0;  i < 3; i++)
        {
            int EnemyTurn = 0;
            if (turnControll == 0) EnemyTurn = 1;
            if (PlayerBattleObject[EnemyTurn, i] != null)
            {
                PlayerBattleObject[EnemyTurn, i].SetActive(false);
            }
            
        }

        for (int FigureSurviveCheckCount = 0; FigureSurviveCheckCount < 3; FigureSurviveCheckCount++)
        {
            if (PlayerBattleObject[turnControll, FigureSurviveCheckCount] != null &&
                tempPickFigure != null)
            {
                if (PlayerBattleObject[turnControll, FigureSurviveCheckCount].transform.position == tempPickFigure.transform.position)
                {
                    FigureSurviveCheck = true;
                }
                else 
                {
                    Debug.Log("失敗："+ FigureSurviveCheckCount + "回目");
                }
                
            }
        }

        if (FigureSurviveCheck)
        {
            switch (tempPickFigure.name)
            {
                case "Alicia_solid(Clone)":
                    AttackArea = tempPickFigure.GetComponent<FigureScript>().AttackArea;
                    break;
                case "unitychan(Clone)":
                    AttackArea = tempPickFigure.GetComponent<FigureScript>().AttackArea;
                    break;
                case "PronamaChan(Clone)":
                    AttackArea = tempPickFigure.GetComponent<FigureScript>().AttackArea;
                    break;
            }
        
            if (FindAttackTarget == false)
            {
                if (AttackArea >= 100)
                {
                    for (int AttackArealoop = 1; AttackArealoop <= 63; AttackArealoop++)
                    {
                        GameObject.Find(AttackArealoop.ToString()).GetComponent<Renderer>().material = ATTACKPiece;
                    }
                    FindAttackTarget = true;
                }
                else
                {
                    if (AttackArea == 2) loopNum = 12;
                    for (int FlagValue = 0; FlagValue < loopNum; FlagValue++)
                    {
                        Vector3 NowCorsorSetPosition = tempPickFigure.transform.position + Cursor[FlagValue];
                        for (int turnNum = 0; turnNum < 2; turnNum++)
                        {
                            for (int FigureNum = 0; FigureNum < 5; FigureNum++)
                            {
                                if (PlayerBattleObject[turnNum, FigureNum] != null)
                                {
                                    if ((PlayerBattleObject[turnNum, FigureNum].transform.position.x == NowCorsorSetPosition.x) &&
                                    PlayerBattleObject[turnNum, FigureNum].transform.position.z == NowCorsorSetPosition.z)
                                    {
                                        if ((turnControll == 0 && turnNum == 1) ||
                                            (turnControll == 1 && turnNum == 0))
                                        {
                                            if (PlayerBattleObject[turnNum, FigureNum].name != "Mine(Clone)")
                                            {
                                                PlayerBattleObject[turnNum, FigureNum].SetActive(true);
                                                SetEffectFlag[FlagValue] = MatchPosition;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (SetEffectFlag[FlagValue] != MatchPosition)
                                        {
                                            SetEffectFlag[FlagValue] = NotMatchPosition;
                                        }
                                    }
                                }
                            }
                        }
                        if (SetEffectFlag[FlagValue] == NotMatchPosition)
                        {

                        }
                        else if (SetEffectFlag[FlagValue] == MatchPosition)
                        {
                            FindAttackTarget = true;
                            Debug.Log("戦う敵を見つけたよ");

                            GameObject tempPiece = FindPiece(tempPickFigure.transform.position);

                            int[] tempPieceNameNum = new int[loopNum + 1];
                            tempPieceNameNum[0] = int.Parse(tempPiece.name);
                            if (tempPieceNameNum[0] < 57)
                            {
                                tempPieceNameNum[1] = int.Parse(tempPiece.name) + 7; //down
                                Debug.Log("下");
                            }
                            else
                            {
                                tempPieceNameNum[1] = 0;
                            }

                            if (tempPieceNameNum[0] > 7)
                            {
                                tempPieceNameNum[2] = int.Parse(tempPiece.name) - 7; //up
                                Debug.Log("上");
                            }
                            else
                            {
                                tempPieceNameNum[2] = 0;
                                Debug.LogError("左");
                            }
                            if (tempPieceNameNum[0] % 7 != 0)
                            {
                                tempPieceNameNum[3] = int.Parse(tempPiece.name) + 1; //left
                                Debug.Log("左");
                            }
                            else
                            {
                                tempPieceNameNum[3] = 0;
                                Debug.LogError("右");
                            }
                            if ((tempPieceNameNum[0] - 1) % 7 != 0)
                            {
                                tempPieceNameNum[4] = int.Parse(tempPiece.name) - 1; //right
                                Debug.Log("右");
                            }
                            else
                            {
                                tempPieceNameNum[4] = 0;
                            }
                            if (loopNum == 12)
                            {
                                if (tempPieceNameNum[0] % 7 == 0 || (tempPieceNameNum[0] + 1) % 7 == 0)
                                {
                                    tempPieceNameNum[5] = 0;
                                    Debug.LogError("左左");
                                }
                                else
                                {
                                    tempPieceNameNum[5] = int.Parse(tempPiece.name) + 2; //leftleft
                                    Debug.Log("左左");
                                }
                                if ((tempPieceNameNum[0] - 1) % 7 == 0 || (tempPieceNameNum[0] - 2) % 7 == 0)
                                {
                                    tempPieceNameNum[6] = 0;
                                    Debug.LogError("右右");
                                }
                                else
                                {
                                    tempPieceNameNum[6] = int.Parse(tempPiece.name) - 2; //rightright
                                    Debug.Log("右→");
                                }
                                if (tempPieceNameNum[0] > 7) { //up
                                    if (tempPieceNameNum[0] % 7 != 0)
                                    {
                                        tempPieceNameNum[7] = int.Parse(tempPiece.name) - 6; //upleft
                                        Debug.Log("上左");
                                    }
                                    else
                                    {
                                        tempPieceNameNum[7] = 0;
                                        Debug.LogError("上左");
                                    }
                                    if ((tempPieceNameNum[0] - 1) % 7 != 0)
                                    {
                                        tempPieceNameNum[8] = int.Parse(tempPiece.name) - 8; //upright
                                        Debug.Log("上→");
                                    }
                                    else
                                    {
                                        tempPieceNameNum[8] = 0;
                                        Debug.LogError("上→");
                                    }
                                    if (tempPieceNameNum[0] > 15)
                                    {
                                        tempPieceNameNum[9] = int.Parse(tempPiece.name) - 14; //upup
                                        Debug.Log("上上");
                                    }
                                    else
                                    {
                                        tempPieceNameNum[9] = 0;
                                        Debug.LogError("上上");
                                    }
                                }
                                if (tempPieceNameNum[0] < 57) {
                                    if (tempPieceNameNum[0] % 7 != 0)
                                    {
                                        tempPieceNameNum[10] = int.Parse(tempPiece.name) + 8; //downleft
                                        Debug.Log("下左");
                                    }
                                    else
                                    {
                                        tempPieceNameNum[10] = 0;
                                        Debug.LogError("下左");
                                    }
                                    if ((tempPieceNameNum[0] - 1) % 7 != 0)
                                    {
                                        tempPieceNameNum[11] = int.Parse(tempPiece.name) + 6; //downright
                                        Debug.Log("下右");
                                    }
                                    else
                                    {
                                        tempPieceNameNum[11] = 0;
                                        Debug.LogError("下→");
                                    }
                                    if (tempPieceNameNum[0] < 49)
                                    {
                                        tempPieceNameNum[12] = int.Parse(tempPiece.name) + 14; //downdown
                                        Debug.Log("下下");
                                    }
                                    else
                                    {
                                        tempPieceNameNum[12] = 0;
                                        Debug.LogError("下下");
                                    }
                                }
                            }
                            Debug.Log("マテリアルチェンジ");
                            for (int loopValue = 0; loopValue < loopNum; loopValue++)
                            {
                                AttackAreaPiece[loopValue] = GameObject.Find(tempPieceNameNum[loopValue + 1].ToString());
                                if (AttackAreaPiece[loopValue] != null)
                                {
                                    AttackAreaPiece[loopValue].GetComponent<Renderer>().material = ATTACKPiece;
                                }
                            }
                            tempPiece.GetComponent<Renderer>().material = ATTACKPiece;
                        }
                        else
                        {
                            Debug.Log("!!--ERROR--!! : C#00001:" + SetEffectFlag[FlagValue]);
                        }
                    }
                }
            }
            if (FindAttackTarget)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ClickObject();
                    int EnemyPlayer = 1;
                    if (turnControll == 1)
                    {
                        EnemyPlayer = 0;
                    }
                    if (clickedGameObject.tag == "Player")
                    {
                        for (int BattleObjectNum = 0; BattleObjectNum < 3; BattleObjectNum++)
                        {
                            if (clickedGameObject == PlayerBattleObject[EnemyPlayer, BattleObjectNum])
                            {
                                PlayerBattleObject[EnemyPlayer, BattleObjectNum].GetComponent<FigureScript>().helth -= 1;
                                if (PlayerBattleObject[EnemyPlayer, BattleObjectNum].GetComponent<FigureScript>().helth <= 0)
                                {
                                    Destroy(PlayerBattleObject[EnemyPlayer, BattleObjectNum]);
                                    GameSetJudge[EnemyPlayer, BattleObjectNum] = OK;
                                    EnemyDestroyFlag = true;
                                }
                                if (GameSetJudge[EnemyPlayer, 0] == OK &&
                                    GameSetJudge[EnemyPlayer, 1] == OK &&
                                    GameSetJudge[EnemyPlayer, 2] == OK)
                                {
                                    BattleEnd(turnControll,EnemyPlayer);
                                }
                                else
                                {
                                    AttackMoviePlay();
                                }
                            }
                            if (BattleObjectNum == 2)
                            {
                                BattlePhaseKey = 3;
                            }
                        }
                    }
                    else if (AttackArea == 100 && clickedGameObject.tag == "Piece")
                    {
                        for (int BattleObjectNum = 0; BattleObjectNum < 3; BattleObjectNum++)
                        {
                            if (PlayerBattleObject[EnemyPlayer, BattleObjectNum] != null)
                            {
                                if (clickedGameObject.transform.position.x == PlayerBattleObject[EnemyPlayer, BattleObjectNum].transform.position.x &&
                                    clickedGameObject.transform.position.z == PlayerBattleObject[EnemyPlayer, BattleObjectNum].transform.position.z)
                                {
                                    PlayerBattleObject[EnemyPlayer, BattleObjectNum].GetComponent<FigureScript>().helth -= 1;
                                    if (PlayerBattleObject[EnemyPlayer, BattleObjectNum].GetComponent<FigureScript>().helth <= 0)
                                    {
                                        Destroy(PlayerBattleObject[EnemyPlayer, BattleObjectNum]);
                                        GameSetJudge[EnemyPlayer, BattleObjectNum] = OK;
                                        EnemyDestroyFlag = true;
                                    }
                                    if (GameSetJudge[EnemyPlayer, 0] == OK &&
                                        GameSetJudge[EnemyPlayer, 1] == OK &&
                                        GameSetJudge[EnemyPlayer, 2] == OK)
                                    {

                                    }
                                    else
                                    {
                                        AttackMoviePlay();
                                    }
                                }
                            }
                            if (BattleObjectNum == 2)
                            {
                                BattlePhaseKey = 3;
                            }
                        }
                    }
                    else if (AttackAreaPiece[0] != null &&
                             AttackAreaPiece[1] != null &&
                             AttackAreaPiece[2] != null &&
                             AttackAreaPiece[3] != null)
                    {
                        if (clickedGameObject.name == AttackAreaPiece[0].name ||
                            clickedGameObject.name == AttackAreaPiece[1].name ||
                            clickedGameObject.name == AttackAreaPiece[2].name ||
                            clickedGameObject.name == AttackAreaPiece[3].name)
                        {
                            BattlePhaseKey = 3;
                        }
                    }
                }
            }
            else
            {
                BattlePhaseKey = 3;
                Debug.LogWarning("Not Find Target");
            }
        }
    }

    GameObject FindPiece(Vector3 SearchPiecePosition)
    {
        GameObject tempObject;
        GameObject CliticalObjectPiece = null;
        const int MaxPieceNum = 63;
        for (int CountPiece = 1; CountPiece <= MaxPieceNum; CountPiece++)
        {
            tempObject = GameObject.Find("" + CountPiece);
            if (tempObject != null) {
                if ((SearchPiecePosition.x == tempObject.transform.position.x) &&
                    (SearchPiecePosition.z == tempObject.transform.position.z))
                {
                    CliticalObjectPiece = tempObject;
                }
            }
        }
        return CliticalObjectPiece;
    }

    public void RaderSearch()
    {


        //GameObject RaderSetPiece = FindPiece(PlayerBattleObject[turnControll,3].transform.position);
        //GameObject RaderSetPieceTwo = FindPiece(PlayerBattleObject[turnControll,4].transform.position);
        //int[] TempIntPieceName = new int[25];
        //GameObject[] TempObjectPiece = new GameObject[25];Rader(Clone)

        int EnemyTurn = 0;

        if (turnControll == 0)
        {
            EnemyTurn = 1;
        }

        if (PlayerBattleObject[turnControll, 3] != null) {
            if (PlayerBattleObject[turnControll, 3].name == "Rader(Clone)")
            {
                Searcher(FindPiece(PlayerBattleObject[turnControll, 3].transform.position));
            }
        }
        if (PlayerBattleObject[turnControll, 4] != null) {
            if (PlayerBattleObject[turnControll, 4].name == "Rader(Clone)")
            {
                Searcher(FindPiece(PlayerBattleObject[turnControll, 4].transform.position));
            }
        }


        void Searcher(GameObject RaderSetPiece)
        {
            bool LEFT_UP_FLAG = false;
            bool RIGHT_UP_FLAG = false;
            bool RIGHT_DOWN_FLAG = false;
            bool LEFT_DOWN_FLAG = false;

            //近くに入ったね？判定
            for (int EnemyCounter = 0; EnemyCounter < 3; EnemyCounter++)
            {
                if (PlayerBattleObject[EnemyTurn, EnemyCounter] != null)
                {
                    for (int RaderAreaX = 3; RaderAreaX >= -3; RaderAreaX--)
                    {
                        for (int RaderAreaZ = 3; RaderAreaZ >= -3; RaderAreaZ--)
                        {
                            int judgeValue = RaderAreaX + RaderAreaZ;

                            if (judgeValue <= 3 && judgeValue >= -3)
                            {
                                if (RaderAreaX >= 0 && RaderAreaZ <= 0)
                                {

                                    //右下
                                    if ((judgeValue >= -2 && RaderAreaZ == -3) ||
                                        (judgeValue == 0 && RaderAreaX >= 2) ||
                                        (judgeValue <= 2 && RaderAreaX == 3))
                                    {

                                    }
                                    else
                                    {
                                        if (RaderSetPiece.transform.position.x == PlayerBattleObject[EnemyTurn, EnemyCounter].transform.position.x - RaderAreaX &&
                                            RaderSetPiece.transform.position.z == PlayerBattleObject[EnemyTurn, EnemyCounter].transform.position.z - RaderAreaZ)
                                        {
                                            RIGHT_DOWN_FLAG = true;
                                        }
                                    }
                                }
                                if (RaderAreaX <= 0 && RaderAreaZ >= 0)
                                {
                                    //左上
                                    if ((judgeValue >= -2 && RaderAreaX == -3) ||
                                        (judgeValue == 0 && RaderAreaZ >= 2) ||
                                        (judgeValue <= 2 && RaderAreaZ == 3))
                                    {

                                    }
                                    else
                                    {
                                        if (RaderSetPiece.transform.position.x == PlayerBattleObject[EnemyTurn, EnemyCounter].transform.position.x - RaderAreaX &&
                                            RaderSetPiece.transform.position.z == PlayerBattleObject[EnemyTurn, EnemyCounter].transform.position.z - RaderAreaZ)
                                        {
                                            LEFT_UP_FLAG = true;
                                        }
                                    }
                                }
                                if (RaderAreaX >= 0 && RaderAreaZ >= 0)
                                {
                                    //右上
                                    if (RaderSetPiece.transform.position.x == PlayerBattleObject[EnemyTurn, EnemyCounter].transform.position.x - RaderAreaX &&
                                        RaderSetPiece.transform.position.z == PlayerBattleObject[EnemyTurn, EnemyCounter].transform.position.z - RaderAreaZ)
                                    {
                                        RIGHT_UP_FLAG = true;
                                    }
                                }
                                if (RaderAreaX <= 0 && RaderAreaZ <= 0)
                                {
                                    //左下
                                    if (RaderSetPiece.transform.position.x == PlayerBattleObject[EnemyTurn, EnemyCounter].transform.position.x - RaderAreaX &&
                                        RaderSetPiece.transform.position.z == PlayerBattleObject[EnemyTurn, EnemyCounter].transform.position.z - RaderAreaZ)
                                    {
                                        LEFT_DOWN_FLAG = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            for (int RaderAreaX = 3; RaderAreaX >= -3; RaderAreaX--)
            {
                for (int RaderAreaZ = 3; RaderAreaZ >= -3; RaderAreaZ--)
                {
                    int judgeValue = RaderAreaX + RaderAreaZ;

                    if (judgeValue <= 3 && judgeValue >= -3)
                    {
                        GameObject RaderAreaPiece;
                        if (RaderAreaX >= 0 && RaderAreaZ <= 0 && RIGHT_DOWN_FLAG)
                        {
                            //右下
                            if ((judgeValue >= -2 && RaderAreaZ == -3) ||
                                (judgeValue == 0 && RaderAreaX >= 2) ||
                                (judgeValue <= 2 && RaderAreaX == 3))
                            {

                            }
                            else
                            {
                                RaderAreaPiece = FindPiece(RaderSetPiece.transform.position + new Vector3(RaderAreaX, 0, RaderAreaZ));
                                if (RaderAreaPiece != null)
                                {
                                    RaderAreaPiece.GetComponent<Renderer>().material = RaderPiece;
                                }
                            }
                        }
                        if (RaderAreaX <= 0 && RaderAreaZ >= 0 && LEFT_UP_FLAG)
                        {
                            //左上
                            if ((judgeValue >= -2 && RaderAreaX == -3) ||
                                (judgeValue == 0 && RaderAreaZ >= 2) ||
                                (judgeValue <= 2 && RaderAreaZ == 3))
                            {

                            }
                            else
                            {
                                RaderAreaPiece = FindPiece(RaderSetPiece.transform.position + new Vector3(RaderAreaX, 0, RaderAreaZ));
                                if (RaderAreaPiece != null)
                                {
                                    RaderAreaPiece.GetComponent<Renderer>().material = RaderPiece;
                                }
                            }
                        }
                        if (RaderAreaX >= 0 && RaderAreaZ >= 0 && RIGHT_UP_FLAG)
                        {
                            //右上
                            RaderAreaPiece = FindPiece(RaderSetPiece.transform.position + new Vector3(RaderAreaX, 0, RaderAreaZ));
                            if (RaderAreaPiece != null)
                            {
                                RaderAreaPiece.GetComponent<Renderer>().material = RaderPiece;
                            }
                        }
                        if (RaderAreaX <= 0 && RaderAreaZ <= 0 && LEFT_DOWN_FLAG)
                        {
                            //左下
                            RaderAreaPiece = FindPiece(RaderSetPiece.transform.position + new Vector3(RaderAreaX, 0, RaderAreaZ));
                            if (RaderAreaPiece != null)
                            {
                                RaderAreaPiece.GetComponent<Renderer>().material = RaderPiece;
                            }
                        }

                    }
                }
            }
        }
    }

    void SkillReset()
    {
        Debug.Log("スキルリセット");
        for (int i = 0; i < 4; i++)
        {
            if (SkillActiveFlag[i])
            {
                Debug.Log((i + 1) +"回目");
                if (i > 0) {
                    Debug.Log(PlayerBattleObject[turnControll, i - 1].name);
                    if (PlayerBattleObject[turnControll, i - 1].transform.name == "unitychan(Clone)")
                    {
                        PlayerBattleObject[turnControll, i - 1].GetComponent<FigureScript>().MovePower -= 1;
                        Debug.Log("ゆにリセット");
                    }
                    else if (PlayerBattleObject[turnControll, i - 1].transform.name == "PronamaChan(Clone)")
                    {
                        PlayerBattleObject[turnControll, i - 1].GetComponent<FigureScript>().AttackArea -= 1;
                        Debug.Log("ぷろなまりせっと");
                    }
                }
                SkillActiveFlag[i] = false;
            }
        }

        SkillIconOneEffect.SetActive(false);
        SkillIconTwoEffect.SetActive(false);
        SkillIconThreeEffect.SetActive(false);
    }

    public void SetSkillPictures()
    {
        Debug.Log("SetSkillPic");
        GameObject Canvas = GameObject.Find("Canvas");
        Pictures PicScript;
        PicScript = Canvas.GetComponent<Pictures>();

        Image[] SetSkillIcon = new Image[3];
        SetSkillIcon[0] = SkillIconOne.GetComponent<Image>();
        SetSkillIcon[1] = SkillIconTwo.GetComponent<Image>();
        SetSkillIcon[2] = SkillIconThree.GetComponent<Image>();

        for (int i = 0; i < 3; i++)
        {
            if (PlayerBattleObject[turnControll, i] != null)
            {
                if (PlayerBattleObject[turnControll, i].GetComponent<FigureScript>().helth != 0) 
                {
                    SetSkillIcon[i].sprite = SettingIcon(PlayerBattleObject[turnControll, i]);
                }
                else
                {
                    SetSkillIcon[i].sprite = PicScript.DeletePic;
                }
            }
            else{
                SetSkillIcon[i].sprite = PicScript.DeletePic;
            }
        }

        SettingEffectIcons(SkillIconOne,0);
        SettingEffectIcons(SkillIconTwo,1);
        SettingEffectIcons(SkillIconThree,2);

        Sprite SettingIcon(GameObject tempObject)
        {
            switch (tempObject.name)
            {
                case "Alicia_solid(Clone)":
                    return PicScript.NicoChanPic;
                case "unitychan(Clone)":
                    return PicScript.UnityChanPic;
                case "PronamaChan(Clone)":
                    return PicScript.ProNamaChanPic;
            }
            return PicScript.MasterPic;
        }

        void SettingEffectIcons(GameObject TempIcons,int RoleNum)
        {
            GameObject SkillEffects = TempIcons.transform.GetChild(0).gameObject;
            GameObject RoleIcons = TempIcons.transform.GetChild(1).gameObject;

            if (TempIcons.GetComponent<Image>().sprite.name == PicScript.NicoChanPic.name)
            {
                SkillEffects.GetComponent<Image>().sprite = PicScript.HelthUpPic;;
            }
            else if (TempIcons.GetComponent<Image>().sprite.name == PicScript.UnityChanPic.name)
            {
                SkillEffects.GetComponent<Image>().sprite = PicScript.MoveUpPic;
            }
            else if (TempIcons.GetComponent<Image>().sprite.name == PicScript.ProNamaChanPic.name)
            {
                SkillEffects.GetComponent<Image>().sprite = PicScript.AttackAreaPic;
            }

            if (PlayerBattleObject[turnControll, RoleNum] != null) 
            {
                RoleIcons.SetActive(true);
                switch (PlayerBattleObject[turnControll, RoleNum].GetComponent<FigureScript>().ClassNum)
                {
                    case 0:
                        RoleIcons.GetComponent<Image>().sprite = PicScript.SoliderPic;
                        break;
                    case 1:
                        RoleIcons.GetComponent<Image>().sprite = PicScript.AssaultPic;
                        break;
                    case 2:
                        RoleIcons.GetComponent<Image>().sprite = PicScript.SnipePic;
                        break;
                }

                if (PlayerBattleObject[turnControll, RoleNum].GetComponent<FigureScript>().helth == 0)
                {
                    RoleIcons.SetActive(false);
                }
            }
            else
            {
                RoleIcons.SetActive(false);
            }
        }
    }

    void SettingIconScript(int NowTurnPlayer)
    {
        GameObject Canvas = GameObject.Find("Canvas");
        Pictures PicScript;
        PicScript = Canvas.GetComponent<Pictures>();

        /*if (NowTurnPlayer == 0)
        {
            Icon_Niko.GetComponent<Image>().sprite = SettingUnitIcons(DeckSelectScript.FirstPlayerDeck.FirstUnit.UnitNum);
            Icon_UnityChan.GetComponent<Image>().sprite = SettingUnitIcons(DeckSelectScript.FirstPlayerDeck.SecondUnit.UnitNum);
            Icon_Pronama.GetComponent<Image>().sprite = SettingUnitIcons(DeckSelectScript.FirstPlayerDeck.ThirdUnit.UnitNum);
            Icon_Rader.GetComponent<Image>().sprite = SettingWepIcons(DeckSelectScript.FirstPlayerDeck.WepNumOne);
            Icon_Mine.GetComponent<Image>().sprite = SettingWepIcons(DeckSelectScript.FirstPlayerDeck.WepNumTwo);
        }
        else if (NowTurnPlayer == 1)
        {
            Icon_Niko.GetComponent<Image>().sprite = SettingUnitIcons(DeckSelectScript.SecondPlayerDeck.FirstUnit.UnitNum);
            Icon_UnityChan.GetComponent<Image>().sprite = SettingUnitIcons(DeckSelectScript.SecondPlayerDeck.SecondUnit.UnitNum);
            Icon_Pronama.GetComponent<Image>().sprite = SettingUnitIcons(DeckSelectScript.SecondPlayerDeck.ThirdUnit.UnitNum);
            Icon_Rader.GetComponent<Image>().sprite = SettingWepIcons(DeckSelectScript.SecondPlayerDeck.WepNumOne);
            Icon_Mine.GetComponent<Image>().sprite = SettingWepIcons(DeckSelectScript.SecondPlayerDeck.WepNumTwo);
        }
        else 
        {
            Debug.Log("error");
        }*/

        Icon_Niko.GetComponent<Image>().sprite = SettingUnitIcons(MyDeckData.FirstUnit.UnitNum);
        Icon_UnityChan.GetComponent<Image>().sprite = SettingUnitIcons(MyDeckData.SecondUnit.UnitNum);
        Icon_Pronama.GetComponent<Image>().sprite = SettingUnitIcons(MyDeckData.ThirdUnit.UnitNum);
        Icon_Rader.GetComponent<Image>().sprite = SettingWepIcons(MyDeckData.WepNumOne);
        Icon_Mine.GetComponent<Image>().sprite = SettingWepIcons(MyDeckData.WepNumTwo);

        Sprite SettingUnitIcons(int UnitNumber)
        {
            switch (UnitNumber)
            {
                case 0:
                    return PicScript.NicoChanPic;
                case 1:
                    return PicScript.UnityChanPic;
                case 2:
                    return PicScript.ProNamaChanPic;
                default:
                    return PicScript.MasterPic;
            }
        }
        Sprite SettingWepIcons(int UnitNumber)
        {
            switch (UnitNumber)
            {
                case 0:
                    return PicScript.RaderPic;
                case 1:
                    return PicScript.MinePic;
                default:
                    return PicScript.MasterPic;
            }
        }
    }

    public void HelthColorChange()
    {
        GameObject TempUnit;
        for (int ObjectSearchLoop = 0; ObjectSearchLoop < 3; ObjectSearchLoop++)
        {
            if (PlayerBattleObject[turnControll,ObjectSearchLoop] != null)
            {
                Debug.Log(ObjectSearchLoop+"回目");
                Debug.Log(PlayerBattleObject[turnControll, ObjectSearchLoop].name + "の残りHP:"+ PlayerBattleObject[turnControll, ObjectSearchLoop].GetComponent<FigureScript>().helth);
                switch (ObjectSearchLoop)
                {
                    case 0:
                        TempUnit = SkillIconOne.transform.parent.gameObject;
                        ColorChange(PlayerBattleObject[turnControll, ObjectSearchLoop], ref TempUnit);
                        break;
                    case 1:
                        TempUnit = SkillIconTwo.transform.parent.gameObject;
                        ColorChange(PlayerBattleObject[turnControll, ObjectSearchLoop], ref TempUnit);
                        break;
                    case 2:
                        TempUnit = SkillIconThree.transform.parent.gameObject;
                        ColorChange(PlayerBattleObject[turnControll, ObjectSearchLoop], ref TempUnit);
                        break;
                }

            }
            else
            {
                switch (ObjectSearchLoop)
                {
                    case 0:
                        SkillIconOne.transform.parent.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 1);
                        break;
                    case 1:
                        SkillIconTwo.transform.parent.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 1);
                        break;
                    case 2:
                        SkillIconThree.transform.parent.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 1);
                        break;
                }
            }
        }
        void ColorChange(GameObject PlayerObject, ref GameObject HelthObject)
        {
            Debug.Log(HelthObject.name);
            if (PlayerObject.GetComponent<FigureScript>().helth >= 3)
            {
                Debug.Log("3以上なので青く");
                HelthObject.GetComponent<Image>().color = new Color(30f/255f, 144f / 255f, 255f / 255f, 1);
            }
            else if (PlayerObject.GetComponent<FigureScript>().helth == 2)
            {
                Debug.Log("2なので緑へ");
                HelthObject.GetComponent<Image>().color = new Color(46f / 255f, 139f / 255f, 87f / 255f, 1);
            }
            else if(PlayerObject.GetComponent<FigureScript>().helth == 1)
            {
                Debug.Log("1なので赤く");
                HelthObject.GetComponent<Image>().color = new Color(220f / 255f, 20f / 255f, 60f / 255f, 1);
            }
            else if(PlayerObject.GetComponent<FigureScript>().helth <= 0)
            {
                Debug.Log("0以下なので真っ白");
                HelthObject.GetComponent<Image>().color = new Color(255, 255, 255,1);
            }
            else
            {
                Debug.Log("ここはありえないはず");
            }

        }
    }
    void AttackedMovie(GameObject AttackFigure)
    {
        switch (AttackFigure.name)
        {
            case "Alicia_solid(Clone)":
                switch (AttackFigure.GetComponent<FigureScript>().ClassNum)
                {
                    case 0: //歩
                        //AttackMovie.GetComponent<VideoPlayer>().clip = NicoHandMovie;
                        VideoPlayer.GetComponent<VideoPlayer>().clip = NicoHandMovie;
                        break;
                    case 1: //突
                        //AttackMovie.GetComponent<VideoPlayer>().clip = NicoAssultMovie;
                        VideoPlayer.GetComponent<VideoPlayer>().clip = NicoAssultMovie;
                        break;
                    case 2:　//狙
                        //AttackMovie.GetComponent<VideoPlayer>().clip = NicoSniperMovie;
                        VideoPlayer.GetComponent<VideoPlayer>().clip = NicoSniperMovie;
                        break;
                }
                break;
            case "unitychan(Clone)":
                switch (AttackFigure.GetComponent<FigureScript>().ClassNum)
                {
                    case 0: //歩
                        //AttackMovie.GetComponent<VideoPlayer>().clip = UnityHandMovie;
                        VideoPlayer.GetComponent<VideoPlayer>().clip = UnityHandMovie;
                        break;
                    case 1: //突
                        //AttackMovie.GetComponent<VideoPlayer>().clip = UnityAssultMovie;
                        VideoPlayer.GetComponent<VideoPlayer>().clip = UnityAssultMovie;
                        break;
                    case 2:　//狙
                        //AttackMovie.GetComponent<VideoPlayer>().clip = UnitySniperMovie;
                        VideoPlayer.GetComponent<VideoPlayer>().clip = UnitySniperMovie;
                        break;
                }
                break;
            case "PronamaChan(Clone)":
                switch (AttackFigure.GetComponent<FigureScript>().ClassNum)
                {
                    case 0: //歩
                        //AttackMovie.GetComponent<VideoPlayer>().clip = PronamaHandMovie;
                        VideoPlayer.GetComponent<VideoPlayer>().clip = PronamaHandMovie;
                        break;
                    case 1: //突
                        //AttackMovie.GetComponent<VideoPlayer>().clip = PronamaAssultMovie;
                        VideoPlayer.GetComponent<VideoPlayer>().clip = PronamaAssultMovie;
                        break;
                    case 2:　//狙
                        //AttackMovie.GetComponent<VideoPlayer>().clip = PronamaSniperMovie;
                        VideoPlayer.GetComponent<VideoPlayer>().clip = PronamaSniperMovie;
                        break;
                }
                break;
        }
    }
    async void AttackMoviePlay() 
    {
        if (turnControll == 0)
        {
            VideoPlayer.GetComponent<VideoPlayer>().targetCamera = F_MainCamera.GetComponent<Camera>();
        }
        else
        {
            VideoPlayer.GetComponent<VideoPlayer>().targetCamera = S_MainCamera.GetComponent<Camera>();
        }

        Debug.Log("VideoName:"+ VideoPlayer.GetComponent<VideoPlayer>().clip);
        GUICanvas.SetActive(false);
        PieceField.SetActive(false);
        BackField.SetActive(false);

        VideoPlayer.SetActive(true);

        VideoPlayer.GetComponent<VideoPlayer>().Play();
        await Task.Delay(5000);
        if (EnemyDestroyFlag) Source_SE.PlayOneShot(bomb_SE);
        await Task.Delay(500);
        GUICanvas.SetActive(true);
        PieceField.SetActive(true);
        BackField.SetActive(true);


        VideoPlayer.SetActive(false);
        VideoPlayer.GetComponent<VideoPlayer>().Prepare();
    }

    void DataUpLoad()
    {
        battleObjectData SaveObjectData = new battleObjectData();
        //自オブジェクト
        if (PlayerBattleObject[turnControll, 0] != null)
        {
            SaveObjectData.BattleObject1Helth = PlayerBattleObject[turnControll, 0].GetComponent<FigureScript>().helth;
            SaveObjectData.BattleObject1Name = PlayerBattleObject[turnControll, 0].name;
            SaveObjectData.BattleObject1Position = PlayerBattleObject[turnControll, 0].transform.position;
        }
        else
        {
            SaveObjectData.BattleObject1Helth = 0;
        }
        if (PlayerBattleObject[turnControll, 1] != null)
        {
            SaveObjectData.BattleObject2Helth = PlayerBattleObject[turnControll, 1].GetComponent<FigureScript>().helth;
            SaveObjectData.BattleObject2Name = PlayerBattleObject[turnControll, 1].name;
            SaveObjectData.BattleObject2Position = PlayerBattleObject[turnControll, 1].transform.position;
        }
        else
        {
            SaveObjectData.BattleObject2Helth = 0;
        }
        if (PlayerBattleObject[turnControll, 2] != null)
        {
            SaveObjectData.BattleObject3Helth = PlayerBattleObject[turnControll, 2].GetComponent<FigureScript>().helth;
            SaveObjectData.BattleObject3Name = PlayerBattleObject[turnControll, 2].name;
            SaveObjectData.BattleObject3Position = PlayerBattleObject[turnControll, 2].transform.position;
        }
        else
        {
            SaveObjectData.BattleObject3Helth = 0;
        }
        if (PlayerBattleObject[turnControll, 3] != null)
        {
            SaveObjectData.BattleUnit1Name = PlayerBattleObject[turnControll, 3].name;
            SaveObjectData.BattleUnit1Position = PlayerBattleObject[turnControll, 3].transform.position;
        }
        else
        {
            SaveObjectData.BattleUnit1Position = new Vector3(5f,5f,5f);
        }
        if (PlayerBattleObject[turnControll, 4] != null)
        {
            SaveObjectData.BattleUnit2Name = PlayerBattleObject[turnControll, 4].name;
            SaveObjectData.BattleUnit2Position = PlayerBattleObject[turnControll, 4].transform.position;
        }
        else
        {
            SaveObjectData.BattleUnit2Position = new Vector3(5f, 5f, 5f);
        }
        if (PlayerBattleObject[turnControll, 5] != null)
        {
            SaveObjectData.HomePosition = PlayerBattleObject[turnControll, 5].transform.position;
        }
        else
        {
            OnlineRoomClass["Winner"] = (1 - turnControll).ToString();
        }

        string SaveObjectJson = JsonUtility.ToJson(SaveObjectData);
        Debug.Log(SaveObjectJson);
        if (turnControll == 0)
        {
            OnlineRoomClass["Player1GameObjectData"] = SaveObjectJson;
            OnlineRoomClass["Player1DataSaveFlag"] = "true";
        }
        else
        {
            OnlineRoomClass["Player2GameObjectData"] = SaveObjectJson;
            OnlineRoomClass["Player2DataSaveFlag"] = "true";
        }

        if (PhaseControll != 0)
        {
            Debug.Log("えらー2");
            if (OnlineRoomClass["Player1DataSaveFlag"].ToString() == "true" && OnlineRoomClass["Player2DataSaveFlag"].ToString() == "true")
            {

                //相手オブジェクト
                if (PlayerBattleObject[1 - turnControll, 0] != null)
                {
                    SaveObjectData.BattleObject1Helth = PlayerBattleObject[1 - turnControll, 0].GetComponent<FigureScript>().helth;
                    SaveObjectData.BattleObject1Name = PlayerBattleObject[1 - turnControll, 0].name;
                    SaveObjectData.BattleObject1Position = PlayerBattleObject[1 - turnControll, 0].transform.position;
                }
                else
                {
                    SaveObjectData.BattleObject1Helth = 0;
                }
                if (PlayerBattleObject[1 - turnControll, 1] != null)
                {
                    SaveObjectData.BattleObject2Helth = PlayerBattleObject[1 - turnControll, 1].GetComponent<FigureScript>().helth;
                    SaveObjectData.BattleObject2Name = PlayerBattleObject[1 - turnControll, 1].name;
                    SaveObjectData.BattleObject2Position = PlayerBattleObject[1 - turnControll, 1].transform.position;
                }
                else
                {
                    SaveObjectData.BattleObject2Helth = 0;
                }
                if (PlayerBattleObject[1 - turnControll, 2] != null)
                {
                    SaveObjectData.BattleObject3Helth = PlayerBattleObject[1 - turnControll, 2].GetComponent<FigureScript>().helth;
                    SaveObjectData.BattleObject3Name = PlayerBattleObject[1 - turnControll, 2].name;
                    SaveObjectData.BattleObject3Position = PlayerBattleObject[1 - turnControll, 2].transform.position;
                }
                else
                {
                    SaveObjectData.BattleObject3Helth = 0;
                }
                if (PlayerBattleObject[1 - turnControll, 3] != null)
                {
                    SaveObjectData.BattleUnit1Name = PlayerBattleObject[1 - turnControll, 3].name;
                    SaveObjectData.BattleUnit1Position = PlayerBattleObject[1 - turnControll, 3].transform.position;
                }
                else
                {
                    SaveObjectData.BattleUnit1Position = new Vector3(5f, 5f, 5f);
                }
                if (PlayerBattleObject[1 - turnControll, 4] != null)
                {
                    SaveObjectData.BattleUnit2Name = PlayerBattleObject[1 - turnControll, 4].name;
                    SaveObjectData.BattleUnit2Position = PlayerBattleObject[1 - turnControll, 4].transform.position;
                }
                else
                {
                    SaveObjectData.BattleUnit2Position = new Vector3(5f, 5f, 5f);
                }
                if (PlayerBattleObject[1 - turnControll, 5] != null)
                {
                    SaveObjectData.HomePosition = PlayerBattleObject[1 - turnControll, 5].transform.position;
                }
                else
                {
                    OnlineRoomClass["Winner"] = (turnControll).ToString();
                }

                SaveObjectJson = JsonUtility.ToJson(SaveObjectData);
                Debug.Log(SaveObjectJson);
                if (turnControll != 0)
                {
                    OnlineRoomClass["Player1GameObjectData"] = SaveObjectJson;
                }
                else
                {
                    OnlineRoomClass["Player2GameObjectData"] = SaveObjectJson;
                }
            }
        }
        Debug.Log("えらー１");
        OnlineRoomClass.SaveAsync();
        Debug.Log("NowUploaded");
    }
}
