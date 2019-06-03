using UnityEngine;

static class Utils {
    public static bool MaskContainsLayer(LayerMask mask, LayerMask layer) {
        return ( (1 << layer) & mask) != 0;
    } 

    public static float QuadraticFormula(float a,float b,float c) {
        float sqrt = Mathf.Sqrt(Mathf.Pow(b,2) - 4*a*c);
        float plus = (-b + (sqrt)) / (2*a);
        float minus = (-b - (sqrt)) / (2*a);
        
        return plus >= 0 ? plus : minus >= 0 ? minus : 0;
    }

    public static bool ColorEquals(Color color1, Color color2) {
        return (
            Mathf.Approximately(color1.r, color2.r) &&
            Mathf.Approximately(color1.g, color2.g) &&
            Mathf.Approximately(color1.b, color2.b) &&
            Mathf.Approximately(color1.a, color2.a)
        );
    }

}