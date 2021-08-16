using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Character_Controll : MonoBehaviourPunCallbacks
{
    float inputHorizontal;
    float inputVertical;
    Rigidbody rb;
    private Animator S_animator;
    private Vector3 velocity;

    public float moveSpeed = 20f;
    [SerializeField] private float applySpeed = 0.2f;

    //オンライン化に必要なコンポーネントを設定
    public PhotonView myPV;
    public PhotonTransformView myPTV;
    private Camera mainCam;

    private static Hashtable hashtable = new Hashtable();
    bool transformFlag = false;
    string targetKillerID = null;

    public GameObject KillerObject = null;
    public bool MoveLock = false;
    GameObject GameManager;
    GameObject AliveSettingObject;

    public AudioClip TransSE;
    AudioSource audioSource;

    void Start()
    {
        if (myPV.IsMine)    //自キャラであれば実行
        {
            //MainCameraのtargetにこのゲームオブジェクトを設定
            mainCam = Camera.main;
            mainCam.GetComponent<Player_Fallow_camera>().targetObj = this.gameObject;
        }
        GameManager = GameObject.Find("GameManager");
        AliveSettingObject = GameObject.Find("AliveSetting");
        rb = GetComponent<Rigidbody>();
        S_animator = GetComponent<Animator>();
        audioSource = this.transform.root.gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        //サーバーから情報取得
        if (targetKillerID == null) {
            if (PhotonNetwork.CurrentRoom.CustomProperties["KillerUserID"] is string KillerID)
            {
                targetKillerID = KillerID;
            }
        }
        if (PhotonNetwork.CurrentRoom.CustomProperties["KillerTransformFlag"] is bool KillerTransformFlag)
        {
            if (KillerTransformFlag && transformFlag == false)
            {
                var PlayerObjects = GameObject.FindGameObjectsWithTag("Player");
                if (PhotonNetwork.CurrentRoom.CustomProperties["ChangeKillerNum"] is int ChangeKillerNum)
                {
                    foreach (var PlayerObject in PlayerObjects)
                    {
                        if (PlayerObject.GetComponent<PhotonView>().CreatorActorNr == ChangeKillerNum)
                        {
                            GameObject ChangeWolfObject = PlayerObject.transform.root.gameObject.transform.GetChild(1).gameObject;
                            GameObject ChangeSheepObject = PlayerObject.transform.root.gameObject.transform.GetChild(0).gameObject;

                            transformWareWolf(ChangeSheepObject, ChangeWolfObject);
                            KillerObject = ChangeWolfObject;
                            transformFlag = true;
                        }
                    }
                }
                
            }
            if (KillerObject != null)
            {
                if (PhotonNetwork.CurrentRoom.CustomProperties["KillerRunningFlag"] is bool KillerRunningFlag)
                {
                    if (KillerRunningFlag)
                    {
                        KillerObject.GetComponent<Animator>().SetBool("IsRunning", true);
                    }
                    else
                    {
                        KillerObject.GetComponent<Animator>().SetBool("IsRunning", false);
                    }
                }
                if (PhotonNetwork.CurrentRoom.CustomProperties["KillerAttackFlag"] is bool KillerAttackFlag)
                {
                    if (KillerAttackFlag)
                    {
                        KillerObject.GetComponent<Animator>().SetBool("IsAttack", true);
                        KillerObject.GetComponent<BoxCollider>().enabled = true;
                    }
                    else
                    {
                        KillerObject.GetComponent<Animator>().SetBool("IsAttack", false);
                        KillerObject.GetComponent<BoxCollider>().enabled = false;
                    }
                }
            }
        }

    }

    void FixedUpdate()
    {
        if (!myPV.IsMine)
        {
            return;
        }

        //羊状態で攻撃されている間は移動不可
        if (MoveLock)
        {
            return;
        }
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;

        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        
        if (this.gameObject == this.gameObject.transform.root.gameObject.transform.GetChild(0).gameObject)
        {

            if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
            {
                rb.velocity = moveForward * moveSpeed + new Vector3(0, rb.velocity.y, 0);
            }
            else
            {
                rb.velocity = moveForward * (moveSpeed / 3) + new Vector3(0, rb.velocity.y, 0);
            }
        }
        else
        {
            rb.velocity = moveForward * (moveSpeed * 1.5f) + new Vector3(0, rb.velocity.y, 0);
        }

        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero)
        {
            if (this.gameObject == this.gameObject.transform.root.gameObject.transform.GetChild(0).gameObject)
            {
                if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
                {
                    S_animator.SetBool("IsRunning", true);
                }
                else
                {
                    S_animator.SetBool("IsWalking", true);
                    S_animator.SetBool("IsRunning", false);
                }
            }
            else
            {
                hashtable["KillerRunningFlag"] = true;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(moveForward),
                                                  applySpeed);
        }
        else
        {
            S_animator.SetBool("IsRunning", false);
            S_animator.SetBool("IsWalking", false);
            if (this.gameObject == transform.root.gameObject.transform.GetChild(1).gameObject)
            {
                hashtable["KillerRunningFlag"] = false;
            }
        }

        //変身処理
        if (GameManager.GetComponent<CGameManagerScript>().SecondPhaseFlag == true && AliveSettingObject.GetComponent<AliveSettingScript>().Post_Distribution[this.photonView.CreatorActorNr - 1] == "人狼")
        {
            if (Input.GetKeyDown(KeyCode.Space) && this.gameObject != transform.root.gameObject.transform.GetChild(1).gameObject)
            {
                hashtable["KillerTransformFlag"] = true;
                hashtable["ChangeKillerNum"] = this.photonView.CreatorActorNr;
            }
        }

        //まとめて送信
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        hashtable.Clear();
    }
    public void transformWareWolf(GameObject SheepObject,GameObject WolfObject)
    {
        Vector3 NowPostion = transform.position;
        WolfObject.GetComponent<Animator>().SetBool("IsTransform", true);
        WolfObject.SetActive(true);
        WolfObject.transform.position = NowPostion;
        SheepObject.gameObject.SetActive(false);
        audioSource.PlayOneShot(TransSE);
    }
}