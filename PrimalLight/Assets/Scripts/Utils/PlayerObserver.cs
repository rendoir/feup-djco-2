using UnityEngine;
using System.Collections.Generic;

public interface InteractionObserver
{
    void OnPlayerInteract(); 
}

public interface DeathObserver {
    void OnPlayerDeath();
    void OnPlayerAlive();
}
