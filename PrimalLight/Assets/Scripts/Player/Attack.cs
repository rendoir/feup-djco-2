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
    public Vector3 offset;
    public Animator animator;
    public float animationDelay = 2f;
    private float animationDelayCounter = -1f;
    private bool isFiring = false;
    public float rotateSpeed = 20f;

    void Start()
    {
        glowLight.gameObject.SetActive(true);
        SetEnabled(false);
        lightning.StartPosition = offset;
    }

    void FixedUpdate()
    {
        isFiring = Input.GetKey(KeyCode.Mouse0);
        
        Animate();
        PhysicsCheck();
        Rotate();
    }

    private void PhysicsCheck() {
        //If user fires lightning
        if(isFiring) {
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
                glowLight.intensity = Random.Range(0.5f,3f);
                SetEnabled(true, true);
                OnHit(hit);
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
        bool inDelay = animationDelayCounter < animationDelay;

        lightning.enabled = enable && !inDelay;
        lineRenderer.enabled = enable && !inDelay;
        glowLight.enabled = enable && enableEffect && !inDelay;
        
        if(enable && enableEffect && !inDelay && (!impactEffect.isPlaying || impactEffect.isStopped))
            impactEffect.Play();
        else if((!enable || !enableEffect) && impactEffect.isPlaying && !impactEffect.isStopped) 
            impactEffect.Stop();
    }

    private void OnHit(RaycastHit hit) {
        GameObject obj = hit.transform.gameObject;
        Enemy enemy = obj.GetComponent<Enemy>();
        if(enemy)
            enemy.OnHit();
    }

    private void Animate() {
        if(isFiring) {
            animator.SetBool("isAttacking", true);
            if(animationDelayCounter >= 0f) {
                animationDelayCounter += Time.deltaTime;
                animationDelayCounter = Mathf.Min(animationDelayCounter, animationDelay);
            } else animationDelayCounter = 0f;
        } else {
            animationDelayCounter = -1f;
            animator.SetBool("isAttacking", false);
        }
    }

    private void Rotate() {
        /*Vector3 newDirection = Vector3.RotateTowards(transform.forward, 
            lightning.EndPosition,
            rotateSpeed * Time.deltaTime, 0.0f);
        newDirection.y = transform.forward.y;
        transform.rotation = Quaternion.LookRotation(newDirection);

        lightning.StartPosition = Vector3.RotateTowards(lightning.StartPosition, 
            new Vector3(lightning.EndPosition.x, lightning.StartPosition.y, lightning.EndPosition.z),
            rotateSpeed * Time.deltaTime, 0.0f);
        */
    }
}
