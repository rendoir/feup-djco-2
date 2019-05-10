using UnityEngine;

public class MixingStationController : MonoBehaviour
{
    [Header("Object Reference")]
    public GameObject[] Locks;
    public GameObject canister;
    private Renderer canisterRenderer;

    private int currentLockIndex = 0;
    private GameObject currentLock;
    private LockController currentLockController;

    void Start()
    {
        canisterRenderer = canister.GetComponent<Renderer>();
        setCurrentLock();
        setMixerColor();
    }

    private void setCurrentLock()
    {
        currentLock = Locks[currentLockIndex];
        currentLockController = currentLock.GetComponent<LockController>();
    }

    public void addColor(Color delta)
    {
        currentLockController.addColor(delta);
        setMixerColor();
    }

    public void removeColor(Color delta)
    {
        currentLockController.removeColor(delta);
        setMixerColor();
    }

    private void setMixerColor()
    {
        canisterRenderer.material.shader = Shader.Find("_Color");
        canisterRenderer.material.SetColor("_Color", currentLockController.getCurrentColor());

        //Find the Specular shader and change its Color to red
        canisterRenderer.material.shader = Shader.Find("Specular");
        canisterRenderer.material.SetColor("_SpecColor", currentLockController.getCurrentColor());
    }
}
