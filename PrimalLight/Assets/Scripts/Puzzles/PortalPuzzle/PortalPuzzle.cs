using UnityEngine;

public class PortalPuzzle : MonoBehaviour
{
    [Header("Matching Color")]
    public Color matchingColor;
    public Renderer matchingSurfaceRenderer;

    [Header("Canisters")]
    public GameObject redCanisterFluid;
    public GameObject greenCanisterFluid;
    public GameObject blueCanisterFluid;

    [Header("Portal")]
    public Renderer portal;
    public Renderer beam;
    public Light pointLight; 

    [Header("Puzzle")]
    private Color currentColor;
    public float extractionRate = 0.1f;
    public float epsilon = 1e-14f;
    private float initialCanisterScale;


    void Start() {
        currentColor = Color.black;
        matchingSurfaceRenderer.material.SetColor("_EmissionColor", matchingColor);
        InitCanister(redCanisterFluid, Color.red);
        InitCanister(greenCanisterFluid, Color.green);
        InitCanister(blueCanisterFluid, Color.blue);
        initialCanisterScale = redCanisterFluid.transform.localScale.y;
    }

    void InitCanister(GameObject canister, Color color) {
        Renderer canisterRenderer = canister.GetComponent<Renderer>();
        canisterRenderer.material.SetColor("_EmissionColor", color * 0.5f);
        canisterRenderer.material.SetColor("_Color", color);
    }

    void FixedUpdate() {
        HandleInput();
        UpdateWithCurrentColor();
    }

    void HandleInput() {
        int sign = 1;
        
        if(GameInput.colorInput.remove)
            sign = -1;

        if(GameInput.colorInput.R) OnColorChanged(redCanisterFluid, Color.red, sign);
        if(GameInput.colorInput.G) OnColorChanged(greenCanisterFluid, Color.green, sign);
        if(GameInput.colorInput.B) OnColorChanged(blueCanisterFluid, Color.blue, sign);
    }

    void UpdateWithCurrentColor() {
        currentColor.a = 1f;
        currentColor.r = Mathf.Clamp(currentColor.r, 0f, 1f);
        currentColor.g = Mathf.Clamp(currentColor.g, 0f, 1f);
        currentColor.b = Mathf.Clamp(currentColor.b, 0f, 1f);
        portal.material.SetColor("_EmissionColor", currentColor * 0.5f);
        beam.material.SetColor("_Color", currentColor);
        pointLight.color = currentColor;
    }

    void OnColorChanged(GameObject canisterFluid, Color color, int sign) {
        float extraction = extractionRate * Time.fixedDeltaTime * sign;
        float yScale = canisterFluid.transform.localScale.y - extraction * initialCanisterScale;
        yScale = Mathf.Clamp(yScale, epsilon, initialCanisterScale);

        currentColor += color * extraction;
        canisterFluid.transform.localScale = new Vector3(canisterFluid.transform.localScale.x, yScale, canisterFluid.transform.localScale.z);
    }
}
