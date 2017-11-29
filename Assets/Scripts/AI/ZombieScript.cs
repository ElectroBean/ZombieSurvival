using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieScript : MonoBehaviour {

    public NavMeshAgent navMesh;
    public Animator anim;
    public Rigidbody rb;
    bool moving;
    bool attacking;
    bool isDead;

    private void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        anim.SetBool("Moving", moving);
        anim.SetBool("Attacking", attacking);
        anim.SetBool("Dead", isDead);
        if(navMesh.velocity.magnitude != 0)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }
	}

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            navMesh.SetDestination(other.transform.position);
        }
    }

    public void Die()
    {
        foreach(Collider col in gameObject.GetComponents<Collider>())
        {
            col.enabled = false;
        }
        navMesh.enabled = false;
        //rb.isKinematic = true;
        
        isDead = true;
    }
}
