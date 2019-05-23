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
        lr.SetPosition(0, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    public LineRenderer getLineRenderer(){
        return lr;
    }
}
