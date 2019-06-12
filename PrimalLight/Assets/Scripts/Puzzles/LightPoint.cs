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
    public float removeLaserTime = 1f;

    public GameObject laserObject;
    public LayerMask mirrorMask;
    public LayerMask surfaceMask;
    public LayerMask playerMask;
    public GameObject target;

    private GameObject laserClone;
    private LineRenderer laserLR;
    private float tooltipTimeCounter;
    private Coroutine inputCoroutine;
    private Coroutine tooltipCoroutine;

    public GameObject wallToRemove;

    public Vector3 offset;
	public IEnumerator movement;
	public float movementSpeed = 0.5f;
	private Vector3 initPos;


    void Start() {
        interactionTrigger.SetObserver(this);
        isPlayerInteracting = false;
        isPuzzleComplete = false;
        playerCancelled = false;
        tooltipTimeCounter = tooltipTime;
        inputCoroutine = null;
    }

    void FixedUpdate(){

        if(isPuzzleComplete) {
            return;
        }

        if (playerCancelled) {
            return;
        }

        if(!isPlayerInteracting)
            return;

        //Rotate LightPoint
        float xAngle = GameInput.rotation * rotateSpeed;
        float yAngle = GameInput.horizontal * rotateSpeed;
        float zAngle = GameInput.vertical * rotateSpeed;
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

        Ray ray = new Ray(position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxStepDistance, mirrorMask))
        {
            
            position = hit.point;
            if(hit.collider.gameObject == target)
                StartCoroutine(OnPuzzleComplete());
            else{
                direction = Vector3.Reflect(direction, hit.normal);
            }
        }
        else if(!Physics.Raycast(ray, out hit, maxStepDistance))
        {
            position += direction * maxStepDistance;
        }else{
            position = hit.point;
            direction = new Vector3(0,0,0);
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
        if(isPlayerInteracting) {
            if(inputCoroutine != null)
                StopCoroutine(inputCoroutine);
            GameInput.CaptureInput(true);
        } else inputCoroutine = StartCoroutine(FreeInput());

        if(tooltipCoroutine != null) 
            StopCoroutine(tooltipCoroutine);

        tooltip.SetActive(true);
        tooltipCoroutine = StartCoroutine(HideTooltip());

        if(playerCancelled){
            Destroy(laserClone);
            return;
        }

        laserClone = (GameObject) Instantiate(laserObject, transform.position, transform.rotation);
        

        Laser laser = laserClone.GetComponent<Laser>();
        laser.Start();

        laserLR = laser.getLineRenderer();
        laserLR.transform.position = transform.position;
        laserLR.SetPosition(0, transform.position);
        laserLR.positionCount = maxReflectionCount + 1;
        DrawPredictedReflectionPattern(this.transform.position + this.transform.forward * 0.75f, this.transform.forward, 0);        
    }

    public IEnumerator OnPuzzleComplete() {
        isPuzzleComplete = true;
        isPlayerInteracting = false;
        interactionTrigger.gameObject.SetActive(false);
        GameSound.Play("PuzzleComplete");        
        
        yield return new WaitForSeconds(removeLaserTime);
        
        initPos = wallToRemove.transform.position;
        WallGoUp();

        Destroy(laserClone);
        StartCoroutine(FreeInput());
    }

     public void WallGoUp() {
         Vector3 targetPos = initPos+offset;
		if(movement != null)
			StopCoroutine(movement);
		movement = MovementUtils.SmoothMovement((bool end) => {
	        			movement = null;
	       			},wallToRemove,targetPos,movementSpeed);
		StartCoroutine(movement);
     }

     public IEnumerator FreeInput() {
        yield return new WaitForSeconds(fadeTime);
        GameInput.CaptureInput(false);
    }

    public IEnumerator HideTooltip() {
        if(isPlayerInteracting && !playerCancelled && !isPuzzleComplete)
            yield return new WaitForSeconds(tooltipTime);
        tooltip.SetActive(false);
    }
}
