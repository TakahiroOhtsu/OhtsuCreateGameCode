using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour
{
    GameObject[] PageObject = new GameObject[22];
    public AudioClip click_SE;
    AudioSource Source_SE;

    void Start()
    {
        Source_SE = GetComponent<AudioSource>();
        for (int i=1; i <= 22; i++)
        {
            string PageName = i.ToString() + "Page";
            PageObject[i - 1] = GameObject.Find(PageName);
            if (i != 1)
            {
                PageObject[i-1].SetActive(false);
            }
            
        }
    }

    int NowPage = 0;
    // Start is called before the first frame update
    public void NextPage()
    {
        Source_SE.PlayOneShot(click_SE);
        if (NowPage < 21)
        {

            PageObject[NowPage].SetActive(false);

            NowPage++;

            if(NowPage <= 21) PageObject[NowPage].SetActive(true);

        }
        else
        {
            SceneManager.LoadScene("Mode");
        }
    }

    public void BackPage()
    {
        Source_SE.PlayOneShot(click_SE);
        if (NowPage > 0)
        {
            PageObject[NowPage].SetActive(false);

            NowPage--;

            if (NowPage >= 0) PageObject[NowPage].SetActive(true);

        }
        else
        {
            SceneManager.LoadScene("Mode");
        }
    }
}
