using UnityEngine;
using Cinemachine;
using System.Collections;

public class HolderReward : MonoBehaviour, InteractionObserver
{
    public GameObject rewardObject;
    public InteractionTrigger interactionTrigger;
    public float duration = 3f;
    public float moveOffset = 1.65f;
    
    private Quaternion targetRotation;
    private bool canReward;

    void Start() {
        interactionTrigger.SetObserver(this);
        targetRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        canReward = false;
        interactionTrigger.gameObject.SetActive(false);
    }

    public void OnReward() {
        canReward = true;
        interactionTrigger.gameObject.SetActive(true);
        StartCoroutine( Move(gameObject.transform, gameObject.transform.position + Vector3.up * moveOffset, duration) );
    }


    IEnumerator Move(Transform fromPosition, Vector3 toPosition, float duration)
    {
        float elapsed = 0.0f;
        Vector3 startPosition = fromPosition.position;

        while (elapsed < duration) {
            fromPosition.position = Vector3.Slerp(startPosition, toPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        fromPosition.position = toPosition;
    }

    public void OnPlayerInteract() {
        if(canReward) {
            rewardObject.SetActive(false);
            GameManager.OnRewardObtained(Reward.COLOR_MATCH_PUZZLE);
            interactionTrigger.gameObject.SetActive(false);
        }
    }
}
