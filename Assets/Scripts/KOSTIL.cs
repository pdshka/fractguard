using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KOSTIL : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        transform.Translate(0.4f * Time.deltaTime, 0, 0);
    }
}
