using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MovingObject
{
	[HideInInspector] public int index;
	[HideInInspector] public SlidingPillars puzzle;
	[HideInInspector] public Vector2 pos;

	public void Init(SlidingPillars puzzle, int index, Vector2 pos){
		this.puzzle = puzzle;
		this.index = index;
		this.pos = pos;
	}

    public void Move(string dir){
    	Vector3 endPos;
    	if(puzzle.Move(index,dir, out endPos)){
    		SetPadsActiveState(false);
    		StartCoroutine(SmoothMovement( (bool done) => { SetPadsActiveState(true); }, endPos, 5f));
    	}
    }

    private void SetPadsActiveState(bool state){
    	foreach(Transform pad in transform){
            if(pad.gameObject.tag == "pillar")
    		  pad.gameObject.SetActive(state);
    	}
    }
}