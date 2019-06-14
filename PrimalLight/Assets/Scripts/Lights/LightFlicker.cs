using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
	public Vector2 framesOffset;
	public Vector2 rangeOffset;
	public Vector2 intensityOffset;
	public Color baseColor;
	public Color topColor;

	private Light sourceLight;
	private int framesToUpdate;
	private int framesToWait = 0;
	private float lastColorT = 0;

    // Start is called before the first frame update
    void Start()
    {
        sourceLight = transform.GetChild(0).GetComponent<Light>();
        SetRandomValues();
        SetWaitFrames();
    }

    // Update is called once per frame
    void Update()
    {
        framesToUpdate++;
        if(framesToUpdate >= framesToWait){
        	SetRandomValues();
        	SetWaitFrames();
        }
    }

    void SetRandomValues(){
    	float t = Random.Range(0,1f);
    	float colorT = lastColorT + Random.Range(-0.2f,0.2f);
    	if(colorT < 0)
    		colorT = 0;
    	else if (colorT > 1f)
    		colorT = 1f; 
		lastColorT = colorT;
    	sourceLight.color = Color.Lerp(baseColor, topColor, colorT);
    	sourceLight.range = Mathf.Lerp(rangeOffset.x, rangeOffset.y, t);
    	sourceLight.intensity = Mathf.Lerp(intensityOffset.x, intensityOffset.y, t);
    }

    void SetWaitFrames(){
    	framesToWait = Random.Range((int) framesOffset.x, (int) framesOffset.y);
    	framesToUpdate = 0;
    }
}
