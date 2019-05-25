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
    public float extractionRate = 0.1f;
    public float epsilon = 1e-14f;
    public float acceptableError = 0.05f;
    private Color currentColor;
    private bool isPuzzleComplete;
    private float initialCanisterScale;
    private float initialCanisterPosition;


    void Start() {
        isPuzzleComplete = false;
        currentColor = Color.black;
        matchingSurfaceRenderer.material.SetColor("_EmissionColor", matchingColor);
        InitCanister(redCanisterFluid, Color.red);
        InitCanister(greenCanisterFluid, Color.green);
        InitCanister(blueCanisterFluid, Color.blue);
        initialCanisterScale = redCanisterFluid.transform.localScale.y;
        initialCanisterPosition = redCanisterFluid.transform.localPosition.y;
    }

    void InitCanister(GameObject canister, Color color) {
        Renderer canisterRenderer = canister.GetComponent<Renderer>();
        canisterRenderer.material.SetColor("_EmissionColor", color * 0.5f);
        canisterRenderer.material.SetColor("_Color", color);
    }

    void FixedUpdate() {
        if(isPuzzleComplete)
            return;

        HandleInput();
        UpdateWithCurrentColor();
        CheckCompleteness();
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
        beam.material.SetColor("_EmissionColor", currentColor);
        pointLight.color = currentColor;
    }

    void CheckCompleteness() {
        if (Mathf.Abs(currentColor.r - matchingColor.r) <= acceptableError 
            && Mathf.Abs(currentColor.g - matchingColor.g) <= acceptableError
            && Mathf.Abs(currentColor.b - matchingColor.b) <= acceptableError)
                OnPuzzleComplete();
    }

    void OnColorChanged(GameObject canisterFluid, Color color, int sign) {
        float extraction = extractionRate * Time.fixedDeltaTime * sign;
        float yScale = canisterFluid.transform.localScale.y - extraction * initialCanisterScale;
        yScale = Mathf.Clamp(yScale, epsilon, initialCanisterScale);
        float yPosition;
        if(sign > 0)
            yPosition = initialCanisterPosition - (initialCanisterScale - yScale);
        else yPosition = initialCanisterPosition + (yScale - initialCanisterScale);

        currentColor += color * extraction;
        canisterFluid.transform.localScale = new Vector3(canisterFluid.transform.localScale.x, yScale, canisterFluid.transform.localScale.z);
        canisterFluid.transform.localPosition = new Vector3(canisterFluid.transform.localPosition.x, yPosition, canisterFluid.transform.localPosition.z);
    }

    void OnPuzzleComplete() {
        isPuzzleComplete = true;
    }
}