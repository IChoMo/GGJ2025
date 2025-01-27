using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float bobHeight = 0.5f;
    public float bobSpeed = 1f;
    private Vector3 startPos;

    public GameObject Player;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        //Bob Bubble up and Down
        transform.position = new Vector3(startPos.x, startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight, startPos.z);
    }

    //REPLACED WITH NEW Optional system
    /*private void OnTriggerEnter(Collider other)
    {
        Player.GetComponent<PlayerController>().AirIntank = 100;
        Destroy(gameObject);
    }*/
}
