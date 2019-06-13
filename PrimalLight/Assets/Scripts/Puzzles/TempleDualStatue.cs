using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleDualStatue : MonoBehaviour
{
   	
   	private TempleDualStatueDoor leftDoor;
   	private TempleDualStatueDoor rightDoor;

    void Start()
    {
    	leftDoor = transform.GetChild(0).GetComponent<TempleDualStatueDoor>();
    	rightDoor = transform.GetChild(1).GetComponent<TempleDualStatueDoor>();
    }

    public void UnlockedDoor(){
  		if(!leftDoor.locked && !rightDoor.locked){
        GameSound.Play("PuzzleComplete");
  			leftDoor.Open();
  			rightDoor.Open();
  		}
    }
}
