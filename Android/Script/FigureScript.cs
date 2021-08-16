using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureScript : MonoBehaviour
{

    Collider NewCollider;

    [SerializeField] public int helth;
    [SerializeField] public int MovePower;
    [SerializeField] public int AttackArea;
    [SerializeField] public int ClassNum;


    Vector3 up,down,left,right;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}
