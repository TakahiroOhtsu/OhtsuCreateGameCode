using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CGameManagerScript : MonoBehaviourPunCallbacks
{
    //誰かがログインする度に生成するプレイヤーPrefab
    public GameObject playerPrefab;
    public bool SecondPhaseFlag = false;
    GameObject AliveSetting;
    public AudioClip BGM;
    AudioSource audioSource;
    private static Hashtable hashtable = new Hashtable();
    bool CreateAvator = false;

    void Start()
    {
        if (!PhotonNetwork.IsConnected)   //Phootnに接続されていなければ
        {
            SceneManager.LoadScene("RoomSetting"); //ログイン画面に戻る
            return;
        }
        AliveSetting = GameObject.Find("AliveSetting");
        if (SceneManager.GetActiveScene().name == "MatchingRoom")
        {
            GameObject Player = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 10f, 0f), Quaternion.identity, 0);
        }
        else if (SceneManager.GetActiveScene().name == "MainField")
        {
            //Photonに接続していれば(+生きていれば)自プレイヤーを生成
            if (AliveSetting.GetComponent<AliveSettingScript>().AliveFlag[PhotonNetwork.LocalPlayer.ActorNumber - 1])
            {
                if (!CreateAvator)
                {
                    CreateAvator = true;
                    GameObject Player = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 10f, 0f), Quaternion.identity, 0);
                }
            }
            else
            {
                bool KillerSetFlag = false;
                int KillerPlayerInt = 10; //default値
                for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
                {
                    if (AliveSetting.GetComponent<AliveSettingScript>().AliveFlag[i] == true)
                    {
                        var PlayerObjects = GameObject.FindGameObjectsWithTag("Player");
                        foreach (var PlayerObject in PlayerObjects)
                        {
                            if (PlayerObject.GetComponent<PhotonView>().CreatorActorNr == i)
                            {
                                Camera.main.GetComponent<Player_Fallow_camera>().targetObj = PlayerObject.transform.root.gameObject;
                            }
                        }
                    }
                    else if (AliveSetting.GetComponent<AliveSettingScript>().Post_Distribution[i] == "人狼")
                    {
                        KillerSetFlag = true;
                        KillerPlayerInt = i;
                    }
                }
                if (KillerSetFlag)
                {
                    var PlayerObjects = GameObject.FindGameObjectsWithTag("Player");
                    foreach (var PlayerObject in PlayerObjects)
                    {
                        if (PlayerObject.GetComponent<PhotonView>().CreatorActorNr == KillerPlayerInt)
                        {
                            Camera.main.GetComponent<Player_Fallow_camera>().targetObj = PlayerObject.transform.root.gameObject;
                        }
                    }
                }
            }
        }

        //マスター生成時にキラーに必要なパラメータを初期化してるだけ。
        for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++){
            if (AliveSetting.GetComponent<AliveSettingScript>().Post_Distribution[i] == "人狼")
            {
                hashtable["KillerUserID"] = PhotonNetwork.PlayerList[i].UserId;
                hashtable["KillerTransformFlag"] = false;
                hashtable["KillerRunningFlag"] = false;
                hashtable["KillerAttackFlag"] = false;

                Debug.Log("ぱおん" + PhotonNetwork.CurrentRoom.Name);
                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
                hashtable.Clear();
            }
        }

        if (AliveSetting.GetComponent<AliveSettingScript>().Post_Distribution[PhotonNetwork.LocalPlayer.ActorNumber - 1] == "騎士")
        {
            CLocalVariables.currentHP = CLocalVariables.currentHP + 50; //騎士なら一回分多くダメージを受けれるように+50する
        }
        audioSource = AliveSetting.GetComponent<AudioSource>();
        audioSource.clip = BGM;
        audioSource.Play();
    }
    void OnGUI()
    {
        //ログインの状態を画面上に出力
        Photon.Realtime.Player player = PhotonNetwork.LocalPlayer;
        //GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
        GUILayout.Label(PhotonNetwork.LocalPlayer.ToString());
        //Debug.Log("PlayerNo" + player.ActorNumber);
        //Debug.Log("プレイヤーID" + player.UserId);
    }
}