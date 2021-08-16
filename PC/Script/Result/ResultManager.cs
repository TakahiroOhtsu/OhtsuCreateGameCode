using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    public Sprite[] JobIcons = new Sprite[9];
    
    public GameObject CanvasObject;
    GameObject targetOBJ;
    GameObject AliveSetting;


    // Start is called before the first frame update
    void Start()
    {
        AliveSetting = GameObject.Find("AliveSetting");
        int LoopStart;
        if (PhotonNetwork.CurrentRoom.MaxPlayers <= 4)
        {
            targetOBJ = CanvasObject.transform.GetChild(0).gameObject;
            LoopStart = 4;
        }
        else
        {
            targetOBJ = CanvasObject.transform.GetChild(1).gameObject;
            LoopStart = 7;
        }
        CanvasObject.SetActive(true);
        targetOBJ.SetActive(true);
        Debug.LogWarning("いっちょあ");
        for (int i = PhotonNetwork.CurrentRoom.MaxPlayers; i > LoopStart ; i++)
        {
            targetOBJ.transform.GetChild(i).gameObject.SetActive(false);
        }
        Debug.LogWarning("いっちょあが");
        for (int i = 0; i > PhotonNetwork.CurrentRoom.MaxPlayers; i++)
        {
            switch (AliveSetting.GetComponent<AliveSettingScript>().Post_Distribution[i])
            {
                case "占い師":
                    targetOBJ.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = JobIcons[0];
                    break;
                case "霊媒師":
                    targetOBJ.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = JobIcons[1];
                    break;
                case "騎士":
                    targetOBJ.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = JobIcons[2];
                    break;
                case "共有者":
                    targetOBJ.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = JobIcons[3];
                    break;
                case "神官":
                    targetOBJ.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = JobIcons[4];
                    break;
                case "村人":
                    targetOBJ.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = JobIcons[5];
                    break;
                case "妖狐":
                    targetOBJ.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = JobIcons[6];
                    break;
                case "狂人":
                    targetOBJ.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = JobIcons[7];
                    break;
                default:
                    targetOBJ.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = JobIcons[8];
                    break;
            }
            targetOBJ.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName;
            Debug.LogWarning("いっちょあがり");
        }
    }

}
