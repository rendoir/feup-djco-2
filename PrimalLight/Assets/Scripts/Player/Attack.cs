using UnityEngine;
using DigitalRuby.LightningBolt;

public class Attack : MonoBehaviour
{
    public LightningBoltScript lightning;
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light glowLight;
    public float effectOffset = 0.5f;
    public LayerMask ignoreMask;
    public float maxDistance = 20f;

    void Start()
    {
        SetEnabled(false);
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
                Vector3 directionToPlayer = hit.point - transform.position;
                
                float pw = directionToPlayer.magnitude;
                if(pw > maxDistance)
                    miss = true;

                lightning.EndPosition = hit.point;
                impactEffect.transform.position = hit.point - directionToPlayer.normalized*effectOffset;
                impactEffect.transform.rotation = Quaternion.LookRotation(-directionToPlayer);
                glowLight.transform.position = hit.point - directionToPlayer.normalized*effectOffset;
                SetEnabled(true, true);
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
                SetEnabled(true);
            }
        } else {
            SetEnabled(false);
        }
    }

    private void SetEnabled(bool enable, bool enableEffect = false) {
        lightning.enabled = enable;
        lineRenderer.enabled = enable && enableEffect;
        glowLight.enabled = enable;
        
        if(enable && enableEffect && (!impactEffect.isPlaying || impactEffect.isStopped))
            impactEffect.Play();
        else if((!enable || !enableEffect) && impactEffect.isPlaying && !impactEffect.isStopped) 
            impactEffect.Stop();
    }
}
