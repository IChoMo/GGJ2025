using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float CurrentPlayerSpeed;
    private float AppliedPlayerSpeed;
    public float PlayerRunSpeed = 4.2f;
    public float PlayerWalkSpeed = 1.5f;
    public float CamMoveSpeed = 500;
    public float jumpForce = 3;
    public Rigidbody rb;
    public GameObject PlayerMesh;
    public GameObject myCamera;

    public GameObject AirSlider;
    public GameObject EnergySlider;
    public float AirIntank = 100;
    public float AirLoseRate = 0.5f;

    public float Energy = 100;
    public float EnergyLoseRate = 1f;

    public Animator anim;

    public bool Pause = false;
    private bool StartSequence = true; //TODO MUST SET THIS BACK TO TRUE BEFORE BUILD

    public Transform BubbleHolder;
    public Transform closetsBubble;
    public float BubbleReach = 5;

    public Transform Speeder;
    public Transform DigText;
    public Transform FuleText;
    public Transform DriveText;
    public Transform SpeedSpwnLoc;

    public GameObject PauseMenu;

    public GameObject TombStone;
    public GameObject TombCanvas;
    public TextMeshProUGUI TombText;
    public float energyDeposits = 0;

    public GameObject Rocky;
    public GameObject BubbleExplodePtx;
    public GameObject RockExplodePtx;

    public GameObject AttackPannel;
    public GameObject Projectile;

    public AudioSource audioSource;
    public AudioClip bubbleCollectSFX;
    public AudioClip uncoveringCarSFX;

    void Update()
    {
        if (StartSequence) { return; }

        //Pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Pause)
            {
                Pause = false;
            }
            else
            {
                Pause = true;
            }
        }

        if (Pause)
        {
            //enable mouse
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            PauseMenu.SetActive(true);

            return;
        }
        else
        {
            //disable mouse
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            PauseMenu.SetActive(false);
        }

        //if not driving then move player
        if (!Speeder.GetComponent<Speeder>().Driving)
        {
            //WASD
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");

            if (y != 0 || x != 0)
            {
                rb.transform.position += AppliedPlayerSpeed * Time.deltaTime * y * rb.transform.forward;
                rb.transform.position += AppliedPlayerSpeed * Time.deltaTime * x * rb.transform.right;

                anim.SetBool("Walking", true);
            }
            else
            {
                anim.SetBool("Walking", false);
            }

            //Running
            if (Input.GetKey(KeyCode.LeftShift) && anim.GetBool("OnGround") && Energy > 0)
            {
                Energy -= Time.deltaTime * EnergyLoseRate;
                CurrentPlayerSpeed = PlayerRunSpeed;
                anim.SetBool("Running", true);
            }
            else
            {
                CurrentPlayerSpeed = PlayerWalkSpeed;
                anim.SetBool("Running", false);
            }

            //Mouse Camera
            var Camx = Input.GetAxis("Mouse X") * Time.deltaTime * CamMoveSpeed;
            transform.Rotate(0, Camx, 0, Space.World);

            //var Camy = Input.GetAxis("Mouse Y") * Time.deltaTime * CamMoveSpeed;
            //myCamera.transform.rotation = new Quaternion(Camy, 0, 0, 0);

            //Make Player Mesh Face Direction of Movement
            if (x != 0 || y != 0)
            {
                Vector3 dir = transform.position;
                dir.y = 0; dir.x = x; dir.z = y;
                Quaternion myRot = Quaternion.Slerp(PlayerMesh.transform.rotation, Quaternion.LookRotation(dir) * transform.rotation, 0.1f);
                PlayerMesh.transform.rotation = myRot;
            }
            else
            {
                //counteract player rotation to cam rotation so that the camera can rotate around the player when not moving
                PlayerMesh.transform.Rotate(0, -Camx, 0, Space.World);
            }
        }

        //Air Tank
        if (AirIntank > 0)
        {
            AirIntank -= Time.deltaTime * AirLoseRate;
            AirSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(50, AirIntank);
        }
        else
        {
            //die?
        }

        //Energy bar
        EnergySlider.GetComponent<RectTransform>().sizeDelta = new Vector2(50, Energy);

        //slow speed if Air low
        if (AirIntank < 10)
        {
            //speed minus the value left as a percent of current speed
            AppliedPlayerSpeed = AirIntank * CurrentPlayerSpeed / 10;
        }
        else
        {
            AppliedPlayerSpeed = CurrentPlayerSpeed;
        }

        //ALWAYS find the closets buble
        foreach (Transform buble in BubbleHolder)
        {
            if (closetsBubble == null)
            {
                closetsBubble = BubbleHolder.GetChild(0);
            }

            float distAway = Vector3.Distance(transform.position, buble.position);

            if (distAway < Vector3.Distance(transform.position, closetsBubble.position))
            {
                closetsBubble = buble;
            }
        }

        //highlight closets buble
        if (BubbleReach > Vector3.Distance(transform.position, closetsBubble.position))
        {
            closetsBubble.GetChild(0).gameObject.SetActive(true);
            closetsBubble.GetChild(1).gameObject.SetActive(true);

            //TODO ADD ANIMATIONS, PARTICALS, SFX

            if (Input.GetKey(KeyCode.Q))
            {
                //Air
                AirIntank = 100;
                Destroy(closetsBubble.gameObject);
                closetsBubble = BubbleHolder.GetChild(0);

                audioSource.PlayOneShot(bubbleCollectSFX);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                //Energy
                Energy = 100;
                Destroy(closetsBubble.gameObject);
                closetsBubble = BubbleHolder.GetChild(0);

                audioSource.PlayOneShot(bubbleCollectSFX);
            }
        }
        else
        {
            closetsBubble.GetChild(0).gameObject.SetActive(false);
            closetsBubble.GetChild(1).gameObject.SetActive(false);
        }

        //speeder
        if (BubbleReach > Vector3.Distance(transform.position, Speeder.position) && !Speeder.GetComponent<Speeder>().Driving)
        {
            Speeder.GetChild(0).gameObject.SetActive(true);

            if (Input.GetKey(KeyCode.E) && DigText.gameObject.activeSelf && Energy > 0)
            {
                //DIG!

                audioSource.PlayOneShot(uncoveringCarSFX);
                //TODO ADD ANIMATIONS, PARTICALS, SFX

                Energy = 0;
                Speeder.SetLocalPositionAndRotation(SpeedSpwnLoc.position, Quaternion.identity);
                Speeder.GetChild(0).SetLocalPositionAndRotation(new Vector3(0, 2, 0), Quaternion.identity);
                Speeder.gameObject.GetComponent<Rigidbody>().useGravity = true;
                Speeder.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                Speeder.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

                DigText.gameObject.SetActive(false);
                FuleText.gameObject.SetActive(true);

            }
            else if (Input.GetKey(KeyCode.E) && FuleText.gameObject.activeSelf && Energy > 0)
            {
                //Add fule
                Speeder.GetComponent<Speeder>().AddEnergy(Energy);
                Energy = 0;

                if (Speeder.GetComponent<Speeder>().Energy1 > 0)
                {
                    DriveText.gameObject.SetActive(true);
                    Speeder.GetComponent<Speeder>().anim.SetBool("Driving", false);
                }
            }

            if (Input.GetKey(KeyCode.Q) && DriveText.gameObject.activeSelf)
            {
                //Get in
                transform.parent = Speeder;
                GetComponent<CapsuleCollider>().enabled = false;
                transform.SetPositionAndRotation(Speeder.position, new Quaternion(0,0,0,0));
                Speeder.GetComponent<Speeder>().Driving = true;
                Speeder.GetComponent<Speeder>().anim.SetBool("Driving", true);
                //anim to get in
            }
        }
        else
        {
            Speeder.GetChild(0).gameObject.SetActive(false);
        }

        //TombStone
        if (BubbleReach > Vector3.Distance(transform.position, TombStone.transform.position) && !Speeder.GetComponent<Speeder>().Driving)
        {
            TombCanvas.SetActive(true);

            if (Input.GetKey(KeyCode.E) && Energy > 0)
            {
                Energy = 0;
                energyDeposits += 1;

                if(energyDeposits >= 5)
                {
                    audioSource.PlayOneShot(uncoveringCarSFX);

                    //SPAWN ROCKY
                    TombStone.SetActive(false);
                    Rocky.SetActive(true);
                    Rocky.transform.SetLocalPositionAndRotation(new Vector3(TombStone.transform.position.x, TombStone.transform.position.y + 15, TombStone.transform.position.z), Quaternion.identity);

                    var ptx = Instantiate(BubbleExplodePtx);
                    ptx.transform.position = TombStone.transform.position;
                    Destroy(ptx, 10);

                    var ptx2 = Instantiate(RockExplodePtx);
                    ptx2.transform.position = TombStone.transform.position;
                    Destroy(ptx2, 10);

                    AttackPannel.SetActive(true);
                }
            }

            TombText.text = "E to add \n" + energyDeposits + "/5";
        }
        else
        {
            TombCanvas.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0) && Energy > 5 && Rocky.activeSelf)
        {
            //fire projectile
            var bullet = Instantiate(Projectile);
            bullet.transform.position = transform.position;
            bullet.transform.LookAt(Rocky.transform);
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 500);
            Energy -= 5;

            Destroy(bullet, 20);
        }
    }

    //Jump
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.Space) && !Pause)
        {
            anim.SetBool("OnGround", false);
            rb.linearVelocity = new Vector3(0, jumpForce, 0);
        }
    }

    //Land
    private void OnTriggerEnter(Collider other)
    {
        anim.SetBool("OnGround", true);
    }

    public void EndStart()
    {
        StartSequence = false;
    }

    public void ResumeGame()
    {
        Pause = false;
    }
}