using UnityEngine;
using UnityEngine.Audio;

public class PlayerSound : MonoBehaviour {
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip jumpClip;
    public AudioClip[] footstepClips;

	private float lastFrameFootstepLeft = 0;
	private float lastFrameFootstepRight = 0;
    private bool lastFrameJumping = false;


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

        //Jump
        bool currentFrameJumping = animator.GetBool("isJumping");
        if(!currentFrameJumping && lastFrameJumping)
            JumpSound();
        lastFrameJumping = currentFrameJumping;
	}

    public void StepSound()
    {
        AudioClip clip = GetRandomClip(footstepClips);
        audioSource.PlayOneShot(clip);
    }

    public void JumpSound()
    {
        audioSource.PlayOneShot(jumpClip);
    }

    AudioClip GetRandomClip(AudioClip[] clips) {
        return clips[Random.Range(0, clips.Length)];
    }
}