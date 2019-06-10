using UnityEngine;
using UnityEngine.Audio;

public class FriendSound : MonoBehaviour {
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip deathClip;
    public AudioClip[] footstepClips;

	private float lastFrameFootstepLeft = 0;
	private float lastFrameFootstepRight = 0;


	void Update () 
    {
        //Footstep left feet
		float currentFrameFootstepLeft = animator.GetFloat("FootstepLeft");
		if (currentFrameFootstepLeft > 0 && lastFrameFootstepLeft < 0)
			StepSound();
		lastFrameFootstepLeft = currentFrameFootstepLeft;

        //Footstep right feet
		float currentFrameFootstepRight = animator.GetFloat("FootstepRight");
        if (currentFrameFootstepRight < 0 && lastFrameFootstepRight > 0)
            StepSound();																				
		lastFrameFootstepRight = currentFrameFootstepRight;
	}

    public void StepSound()
    {
        AudioClip clip = GetRandomClip(footstepClips);
        audioSource.PlayOneShot(clip);
    }

    public void DeathSound()
    {
        audioSource.PlayOneShot(deathClip);
    }

    AudioClip GetRandomClip(AudioClip[] clips) {
        return clips[Random.Range(0, clips.Length)];
    }
}