using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MovingObject
{
    public bool isKey = false;
    public Color gemEmissionColor;
	[HideInInspector] public int index;
	[HideInInspector] public SlidingPillars puzzle;
	[HideInInspector] public Vector2 pos;

    private GameObject gem;
    private Renderer gemRend;
    private Shader initGemShader;
    private Color initGemEmissionColor;
    private Shader glowGemShader;

    void Awake(){
        if(isKey){
            gem = transform.GetChild(1).gameObject;
            gemRend = gem.GetComponent<Renderer>();
            initGemShader = gemRend.material.shader;
            initGemEmissionColor = gemRend.material.GetColor("_EmissionColor");
            glowGemShader = Shader.Find("MK/Glow/Selective/Standard");
        }
    }
	
    public void Init(SlidingPillars puzzle, int index, Vector2 pos){
		this.puzzle = puzzle;
		this.index = index;
		this.pos = pos;
	}

    public void Move(string dir){
    	Vector3 endPos;
        bool target;

    	if(puzzle.Move(index,dir, out endPos, out target)){
    		SetPadsActiveState(false);
    		StartCoroutine(SmoothMovement( (bool done) => { 
                SetPadsActiveState(true);
                if(isKey){
                    if(target){
                        gemRend.material.shader = glowGemShader;
                        gemRend.material.SetColor("_EmissionColor", gemEmissionColor);
                    }
                }
            }, endPos, 5f));
            if(isKey){
                if(!target){
                    gemRend.material.shader = initGemShader;
                    gemRend.material.SetColor("_EmissionColor", initGemEmissionColor);
                }
            }
    	}
    }

    private void SetPadsActiveState(bool state){
    	foreach(Transform pad in transform){
            if(pad.gameObject.tag == "pillar")
    		  pad.gameObject.SetActive(state);
    	}
    }
}