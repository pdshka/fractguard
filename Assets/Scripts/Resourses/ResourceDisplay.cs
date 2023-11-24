using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField]
    private Resource resource;

    private void Start()
    {
        GetComponent<Image>().sprite = resource.sprite;
    }
}
