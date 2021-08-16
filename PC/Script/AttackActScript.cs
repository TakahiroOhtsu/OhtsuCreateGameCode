using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class AttackActScript : MonoBehaviourPunCallbacks
{
    private Animator W_animator;
    // Start is called before the first frame update

    private bool keyIsBlock = false; //キー入力ブロックフラグ
    private DateTime pressedKeyTime; //前回キー入力された時間
    private TimeSpan elapsedTime; //キー入力されてからの経過時間
    private TimeSpan blockTime = new TimeSpan(0, 0, 1); //ブロックする時間　1s

    private static Hashtable hashtable = new Hashtable();

    float NowSpeed = 0f;

    public GameObject KillerUI;
    bool CreateUIFlag = false;
    bool KillerIconSetFlag = false;
    int NowIconSetNum = 0;
    bool KillerChallengeFlag = true;
    int KillerChallengeLevel = 0;
    public GameObject TimeLimitSlider;
    bool CountStartFlag = false;
    float TimeLimit = 0;
    float NowTime = 0;
    int PushKeyCount = 0; //押された数をカウントする。

    public GameObject PushUI = null;
    GameObject[] KillerPushKeyIcon = new GameObject[4];//0:上 1:下: 2:右 3:左
    PhotonView myPV;
    GameObject TargetObject;
    Character_Controll CCScript;
    Animator animatior;
    bool DamageFlagChangeChecker = false;

    public AudioClip AttackSE;
    AudioSource audioSource;

    void Start()
    {
        myPV = this.GetComponent<Character_Controll>().myPV;
        W_animator = GetComponent<Animator>();
        NowSpeed = this.GetComponent<Character_Controll>().moveSpeed;
        Debug.Log("A:" + NowSpeed + "/ B:" + this.GetComponent<Character_Controll>().moveSpeed);
        animatior = this.gameObject.GetComponent<Animator>();
        audioSource = this.transform.root.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!myPV.IsMine)
        {
            return;
        }

        //Debug.LogWarning("KillerChallengeFlag2:" + KillerChallengeFlag);
        if (KillerChallengeFlag && PushUI != null)
        {
            Debug.LogWarning("呼ばれちゃう");
            KillerResistMotion();
            return;
        }
        CCScript = this.gameObject.GetComponent<Character_Controll>();
        CCScript.MoveLock = false;
        if (PhotonNetwork.CurrentRoom.CustomProperties["SurviverDamageFlag"] is bool SurviverDamageFlag)
        {
            Debug.LogWarning("SurviverDamageFlag:" + SurviverDamageFlag);
            Debug.LogWarning("KillerChallengeFlag:" + KillerChallengeFlag);
            
            if (DamageFlagChangeChecker != SurviverDamageFlag)
            {
                DamageFlagChangeChecker = SurviverDamageFlag;
                if (SurviverDamageFlag)
                {
                    CCScript.MoveLock = true;//移動ロックON
                    animatior.SetBool("Hit", true);
                    if (SurviverDamageFlag && PhotonNetwork.CurrentRoom.CustomProperties["ChangeKillerNum"] is int ChangeKillerNum && PushUI == null)
                    {
                        if (ChangeKillerNum == this.gameObject.GetComponent<PhotonView>().CreatorActorNr)
                        {
                            float CenterWitdth = Screen.width / 2;
                            float CenterHeight = Screen.height / 2;
                            Debug.LogWarning("Width:" + Screen.width + "/ Height:" + Screen.height);
                            GameObject Canvas = GameObject.Find("Canvas");
                            PushUI = Instantiate(KillerUI, new Vector3(CenterWitdth, CenterHeight, 0), Canvas.transform.rotation);
                            PushUI.transform.parent = Canvas.transform;
                            for (int i = 0; i < 4; i++)
                            {
                                KillerPushKeyIcon[i] = PushUI.transform.GetChild(i).gameObject;
                                KillerPushKeyIcon[i].SetActive(false);
                            }
                            TimeLimitSlider = PushUI.transform.GetChild(4).gameObject;
                        }
                    }
                    KillerChallengeFlag = true;
                    KillerIconSetFlag = false;
                }
            }
            
        }
        /*
        if (keyIsBlock)
        {
            elapsedTime = DateTime.Now - pressedKeyTime;
            if (elapsedTime > blockTime)
            {
                keyIsBlock = false;
            }
            else
            {
                return;
            }
        }*/

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

    }
    public void AttackEnd()
    {
        W_animator.SetBool("IsAttack", false);
        this.GetComponent<Character_Controll>().moveSpeed = NowSpeed;
        hashtable["KillerAttackFlag"] = false;
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        hashtable.Clear();
    }

    void Attack()
    {
        this.GetComponent<Character_Controll>().moveSpeed = NowSpeed / 5;
        W_animator.SetBool("IsAttack", true);
        hashtable["KillerAttackFlag"] = true;
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        hashtable.Clear();
        audioSource.PlayOneShot(AttackSE);
        Invoke("AttackEnd", 1f);
        keyIsBlock = true;
        pressedKeyTime = DateTime.Now;
    }

    void KillerResistMotion() 
    {
        Debug.LogWarning("Why?");
        if (KillerChallengeFlag)
        {

            Debug.LogWarning("Why??");

            if (KillerIconSetFlag == false)
            {

                Debug.LogWarning("Why???");
                int min = 0;
                int max = 4;
                NowIconSetNum = UnityEngine.Random.Range(min, max);
                KillerPushKeyIcon[0].SetActive(false);
                KillerPushKeyIcon[1].SetActive(false);
                KillerPushKeyIcon[2].SetActive(false);
                KillerPushKeyIcon[3].SetActive(false);
                KillerPushKeyIcon[NowIconSetNum].SetActive(true);
                KillerIconSetFlag = true;
            }

            if (PhotonNetwork.CurrentRoom.CustomProperties["KillerResistIntervalLevel"] is int KillerResistIntervalLevel)
            {
                KillerChallengeLevel = KillerResistIntervalLevel;
            }
            else
            {
                Debug.Log("KillerResistIntervalLevelは取得できなかった");
            }
            if (CountStartFlag == false)
            {

                Debug.LogWarning("Why????");
                switch (KillerChallengeLevel)
                {
                    case 0: //長時間立ってる場合
                        KillerChallenge(2f);
                        break;
                    case 1: //1秒~1.5秒
                        KillerChallenge(1.5f);
                        break;
                    case 2: //0.5~1
                        KillerChallenge(1f);
                        break;
                    case 3: //0.5秒以内
                        KillerChallenge(0.5f);
                        break;
                }
            }
            if (NowTime <= TimeLimit)
            {

                Debug.LogWarning("Why?????");
                //時間以内であるなら
                PushCheck();
                NowTime = NowTime + Time.deltaTime;
            }
            else
            {

                Debug.LogWarning("Why??????");
                //時間以内に押せなかった場合
                NowTime = 0;
                Debug.LogWarning("時間以内に押せなかった");
                CountStartFlag = false;
                KillerIconSetFlag = false;
                KillerChallengeFlag = false;
                PushKeyCount = 0;
                hashtable["DamageHit"] = 2; //0:判定中 1:クリティカル　2: ヒット

                EndFlagSend();

            }
            TimeLimitSlider.GetComponent<Slider>().value = NowTime;
        }
        void KillerChallenge(float CountTime) 
        {
            NowTime = 0;
            TimeLimit = CountTime;
            CountStartFlag = true;

        }
        void PushCheck() {
            switch (NowIconSetNum)
            {
                case 0:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        Debug.LogWarning("押された:W");
                        PushAction();
                    }
                    else if(Input.anyKeyDown)
                    {
                        NowTime = 0;
                        Debug.LogWarning("W以外が押された");
                        CountStartFlag = false;
                        KillerIconSetFlag = false;
                        KillerChallengeFlag = false;
                        PushKeyCount = 0;
                        hashtable["DamageHit"] = 2; //0:判定中 1:クリティカル　2: ヒット
                        EndFlagSend();
                    }
                    break;
                case 1:
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        Debug.LogWarning("押された:S");
                        PushAction();
                    }
                    else if (Input.anyKeyDown)
                    {
                        NowTime = 0;
                        Debug.LogWarning("S以外が押された");
                        CountStartFlag = false;
                        KillerIconSetFlag = false;
                        KillerChallengeFlag = false;
                        PushKeyCount = 0;
                        hashtable["DamageHit"] = 2; //0:判定中 1:クリティカル　2: ヒット
                        EndFlagSend();
                    }
                    break;
                case 2:
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        Debug.LogWarning("押された:D");
                        PushAction();
                    }
                    else if (Input.anyKeyDown)
                    {
                        NowTime = 0;
                        Debug.LogWarning("A以外が押された");
                        CountStartFlag = false;
                        KillerIconSetFlag = false;
                        KillerChallengeFlag = false;
                        PushKeyCount = 0;
                        hashtable["DamageHit"] = 2; //0:判定中 1:クリティカル　2: ヒット
                        EndFlagSend();
                    }
                    break;
                case 3:
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        Debug.LogWarning("押された:A");
                        PushAction();
                    }
                    else if (Input.anyKeyDown)
                    {
                        NowTime = 0;
                        Debug.LogWarning("A以外が押された");
                        CountStartFlag = false;
                        KillerIconSetFlag = false;
                        KillerChallengeFlag = false;
                        PushKeyCount = 0;
                        hashtable["DamageHit"] = 2; //0:判定中 1:クリティカル　2: ヒット
                        EndFlagSend();
                    }
                    break;
                default:
                    break;

            }
        }
        void PushAction()
        {
            NowTime = 0;
            CountStartFlag = false;
            KillerIconSetFlag = false;
            if (PushKeyCount >= 10)//10回まで繰り返す
            {
                Debug.LogWarning("成功");
                PushKeyCount = 0;
                hashtable["DamageHit"] = 1;//0:判定中 1:クリティカル　2: ヒット

                EndFlagSend();
            }
            else
            {
                KillerPushKeyIcon[NowIconSetNum].SetActive(false);
                PushKeyCount++;
            }
                

        }
        void EndFlagSend()
        {
            Destroy(PushUI,0.1f);

            KillerChallengeFlag = false;
            CCScript.MoveLock = false; //移動ロックOFF
            hashtable["SurviverDamageFlag"] = false;
            animatior.SetBool("Hit", false);
            CreateUIFlag = false;
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
            hashtable.Clear();
        }
    }


}