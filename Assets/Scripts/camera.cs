using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera cameraComponent;


    private void Start()
    {
        cameraComponent = GetComponent<Camera>();

    }

    private void Update()
    {
        Debug.Log("Screen.height " + Screen.height);
        Debug.Log("Screen.width " + Screen.width);

    }
}