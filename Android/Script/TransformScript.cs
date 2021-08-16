using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformScript : MonoBehaviour
{
    // Start is called before the first frame update

    float timeCount = 0;
    Vector3 FirstTitlePosition;
    Vector3 FirstSacle;

    void Start()
    {
        Vector3 NowTitlePosition;
        FirstTitlePosition = this.transform.position;
        FirstSacle = this.transform.localScale;
        NowTitlePosition = this.transform.position;
        NowTitlePosition.y += 500;
        this.transform.position = NowTitlePosition;
        this.transform.localScale = new Vector3(0.5f,0.5f,1);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y > FirstTitlePosition.y)
        {
            Vector3 NowTitlePosition;
            NowTitlePosition = this.transform.position;
            NowTitlePosition.y -= 5;
            this.transform.position = NowTitlePosition;
        }
        if (this.transform.position.y == FirstTitlePosition.y &&
            this.transform.localScale != FirstSacle)
        {
            Vector3 NowScale;
            NowScale = this.transform.localScale;
            NowScale += new Vector3(0.1f, 0.1f, 0);
            this.transform.localScale = NowScale;

        }
    }
}
