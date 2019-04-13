using UnityEngine;
using System.Collections;

public class AppleOfEden : MonoBehaviour, InteractionObserver
{
    public InteractionTrigger interactionTrigger;
    public GameObject appleOfEden;
    public Transform matchTransform;

    public bool isPuzzleComplete;
    public bool isPlayerInteracting;
    
    public float allowableError = 5f;
    public float fadeTime = 0.5f;
    public float towardsSpeed = 5f;
    public float rotateSpeed = 1.5f;

    void Start() {
        interactionTrigger.SetObserver(this);
        isPlayerInteracting = false;
        isPuzzleComplete = false;
    }

    void FixedUpdate() {
        if(isPuzzleComplete) {
            //After the puzzle is complete, rotate towards the target to match exactly with it
            appleOfEden.transform.rotation = Quaternion.RotateTowards(appleOfEden.transform.rotation, matchTransform.rotation, Time.deltaTime * towardsSpeed);
            return;
        }
            
        if(!isPlayerInteracting)
            return;

        //Rotate AoE
        float xAngle = Input.GetAxis("Vertical") * rotateSpeed;
        float yAngle = Input.GetAxis("Horizontal") * rotateSpeed;
        appleOfEden.transform.Rotate(xAngle, yAngle, 0f, Space.World);

        //Check if it matches
        float angle = Quaternion.Angle(matchTransform.rotation, appleOfEden.transform.rotation);
        bool sameRotation = Mathf.Abs (angle) < allowableError;
        if(sameRotation)
            OnPuzzleComplete();
    }

    public void OnPlayerInteract() {
        if(isPuzzleComplete)
            return;

        //Pressing the interact key should toggle the boolean
        //This means the player can try to solve the puzzle or cancel
        isPlayerInteracting = !isPlayerInteracting;
        GameManager.CaptureInput(isPlayerInteracting);
    }

    public void OnPuzzleComplete() {
        //Debug.Log("Puzzle complete");
        isPuzzleComplete = true;
        isPlayerInteracting = false;
        StartCoroutine(FreeInput());
    }

    public IEnumerator FreeInput() {
        yield return new WaitForSeconds(fadeTime);
        GameManager.CaptureInput(false);
    }
}
