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
        if (GameInput.colorInput.R)
        {
            RedCanister.GetComponent<CanisterController>().addColor();
        }
        if (GameInput.colorInput.G)
        {
            GreenCanister.GetComponent<CanisterController>().addColor();
        }
        if (GameInput.colorInput.B)
        {
            BlueCanister.GetComponent<CanisterController>().addColor();
        }
    }
}
