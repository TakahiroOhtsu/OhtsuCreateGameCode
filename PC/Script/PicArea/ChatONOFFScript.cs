using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatONOFFScript : MonoBehaviour
{

    public GameObject ChatManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChatVisable()
    {
        /*
        if (ChatManager.GetComponent<CInRoomChat>().IsVisible)
        {
            ChatManager.GetComponent<CInRoomChat>().IsVisible = false;
        }
        else
        {
            ChatManager.GetComponent<CInRoomChat>().IsVisible = true;
        }*/
        if (ChatManager.activeSelf == true)
        {
            ChatManager.SetActive(false);
        }
        else
        {
            ChatManager.SetActive(true);
        }
    }
}
