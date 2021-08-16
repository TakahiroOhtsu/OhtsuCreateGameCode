using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[System.Serializable]
public class User
{
    //public string nickname;
    //public List<DeckData> DeckData;
    
    public DeckSaveData Deckdata;
}
public class GameManeger : MonoBehaviour
{
    public User user;

    public Dropdown DeckNumDD;
    public Dropdown FirstUnitDD;
    public Dropdown SecondUnitDD;
    public Dropdown ThirdUnitDD;
    public Dropdown FirstUnitTypeDD;
    public Dropdown SecondUnitTypeDD;
    public Dropdown ThirdUnitTypeDD;
    public Dropdown FirstWepDD;
    public Dropdown SecondWepDD;

    public AudioClip click_SE;
    AudioSource Source_SE;

    int NowSelectDeck = 0; //初期値

    // Start is called before the first frame update
    void Start()
    {
        LoadData();
        Source_SE = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (NowSelectDeck != DeckNumDD.value)
        {
            PreSave();
            NowSelectDeck = DeckNumDD.value;
            PreLoadData();
        }
        else
        {

        }
    }

    async public void onClickEnter()
    {
        Source_SE.PlayOneShot(click_SE);
        await Task.Delay(100);
        PreSave();
        if (SaveChacker())
        {
            string json = JsonUtility.ToJson(user);
            print(json);
            PlayerPrefs.SetString("PlayerDeckData", json);
            Debug.Log("セーブしたよ");
            SceneManager.LoadScene("Mode");

        }
        else
        {
            Debug.Log("セーブ出来なかったよ");
        }
        

    }

    bool SaveChacker()
    {
        bool tempFlag = true;
        if (user.Deckdata.FirstDeck.FirstFigureNum == user.Deckdata.FirstDeck.SecondFigureNum ||
            user.Deckdata.FirstDeck.SecondFigureNum == user.Deckdata.FirstDeck.ThirdFigureNum ||
            user.Deckdata.FirstDeck.ThirdFigureNum == user.Deckdata.FirstDeck.FirstFigureNum)
        {
            Debug.Log("デッキ1が被った");
            tempFlag = false;
        }
        if(user.Deckdata.SecondDeck.FirstFigureNum == user.Deckdata.SecondDeck.SecondFigureNum ||
            user.Deckdata.SecondDeck.SecondFigureNum == user.Deckdata.SecondDeck.ThirdFigureNum ||
            user.Deckdata.SecondDeck.ThirdFigureNum == user.Deckdata.SecondDeck.FirstFigureNum)
        {
            Debug.Log("デッキ2が被った");
            tempFlag = false;
        }
        if (user.Deckdata.ThirdDeck.FirstFigureNum == user.Deckdata.ThirdDeck.SecondFigureNum ||
            user.Deckdata.ThirdDeck.SecondFigureNum == user.Deckdata.ThirdDeck.ThirdFigureNum ||
            user.Deckdata.ThirdDeck.ThirdFigureNum == user.Deckdata.ThirdDeck.FirstFigureNum)
        {
            Debug.Log("デッキ3が被った");
            tempFlag = false;
        }
        if (user.Deckdata.FourDeck.FirstFigureNum == user.Deckdata.FourDeck.SecondFigureNum ||
            user.Deckdata.FourDeck.SecondFigureNum == user.Deckdata.FourDeck.ThirdFigureNum ||
            user.Deckdata.FourDeck.ThirdFigureNum == user.Deckdata.FourDeck.FirstFigureNum)
        {
            Debug.Log("デッキ4が被った");
            tempFlag = false;
        }
        if (user.Deckdata.FifthDeck.FirstFigureNum == user.Deckdata.FifthDeck.SecondFigureNum ||
            user.Deckdata.FifthDeck.SecondFigureNum == user.Deckdata.FifthDeck.ThirdFigureNum ||
            user.Deckdata.FifthDeck.ThirdFigureNum == user.Deckdata.FifthDeck.FirstFigureNum)
        {
            Debug.Log("デッキ5が被った");
            tempFlag = false;
        }

        if (tempFlag)
        {
            Debug.Log("被るフィギュアはなかったよ");
            return true;
        }
        else
        {
            Debug.Log("被るフィギュアがあったよ");
            DialogManager.Instance.ShowSubmitDialog(
                "[Attention]",
                "いずれかのデッキでキャラクターが被っています。\nキャラクターは被らないようにしてください。",
                (bool result) => { Debug.Log("submited!"); }
            );
            return false;
        }
    }

