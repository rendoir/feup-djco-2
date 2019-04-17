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

    void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.ArrowHandleCap(0, this.transform.position + this.transform.forward * 0.25f, this.transform.rotation, 0.5f, EventType.Repaint);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, 0.25f);

        
    }

    private void DrawPredictedReflectionPattern(Vector3 position, Vector3 direction, int refletionsMade)
    {
        if (refletionsMade == maxReflectionCount)
        {
            return;
        }

        Vector3 startingPosition = position;

        Ray ray = new Ray(position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxStepDistance))
        {
            direction = Vector3.Reflect(direction, hit.normal);
            position = hit.point;
            // Debug.Log(hit.point);
        }
        else
        {
            position += direction * maxStepDistance;
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
        Debug.Log(laserClone.gameObject);
        

        Laser laser = laserClone.GetComponent<Laser>();
        laser.Start();

        laserLR = laser.getLineRenderer();
        laserLR.transform.position = transform.position;
        laserLR.SetPosition(0, transform.position);
        laserLR.positionCount = maxReflectionCount + 1;
        Debug.Log(laserLR.positionCount);
        DrawPredictedReflectionPattern(this.transform.position + this.transform.forward * 0.75f, this.transform.forward, 0);

        Input.ResetInputAxes();
        
    }
}
