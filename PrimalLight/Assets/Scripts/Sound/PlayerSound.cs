using UnityEngine;
using UnityEngine.Audio;

public class PlayerSound : MonoBehaviour {
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip[] footstepClips;
    public GameObject leftFoot;
	public GameObject rightFoot;

	private float currentFrameFootstepLeft;
	private float currentFrameFootstepRight;
	private float lastFrameFootstepLeft = 0;
	private float lastFrameFootstepRight = 0;


	void Update () 
    {
		currentFrameFootstepLeft = animator.GetFloat("FootstepLeft");
		if (currentFrameFootstepLeft > 0 && lastFrameFootstepLeft < 0) {
			Step();
        }
		lastFrameFootstepLeft = animator.GetFloat("FootstepLeft");


		currentFrameFootstepRight = animator.GetFloat("FootstepRight");
        if (currentFrameFootstepRight < 0 && lastFrameFootstepRight > 0) {
            Step();
        }																					
		lastFrameFootstepRight = animator.GetFloat("FootstepRight");
	}

    public void Step()
    {
        AudioClip clip = GetRandomClip(footstepClips);
        audioSource.PlayOneShot(clip);
    }

    AudioClip GetRandomClip(AudioClip[] clips) {
        return clips[Random.Range(0, clips.Length)];
    }
}