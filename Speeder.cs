using UnityEngine;

public class Speeder : MonoBehaviour
{
    public GameObject EnergySlider1;
    public GameObject EnergySlider2;
    public GameObject EnergySlider3;
    public GameObject EnergySlider4;

    public float Energy1 = 0;
    public float Energy2 = 0;
    public float Energy3 = 0;
    public float Energy4 = 0;

    public bool Driving = false;

    public float speed = 10;

    public GameObject Player;

    public float EnergyLoseRate = 2f;

    public Animator anim;

    private void Update()
    {
        //uncover speeder
        //get in speeder - with ANIMS

        if (Driving)
        {
            Player.transform.SetPositionAndRotation(transform.position, transform.rotation);

            //WASD
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");

            if (Energy1 > 0)
            {
                if (y != 0 || x != 0)
                {
                    transform.position += speed * Time.deltaTime * y * transform.forward;
                    transform.position += speed * Time.deltaTime * x * transform.right;
                    SpendEnergy();
                }
            }

            //Mouse Camera
            var Camx = Input.GetAxis("Mouse X") * Time.deltaTime * Player.GetComponent<PlayerController>().CamMoveSpeed;
            transform.Rotate(0, Camx, 0, Space.World);

            //move controls
            //particals
            //sounds

            //get out
            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("Driving", false);

                Driving = false;
                Player.transform.parent = null;
                Player.transform.rotation = Quaternion.identity;
                Player.GetComponent<CapsuleCollider>().enabled = true;
            }
        }
    }

    public void AddEnergy(float energyAmount)
    {
        if(Energy1 < 100)
        {
            Energy1 += energyAmount;

            if (Energy1 > 100)
            {
                Energy2 += (Energy1 - 100);
                Energy1 = 100;
            }
        }
        else if (Energy2 < 100)
        {
            Energy2 += energyAmount;

            if (Energy2 > 100)
            {
                Energy3 += (Energy2 - 100);
                Energy2 = 100;
            }
        }
        else if (Energy3 < 100)
        {
            Energy3 += energyAmount;

            if (Energy3 > 100)
            {
                Energy4 += (Energy3 - 100);
                Energy3 = 100;
            }
        }
        else if (Energy4 < 100)
        {
            Energy4 += energyAmount;

            if (Energy4 > 100)
            {
                float exsessEnergy = Energy4 - 100;
                Energy3 = 100;

                //player.give back energy
            }
        }

        //update all energy bars
        EnergySlider1.GetComponent<RectTransform>().sizeDelta = new Vector2(210, Energy1*3);
        EnergySlider2.GetComponent<RectTransform>().sizeDelta = new Vector2(210, Energy2*3);
        EnergySlider3.GetComponent<RectTransform>().sizeDelta = new Vector2(210, Energy3*3);
        EnergySlider4.GetComponent<RectTransform>().sizeDelta = new Vector2(210, Energy4*3);
    }

    public void SpendEnergy()
    {
        if (Energy4 > 0)
        {
            Energy4 -= Time.deltaTime * EnergyLoseRate;
        }
        else if (Energy3 > 0)
        {
            Energy3 -= Time.deltaTime * EnergyLoseRate;
        }
        else if (Energy2 > 0)
        {
            Energy2 -= Time.deltaTime * EnergyLoseRate;
        }
        else if (Energy1 > 0)
        {
            Energy1 -= Time.deltaTime * EnergyLoseRate;
        }
        else
        {
            //all four empty, cannot do the thing anymore
        }

        //update all energy bars
        EnergySlider1.GetComponent<RectTransform>().sizeDelta = new Vector2(210, Energy1 * 3);
        EnergySlider2.GetComponent<RectTransform>().sizeDelta = new Vector2(210, Energy2 * 3);
        EnergySlider3.GetComponent<RectTransform>().sizeDelta = new Vector2(210, Energy3 * 3);
        EnergySlider4.GetComponent<RectTransform>().sizeDelta = new Vector2(210, Energy4 * 3);
    }
}