using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLockInputHandler : MonoBehaviour
{
    public GameObject RedCanister;
    public GameObject GreenCanister;
    public GameObject BlueCanister;

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RedCanister.GetComponent<CanisterController>().addColor();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            GreenCanister.GetComponent<CanisterController>().addColor();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            BlueCanister.GetComponent<CanisterController>().addColor();
        }
    }
}