    void PreSave()
    {
        if (NowSelectDeck == 0)
        {
            user.Deckdata.FirstDeck.FirstFigureNum = FirstUnitDD.value;
            user.Deckdata.FirstDeck.SecondFigureNum = SecondUnitDD.value;
            user.Deckdata.FirstDeck.ThirdFigureNum = ThirdUnitDD.value;
            user.Deckdata.FirstDeck.FirstRoleNum = FirstUnitTypeDD.value;
            user.Deckdata.FirstDeck.SecondRoleNum = SecondUnitTypeDD.value;
            user.Deckdata.FirstDeck.ThirdRoleNum = ThirdUnitTypeDD.value;
            user.Deckdata.FirstDeck.FirstWeaponNum = FirstWepDD.value;
            user.Deckdata.FirstDeck.SecondWeaponNum = SecondWepDD.value;
            Debug.Log("1つ目保存したよ");
        }
        else if (NowSelectDeck == 1)
        {
            user.Deckdata.SecondDeck.FirstFigureNum = FirstUnitDD.value;
            user.Deckdata.SecondDeck.SecondFigureNum = SecondUnitDD.value;
            user.Deckdata.SecondDeck.ThirdFigureNum = ThirdUnitDD.value;
            user.Deckdata.SecondDeck.FirstRoleNum = FirstUnitTypeDD.value;
            user.Deckdata.SecondDeck.SecondRoleNum = SecondUnitTypeDD.value;
            user.Deckdata.SecondDeck.ThirdRoleNum = ThirdUnitTypeDD.value;
            user.Deckdata.SecondDeck.FirstWeaponNum = FirstWepDD.value;
            user.Deckdata.SecondDeck.SecondWeaponNum = SecondWepDD.value;
            Debug.Log("2つ目保存したよ");
        }
        else if (NowSelectDeck == 2)
        {
            user.Deckdata.ThirdDeck.FirstFigureNum = FirstUnitDD.value;
            user.Deckdata.ThirdDeck.SecondFigureNum = SecondUnitDD.value;
            user.Deckdata.ThirdDeck.ThirdFigureNum = ThirdUnitDD.value;
            user.Deckdata.ThirdDeck.FirstRoleNum = FirstUnitTypeDD.value;
            user.Deckdata.ThirdDeck.SecondRoleNum = SecondUnitTypeDD.value;
            user.Deckdata.ThirdDeck.ThirdRoleNum = ThirdUnitTypeDD.value;
            user.Deckdata.ThirdDeck.FirstWeaponNum = FirstWepDD.value;
            user.Deckdata.ThirdDeck.SecondWeaponNum = SecondWepDD.value;
            Debug.Log("3つ目保存したよ");
        }
        else if (NowSelectDeck == 3)
        {
            user.Deckdata.FourDeck.FirstFigureNum = FirstUnitDD.value;
            user.Deckdata.FourDeck.SecondFigureNum = SecondUnitDD.value;
            user.Deckdata.FourDeck.ThirdFigureNum = ThirdUnitDD.value;
            user.Deckdata.FourDeck.FirstRoleNum = FirstUnitTypeDD.value;
            user.Deckdata.FourDeck.SecondRoleNum = SecondUnitTypeDD.value;
            user.Deckdata.FourDeck.ThirdRoleNum = ThirdUnitTypeDD.value;
            user.Deckdata.FourDeck.FirstWeaponNum = FirstWepDD.value;
            user.Deckdata.FourDeck.SecondWeaponNum = SecondWepDD.value;
            Debug.Log("4つ目保存したよ");
        }
        else if (NowSelectDeck == 4)
        {
            user.Deckdata.FifthDeck.FirstFigureNum = FirstUnitDD.value;
            user.Deckdata.FifthDeck.SecondFigureNum = SecondUnitDD.value;
            user.Deckdata.FifthDeck.ThirdFigureNum = ThirdUnitDD.value;
            user.Deckdata.FifthDeck.FirstRoleNum = FirstUnitTypeDD.value;
            user.Deckdata.FifthDeck.SecondRoleNum = SecondUnitTypeDD.value;
            user.Deckdata.FifthDeck.ThirdRoleNum = ThirdUnitTypeDD.value;
            user.Deckdata.FifthDeck.FirstWeaponNum = FirstWepDD.value;
            user.Deckdata.FifthDeck.SecondWeaponNum = SecondWepDD.value;
            Debug.Log("5つ目保存したよ");
        }
        else
        {

        }
        Debug.Log("プリセーブしたよ");
    }
    void PreLoadData() 
    {
        if (DeckNumDD.value == 0)
        {
            FirstUnitDD.value = user.Deckdata.FirstDeck.FirstFigureNum;
            SecondUnitDD.value = user.Deckdata.FirstDeck.SecondFigureNum;
            ThirdUnitDD.value = user.Deckdata.FirstDeck.ThirdFigureNum;
            FirstUnitTypeDD.value = user.Deckdata.FirstDeck.FirstRoleNum;
            SecondUnitTypeDD.value = user.Deckdata.FirstDeck.SecondRoleNum;
            ThirdUnitTypeDD.value = user.Deckdata.FirstDeck.ThirdRoleNum;
            FirstWepDD.value = user.Deckdata.FirstDeck.FirstWeaponNum;
            SecondWepDD.value = user.Deckdata.FirstDeck.SecondWeaponNum;
            Debug.Log("1つ目ロードしたよ");
        }
        else if (DeckNumDD.value == 1)
        {
            FirstUnitDD.value = user.Deckdata.SecondDeck.FirstFigureNum;
            SecondUnitDD.value = user.Deckdata.SecondDeck.SecondFigureNum;
            ThirdUnitDD.value = user.Deckdata.SecondDeck.ThirdFigureNum;
            FirstUnitTypeDD.value = user.Deckdata.SecondDeck.FirstRoleNum;
            SecondUnitTypeDD.value = user.Deckdata.SecondDeck.SecondRoleNum;
            ThirdUnitTypeDD.value = user.Deckdata.SecondDeck.ThirdRoleNum;
            FirstWepDD.value = user.Deckdata.SecondDeck.FirstWeaponNum;
            SecondWepDD.value = user.Deckdata.SecondDeck.SecondWeaponNum;
            Debug.Log("2つ目ロードしたよ");
        }
        else if (DeckNumDD.value == 2)
        {
            FirstUnitDD.value = user.Deckdata.ThirdDeck.FirstFigureNum;
            SecondUnitDD.value = user.Deckdata.ThirdDeck.SecondFigureNum;
            ThirdUnitDD.value = user.Deckdata.ThirdDeck.ThirdFigureNum;
            FirstUnitTypeDD.value = user.Deckdata.ThirdDeck.FirstRoleNum;
            SecondUnitTypeDD.value = user.Deckdata.ThirdDeck.SecondRoleNum;
            ThirdUnitTypeDD.value = user.Deckdata.ThirdDeck.ThirdRoleNum;
            FirstWepDD.value = user.Deckdata.ThirdDeck.FirstWeaponNum;
            SecondWepDD.value = user.Deckdata.ThirdDeck.SecondWeaponNum;
            Debug.Log("3つ目ロードしたよ");
        }
        else if (DeckNumDD.value == 3)
        {
            FirstUnitDD.value = user.Deckdata.FourDeck.FirstFigureNum;
            SecondUnitDD.value = user.Deckdata.FourDeck.SecondFigureNum;
            ThirdUnitDD.value = user.Deckdata.FourDeck.ThirdFigureNum;
            FirstUnitTypeDD.value = user.Deckdata.FourDeck.FirstRoleNum;
            SecondUnitTypeDD.value = user.Deckdata.FourDeck.SecondRoleNum;
            ThirdUnitTypeDD.value = user.Deckdata.FourDeck.ThirdRoleNum;
            FirstWepDD.value = user.Deckdata.FourDeck.FirstWeaponNum;
            SecondWepDD.value = user.Deckdata.FourDeck.SecondWeaponNum;
            Debug.Log("4つ目ロードしたよ");
        }
        else if (DeckNumDD.value == 4)
        {
            FirstUnitDD.value = user.Deckdata.FifthDeck.FirstFigureNum;
            SecondUnitDD.value = user.Deckdata.FifthDeck.SecondFigureNum;
            ThirdUnitDD.value = user.Deckdata.FifthDeck.ThirdFigureNum;
            FirstUnitTypeDD.value = user.Deckdata.FifthDeck.FirstRoleNum;
            SecondUnitTypeDD.value = user.Deckdata.FifthDeck.SecondRoleNum;
            ThirdUnitTypeDD.value = user.Deckdata.FifthDeck.ThirdRoleNum;
            FirstWepDD.value = user.Deckdata.FifthDeck.FirstWeaponNum;
            SecondWepDD.value = user.Deckdata.FifthDeck.SecondWeaponNum;
            Debug.Log("5つ目ロードしたよ");
        }
        else
        {

        }
        Debug.Log("プリロードしたよ");
    }

    void LoadData()
    {

        string loadDeck = PlayerPrefs.GetString("PlayerDeckData","");
        if (string.IsNullOrEmpty(loadDeck))
        {
            Debug.Log("ロードできなかったよ！");
        }
        else
        {
            Debug.Log("なうろーでぃんぐ");
            user = JsonUtility.FromJson<User>(loadDeck);
            PreLoadData();
        }

    }
}
