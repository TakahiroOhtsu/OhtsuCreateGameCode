using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using NCMB;
using UnityEngine.SceneManagement;

public class JoinRoomScript : MonoBehaviour
{
    public User user;
    [SerializeField] public GameObject InputFieldNum;
    [SerializeField] public Dropdown DeckNumList;
    [SerializeField] public GameObject StartButton;

    public static SelectDeckNum NowPlayerDeck;
    float TimeCount = 10; 
    public static NCMBObject OnlineRoomClass = new NCMBObject("OnlineRoomClass");
    string RoomNumString;
    bool RoomSearchFlag = false;

    private AsyncOperation async;
    public GameObject LoadingUi;
    public Slider Slider;
    public AudioClip click_SE;
    AudioSource Source_SE;

    public GameObject Backkey;

    private void Start()
    {
        NowPlayerDeck = new SelectDeckNum();
        NowPlayerDeck.InitUnitData();
    }

    public void OnClickJoinButton()
    {
        RoomNumString = InputFieldNum.GetComponent<InputField>().text;

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("OnlineRoomClass");

        query.WhereEqualTo("RoomNumString", RoomNumString);
        query.FindAsync((List<NCMBObject> objList, NCMBException QueryResult) =>
        {
            if (QueryResult != null)
            {
                //検索失敗時の処理

                Debug.Log("失敗:該当する部屋がありませんでした1。");
                DialogManager.Instance.ShowSubmitDialog(
               "[Attention]",
               "指定した部屋がありませんでした。\n番号をよく確認の上、再入力してください。",
               (bool result) => { Debug.Log("submited!"); }
                );
                Debug.Log("失敗:該当する部屋がありませんでした。");
            }
            else
            {
                foreach (NCMBObject obj in objList)
                {
                    Debug.Log("objectId:" + obj.ObjectId);
                    Debug.Log("成功");
                    OnlineRoomClass.ObjectId = obj.ObjectId;
                    
                }
                OnlineRoomClass.FetchAsync((NCMBException e) => {
                    if (e != null)
                    {
                        //エラー処理
                    }
                    else
                    {
                        //成功時の処理
                        OnlineRoomClass["MatchCount"] = "true";
                        OnlineRoomClass["Player2ReadyFlag"] = "false";
                        OnlineRoomClass["Player2DataSaveFlag"] = "false";
                        OnlineRoomClass.SaveAsync();
                        RoomSearchFlag = true;

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

                    }
                });
            }
        });


        bool SetDeckData(DeckSaveData.DeckStat SelectDeck)
        {
            
            NowPlayerDeck.FirstUnit.UnitNum = 0;
            NowPlayerDeck.SecondUnit.UnitNum = 0;
            NowPlayerDeck.ThirdUnit.UnitNum = 0;
            NowPlayerDeck.FirstUnit.UnitCategory = 0;
            NowPlayerDeck.SecondUnit.UnitCategory = 0;
            NowPlayerDeck.ThirdUnit.UnitCategory = 0;
            NowPlayerDeck.WepNumOne = 0;
            Debug.Log(NowPlayerDeck.FirstUnit.UnitNum);
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
                //Source_SE.PlayOneShot(click_SE);
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
        IEnumerator LoadScene()
        {
            async = SceneManager.LoadSceneAsync("OnlineBattleSetting");

            while (!async.isDone)
            {
                Slider.value = async.progress;
                yield return null;
            }
        }

    }
}
