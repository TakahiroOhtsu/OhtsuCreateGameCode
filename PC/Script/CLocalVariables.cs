using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLocalVariables : MonoBehaviour
{
    // Start is called before the first frame update
    //現在のHP
    static public int currentHP = 100;
    static public bool TransformWolfFlag = false;

    // Use this for initialization
    void Start()
    {
        VariableReset();
    }

    static public void VariableReset() //変数初期化
    {
        currentHP = 100;
        TransformWolfFlag = false;
    }
}
