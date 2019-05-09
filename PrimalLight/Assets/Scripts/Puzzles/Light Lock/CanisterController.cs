using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanisterController : MonoBehaviour
{
    public Color CanisterColor;
    public GameObject Canister;
    public GameObject MixingStation;

    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer = Canister.GetComponent<Renderer>();
        renderer.material.shader = Shader.Find("_Color");
        renderer.material.SetColor("_Color", CanisterColor);

        //Find the Specular shader and change its Color to red
        renderer.material.shader = Shader.Find("Specular");
        renderer.material.SetColor("_SpecColor", CanisterColor);
    }
}
