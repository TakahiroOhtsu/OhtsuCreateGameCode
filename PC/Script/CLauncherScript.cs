using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CLauncherScript : MonoBehaviourPunCallbacks
{


    #region Public変数定義

    private const string KillerUserID = "StartTime"; // 鬼にされた人のユーザーIDを格納する
    private const bool KillerTransformFlag = false; // 鬼に変身してるか判別する

    #endregion

    #region Private変数
    //Private変数の定義はココで
    #endregion

    #region Public Methods
    //ログインボタンを押したときに実行される
    public void Connect()
    {
        //Photonに接続できていなければ
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();   //Photonに接続する
            Debug.Log("Photonに接続しました。");
            if (string.IsNullOrEmpty(PhotonNetwork.NickName))
            {
                PhotonNetwork.NickName = "player" + Random.Range(1, 99999);
            }

            //FadeManager.Instance.LoadScene("CreateRoom", 0.5f);
            SceneManager.LoadScene("CreateRoom");
        }
    }
    public void Connect2()
    {
        //Photonに接続できていなければ
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();   //Photonに接続する
            Debug.Log("Photonに接続しました。");
        }
    }
    #endregion

    #region Photonコールバック
    //ルームに入室前に呼び出される
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMasterが呼ばれました");
        // "room"という名前のルームに参加する（ルームが無ければ作成してから参加する）
        //PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);
        //string KillerUserID = "わおｎ";
        PhotonNetwork.JoinOrCreateRoom("room", CreateRoomOptions(KillerUserID), TypedLobby.Default);
    }

    //ルームに入った時に呼ばれる
    public override void OnJoinedRoom()
    {
        Debug.Log("ルームに入りました。");
        //battleシーンをロード
        //PhotonNetwork.LoadLevel("PicArea"); //デバッグ先
        //PhotonNetwork.LoadLevel("MainField"); //デバッグ先
        //PhotonNetwork.LoadLevel("MatchingRoom"); //本来
        //FadeManager.Instance.LoadScene("MatchingRoom", 1.0f);

    }

    #endregion

    public static RoomOptions CreateRoomOptions(string KillerUserID)
    {
        return new RoomOptions()
        {
            // カスタムプロパティの初期設定
            CustomRoomProperties = new Hashtable() {
                { KillerUserID, KillerTransformFlag }
            }
        };
    }
}