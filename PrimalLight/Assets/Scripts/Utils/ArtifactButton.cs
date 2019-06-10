using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArtifactButton : MonoBehaviour {
    private ActivateArtifactState state;

    public float offset;
    private Vector3 initPos;
    private Vector3 activePos;

    public LayerMask playerLayer;
    
    void Start(){
    	initPos = transform.localPosition;
    	activePos = initPos + new Vector3(0, offset, 0);
    }

    void Update() {
        activePos = initPos + new Vector3(0, offset, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if(state == null)
            return;

        if(Utils.MaskContainsLayer(playerLayer, other.gameObject.layer)) {
        	transform.localPosition = activePos;
            state.OnButtonPressed(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(state == null)
            return;
            
        if(Utils.MaskContainsLayer(playerLayer, other.gameObject.layer)) {
            transform.localPosition = initPos;
            state.OnButtonPressed(false);
        }
    }

    public void SetState(ActivateArtifactState currentState) {
        state = currentState;
    }
}
