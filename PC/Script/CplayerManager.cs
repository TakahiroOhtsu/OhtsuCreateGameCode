using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using Photon.Pun;
using Photon.Realtime;

public class CplayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    //頭上のUIのPrefab
    public GameObject PlayerUiPrefab;

    //現在のHP
    public int HP = 100;

    //Localのプレイヤーを設定
    public static GameObject LocalPlayerInstance;

    #region プレイヤー初期設定
    void Awake()
    {
        if (photonView.IsMine)
        {
            CplayerManager.LocalPlayerInstance = this.gameObject;
        }
    }
    #endregion

    #region 頭上UIの生成
    void Start()
    {
    }
    #endregion

    void Update()
    {
        if (!photonView.IsMine) //このオブジェクトがLocalでなければ実行しない
        {
            return;
        }
        //CLocalVariablesを参照し、現在のHPを更新
        HP = CLocalVariables.currentHP;
    }

    #region OnPhotonSerializeView同期
    //プレイヤーのHP,チャットを同期
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.HP);
        }
        else
        {
            this.HP = (int)stream.ReceiveNext();
        }
    }
    #endregion
}
