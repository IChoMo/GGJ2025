using UnityEngine;

public class Highight : MonoBehaviour
{
    public float pulseScale = 0.5f;
    public float pulseSpeed = 1f;
    public float OGscale = 1;

    void Update()
    {
        //pulsating size
        float Pulse = OGscale + Mathf.Sin(Time.time * pulseSpeed) * pulseScale;
        transform.localScale = new Vector3(Pulse, Pulse, Pulse);
    }
}
