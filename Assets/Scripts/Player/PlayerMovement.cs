using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody rb;
    public Animator anim;
    public float movementSpeed;
    public float direction;

    public bool aiming;

    public GameObject spawnPoint;
    public LineRenderer lr;
    public float linerendererDistance;

    public float rotateSpeed;
    public float speedMultiplier;
    public bool sprinting;


    float xRotation;
    float yRotation;
    float currentXRotation;
    float currentYRotation;
    float xRotationV;
    float yRotationV;

    float ShootTimer = 0.0f;
    public float ShootCD;
    private SoundManager sm;
    bool justPressed;

    public float turnAroundCD;
    float turnCD = 0;

    float maxX;
    float maxY;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        lr = GetComponent<LineRenderer>();
        sm = GetComponent<SoundManager>();
    }

    // Use this for initialization
    void Start()
    {
        lr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Sprinting", sprinting);
        ShootTimer -= Time.deltaTime;
        turnCD -= Time.deltaTime;
        Cursor.lockState = CursorLockMode.Locked;

        if (!aiming)
        {
            if (Input.GetKey(KeyCode.W))
            {
                direction = 1;
                anim.SetBool("IsMoving", true);
                anim.SetBool("Forwards", true);
            }

            if (!sprinting)
            {
                if (Input.GetKey(KeyCode.S))
                {
                    direction = -1;
                    anim.SetBool("IsMoving", true);
                    anim.SetBool("Forwards", false);
                }

            }


        }


        if (!Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speedMultiplier = 2.0f;
                sprinting = true;
            }
            else
            {
                speedMultiplier = 1.0f;
                sprinting = false;
            }

        }



        if (Input.GetKey(KeyCode.A))
        {
            if (aiming)
            {
                transform.Rotate(new Vector3(0, -rotateSpeed * 2, 0));
            }
            else
                transform.Rotate(new Vector3(0, -rotateSpeed, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (aiming)
            {
                transform.Rotate(new Vector3(0, rotateSpeed * 2, 0));
            }
            else
                transform.Rotate(new Vector3(0, rotateSpeed, 0));
        }

        if (Input.GetMouseButton(1))
        {
            if (!justPressed)
            {
                yRotation = gameObject.transform.rotation.eulerAngles.y;
                xRotation = gameObject.transform.rotation.eulerAngles.x;
                // maxX = gameObject.transform.rotation.eulerAngles.y + 90;
                // maxY = gameObject.transform.rotation.eulerAngles.x + 90;
                justPressed = true;
            }
            aiming = true;
            direction = 0;
            lr.enabled = true;
            anim.SetBool("Aiming", true);
        }
        else
        {
            justPressed = false;
            aiming = false;
            lr.enabled = false;
            anim.SetBool("Aiming", false);
        }

        if (aiming == true)
        {
            lr.SetPosition(0, spawnPoint.transform.position);
            lr.SetPosition(1, spawnPoint.transform.position + spawnPoint.transform.forward * linerendererDistance);
            //aiming

            yRotation += Input.GetAxis("Mouse X") * 3;
            xRotation -= Input.GetAxis("Mouse Y") * 3;

            //xRotation = Mathf.Clamp(xRotation, -maxX, maxX);
            //yRotation = Mathf.Clamp(yRotation, -maxY, maxY);

            currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationV, 0.02f);
            currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationV, 0.02f);

            spawnPoint.transform.rotation = Quaternion.Euler(currentXRotation, currentYRotation, 0);

            //not aiming anymore
            RaycastHit linerenderHit;
            if (Physics.Raycast(spawnPoint.transform.position, spawnPoint.transform.forward * linerendererDistance, out linerenderHit))
            {
                if (!linerenderHit.collider.isTrigger)
                {
                    //Debug.Log(linerenderHit.collider.gameObject.name);
                    lr.SetPosition(1, lr.GetPosition(0) + spawnPoint.transform.forward * linerenderHit.distance);
                }
            }


            if (Input.GetMouseButton(0))
            {
                if (ShootTimer <= 0)
                {
                    sm.PlaySound("gunshot", false, 0.05f, 128);

                    float maxDistance = linerendererDistance;
                    RaycastHit rayHit;
                    if (Physics.Raycast(spawnPoint.transform.position, spawnPoint.transform.forward * linerendererDistance, out rayHit))
                    {
                        Collider col = rayHit.collider;
                        if (col.isTrigger == false)
                        {
                            if (col.gameObject.tag == "Enemy")
                            {
                                col.gameObject.GetComponent<ZombieScript>().Die();
                            }

                        }
                    }

                    ShootTimer = ShootCD;
                }
            }
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S))
        {
            if (turnCD <= 0)
            {
                transform.Rotate(new Vector3(0, 180, 0));
                turnCD = turnAroundCD;
            }
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            direction = 0;
            anim.SetBool("IsMoving", false);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + (direction * transform.forward) * (movementSpeed * speedMultiplier) * Time.deltaTime);
    }
}
