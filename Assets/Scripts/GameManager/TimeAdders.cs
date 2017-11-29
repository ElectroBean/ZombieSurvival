using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAdders : MonoBehaviour {

    GameManager gm;
    public float TimeToAdd;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            gm.StartCoroutine(gm.AddToTime(TimeToAdd));
            Destroy(gameObject);
        }
    }
}
