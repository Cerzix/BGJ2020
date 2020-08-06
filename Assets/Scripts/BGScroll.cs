using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed;
    // Update is called once per frame
    void Update()
    {
        float yVal = this.transform.position.y;

        this.transform.position = new Vector3(this.transform.position.x, yVal -= scrollSpeed * Time.deltaTime, this.transform.position.z);

        if(this.transform.position.y <= -1000)
        {
            this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        }
    }
}
