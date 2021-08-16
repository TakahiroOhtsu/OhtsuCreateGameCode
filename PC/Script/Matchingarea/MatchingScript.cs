using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MatchingScript : MonoBehaviour
{
    // Start is called before the first frame update
    public PhotonView myPV;
    public GameObject AliveSetting;
    public GameObject StartButton;

    void Start()
    {
        AliveSetting.GetComponent<AliveSettingScript>().AlivePlayerCount = PhotonNetwork.CurrentRoom.MaxPlayers;
        AliveSetting.GetComponent<AliveSettingScript>().StartSetting();
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
            //Debug.Log(PhotonNetwork.CurrentRoom.MaxPlayers);
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                StartButton.SetActive(true);
                if (Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    myPV.RPC("RPCNextRoomRead", RpcTarget.AllViaServer);
                }
                //myPV.RPC("RPCNextRoomRead", RpcTarget.AllViaServer);
            }
        }
    }

    public void OnClickStartButton()
    {
        myPV.RPC("RPCNextRoomRead", RpcTarget.AllViaServer);
    }
    [PunRPC]
    void RPCNextRoomRead()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Camera.main.gameObject.GetComponent<CursorController>().enabled = false;
        FadeManager.Instance.PLoadScene("DistributeArea", 0.5f);
        //PhotonNetwork.LoadLevel("DistributeArea"); //本来
        //PhotonNetwork.LoadLevel("PicArea");//デバッグ
    }
}
