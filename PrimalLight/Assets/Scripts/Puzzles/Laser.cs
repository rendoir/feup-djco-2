using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lr;
    // Start is called before the first frame update
    public void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.startWidth = 0.03f;
        lr.endWidth = 0.03f;
        lr.SetPosition(0, transform.position);
        lr.startColor = Color.red;
        lr.endColor = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    public LineRenderer getLineRenderer(){
        return lr;
    }
}
