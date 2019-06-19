using UnityEngine;
using Cinemachine;
using System.Collections;

public class MirrorReward : MonoBehaviour, InteractionObserver
{
    public GameObject rewardObject;
    public InteractionTrigger interactionTrigger;

    void Start() {
        interactionTrigger.SetObserver(this);
    }
    
    public void OnPlayerInteract() {
            rewardObject.SetActive(false);
            GameManager.OnRewardObtained(Reward.REFLECTION_PUZZLE);
            TempleManager.instance.OnRewardCollected();
            interactionTrigger.gameObject.SetActive(false);
    }
}
