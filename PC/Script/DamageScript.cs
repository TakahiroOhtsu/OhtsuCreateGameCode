using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class DamageScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool MoveLock = false;                  //移動ロックフラグ
    public bool AttackLock = false;                //連射防止用攻撃ロックフラグ
    public bool invincible = false;                //無敵フラグ
    public bool Deadflag = false;                   //死亡フラグ
    GameObject AliveSettingObject;

    private static Hashtable hashtable = new Hashtable();

    public GameObject DamagePanel;

    Animator animator;

    PhotonView myPV;
    PhotonTransformView myPTV;

    bool Resist_Flag = false;
    bool CountTimeFlag = false;

    GameObject LeftKey;
    GameObject RightKey;

    GameObject PushUI = null;

    float SetTime;

    public AudioClip DamageSE;
    AudioSource audioSource;

    private void Start()
    {
        myPV = this.GetComponent<Character_Controll>().myPV;
        myPTV = this.GetComponent<Character_Controll>().myPTV;
        animator = GetComponent<Animator>();
        AliveSettingObject = GameObject.Find("AliveSetting");
        audioSource = this.transform.root.gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        Resist_Challange();
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.LogWarning("キラーが攻撃して");
        // 自キャラ以外なら処理しない
        if (!myPV.IsMine)
        {
            return;
        }
        Debug.LogWarning("キラーが攻撃してき");
        if (Deadflag || invincible) //死亡時または無敵時は処理しない
        {
            return;
        }

        Debug.LogWarning("キラーが攻撃してきた");
        //キラーがまだ存在しないなら
        if (this.GetComponent<Character_Controll>().KillerObject == null)
        {
            return;
        }
        Debug.LogWarning("キラーが攻撃してきた！");
        //キラーが攻撃してきたら
        if (col.gameObject == this.GetComponent<Character_Controll>().KillerObject)
        {
            
            //ダメージを与える
            Debug.LogWarning("ダメージ："　+ CLocalVariables.currentHP);

            //攻撃側プレイヤーのkillcount++処理
            if (CLocalVariables.currentHP > 0)
            {
                myPV.RPC("Damaged", RpcTarget.AllViaServer);  //被弾処理RPC
                //StartCoroutine(_rigor(.5f));    //被弾硬直処理
                DamageUI_ON();
                Resist_Flag = true;
            }
        }
        else
        {
            
        }
    }
    //被弾処理同期用RPC
    [PunRPC]
    void Damaged()
    {
        audioSource.PlayOneShot(DamageSE);
        MoveLock = true;    //硬直のため移動ロックON
        this.gameObject.GetComponent<Character_Controll>().MoveLock = true;    //硬直のため移動ロックON
        animator.SetBool("IsResist",true);
        hashtable["SurviverDamageFlag"] = true;
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        hashtable.Clear();

    }

    [PunRPC]
    void DamagedAnimationReject()
    {
        animator.SetBool("IsResist", false);
    }
    //ヒット時硬直処理
    IEnumerator _rigor(float pausetime)
    {
        yield return new WaitForSeconds(pausetime); //倒れている時間
        //animator.SetBool("IsResist", false);
        Debug.Log("よんだ？");
        //MoveLock = false;   //移動ロック解除
        
    }

    //死亡処理同期用RPC
    [PunRPC]
    void Dead()
    {
        Deadflag = true;    //死亡フラグON
        AttackLock = true;  //攻撃ロックON
        MoveLock = true;    //移動ロックON
        this.gameObject.GetComponent<Character_Controll>().MoveLock = true;//移動ロックON
        this.gameObject.transform.root.gameObject.SetActive(false);
        AliveSettingObject.GetComponent<AliveSettingScript>().AliveFlag[this.gameObject.GetComponent<PhotonView>().CreatorActorNr - 1] = false; //生きてるフラグさよなら
        AliveSettingObject.GetComponent<AliveSettingScript>().AlivePlayerCount = AliveSettingObject.GetComponent<AliveSettingScript>().AlivePlayerCount - 1;
        AliveSettingObject.GetComponent<ResultChecker>().ResultChecking();
        //animator.SetTrigger("DeathTrigger");    //死亡アニメーションON
    }

    void DamageUI_ON()
    {
        if (PushUI == null) 
        {
            PushUI = Instantiate(DamagePanel, transform.position, transform.rotation);
            GameObject Canvas = GameObject.Find("Canvas");
            PushUI.transform.parent = Canvas.transform;
            PushUI.GetComponent<RectTransform>().offsetMin = new Vector2(100, 0);
            PushUI.GetComponent<RectTransform>().offsetMax = new Vector2(-100, -200);
            LeftKey = PushUI.transform.GetChild(0).gameObject;
            RightKey = PushUI.transform.GetChild(1).gameObject;
            RightKey.SetActive(false);
        }
        else
        {
            PushUI.SetActive(true);
            LeftKey.SetActive(true);
            RightKey.SetActive(true);
        }
    }

    void Resist_Challange()
    {
        if (Resist_Flag)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties["DamageHit"] is int DamageHit)
            {
                if (DamageHit > 0)
                {
                    if (DamageHit > 1)//ヒット時
                    {
                        CLocalVariables.currentHP -= 50;
                    }
                    else //クリティカル
                    {
                        CLocalVariables.currentHP -= 100;
                    }
                    myPV.RPC("DamagedAnimationReject", RpcTarget.AllViaServer);
                    hashtable["DamageHit"] = 0;
                    hashtable["SurviverDamageFlag"] = false;
                    Resist_Flag = false;
                    Debug.LogWarning("わおん2");
                    PushUI.SetActive(false);
                    this.gameObject.GetComponent<Character_Controll>().MoveLock = false;//移動ロックON
                    PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
                    hashtable.Clear();
                    if (CLocalVariables.currentHP <= 0)
                    {
                        myPV.RPC("Dead", RpcTarget.AllViaServer);    //死亡処理RPC
                        Camera.main.GetComponent<Player_Fallow_camera>().targetObj = this.GetComponent<Character_Controll>().KillerObject;//カメラはやられた個人のみに適用
                    }
                }
            }
            if (RightKey.activeSelf && Input.GetKeyDown(KeyCode.RightArrow))
            {
                RightKey.SetActive(false);
                LeftKey.SetActive(true);
                CountTimeFlag = true;
                SetTime = 0;
            }
            if (LeftKey.activeSelf && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                RightKey.SetActive(true);
                LeftKey.SetActive(false);
                Debug.LogWarning("入力感覚："+ SetTime);
                CountTimeFlag = false;
                int KillerResistIntervalLevel;
                if (SetTime <= 1.5f)
                {
                    KillerResistIntervalLevel = 1;
                    if (SetTime <= 1f)
                    {
                        KillerResistIntervalLevel = 2;
                        if (SetTime <= 0.5f)
                        {
                            KillerResistIntervalLevel = 3;
                        }
                    }
                }
                else
                {
                    KillerResistIntervalLevel = 0;
                }
                hashtable["KillerResistIntervalLevel"] = KillerResistIntervalLevel;
                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
                hashtable.Clear();
            }
        }
        if (CountTimeFlag)
        {
            SetTime = SetTime + Time.deltaTime;
        }
    }
}
