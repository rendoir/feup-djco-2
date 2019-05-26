using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lr;

    public void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, transform.position);
    }

    public LineRenderer getLineRenderer(){
        return lr;
    }
}
