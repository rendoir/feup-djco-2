using UnityEngine;

public abstract class State
{
    public virtual void Update() { }
    public abstract State Next();
    public virtual string GetMessage() { return ""; }
}
