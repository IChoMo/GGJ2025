using UnityEngine;

public class AnimationsEventsRocky : MonoBehaviour
{
    public GameObject BubblePTX;
    public Transform spawnLocation;

    public GameObject SmashPTX;
    public GameObject SmashPTX2;
    public Transform spawnLocation2;

    public AudioSource audioSource;
    public AudioClip step1;
    public AudioClip step2;

    public void SpawnParticle()
    {
        var ptx = Instantiate(BubblePTX);
        ptx.transform.position = spawnLocation.position;
        ptx.transform.rotation = spawnLocation.rotation;
        Destroy(ptx, 5);
    }

    public void SpawnParticleSMASH()
    {
        var ptx = Instantiate(SmashPTX);
        ptx.transform.position = spawnLocation2.position;
        Destroy(ptx, 10);

        var ptx2 = Instantiate(SmashPTX2);
        ptx2.transform.position = spawnLocation2.position;
        Destroy(ptx2, 10);
    }

    public void step()
    {
        float randStep = Random.Range(0, 10);

        if (randStep < 5)
        {
            audioSource.PlayOneShot(step1);
        }
        else
        {
            audioSource.PlayOneShot(step2);
        }
    }
}
