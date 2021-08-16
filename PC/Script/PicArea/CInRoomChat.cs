using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class CInRoomChat : MonoBehaviourPunCallbacks
{
    #region 変数宣言
    //範囲チャット実装のためのオブジェクト、変数定義
    GameObject[] players;   //全てのプレイヤーキャラ取得用
    GameObject sender;      //送信キャラ取得用
    //GameObject myPlayer;    //自分のキャラ取得用
    GUIStyle ChatStyle = new GUIStyle();    //範囲チャットStyle
    GUIStyleState ChatStyleState = new GUIStyleState();
    GUIStyle AllChatStyle = new GUIStyle(); //全体チャットStyle
    GUIStyleState AllChatStyleState = new GUIStyleState();
    public Rect GuiRect = new Rect(0, 0, 300, 200); //チャットUIの大きさ設定用
    public bool IsVisible = true;   //チャットUI表示非表示フラグ
    public bool AlignBottom = true;
    public List<string> messages = new List<string>();  //チャットログ格納用List
    public List<int> chatKind = new List<int>(); //チャットログの種類格納用(範囲チャor全チャ)
    public string inputLine = "";//入力文章格納用String
    private Vector2 scrollPos = Vector2.zero;   //スクロールバー位置

    bool aliveFlag = true;
    #endregion

    public void Start()
    {
        onLoadAliveFlag();
        //myPlayerオブジェクト取得(範囲チャット発言時にpositionとmyPM使う)
        //GetmyPlayer();
        //範囲チャットの場合
        ChatStyleState.textColor = Color.blue;
        //文字がＵＩからあふれた場合は折り返す設定
        ChatStyle.normal = ChatStyleState;
        ChatStyle.wordWrap = true;

        //全体チャットの場合
        AllChatStyleState.textColor = Color.white;
        //文字がＵＩからあふれた場合は折り返す設定
        AllChatStyle.normal = AllChatStyleState;
        AllChatStyle.wordWrap = true;
    }
    public void Update()
    {
        //ChatUIの位置を調整
        this.GuiRect.y = Screen.height - this.GuiRect.height;
        /*this.GuiRect.y = Screen.height / 8;
        this.GuiRect.x = Screen.height / 8;*/
        //ChatUIの大きさ調整
        GuiRect.width = Screen.width / 1.2f; //3;
        GuiRect.height = Screen.height / 1.2f; //3;
    }
    public void OnGUI()
    {
        if (!this.IsVisible || !PhotonNetwork.InRoom)   //表示フラグがOFFまたはphotonにつながっていないとき
        {
            //UI非表示
            return;
        }
        //ChatUIの作成開始
        //チャットUI生成　Begin&EndAreaでチャットUIの位置と大きさを設定 
        GUILayout.Window(0, GuiRect, ChatUIWindow, "");   //チャットUIウインドウを作成
                                                          //Enterを押すと
        if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
        {
            //チャット入力待ち状態にする
            GUI.FocusControl("ChatInput");
        }
    }

    int ChatType = 0; // 0 全体チャット、1 範囲チャット
    // チャットUI生成
    void ChatUIWindow(int windowID)
    {
        //FocusがチャットUIに乗ってるときにEnterを押すとチャット発言が実行される
        if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
        {
            if (!string.IsNullOrEmpty(this.inputLine))  //チャット入力欄がNullやEmptyでない場合
            {
                //チャット送信関数実行
                SendChat();
                return;
            }
        }
        //垂直のコントロールグループ開始
        GUILayout.BeginVertical();
        //スクロールビュー開始位置
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        //チャットログ表示用フレキシブルスペース生成
        GUILayout.FlexibleSpace();
        //フレキシブルスペースにチャットログを表示
        //for (int i = 0; i <= messages.Count - 1; i++) 下から新しいログを表示したいならこちら
        for (int i = messages.Count - 1; i >= 0; i--)
        {
            switch (chatKind[i])
            {
                case 0://全チャットであれば
                    GUILayout.Label(messages[i], AllChatStyle);
                    break;
                case 1://範囲チャットであれば
                    GUILayout.Label(messages[i], ChatStyle);
                    break;
            }
        }
        //スクロールビュー終了
        GUILayout.EndScrollView();

        //水平のコントロールグループ開始
        GUILayout.BeginHorizontal();
        //入力テキストフィールド生成、Focusが乗った状態をChatInputと命名
        if (aliveFlag)
        {
            GUI.SetNextControlName("ChatInput");
            inputLine = GUILayout.TextField(inputLine, 200);
        }
        string ButtounName = string.Empty;
        switch (ChatType)
        {
            case 0:
                ButtounName = "all";
                break;
            case 1:
                ButtounName = "party";
                break;
        }
        //「Send」ボタンを生成かつ押したときにはチャット送信
        if (aliveFlag) 
        {
            if (GUILayout.Button(ButtounName, GUILayout.ExpandWidth(false)))
            {
                //チャット送信関数実行
                SendChat();
            }
        }
        
        //チャットの種類を切り替える
        /*
        if (GUILayout.Button("Change", GUILayout.ExpandWidth(false)))
        {
            ChatType = (ChatType + 1) % 2;
        }*/

        //水平のコントロールグループ終了
        GUILayout.EndHorizontal();
        //垂直のコントロールグループ終了
        GUILayout.EndVertical();
    }

    // チャット送信関数
    void SendChat()
    {
        //全体チャットに変更
        if (inputLine == "/s")
        {
            ChatType = 0;
            inputLine = "";
            return;
        }
        //パーティーチャットに変更
        else if (inputLine == "/p")
        {
            ChatType = 1;
            inputLine = "";
            return;
        }//ささやきチャットに変更
        else if (inputLine == "/w")
        {
            //ChatType = 2;
        }

        //chatRPC
        photonView.RPC("Chat", RpcTarget.All,
            //myPlayer.transform.position, GetmyPlayer関数を使う時にはここもコメントアウトを解除すること
            inputLine, ChatType);
        //送信後、入力欄を空にし、スクロール最下位置に移動
        inputLine = "";
    }

    // ChatRPC RPC呼出側：送信者　RPC受信側：受信者
    [PunRPC]
    public void Chat(
        //Vector3 senderposition,
        string newLine, int chat_type, PhotonMessageInfo mi)
    {
        if (messages.Count >= 100)          //チャットログが多くなって来たらログを削除してから受信
        {
            messages.Clear();               //全てのチャットログを削除
            chatKind.Clear();               //全てのチャットの種類情報削除
        }
        switch (chat_type)
        {
            case 0://全チャとして受信
                   //chat受信
                ReceiveChat(newLine, chat_type, mi);
                break;
            case 1://範囲チャとして受信
                   //myPlayerとsenderの距離から受信するか判断
                   //if (Vector3.Distance(myPlayer.transform.position, senderposition) < 10)
                {
                    //chat受信
                    ReceiveChat(newLine, chat_type, mi);
                }
                break;
        }

        //受信したときはスクロール最下位置
        //scrollPos.y = Mathf.Infinity;
    }
    // チャット受信関数
    void ReceiveChat(string _newLine, int chat_type, PhotonMessageInfo _mi)
    {
        //送信者の名前用変数
        string senderName = "anonymous";
        if (_mi.Sender != null)
        {
            //送信者の名前があれば
            if (!string.IsNullOrEmpty(_mi.Sender.NickName))
            {
                senderName = _mi.Sender.NickName;
            }
            else
            {
                senderName = "player " + _mi.Sender.UserId;
            }
        }
        //受信したチャットをログに追加
        this.messages.Add(senderName + ": " + _newLine);
        this.chatKind.Add(chat_type);
        return;
    }

    public void AddLine(string newLine)
    {
        this.messages.Add(newLine);
    }

    void onLoadAliveFlag()
    {
        GameObject AliveSetting = GameObject.Find("AliveSetting");
        if (AliveSetting != null)
        {
            aliveFlag = AliveSetting.GetComponent<AliveSettingScript>().AliveFlag[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        }
        //AliveSetting.GetComponent<AliveSettingScript>().
    }
}