using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBGMScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // 縦
        Screen.autorotateToPortrait = true;
        // 左
        Screen.autorotateToLandscapeLeft = false;
        // 右
        Screen.autorotateToLandscapeRight = false;
        // 上下反転
        Screen.autorotateToPortraitUpsideDown = true;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "BattleSetting"　|| SceneManager.GetActiveScene().name == "OnlineBattleSetting")
        {
            Destroy(this.gameObject);
        }
    }
}
