using System;
using UnityEngine;

public class LockController : MonoBehaviour
{
    [Header("Object references")]
    public GameObject CurrentColorIndicator;
    public GameObject IntendedColorIndicator;
    public GameObject StatusIndicator;

    [Header("Lock Properties")]
    public Color Solution;
    public float SolutionAcceptableError;

    private Renderer CurrentColorIndicatorRenderer;
    private Renderer IntendedColorIndicatorRenderer;
    private Color CurrentColor = new Color(0,0,0);
    private bool isSolved = false;

    private readonly Color incorrectStatusIndicatorColor = new Color(1, 0, 0);
    private readonly Color correctStatusIndicatorColor = new Color(0, 1, 0);
    private Renderer StatusIndicatorRenderer;

    void Start()
    {
        this.CurrentColorIndicatorRenderer = this.CurrentColorIndicator.GetComponent<Renderer>();
        this.IntendedColorIndicatorRenderer = this.IntendedColorIndicator.GetComponent<Renderer>();
        this.StatusIndicatorRenderer = this.StatusIndicator.GetComponent<Renderer>();

        this.setIntendedColorIndicatorColor();
        this.setCurrentColorIndicatorColor();
        this.setStatusindicatorColor();

        this.setColor(Solution);
    }

    public Color getCurrentColor()
    {
        return this.CurrentColor;
    }

    public void setColor(Color color)
    {
        this.CurrentColor = color;
        this.setCurrentColorIndicatorColor();
        //TODO: use acceptable color range with error
        if (this.CurrentColor == Solution)
        {
            this.isSolved = true;
        }
        else
        {
            this.isSolved = false;
        }
        this.setStatusindicatorColor();
    }

    public bool isLockSolved()
    {
        return this.isSolved;
    }

    private void setIntendedColorIndicatorColor()
    {
        this.IntendedColorIndicatorRenderer.material.shader = Shader.Find("_Color");
        this.IntendedColorIndicatorRenderer.material.SetColor("_Color", Solution);

        //Find the Specular shader and change its Color to red
        this.IntendedColorIndicatorRenderer.material.shader = Shader.Find("Specular");
        this.IntendedColorIndicatorRenderer.material.SetColor("_SpecColor", Solution);
    }

    private void setCurrentColorIndicatorColor()
    {
        this.CurrentColorIndicatorRenderer.material.shader = Shader.Find("_Color");
        this.CurrentColorIndicatorRenderer.material.SetColor("_Color", CurrentColor);

        //Find the Specular shader and change its Color to red
        this.CurrentColorIndicatorRenderer.material.shader = Shader.Find("Specular");
        this.CurrentColorIndicatorRenderer.material.SetColor("_SpecColor", CurrentColor);
    }

    private void setStatusindicatorColor()
    {
        Color indicatorColor;
        if (isSolved)
        {
            indicatorColor = this.correctStatusIndicatorColor;
        }
        else
        {
            indicatorColor = this.incorrectStatusIndicatorColor;
        }

        this.StatusIndicatorRenderer.material.shader = Shader.Find("_Color");
        this.StatusIndicatorRenderer.material.SetColor("_Color", indicatorColor);

        //Find the Specular shader and change its Color to red
        this.StatusIndicatorRenderer.material.shader = Shader.Find("Specular");
        this.StatusIndicatorRenderer.material.SetColor("_SpecColor", indicatorColor);
    }
}