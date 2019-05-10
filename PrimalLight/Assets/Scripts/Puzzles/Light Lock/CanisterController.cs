using UnityEngine;

public class CanisterController : MonoBehaviour
{
    [Header("Game Object References")]
    public GameObject Canister;
    public GameObject MixingStation;

    [Header("Canister Properties")]
    public Color CanisterColor;
    public int TotalCanisterFluid = 100;
    public float ExtractionRate = 0.01f;
    private int CurrentCanisterFluid;

    private MixingStationController mxController;
    private Renderer canisRenderer;

    // Start is called before the first frame update
    void Start()
    {
        this.canisRenderer = Canister.GetComponent<Renderer>();
        this.setColor(CanisterColor);
        this.mxController = this.MixingStation.GetComponent<MixingStationController>();

        this.CurrentCanisterFluid = this.TotalCanisterFluid;
    }

    private void setColor(Color color)
    {
        canisRenderer.material.shader = Shader.Find("_Color");
        canisRenderer.material.SetColor("_Color", color);

        //Find the Specular shader and change its Color to red
        canisRenderer.material.shader = Shader.Find("Specular");
        canisRenderer.material.SetColor("_SpecColor", color);
    }

    public void addColor()
    {
        Color delta = CanisterColor;
        delta.r *= ExtractionRate;
        delta.g *= ExtractionRate;
        delta.b *= ExtractionRate;
        this.mxController.addColor(delta);
        this.CanisterColor -= delta;
        this.setColor(CanisterColor);
    }

    public void removeColor()
    {
        Color delta = CanisterColor;
        delta.r *= ExtractionRate;
        delta.g *= ExtractionRate;
        delta.b *= ExtractionRate;
        this.mxController.removeColor(delta);
        this.CanisterColor += delta;
        this.setColor(CanisterColor);
    }
}
