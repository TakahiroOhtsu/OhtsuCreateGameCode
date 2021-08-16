using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;

public class OnlineclickImageIcon : MonoBehaviour
{
    GameObject OnlineGameController;
    OnlineGame_Controller script;

    public AudioClip click_SE;
    public AudioClip skillSE;
    AudioSource Source_SE;


    // Start is called before the first frame update
    void Start()
    {
        OnlineGameController = GameObject.Find("GameControll");
        script = OnlineGameController.GetComponent<OnlineGame_Controller>();
        Source_SE = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool startobjectLoadFlag = false;
    public void OnClickIcon()
    {
        Image tempPictureIcon = transform.GetComponent<Image>();

        //側判別
        if (transform.name == "Nico")
        {
            script.choiseUnit = 1;
            Debug.Log("いちわく");
            Source_SE.PlayOneShot(click_SE);
        }
        else if (transform.name == "UnityChan")
        {
            script.choiseUnit = 2;
            Debug.Log("にわく");
            Source_SE.PlayOneShot(click_SE);
        }
        else if (transform.name == "ProNama")
        {
            script.choiseUnit = 3;
            Debug.Log("さんわく");
            Source_SE.PlayOneShot(click_SE);
        }
        else if (transform.name == "Rader")
        {
            script.choiseUnit = 4;
            Debug.Log("よんわく");
            Source_SE.PlayOneShot(click_SE);
        }
        else if (transform.name == "mine")
        {
            script.choiseUnit = 5;
            Debug.Log("ごわく");
            Source_SE.PlayOneShot(click_SE);

        }
        else if (transform.name == "Unity_pic")
        {
            if (script.OnlineRoomClass["NowTurnPlayer"].ToString() == script.turnControll.ToString() &&
                script.OnlineRoomClass["Player2ReadyFlag"].ToString() == "true" &&
                script.OnlineRoomClass["Player1ReadyFlag"].ToString() == "true")
            {
                Debug.Log("はいった");

                battleObjectData LoadObjectData = new battleObjectData();

                if (script.turnControll == 0)
                {
                    LoadObjectData = JsonUtility.FromJson<battleObjectData>(script.OnlineRoomClass["Player2GameObjectData"].ToString());
                }
                else
                {
                    LoadObjectData = JsonUtility.FromJson<battleObjectData>(script.OnlineRoomClass["Player1GameObjectData"].ToString());
                }
                startObjectCreate(LoadObjectData);

                if (script.PhaseControll == 0)
                {
                    script.BattlePhaseKey = 0;
                }
                else
                {
                    script.BattlePhaseKey = 5;
                }

                if (script.PlayerBattleObject[1 - script.turnControll, 0] != null)
                {
                    script.PlayerBattleObject[1 - script.turnControll, 0].GetComponent<FigureScript>().helth = LoadObjectData.BattleObject1Helth;
                    script.PlayerBattleObject[1 - script.turnControll, 0].transform.position = LoadObjectData.BattleObject1Position;
                }
                if (script.PlayerBattleObject[1 - script.turnControll, 1] != null)
                {
                    script.PlayerBattleObject[1 - script.turnControll, 1].GetComponent<FigureScript>().helth = LoadObjectData.BattleObject2Helth;
                    script.PlayerBattleObject[1 - script.turnControll, 1].transform.position = LoadObjectData.BattleObject2Position;
                }
                if (script.PlayerBattleObject[1 - script.turnControll, 2] != null)
                {
                    script.PlayerBattleObject[1 - script.turnControll, 2].GetComponent<FigureScript>().helth = LoadObjectData.BattleObject3Helth;
                    script.PlayerBattleObject[1 - script.turnControll, 2].transform.position = LoadObjectData.BattleObject3Position;
                }

                if (script.PlayerBattleObject[1 - script.turnControll,3] != null)
                {
                    if (LoadObjectData.BattleUnit1Position == new Vector3(5f, 5f, 5f))
                    {
                        Destroy(script.PlayerBattleObject[1 - script.turnControll, 3]);
                    }
                }
                if (script.PlayerBattleObject[1 - script.turnControll,4] != null)
                {
                    if (LoadObjectData.BattleUnit2Position == new Vector3(5f, 5f, 5f))
                    {
                        Destroy(script.PlayerBattleObject[1 - script.turnControll, 4]);
                    }
                }


                if (script.turnControll != 0)
                {
                    LoadObjectData = JsonUtility.FromJson<battleObjectData>(script.OnlineRoomClass["Player2GameObjectData"].ToString());
                }
                else
                {
                    LoadObjectData = JsonUtility.FromJson<battleObjectData>(script.OnlineRoomClass["Player1GameObjectData"].ToString());
                }
                if (script.PlayerBattleObject[script.turnControll, 0] != null)
                {
                    script.PlayerBattleObject[script.turnControll, 0].GetComponent<FigureScript>().helth = LoadObjectData.BattleObject1Helth;
                }
                if (script.PlayerBattleObject[script.turnControll, 1] != null)
                {
                    script.PlayerBattleObject[script.turnControll, 1].GetComponent<FigureScript>().helth = LoadObjectData.BattleObject2Helth;
                }
                if (script.PlayerBattleObject[script.turnControll, 2] != null)
                {
                    script.PlayerBattleObject[script.turnControll, 2].GetComponent<FigureScript>().helth = LoadObjectData.BattleObject3Helth;
                }
                if (script.PlayerBattleObject[script.turnControll, 3] != null)
                {
                    if (LoadObjectData.BattleUnit1Position == new Vector3(5f, 5f, 5f))
                    {
                        Destroy(script.PlayerBattleObject[script.turnControll, 3]);
                    }
                }
                if (script.PlayerBattleObject[script.turnControll, 4] != null)
                {
                    if (LoadObjectData.BattleUnit2Position == new Vector3(5f, 5f, 5f))
                    {
                        Destroy(script.PlayerBattleObject[script.turnControll, 4]);
                    }
                }


                for (int DeleteLoop = 0; DeleteLoop < 3; DeleteLoop++)
                {
                    if (script.PlayerBattleObject[script.turnControll, DeleteLoop] != null)
                    {
                        Debug.LogWarning(script.PlayerBattleObject[script.turnControll,DeleteLoop].name + ":" + script.PlayerBattleObject[script.turnControll, DeleteLoop].GetComponent<FigureScript>().helth);
                        if (script.PlayerBattleObject[script.turnControll, DeleteLoop].GetComponent<FigureScript>().helth == 0)
                        {
                            Destroy(script.PlayerBattleObject[script.turnControll, DeleteLoop]);
                            script.GameSetJudge[script.turnControll, DeleteLoop] = 1;
                        }
                    }
                    if (script.PlayerBattleObject[1 - script.turnControll, DeleteLoop] != null)
                    {
                        if (script.PlayerBattleObject[1 - script.turnControll, DeleteLoop].GetComponent<FigureScript>().helth == 0)
                        {
                            Destroy(script.PlayerBattleObject[1 - script.turnControll, DeleteLoop]);
                            script.GameSetJudge[1 - script.turnControll, DeleteLoop] = 1;
                        }
                    }
                }
                if (LoadObjectData.HomePosition == new Vector3(5f,5f,5f))
                {
                    Destroy(script.PlayerBattleObject[1 - script.turnControll, 5]);
                }
                script.SetSkillPictures();
                script.HelthColorChange();
                script.SkillIconMaster.SetActive(true);
                script.SkillIconOne.transform.parent.gameObject.SetActive(true);
                script.SkillIconTwo.transform.parent.gameObject.SetActive(true);
                script.SkillIconThree.transform.parent.gameObject.SetActive(true);
                script.RaderSearch();
                script.BlindPicture.SetActive(false);
                script.BattleEnd(script.turnControll, 1 - script.turnControll);
                void startObjectCreate(battleObjectData LoadData)
                {
                    if (startobjectLoadFlag == false)
                    {
                        CreateData(LoadData.BattleObject1Name,0,LoadData.BattleObject1Position);
                        CreateData(LoadData.BattleObject2Name,1,LoadData.BattleObject2Position);
                        CreateData(LoadData.BattleObject3Name,2,LoadData.BattleObject3Position);
                        CreateData(LoadData.BattleUnit1Name,3,LoadData.BattleUnit1Position);
                        CreateData(LoadData.BattleUnit2Name,4,LoadData.BattleUnit2Position);
                        CreateData(script.HOME.name,5,LoadData.HomePosition);
                        script.TurnController();
                        startobjectLoadFlag = true;
                    }
                    void CreateData(string Name,int Num,Vector3 tempPosit)
                    {
                        StringBuilder Rename = new StringBuilder(Name);
                        Name = Rename.Replace("(Clone)","").ToString();
                        Debug.Log(Name);

                        GameObject tempObject;
                        switch (Name)
                        {
                            case "Alicia_solid":
                                tempObject = script.NIKO_CHAN;
                                break;                            
                            case "unitychan":
                                tempObject = script.UNITY_CHAN;
                                break;                            
                            case "PronamaChan":
                                tempObject = script.PRONAMA_CHAN;
                                break;
                            case "Rader":
                                tempObject = script.RADER;
                                break;                            
                            case "Mine":
                                tempObject = script.MINE;
                                break;
                            case "House":
                                tempObject = script.HOME;
                                break;
                            default:
                                tempObject = script.HOME;
                                break;
                        }
                        Debug.Log(tempObject.name + ":" + Name);
                        tempObject.SetActive(true);
                        if ((Name == "Rader") || (Name == "Mine") || (Name == "House"))
                        {
                            script.PlayerBattleObject[1 - script.turnControll, Num] = Instantiate(tempObject, tempPosit, Quaternion.AngleAxis(-90.0f, new Vector3(1.0f, 0.0f, 0.0f)));
                            Debug.LogWarning(script.PlayerBattleObject[1 - script.turnControll, Num]);
                        }
                        else if((Name == "Alicia_solid") || (Name == "unitychan") || (Name == "PronamaChan"))
                        {
                            if (script.turnControll != 0)
                            {
                                script.PlayerBattleObject[1 - script.turnControll, Num] = Instantiate(tempObject, tempPosit, Quaternion.AngleAxis(180f, new Vector3(0.0f, 1.0f, 0.0f)));
                            }
                            else
                            {
                                script.PlayerBattleObject[1 - script.turnControll, Num] = Instantiate(tempObject, tempPosit, Quaternion.identity);
                            }
                        }
                        tempObject.SetActive(false);
                    }
                }
            }
        }
        //中身判別
        if (tempPictureIcon.sprite.name == "NicoIcon")
        {
            script.choiseUnitCategory = 1;
            Debug.Log("中身はにこちゃん");
            Source_SE.PlayOneShot(click_SE);
        }
        else if (tempPictureIcon.sprite.name == "UnityIcon")
        {
            script.choiseUnitCategory = 2;
            Debug.Log("中身はユニティちゃん");
            Source_SE.PlayOneShot(click_SE);
        }
        else if (tempPictureIcon.sprite.name == "PronamaIcon")
        {
            script.choiseUnitCategory = 3;
            Debug.Log("中身はプロ生ちゃん");
            Source_SE.PlayOneShot(click_SE);
        }
        else if (tempPictureIcon.sprite.name == "RaderIcon")
        {
            script.choiseUnitCategory = 4;
            Debug.Log("中身はれーだー");
            Source_SE.PlayOneShot(click_SE);
        }
        else if (tempPictureIcon.sprite.name == "MineIcon")
        {
            script.choiseUnitCategory = 5;
            Debug.Log("中身は地雷");
            Source_SE.PlayOneShot(click_SE);

        }
    }

    public void OnClickSkillCard()
    {
        if (script.BattlePhaseKey == 0)
        {

            switch (transform.name)
            {
                case "100":
                    //マスター
                    //相手ユニットを1体ランダムで1ターン表示
                    if (script.SkillMater[script.turnControll] >= 6 && script.SkillActiveFlag[0] == false)
                    {
                        int RandomValue = Random.Range(0, 2);
                        ActiveFigure(RandomValue);
                        /*if (script.turnControll == 0)
                        {
                            script.PlayerBattleObject[1, RandomValue].SetActive(true);
                        }
                        else
                        {
                            script.PlayerBattleObject[2, RandomValue].SetActive(true);
                        }*/
                        script.SkillMater[script.turnControll] -= 6;
                        script.SkillMaterText.text = script.SkillMater[script.turnControll] + "";
                        script.SkillActiveFlag[0] = true;
                        Source_SE.PlayOneShot(skillSE);
                    }
                    else
                    {
                        DialogManager.Instance.ShowSubmitDialog(
                            "[Attention]",
                            "必要ポイント:6\nランダムで敵ユニットを一時的に表示します。",
                            (bool result) => { Debug.Log("submited!"); }
                        );
                    }
                    break;
                case "101":
                    SkillActive(ref script.PlayerBattleObject[script.turnControll, 0], 1);
                    break;
                case "102":
                    SkillActive(ref script.PlayerBattleObject[script.turnControll, 1], 2);
                    break;
                case "103":
                    SkillActive(ref script.PlayerBattleObject[script.turnControll, 2], 3);
                    break;
                default:
                    break;
            }
        }
        else
        {
            DialogManager.Instance.ShowSubmitDialog(
                "[Attention]",
                "スキルは駒選択前に発動してください。",
                (bool result) => { Debug.Log("submited!"); }
            );
            Debug.Log("今は発動できないよ！");
        }

        void ActiveFigure(int RValue)
        {
            while (true)
            {
                if (script.turnControll == 0)
                {
                    if (script.PlayerBattleObject[1, RValue] != null)
                    {
                        script.PlayerBattleObject[1, RValue].SetActive(true);
                        break;
                    }
                    else
                    {
                        if (RValue == 2)
                        {
                            RValue = 0;
                        }
                        else
                        {
                            RValue++;
                        }
                    }
                }
                else
                {
                    if (script.PlayerBattleObject[0, RValue] != null)
                    {
                        script.PlayerBattleObject[0, RValue].SetActive(true);
                        break;
                    }
                    else
                    {
                        if (RValue == 2)
                        {
                            RValue = 0;
                        }
                        else
                        {
                            RValue++;
                        }
                    }
                }
            }
        }
    }

    void SkillActive(ref GameObject tempUnit,int tempnum)
    {
        if (tempUnit.transform.name == "unitychan(Clone)")
        {
            //移動範囲上昇
            Debug.Log("Move UPGRADE");
            if (script.SkillMater[script.turnControll] >= 3 && script.SkillActiveFlag[1] == false)
            {
                tempUnit.GetComponent<FigureScript>().MovePower += 1;
                script.SkillMater[script.turnControll] -= 3;
                //script.SkillActiveFlag[1] = true;
                EffectSet();
            }
            else
            {
                Debug.Log("ポイントが足りないので使えないよ");
                DialogManager.Instance.ShowSubmitDialog(
                    "[Attention]",
                    "必要ポイント:3\n小雪ちゃんの移動範囲を一時的に上昇します。",
                    (bool result) => { Debug.Log("submited!"); }
                );
            }
        }
        else if (tempUnit.transform.name == "Alicia_solid(Clone)")
        {
            //体力上昇
            Debug.Log("Helth UPGRADE");
            if (script.SkillMater[script.turnControll] >= 5 && script.SkillActiveFlag[2] == false)
            {
                tempUnit.GetComponent<FigureScript>().helth += 1;
                script.SkillMater[script.turnControll] -= 5;
                //script.SkillActiveFlag[2] = true;
                EffectSet();
                script.HelthColorChange();
            }
            else
            {
                Debug.Log("ポイントが足りないので使えないよ");
                DialogManager.Instance.ShowSubmitDialog(
                    "[Attention]",
                    "必要ポイント:5\nニコニ立体ちゃんの体力を1上昇します。",
                    (bool result) => { Debug.Log("submited!"); }
                );
            }
        }
        else if (tempUnit.transform.name == "PronamaChan(Clone)")
        {
            //攻撃範囲上昇
            Debug.Log("Attack UPGRADE");
            if (script.SkillMater[script.turnControll] >= 3 && script.SkillActiveFlag[3] == false)
            {
                tempUnit.GetComponent<FigureScript>().AttackArea += 1;
                script.SkillMater[script.turnControll] -= 3;
                //script.SkillActiveFlag[3] = true;
                EffectSet();
            }
            else
            {
                Debug.Log("ポイントが足りないので使えないよ");
                DialogManager.Instance.ShowSubmitDialog(
                    "[Attention]",
                    "必要ポイント:3\nプロ生ちゃんの攻撃範囲を一時的に上昇します。",
                    (bool result) => { Debug.Log("submited!"); }
                );
            }
        }

        script.SkillMaterText.text = script.SkillMater[script.turnControll] + "";
        

        void EffectSet()
        {
            Source_SE.PlayOneShot(skillSE);
            if (tempnum == 1)
            {
                script.SkillIconOneEffect.SetActive(true);
                script.SkillActiveFlag[1] = true;
            }
            else if (tempnum == 2)
            {
                script.SkillIconTwoEffect.SetActive(true);
                script.SkillActiveFlag[2] = true;
            }
            else if (tempnum == 3)
            {
                script.SkillIconThreeEffect.SetActive(true);
                script.SkillActiveFlag[3] = true;
            }
        }

    }
    public void winnerButton()
    {
        SceneManager.LoadScene("title");
    }
}
