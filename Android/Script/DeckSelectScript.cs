using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SelectDeckNum
{
    public class UnitData{
        public int UnitNum = 0;
        public int UnitCategory = 0;
    }
    public UnitData FirstUnit;
    public UnitData SecondUnit;
    public UnitData ThirdUnit;
    public int WepNumOne = 0;
    public int WepNumTwo = 0;

    public void InitUnitData()
    {
        FirstUnit = new UnitData();
        SecondUnit = new UnitData();
        ThirdUnit = new UnitData();
    }

}
public class DeckSelectScript : MonoBehaviour
{
    public User user;
    public static SelectDeckNum FirstPlayerDeck;
    public static SelectDeckNum SecondPlayerDeck;
    public GameObject Backkey;

    public AudioClip click_SE;
    public AudioClip Pointer_SE;
    AudioSource Source_SE;

    public Dropdown FirstDeckNumDD;
    public Dropdown SecondDeckNumDD;

    // Start is called before the first frame update
    void Start()
    {
        LoadData();
        FirstPlayerDeck = new SelectDeckNum();
        FirstPlayerDeck.InitUnitData();
        SecondPlayerDeck = new SelectDeckNum();
        SecondPlayerDeck.InitUnitData();
        Source_SE = GetComponent<AudioSource>();
    }
    bool PreLoadData() 
    {
        Debug.Log("1Pのロードだよ");

        if (FirstDeckNumDD.value == 0)
        {
            FirstPlayerDeck.FirstUnit.UnitNum = user.Deckdata.FirstDeck.FirstFigureNum;
            FirstPlayerDeck.SecondUnit.UnitNum = user.Deckdata.FirstDeck.SecondFigureNum;
            FirstPlayerDeck.ThirdUnit.UnitNum = user.Deckdata.FirstDeck.ThirdFigureNum;
            FirstPlayerDeck.FirstUnit.UnitCategory = user.Deckdata.FirstDeck.FirstRoleNum;
            FirstPlayerDeck.SecondUnit.UnitCategory = user.Deckdata.FirstDeck.SecondRoleNum;
            FirstPlayerDeck.ThirdUnit.UnitCategory = user.Deckdata.FirstDeck.ThirdRoleNum;
            FirstPlayerDeck.WepNumOne = user.Deckdata.FirstDeck.FirstWeaponNum;
            FirstPlayerDeck.WepNumTwo = user.Deckdata.FirstDeck.SecondWeaponNum;
            Debug.Log("1つ目ロードしたよ");
        }
        else if (FirstDeckNumDD.value == 1)
        {
            FirstPlayerDeck.FirstUnit.UnitNum = user.Deckdata.SecondDeck.FirstFigureNum;
            FirstPlayerDeck.SecondUnit.UnitNum = user.Deckdata.SecondDeck.SecondFigureNum;
            FirstPlayerDeck.ThirdUnit.UnitNum = user.Deckdata.SecondDeck.ThirdFigureNum;
            FirstPlayerDeck.FirstUnit.UnitCategory = user.Deckdata.SecondDeck.FirstRoleNum;
            FirstPlayerDeck.SecondUnit.UnitCategory = user.Deckdata.SecondDeck.SecondRoleNum;
            FirstPlayerDeck.ThirdUnit.UnitCategory = user.Deckdata.SecondDeck.ThirdRoleNum;
            FirstPlayerDeck.WepNumOne = user.Deckdata.SecondDeck.FirstWeaponNum;
            FirstPlayerDeck.WepNumTwo = user.Deckdata.SecondDeck.SecondWeaponNum;
            Debug.Log("2つ目ロードしたよ");
        }
        else if (FirstDeckNumDD.value == 2)
        {
            FirstPlayerDeck.FirstUnit.UnitNum = user.Deckdata.ThirdDeck.FirstFigureNum;
            FirstPlayerDeck.SecondUnit.UnitNum = user.Deckdata.ThirdDeck.SecondFigureNum;
            FirstPlayerDeck.ThirdUnit.UnitNum = user.Deckdata.ThirdDeck.ThirdFigureNum;
            FirstPlayerDeck.FirstUnit.UnitCategory = user.Deckdata.ThirdDeck.FirstRoleNum;
            FirstPlayerDeck.SecondUnit.UnitCategory = user.Deckdata.ThirdDeck.SecondRoleNum;
            FirstPlayerDeck.ThirdUnit.UnitCategory = user.Deckdata.ThirdDeck.ThirdRoleNum;
            FirstPlayerDeck.WepNumOne = user.Deckdata.ThirdDeck.FirstWeaponNum;
            FirstPlayerDeck.WepNumTwo = user.Deckdata.ThirdDeck.SecondWeaponNum;
            Debug.Log("3つ目ロードしたよ");
        }
        else if (FirstDeckNumDD.value == 3)
        {
            FirstPlayerDeck.FirstUnit.UnitNum = user.Deckdata.FourDeck.FirstFigureNum;
            FirstPlayerDeck.SecondUnit.UnitNum = user.Deckdata.FourDeck.SecondFigureNum;
            FirstPlayerDeck.ThirdUnit.UnitNum = user.Deckdata.FourDeck.ThirdFigureNum;
            FirstPlayerDeck.FirstUnit.UnitCategory = user.Deckdata.FourDeck.FirstRoleNum;
            FirstPlayerDeck.SecondUnit.UnitCategory = user.Deckdata.FourDeck.SecondRoleNum;
            FirstPlayerDeck.ThirdUnit.UnitCategory = user.Deckdata.FourDeck.ThirdRoleNum;
            FirstPlayerDeck.WepNumOne = user.Deckdata.FourDeck.FirstWeaponNum;
            FirstPlayerDeck.WepNumTwo = user.Deckdata.FourDeck.SecondWeaponNum;
            Debug.Log("4つ目ロードしたよ");
        }
        else if (FirstDeckNumDD.value == 4)
        {
            FirstPlayerDeck.FirstUnit.UnitNum = user.Deckdata.FifthDeck.FirstFigureNum;
            FirstPlayerDeck.SecondUnit.UnitNum = user.Deckdata.FifthDeck.SecondFigureNum;
            FirstPlayerDeck.ThirdUnit.UnitNum = user.Deckdata.FifthDeck.ThirdFigureNum;
            FirstPlayerDeck.FirstUnit.UnitCategory = user.Deckdata.FifthDeck.FirstRoleNum;
            FirstPlayerDeck.SecondUnit.UnitCategory = user.Deckdata.FifthDeck.SecondRoleNum;
            FirstPlayerDeck.ThirdUnit.UnitCategory = user.Deckdata.FifthDeck.ThirdRoleNum;
            FirstPlayerDeck.WepNumOne = user.Deckdata.FifthDeck.FirstWeaponNum;
            FirstPlayerDeck.WepNumTwo = user.Deckdata.FifthDeck.SecondWeaponNum;
            Debug.Log("5つ目ロードしたよ");
        }
        else
        {

        }

        Debug.Log("2Pのロードだよ");
        if (SecondDeckNumDD.value == 0)
        {
            SecondPlayerDeck.FirstUnit.UnitNum = user.Deckdata.FirstDeck.FirstFigureNum;
            SecondPlayerDeck.SecondUnit.UnitNum = user.Deckdata.FirstDeck.SecondFigureNum;
            SecondPlayerDeck.ThirdUnit.UnitNum = user.Deckdata.FirstDeck.ThirdFigureNum;
            SecondPlayerDeck.FirstUnit.UnitCategory = user.Deckdata.FirstDeck.FirstRoleNum;
            SecondPlayerDeck.SecondUnit.UnitCategory = user.Deckdata.FirstDeck.SecondRoleNum;
            SecondPlayerDeck.ThirdUnit.UnitCategory = user.Deckdata.FirstDeck.ThirdRoleNum;
            SecondPlayerDeck.WepNumOne = user.Deckdata.FirstDeck.FirstWeaponNum;
            SecondPlayerDeck.WepNumTwo = user.Deckdata.FirstDeck.SecondWeaponNum;
            Debug.Log("1つ目ロードしたよ");
        }
        else if (SecondDeckNumDD.value == 1)
        {
            SecondPlayerDeck.FirstUnit.UnitNum = user.Deckdata.SecondDeck.FirstFigureNum;
            SecondPlayerDeck.SecondUnit.UnitNum = user.Deckdata.SecondDeck.SecondFigureNum;
            SecondPlayerDeck.ThirdUnit.UnitNum = user.Deckdata.SecondDeck.ThirdFigureNum;
            SecondPlayerDeck.FirstUnit.UnitCategory = user.Deckdata.SecondDeck.FirstRoleNum;
            SecondPlayerDeck.SecondUnit.UnitCategory = user.Deckdata.SecondDeck.SecondRoleNum;
            SecondPlayerDeck.ThirdUnit.UnitCategory = user.Deckdata.SecondDeck.ThirdRoleNum;
            SecondPlayerDeck.WepNumOne = user.Deckdata.SecondDeck.FirstWeaponNum;
            SecondPlayerDeck.WepNumTwo = user.Deckdata.SecondDeck.SecondWeaponNum;
            Debug.Log("2つ目ロードしたよ");
        }
        else if (SecondDeckNumDD.value == 2)
        {
            SecondPlayerDeck.FirstUnit.UnitNum = user.Deckdata.ThirdDeck.FirstFigureNum;
            SecondPlayerDeck.SecondUnit.UnitNum = user.Deckdata.ThirdDeck.SecondFigureNum;
            SecondPlayerDeck.ThirdUnit.UnitNum = user.Deckdata.ThirdDeck.ThirdFigureNum;
            SecondPlayerDeck.FirstUnit.UnitCategory = user.Deckdata.ThirdDeck.FirstRoleNum;
            SecondPlayerDeck.SecondUnit.UnitCategory = user.Deckdata.ThirdDeck.SecondRoleNum;
            SecondPlayerDeck.ThirdUnit.UnitCategory = user.Deckdata.ThirdDeck.ThirdRoleNum;
            SecondPlayerDeck.WepNumOne = user.Deckdata.ThirdDeck.FirstWeaponNum;
            SecondPlayerDeck.WepNumTwo = user.Deckdata.ThirdDeck.SecondWeaponNum;
            Debug.Log("3つ目ロードしたよ");
        }
        else if (SecondDeckNumDD.value == 3)
        {
            SecondPlayerDeck.FirstUnit.UnitNum = user.Deckdata.FourDeck.FirstFigureNum;
            SecondPlayerDeck.SecondUnit.UnitNum = user.Deckdata.FourDeck.SecondFigureNum;
            SecondPlayerDeck.ThirdUnit.UnitNum = user.Deckdata.FourDeck.ThirdFigureNum;
            SecondPlayerDeck.FirstUnit.UnitCategory = user.Deckdata.FourDeck.FirstRoleNum;
            SecondPlayerDeck.SecondUnit.UnitCategory = user.Deckdata.FourDeck.SecondRoleNum;
            SecondPlayerDeck.ThirdUnit.UnitCategory = user.Deckdata.FourDeck.ThirdRoleNum;
            SecondPlayerDeck.WepNumOne = user.Deckdata.FourDeck.FirstWeaponNum;
            SecondPlayerDeck.WepNumTwo = user.Deckdata.FourDeck.SecondWeaponNum;
            Debug.Log("4つ目ロードしたよ");
        }
        else if (SecondDeckNumDD.value == 4)
        {
            SecondPlayerDeck.FirstUnit.UnitNum = user.Deckdata.FifthDeck.FirstFigureNum;
            SecondPlayerDeck.SecondUnit.UnitNum = user.Deckdata.FifthDeck.SecondFigureNum;
            SecondPlayerDeck.ThirdUnit.UnitNum = user.Deckdata.FifthDeck.ThirdFigureNum;
            SecondPlayerDeck.FirstUnit.UnitCategory = user.Deckdata.FifthDeck.FirstRoleNum;
            SecondPlayerDeck.SecondUnit.UnitCategory = user.Deckdata.FifthDeck.SecondRoleNum;
            SecondPlayerDeck.ThirdUnit.UnitCategory = user.Deckdata.FifthDeck.ThirdRoleNum;
            SecondPlayerDeck.WepNumOne = user.Deckdata.FifthDeck.FirstWeaponNum;
            SecondPlayerDeck.WepNumTwo = user.Deckdata.FifthDeck.SecondWeaponNum;
            Debug.Log("5つ目ロードしたよ");
        }
        else
        {

        }
        Debug.Log("プリロードしたよ");

        if ((FirstPlayerDeck.FirstUnit.UnitNum == FirstPlayerDeck.SecondUnit.UnitNum &&
             FirstPlayerDeck.SecondUnit.UnitNum == FirstPlayerDeck.ThirdUnit.UnitNum &&
             FirstPlayerDeck.FirstUnit.UnitNum == FirstPlayerDeck.ThirdUnit.UnitNum) ||
            (SecondPlayerDeck.FirstUnit.UnitNum == SecondPlayerDeck.SecondUnit.UnitNum &&
             SecondPlayerDeck.SecondUnit.UnitNum == SecondPlayerDeck.ThirdUnit.UnitNum &&
             SecondPlayerDeck.FirstUnit.UnitNum == SecondPlayerDeck.ThirdUnit.UnitNum))
        {
            return false;
        }

        return true;

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
        }

    }

    private AsyncOperation async;
    public GameObject LoadingUi;
    public Slider Slider;

    public void LoadNextScene()
    {
        
        Backkey.SetActive(false);
        bool DeckSelectCheck = PreLoadData();
        if (DeckSelectCheck)
        {
            Source_SE.PlayOneShot(click_SE);
            LoadingUi.SetActive(true);
            StartCoroutine(LoadScene());
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
        async = SceneManager.LoadSceneAsync("BattleSetting");

        while (!async.isDone)
        {
            Slider.value = async.progress;
            yield return null;
        }
    }
    async public void BackKey()
    {
        Source_SE.PlayOneShot(Pointer_SE);
        await Task.Delay(200);
        SceneManager.LoadScene("VS＿CategorySelect");
    }
}
