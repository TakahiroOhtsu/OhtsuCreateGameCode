using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class field_selecter : MonoBehaviour
{
    [SerializeField] public Material NORMAL;
    [SerializeField] public Material MOVE;
    [SerializeField] public Material ATTACK;
    [SerializeField] public Material RADER;

    [SerializeField] public GameObject UP;
    [SerializeField] public GameObject DOWN;
    [SerializeField] public GameObject LEFT;
    [SerializeField] public GameObject RIGHT;

    [SerializeField] public int LetterCount;

    int ColliderTrigger = 0;
    int writeflag = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    /*private void FixedUpdate()
    {
        
    }*/

    private void LateUpdate()
    {

    }

    
    private void OnCollisionEnter(Collision collision)
    {
        ColliderTrigger = 1;
        Debug.Log(ColliderTrigger);
        this.GetComponent<Renderer>().material = MOVE;
    }

    private void OnCollisionExit(Collision collision)
    {
        ColliderTrigger = 0;
        Debug.Log(ColliderTrigger);
        this.GetComponent<Renderer>().material = NORMAL;
    }
}
