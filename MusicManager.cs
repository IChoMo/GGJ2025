using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource aSouice;
    public AudioClip song1;
    public AudioClip song2;

    void Update()
    {
        if (!aSouice.isPlaying)
        {
            float randSong = Random.Range(0, 10);

            if(randSong < 5)
            {
                aSouice.PlayOneShot(song1);
            }
            else
            {
                aSouice.PlayOneShot(song2);
            }
        }
    }
}
