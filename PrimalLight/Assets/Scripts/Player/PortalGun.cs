using UnityEngine;

public class PortalGun : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject elbow;
    public GameObject hand;
    public LayerMask ignoreMask;
    public int maxLength;
    public Vector3 offset;

    void Start()
    {
        lineRenderer.enabled = false;
    }

    void Update() {
        if(lineRenderer.enabled) {
            Vector3 initialPosition = elbow.transform.TransformPoint(offset);
            lineRenderer.SetPosition(0, initialPosition);

            Vector3 beamDirection = hand.transform.position - elbow.transform.position;
            Ray ray = new Ray(initialPosition, beamDirection);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxLength, ignoreMask, QueryTriggerInteraction.Ignore)) {
                lineRenderer.SetPosition(1, hit.point);
            } else {
                lineRenderer.SetPosition(1, initialPosition + beamDirection.normalized * maxLength);
            }
        }
    }

    public void EnableBeam(bool enable) {
        lineRenderer.enabled = enable;
    }
}
