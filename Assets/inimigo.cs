using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimigo : MonoBehaviour
{
    void Update()
    {
        if(transform.position.x <= -5)
        {
            Destroy(gameObject);
            //transform.position = new(5,-3,0);
        }

        transform.position -= Vector3.right * 5 * Time.deltaTime;
    }
}
