using UnityEngine;
using UnityEditor;

public class LightPoint : MonoBehaviour, InteractionObserver
{
    public InteractionTrigger interactionTrigger;

    public bool isPlayerInteracting;

    private bool playerCancelled;

    public int maxReflectionCount = 5;
    public float maxStepDistance = 200;

    public float rotateSpeed = 1.5f;

    public GameObject laserObject;

    private GameObject laserClone;

    private LineRenderer laserLR;


    void Start() {
        interactionTrigger.SetObserver(this);
        isPlayerInteracting = false;
        playerCancelled = false;
        
    }

    void FixedUpdate(){

        if (playerCancelled)
        {
            // Debug.Log(laserClone.gameObject);
            // //When player cancels the puzze, reset the rotation 
            // Destroy(laserClone.gameObject);
            playerCancelled = false;
            return;
        }

        if(!isPlayerInteracting)
            return;

        //Rotate AoE
        float xAngle = Input.GetAxis("Vertical") * rotateSpeed;
        float yAngle = Input.GetAxis("Horizontal") * rotateSpeed;
        laserClone.transform.Rotate(xAngle, yAngle, 0f, Space.World);
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
            // Debug.Log(hit.point);
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
        playerCancelled = isPlayerInteracting;
        isPlayerInteracting = !isPlayerInteracting;
        GameManager.CaptureInput(isPlayerInteracting);
        if(playerCancelled){
            Destroy(laserClone);
            return;
        }
        Debug.Log(playerCancelled);
        Debug.Log("Player Interacted");
     

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
}
