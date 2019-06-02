using UnityEngine;

public class NullState : State {
    public override State Next() {
        return this;
    }
}
