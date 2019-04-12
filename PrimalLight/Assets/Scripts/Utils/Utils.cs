using UnityEngine;

static class Utils {
    public static bool MaskContainsLayer(LayerMask mask, LayerMask layer) {
        return ( (1 << layer) & mask) != 0;
    } 
}