using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public Transform BubbleParent;
    public GameObject bubblePrefab;
    public GameObject myBubble;
    public GameObject bubbleParticle;

    public float TimeTillNextBubble = 30;
    public float TimeTillNextBubbleStartValue = 30;

    void Update()
    {
        if(myBubble == null)
        {
            bubbleParticle.SetActive(true);

            //count down
            TimeTillNextBubble -= 1 * Time.deltaTime;

            if (TimeTillNextBubble < 0)
            {
                SpawnBubble();
                TimeTillNextBubble = TimeTillNextBubbleStartValue;
            }
        }
    }

    void SpawnBubble()
    {
        var newBubble = Instantiate(bubblePrefab);
        newBubble.transform.parent = BubbleParent;
        newBubble.transform.position = new Vector3(transform.position.x + 0.001007067f, transform.position.y + 0.01870015f, transform.position.z - 0.01103891f);
        myBubble = newBubble;
        bubbleParticle.SetActive(false);
    }
}
