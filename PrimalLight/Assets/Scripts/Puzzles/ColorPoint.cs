using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPoint : MonoBehaviour, InteractionObserver
{
    public InteractionTrigger interactionTrigger;

    public bool isPlayerInteracting;
    private bool playerCancelled;

    private GameObject laserClone;

    private LineRenderer laserLR;
    public Color altColor = Color.black;
    public Renderer rend;

    //I do not know why you need this?
    void Example()
    {
        altColor.g = 0f;
        altColor.r = 0f;
        altColor.b = 0f;
        altColor.a = 0f;
    }


    void Start()
    {
        interactionTrigger.SetObserver(this);
        isPlayerInteracting = false;
        playerCancelled = false;

        //Call Example to set all color values to zero.
        Example();
        //Get the renderer of the object so we can access the color
        rend = GetComponent<Renderer>();
        //Set the initial color (0f,0f,0f,0f)
        rend.material.color = altColor;
    }

    void FixedUpdate()
    {
        if (playerCancelled)
        {
            // Debug.Log(laserClone.gameObject);
            // //When player cancels the puzze, reset the rotation 
            // Destroy(laserClone.gameObject);
            playerCancelled = false;
            return;
        }

        if (!isPlayerInteracting)
            return;

        if (GameInput.colorInput.G)
        {
            //Alter the color          
            altColor.g += 0.1f;
            //Assign the changed color to the material.
            rend.material.color = altColor;
        }
        if (GameInput.colorInput.R)
        {
            //Alter the color           
            altColor.r += 0.1f;
            //Assign the changed color to the material. 
            rend.material.color = altColor;
        }
        if (GameInput.colorInput.B)
        {
            //Alter the color            
            altColor.b += 0.1f;
            //Assign the changed color to the material. 
            rend.material.color = altColor;
        }
    }
    

    public void OnPlayerInteract()
    {
        playerCancelled = isPlayerInteracting;
        isPlayerInteracting = !isPlayerInteracting;
        GameManager.CaptureInput(isPlayerInteracting);
        if (playerCancelled)
        {
            return;
        }

    }
}
