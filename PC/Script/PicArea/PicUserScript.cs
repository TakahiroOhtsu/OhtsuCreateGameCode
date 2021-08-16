using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicUserScript : MonoBehaviour
{
    public int PushNum;
    GameObject Manager;
    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.Find("DistributeManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PushSwitch()
    {
        DistributeManager DistributeManagerScript = Manager.GetComponent<DistributeManager>();
        
        DistributeManagerScript.PicPlayerNum = PushNum;
        if (DistributeManagerScript.ChoiceFlag == false)
        {
            Debug.LogWarning("test0");
            DistributeManagerScript.ChoiceFlag = true;
            this.gameObject.transform.parent.gameObject.SetActive(false);
        }
        else//2Phaneめ
        {
            if (DistributeManagerScript.SetJobUI == false)　//名乗るジョブ選択
            {
                Debug.LogWarning("test1");
                DistributeManagerScript.SetJobUI = true;
            }
            else //占い師、霊媒師で使用
            {
                if (DistributeManagerScript.SetSecondPicUI == false)
                {
                    Debug.LogWarning("test2");
                    DistributeManagerScript.setName = this.gameObject.transform.GetChild(0).GetComponent<Text>().text;
                    Debug.LogWarning("PIC"+ DistributeManagerScript.PicPlayerNum);
                    if (DistributeManagerScript.SetJobString == "占い師")
                    {
                        DistributeManagerScript.PicPlayerNum = 0;
                    }
                    else if (DistributeManagerScript.SetJobString == "霊媒師")
                    {
                        DistributeManagerScript.PicPlayerNum = 1;
                    }

                    this.gameObject.transform.parent.gameObject.SetActive(false);
                    DistributeManagerScript.SetSecondPicUI = true;
                }
                else
                {
                    Debug.LogWarning("test3");
                    DistributeManagerScript.PicPlayerNum = 9; //枠切り替え用
                    DistributeManagerScript.lastPicNum = PushNum;

                    this.gameObject.transform.parent.gameObject.SetActive(false);
                }
            }
            this.gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
