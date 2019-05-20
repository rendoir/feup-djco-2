using UnityEngine;
using UnityEditor;
using System.Collections;

public class LightPoint : MonoBehaviour, InteractionObserver
{
    public InteractionTrigger interactionTrigger;
    public GameObject tooltip;

    public bool isPlayerInteracting;

    private bool playerCancelled;
    private bool isPuzzleComplete;

    public int maxReflectionCount = 5;
    public float maxStepDistance = 200;

    public float rotateSpeed = 1.5f;
    public float fadeTime = 0.5f;
    public float tooltipTime = 4f;
    public float removeLaserTime = 3f;

    public GameObject laserObject;

    private GameObject laserClone;

    private LineRenderer laserLR;
    private float tooltipTimeCounter;


    void Start() {
        interactionTrigger.SetObserver(this);
        isPlayerInteracting = false;
        isPuzzleComplete = false;
        playerCancelled = false;
        tooltipTimeCounter = tooltipTime;
        
    }

    void FixedUpdate(){

        if(isPuzzleComplete) {
            return;
        }

        if (playerCancelled)
        {
            // //When player cancels the puzze, reset the rotation 
            // Destroy(laserClone.gameObject);
            playerCancelled = false;
            return;
        }

        if(!isPlayerInteracting)
            return;

        //Rotate LightPoint
        float zAngle = Input.GetAxis("Rotation") * -rotateSpeed;
        float yAngle = Input.GetAxis("Horizontal") * rotateSpeed;
        float xAngle = Input.GetAxis("Vertical") * -rotateSpeed;
        laserClone.transform.Rotate(xAngle, yAngle, zAngle, Space.World);
        DrawPredictedReflectionPattern(laserClone.transform.position + laserClone.transform.forward * 0.75f, laserClone.transform.forward, 0);
    }


    private void DrawPredictedReflectionPattern(Vector3 position, Vector3 direction, int refletionsMade)
    {
        if (refletionsMade == maxReflectionCount)
        {
            return;
        }

        Vector3 startingPosition = position;

        int layer_mask = LayerMask.GetMask("mirror");

        Ray ray = new Ray(position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxStepDistance, layer_mask))
        {
            direction = Vector3.Reflect(direction, hit.normal);
            position = hit.point;
            if(hit.collider.gameObject.name == "Target")
                StartCoroutine(OnPuzzleComplete());
        }
        else if(!Physics.Raycast(ray, out hit, maxStepDistance))
        {
            position += direction * maxStepDistance;
        }else{
            position = hit.point;
        }

        
        if(laserLR != null){
            
            laserLR.SetPosition(refletionsMade + 1, position);
        }

        DrawPredictedReflectionPattern(position, direction, refletionsMade + 1);
    }

    public void OnPlayerInteract() {
        if(isPuzzleComplete)
            return;

        playerCancelled = isPlayerInteracting;
        tooltipTimeCounter = tooltipTime;

        isPlayerInteracting = !isPlayerInteracting;
        if(isPlayerInteracting)
            GameManager.CaptureInput(true);
        else StartCoroutine(FreeInput());

        if(playerCancelled){
            Destroy(laserClone);
            return;
        }
    
        tooltip.SetActive(true);
        StartCoroutine(HideTooltip());
     

        laserClone = (GameObject) Instantiate(laserObject, transform.position, transform.rotation);
        

        Laser laser = laserClone.GetComponent<Laser>();
        laser.Start();

        laserLR = laser.getLineRenderer();
        laserLR.transform.position = transform.position;
        laserLR.SetPosition(0, transform.position);
        laserLR.positionCount = maxReflectionCount + 1;
        DrawPredictedReflectionPattern(this.transform.position + this.transform.forward * 0.75f, this.transform.forward, 0);

        Input.ResetInputAxes();
        
    }

    public IEnumerator OnPuzzleComplete() {
        isPuzzleComplete = true;
        isPlayerInteracting = false;
        interactionTrigger.gameObject.SetActive(false);
        yield return new WaitForSeconds(removeLaserTime);
        Destroy(laserClone);
        StartCoroutine(FreeInput());
    }

     public IEnumerator FreeInput() {
        yield return new WaitForSeconds(fadeTime);
        GameManager.CaptureInput(false);
    }

    public IEnumerator HideTooltip() {
        if(isPlayerInteracting && !playerCancelled && !isPuzzleComplete)
            yield return new WaitForSeconds(tooltipTime);
        tooltip.SetActive(false);
    }
}
