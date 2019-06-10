using UnityEngine;
using Cinemachine;
using System.Collections;

public class AppleOfEden : MonoBehaviour, InteractionObserver
{
    public InteractionTrigger interactionTrigger;
    public GameObject appleOfEden;
    public Transform matchTransform;
    private Quaternion initialRotation;
    public GameObject tooltip;
    public CinemachineFreeLook appleCamera;
    public ChestReward reward;

    public bool isPuzzleComplete;
    public bool isPlayerInteracting;
    private bool playerCancelled;
    
    public float allowableError = 5f;
    public float fadeTime = 0.5f;
    public float tooltipTime = 4f;
    public float towardsSpeed = 5f;
    public float rotateSpeed = 1.5f;
    public float resetSpeed = 50f;

    private Coroutine inputCoroutine;
    private Coroutine tooltipCoroutine;

    void Start() {
        interactionTrigger.SetObserver(this);
        isPlayerInteracting = false;
        isPuzzleComplete = false;
        playerCancelled = false;
        initialRotation = appleOfEden.transform.rotation;
    }

    void FixedUpdate() {
        if(isPuzzleComplete) {
            //After the puzzle is complete, rotate towards the target to match exactly with it
            appleOfEden.transform.rotation = Quaternion.RotateTowards(appleOfEden.transform.rotation, matchTransform.rotation, Time.deltaTime * towardsSpeed);
            return;
        }

        if(playerCancelled) {
            //When player cancels the puzze, reset the rotation 
            appleOfEden.transform.rotation = Quaternion.RotateTowards(appleOfEden.transform.rotation, initialRotation, Time.deltaTime * resetSpeed);
            return;
        }
            
        if(!isPlayerInteracting)
            return;

        //Rotate AoE
        float xAngle = GameInput.rotation * rotateSpeed;
        float yAngle = GameInput.vertical * rotateSpeed;
        float zAngle = GameInput.horizontal * rotateSpeed;
        appleOfEden.transform.Rotate(xAngle, yAngle, zAngle, Space.Self);

        //Check if it matches
        float angle = Quaternion.Angle(matchTransform.rotation, appleOfEden.transform.rotation);
        bool sameRotation = Mathf.Abs (angle) < allowableError;
        if(sameRotation)
            OnPuzzleComplete();
    }

    public void OnPlayerInteract() {
        if(isPuzzleComplete)
            return;

        playerCancelled = isPlayerInteracting;
        //Pressing the interact key should toggle the boolean
        //This means the player can try to solve the puzzle or cancel
        isPlayerInteracting = !isPlayerInteracting;
        if(isPlayerInteracting) {
            if(inputCoroutine != null)
                StopCoroutine(inputCoroutine);
            GameInput.CaptureInput(true);
            EnableCamera();
        }
        else {
            inputCoroutine = StartCoroutine(FreeInput());
            DisableCamera();
        }

        if(tooltipCoroutine != null) 
            StopCoroutine(tooltipCoroutine);
        
        tooltip.SetActive(true);
        tooltipCoroutine = StartCoroutine(HideTooltip());
    }

    public void OnPuzzleComplete() {
        isPuzzleComplete = true;
        isPlayerInteracting = false;
        interactionTrigger.gameObject.SetActive(false);
        StartCoroutine(FreeInput());
        DisableCamera();
        reward.OnReward();
        GameSound.Play("PuzzleComplete");
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

    public void DisableCamera() {
        appleCamera.m_Priority = 0;
    }

    public void EnableCamera() {
        appleCamera.m_Priority = 100;
    }
}
