using UnityEngine;
using TMPro;

public class Rocky : MonoBehaviour
{
    public Animator Anim;
    public float health = 100;

    private float timeTillNextAttack = 10;
    private float timeTillNextAttackStartValue = 10;

    public Transform player;

    public TextMeshProUGUI healthText;

    void Update()
    {
        if (health > 0)
        {
            healthText.text = "Rocky: " + health;

            Anim.SetBool("Walking", true);

            //TODO WALKING AI
            transform.position += 1 * Time.deltaTime * transform.forward;

            var lookPos = player.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1);

            timeTillNextAttack -= 1 * Time.deltaTime;

            if(timeTillNextAttack < 0)
            {
                Attack();
                timeTillNextAttack = timeTillNextAttackStartValue;
            }
        }
        else
        {
            //ROCKY Die
            Anim.SetBool("Walking", false);
            player.GetComponent<PlayerController>().AttackPannel.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    void Attack()
    {
        float randNumb = Random.Range(0, 10);

        if(randNumb > 5)
        {
            Anim.SetTrigger("Beem");
            player.GetComponent<PlayerController>().AirIntank -= Random.Range(2, 8);
        }
        else
        {
            Anim.SetTrigger("Smash");
            player.GetComponent<PlayerController>().Energy -= Random.Range(2, 8);
        }
    }
}
