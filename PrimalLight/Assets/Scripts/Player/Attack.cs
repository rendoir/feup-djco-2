using UnityEngine;
using DigitalRuby.LightningBolt;

public class Attack : MonoBehaviour
{
    public LightningBoltScript lightning;
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

            //If it hits an object, stick to it
            if (Physics.Raycast(ray, out hit, maxDistance)) {
                lightning.EndPosition = hit.point;
                lightning.gameObject.SetActive(true);
            } else {
                //Shoot the lightning at max distance
                //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20f));
                //Vector3 startPosition = lightning.StartObject.transform.position + lightning.StartPosition;
                //Vector3 maxDistancePosition = startPosition + (worldPosition - startPosition).normalized * maxDistance;
                //lightning.EndPosition = maxDistancePosition;
                //lightning.gameObject.SetActive(true);
                lightning.gameObject.SetActive(false);
            }
        } else {
            lightning.gameObject.SetActive(false);
        }
    }
}
