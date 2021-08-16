using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public User user;

    public AudioClip click_SE;
    AudioSource Source_SE;
    // Start is called before the first frame update
    void Start()
    {
        Source_SE = GetComponent<AudioSource>();
        string loadDeck = PlayerPrefs.GetString("PlayerDeckData", "");
        if (string.IsNullOrEmpty(loadDeck))
        {
            user.Deckdata.FirstDeck.FirstFigureNum = 0;
            user.Deckdata.FirstDeck.SecondFigureNum = 1;
            user.Deckdata.FirstDeck.ThirdFigureNum = 2;
            user.Deckdata.FirstDeck.FirstRoleNum = 0;
            user.Deckdata.FirstDeck.SecondRoleNum = 1;
            user.Deckdata.FirstDeck.ThirdRoleNum = 2;
            user.Deckdata.FirstDeck.FirstWeaponNum = 0;
            user.Deckdata.FirstDeck.SecondWeaponNum = 1;
            user.Deckdata.SecondDeck.FirstFigureNum = 0;
            user.Deckdata.SecondDeck.SecondFigureNum = 1;
            user.Deckdata.SecondDeck.ThirdFigureNum = 2;
            user.Deckdata.SecondDeck.FirstRoleNum = 0;
            user.Deckdata.SecondDeck.SecondRoleNum = 1;
            user.Deckdata.SecondDeck.ThirdRoleNum = 2;
            user.Deckdata.SecondDeck.FirstWeaponNum = 0;
            user.Deckdata.SecondDeck.SecondWeaponNum = 1;
            user.Deckdata.ThirdDeck.FirstFigureNum = 0;
            user.Deckdata.ThirdDeck.SecondFigureNum = 1;
            user.Deckdata.ThirdDeck.ThirdFigureNum = 2;
            user.Deckdata.ThirdDeck.FirstRoleNum = 0;
            user.Deckdata.ThirdDeck.SecondRoleNum = 1;
            user.Deckdata.ThirdDeck.ThirdRoleNum = 2;
            user.Deckdata.ThirdDeck.FirstWeaponNum = 0;
            user.Deckdata.ThirdDeck.SecondWeaponNum = 1;
            user.Deckdata.FourDeck.FirstFigureNum = 0;
            user.Deckdata.FourDeck.SecondFigureNum = 1;
            user.Deckdata.FourDeck.ThirdFigureNum = 2;
            user.Deckdata.FourDeck.FirstRoleNum = 0;
            user.Deckdata.FourDeck.SecondRoleNum = 1;
            user.Deckdata.FourDeck.ThirdRoleNum = 2;
            user.Deckdata.FourDeck.FirstWeaponNum = 0;
            user.Deckdata.FourDeck.SecondWeaponNum = 1;
            user.Deckdata.FifthDeck.FirstFigureNum = 0;
            user.Deckdata.FifthDeck.SecondFigureNum = 1;
            user.Deckdata.FifthDeck.ThirdFigureNum = 2;
            user.Deckdata.FifthDeck.FirstRoleNum = 0;
            user.Deckdata.FifthDeck.SecondRoleNum = 1;
            user.Deckdata.FifthDeck.ThirdRoleNum = 2;
            user.Deckdata.FifthDeck.FirstWeaponNum = 0;
            user.Deckdata.FifthDeck.SecondWeaponNum = 1;

            string json = JsonUtility.ToJson(user);
            print(json);
            PlayerPrefs.SetString("PlayerDeckData", json);
        }
    }

    async public void PressStart()
    {
        Source_SE.PlayOneShot(click_SE);
        await Task.Delay(100);
        SceneManager.LoadScene("Mode");
    }



}
