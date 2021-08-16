using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;



public class ResultChecker : MonoBehaviour
{
    GameObject AliveSetting;
    // Start is called before the first frame update
    void Start()
    {
        AliveSetting = GameObject.Find("AliveSetting");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResultChecking()
    {
        if (AliveSetting.GetComponent<AliveSettingScript>().AlivePlayerCount == 2)
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
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
                FadeManager.Instance.LoadScene("YoukoWin", 0.5f);
                //PhotonNetwork.LoadLevel("YoukoWin"); //妖狐勝利
            }
            else
            {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
                FadeManager.Instance.LoadScene("KillerWin", 0.5f);
                //PhotonNetwork.LoadLevel("KillerWin"); //人狼勝利
            }
        }
    }
}
