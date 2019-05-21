using UnityEngine;
using Cinemachine;
using System.Collections;

public class ChestReward : MonoBehaviour, InteractionObserver
{
    public GameObject chestLid;
    public GameObject rewardObject;
    public InteractionTrigger interactionTrigger;
    public float angle = 120;
    public float duration = 1.5f;
    
    private Quaternion targetRotation;
    private bool canReward;

    void Start() {
        interactionTrigger.SetObserver(this);
        targetRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        canReward = false;
    }

    public void OnReward() {
        canReward = true;
        StartCoroutine( Rotate(Vector3.left, angle, duration) );
    }

    IEnumerator Rotate( Vector3 axis, float angle, float duration)
    {
        Quaternion from = chestLid.transform.rotation;
        Quaternion to = chestLid.transform.rotation;
        to *= Quaternion.Euler( axis * angle );
        
        float elapsed = 0.0f;
        while( elapsed < duration ) {
            chestLid.transform.rotation = Quaternion.Slerp(from, to, elapsed / duration );
            elapsed += Time.deltaTime;
            yield return null;
        }
        chestLid.transform.rotation = to;
    }

    public void OnPlayerInteract() {
        if(canReward) {
            rewardObject.SetActive(false);
            GameManager.OnRewardObtained();
        }
    }
}
