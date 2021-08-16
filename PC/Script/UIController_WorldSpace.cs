using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController_WorldSpace : MonoBehaviour
{

    private RectTransform myRectTfm;
    public Text PlayerNameText;
    public CplayerManager _target;
    Vector3 _targetPosition;

    void Start()
    {
        myRectTfm = GetComponent<RectTransform>();
        Debug.Log("呼ばれる：" + _target.photonView.Owner.NickName);
        PlayerNameText.text = _target.photonView.Owner.NickName;

    }

    void LateUpdate()
    {
        // 自身の向きをカメラに向ける
        myRectTfm.LookAt(Camera.main.transform);
    }
    public void SetTarget(CplayerManager target)
    {
        Debug.Log("呼ばれない：" + target);
        if (target == null)//targetがいなければエラーをConsoleに表示
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
            return;
        }
        else
        {
            Debug.Log("あがさ："+ target);
        }
        //targetの情報をこのスクリプト内で使うのでコピー
        _target = target;
        if (PlayerNameText != null)
        {
            PlayerNameText.text = _target.photonView.Owner.NickName;
        }
    }

}