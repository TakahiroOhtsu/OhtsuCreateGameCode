using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using NCMB;
using UnityEngine.SceneManagement;

public class CreateRoomScript : MonoBehaviour
{
    public User user;
    [SerializeField] public Text RoomNumText;
    [SerializeField] public GameObject StartButton;
    [SerializeField] public Dropdown DeckNumList;
    public static SelectDeckNum NowPlayerDeck;

    float TimeCount = 10;
    public static NCMBObject OnlineRoomClass;
    string RoomNumString;

    public AudioClip click_SE;
    AudioSource Source_SE;
    public GameObject Backkey;

    void Start()
    {
        OnlineRoomClass = new NCMBObject("OnlineRoomClass");

        Source_SE = GetComponent<AudioSource>();

        NowPlayerDeck = new SelectDeckNum();
        NowPlayerDeck.InitUnitData();

        RoomNumString = Random.Range(0, 1000000).ToString();

        int RoomNumStringLength = RoomNumString.Length;
        int StartTurnPlayer = Random.Range(0, 2);

        if (RoomNumStringLength < 6)
        {
            RoomNumStringLength = 6 - RoomNumStringLength;
            for (int LoopNum = 0; LoopNum < RoomNumStringLength; LoopNum++)
            {
                RoomNumString = "0" + RoomNumString;
            }
        }

        RoomNumText.text = RoomNumString;

        OnlineRoomClass["RoomNumString"] = RoomNumString;
        OnlineRoomClass["MatchCount"] = "false";
        OnlineRoomClass["NowTurnPlayer"] = StartTurnPlayer;
        OnlineRoomClass["Player1ReadyFlag"] = "false";
        OnlineRoomClass["Player1DataSaveFlag"] = "false";
        OnlineRoomClass["Player2DataSaveFlag"] = "false";
        OnlineRoomClass["Winner"] = "noResult";
        Debug.Log(OnlineRoomClass.ObjectId);
        OnlineRoomClass.SaveAsync();
    }

    bool startflag = false;
    bool ButtonFlag = false;
    NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("OnlineRoomClass");
    void Update()
    {
        TimeCount -= Time.deltaTime;
        if (TimeCount <= 0)
        {
            if (ButtonFlag == false)
            {
                if (startflag == false)
                {
                    query.WhereEqualTo("RoomNumString", RoomNumString);
                    query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
                    {
                        if (e != null)
                        {
                            //検索失敗時の処理
                            Debug.Log("失敗");
                        }
                        else
                        {
                            foreach (NCMBObject obj in objList)
                            {
                                Debug.Log("objectId:" + obj.ObjectId);
                                Debug.Log("成功");
                                OnlineRoomClass.ObjectId = obj.ObjectId;
                                startflag = true;
                            }
                        }
                    });
                }
                else
                {
                    OnlineRoomClass.FetchAsync((NCMBException e) => {
                        if (e != null)
                        {
                            //エラー処理
                        }
                        else
                        {
                            //成功時の処理
                            if ((string)OnlineRoomClass["MatchCount"] == "true")
                            {
                                StartButton.SetActive(true);
                                ButtonFlag = true;
                            }
                        }
                    });
                }
            }
            else
            {
                
            }
            TimeCount = 5;
        }
    }


    private AsyncOperation async;
    public GameObject LoadingUi;
    public Slider Slider;
    public void OnClickBattleStart() 
    {
        string loadDeck = PlayerPrefs.GetString("PlayerDeckData", "");
        if (string.IsNullOrEmpty(loadDeck))
        {
            Debug.Log("ロードできなかったよ！");
        }
        else
        {
            Debug.Log("なうろーでぃんぐ");
            user = JsonUtility.FromJson<User>(loadDeck);
            bool CheckFlag = false;
            switch (DeckNumList.value)
            {
                case 0:
                    CheckFlag = SetDeckData(user.Deckdata.FirstDeck);
                    break;
                case 1:
                    CheckFlag = SetDeckData(user.Deckdata.SecondDeck);
                    break;
                case 2:
                    CheckFlag = SetDeckData(user.Deckdata.ThirdDeck);
                    break;
                case 3:
                    CheckFlag = SetDeckData(user.Deckdata.FourDeck);
                    break;
                case 4:
                    CheckFlag = SetDeckData(user.Deckdata.FifthDeck);
                    break;
                default:
                    break;

            }
            LoadNextScene(CheckFlag);
        }


        bool SetDeckData(DeckSaveData.DeckStat SelectDeck)
        {
            NowPlayerDeck.FirstUnit.UnitNum = SelectDeck.FirstFigureNum;
            NowPlayerDeck.SecondUnit.UnitNum = SelectDeck.SecondFigureNum;
            NowPlayerDeck.ThirdUnit.UnitNum = SelectDeck.ThirdFigureNum;
            NowPlayerDeck.FirstUnit.UnitCategory = SelectDeck.FirstRoleNum;
            NowPlayerDeck.SecondUnit.UnitCategory = SelectDeck.SecondRoleNum;
            NowPlayerDeck.ThirdUnit.UnitCategory = SelectDeck.ThirdRoleNum;
            NowPlayerDeck.WepNumOne = SelectDeck.FirstWeaponNum;
            NowPlayerDeck.WepNumTwo = SelectDeck.SecondWeaponNum;
            Debug.Log("ロード：" + SelectDeck);

            return true;
        }

        void LoadNextScene(bool DeckSelectCheck)
        {
            if (DeckSelectCheck)
            {
                Backkey.SetActive(false);
               // Source_SE.PlayOneShot(click_SE);
                LoadingUi.SetActive(true);
                StartCoroutine(LoadScene());
                SceneNameData.Instance.referer = SceneManager.GetActiveScene().name;
            }
            else
            {
                DialogManager.Instance.ShowSubmitDialog(
                    "[Attention]",
                    "デッキが不正です。\n同じ兵士駒は選択できません。\nデッキを編集してください。",
                    (bool result) => { SceneManager.LoadScene("DeckCreate"); }
                );
            }
        }

        
    }
    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync("OnlineBattleSetting");

        while (!async.isDone)
        {
            Slider.value = async.progress;
            yield return null;
        }
    }

    async public void BackKey()
    {
        Source_SE.PlayOneShot(click_SE);
        await Task.Delay(100);
        OnlineRoomClass.DeleteAsync((NCMBException e) => {
            if (e != null)
            {
                //エラー処理
            }
            else
            {
                //成功時の処理
            }
        });
        SceneManager.LoadScene("VSON");
    }
}
