using System.Collections.Generic;

[System.Serializable]
public class DeckData
{
    public string DeckName;
    public int
        id, //最大5
        FirstFigureNum, //0:None,1:にこに立体ちゃん,2:Unityちゃん,3:プロ生ちゃん,4:オリジナル
        FirstRoleNum,
        SecondFigureNum,
        SecondRoleNum,
        ThirdFigureNum,
        ThirdRoleNum,
        FirstWeaponNum, //0:None,1:レーダー,2:地雷
        SecondWeaponNum;
}

[System.Serializable]
public class DeckSaveData
{
    [System.Serializable]
    public class DeckStat
    {
        public string DeckName;
        public int
            id, //最大5
            FirstFigureNum, //0:None,1:にこに立体ちゃん,2:Unityちゃん,3:プロ生ちゃん,4:オリジナル
            FirstRoleNum,
            SecondFigureNum,
            SecondRoleNum,
            ThirdFigureNum,
            ThirdRoleNum,
            FirstWeaponNum, //0:None,1:レーダー,2:地雷
            SecondWeaponNum;
    }
    public DeckStat FirstDeck;
    public DeckStat SecondDeck;
    public DeckStat ThirdDeck;
    public DeckStat FourDeck;
    public DeckStat FifthDeck;

}