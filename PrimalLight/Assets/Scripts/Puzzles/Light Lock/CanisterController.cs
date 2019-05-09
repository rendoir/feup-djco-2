using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanisterController : MonoBehaviour
{
    [Header("Game Object References")]
    public GameObject Canister;
    public GameObject MixingStation;

    [Header("Canister Properties")]
    public Color CanisterColor;
    public int TotalCanisterFluid = 100;
    private int CurrentCanisterFluid;

    private MixingStationController mxController;

    // Start is called before the first frame update
    void Start()
    {
        this.setColor(CanisterColor);
        this.mxController = this.MixingStation.GetComponent<MixingStationController>();

        this.CurrentCanisterFluid = this.TotalCanisterFluid;
    }

    private void setColor(Color color)
    {
        Renderer renderer = Canister.GetComponent<Renderer>();
        renderer.material.shader = Shader.Find("_Color");
        renderer.material.SetColor("_Color", color);

        //Find the Specular shader and change its Color to red
        renderer.material.shader = Shader.Find("Specular");
        renderer.material.SetColor("_SpecColor", color);
    }
}
