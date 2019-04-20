using UnityEngine;
using DigitalRuby.LightningBolt;

public class Attack : MonoBehaviour
{
    public LightningBoltScript lightning;
    public LayerMask ignoreMask;
    public float maxDistance = 20f;

    void Start()
    {
        lightning.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        //If user fires lightning
        if(Input.GetKey(KeyCode.Mouse0)) {
            //Cast ray from camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool miss = false;

            //If it hits an object, stick to it
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ignoreMask)) {
                float pw = (hit.point - transform.position).magnitude;
                if(pw > maxDistance)
                    miss = true;

                lightning.EndPosition = hit.point;
                lightning.gameObject.SetActive(true);
            } else miss = true;
            
            if(miss) {
                //Shoot the lightning at max distance
                
                //Solve pw² = cp² + cw² + 2*cp*cw*cos(c)
                //Where cos(c) = u_v . CP_v / cp
                //And u_v is the normalized direction vector cw

                Vector3 cp_vector = (transform.position - Camera.main.transform.position);
                Vector3 cw_vector = ray.direction;
                Vector3 u_vector = cw_vector.normalized;

                float pw = maxDistance;
                float cp = cp_vector.magnitude;

                float a = 1; 
                float b = -2*cp*Vector3.Dot(u_vector, cp_vector)/cp;
                float c = -Mathf.Pow(pw,2) + Mathf.Pow(cp,2);
                float cw = Utils.QuadraticFormula(a, b, c);

                Vector3 w = Camera.main.transform.position + cw * u_vector;

                lightning.EndPosition = w;
                lightning.gameObject.SetActive(true);
            }
        } else {
            lightning.gameObject.SetActive(false);
        }
    }
}
