using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Spiner : MonoBehaviour
{
    public float SpinSpeed = 1f;
    public GameObject SpinObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpinObject.transform.Rotate(new Vector3 (0,0,SpinSpeed));
    }
}
