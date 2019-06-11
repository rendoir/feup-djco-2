using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class State
{
    public virtual void Update() { }
    public abstract State Next();
    public virtual string GetMessage() { return ""; }
    public virtual void OnSceneLoaded(Scene scene) { }
}
