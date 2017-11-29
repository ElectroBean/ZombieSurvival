using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [Header("Timer stuff")]
    public float StartingTime;
    float TimerCount;
    public Text TimerText;
    int counter;

    public Text AddTime;


	// Use this for initialization
	void Start () {
        TimerCount = StartingTime;
        AddTime.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        counter = (int)TimerCount;
        TimerText.text = counter.ToString();
        TimerCount -= Time.deltaTime;
        
	}
    

    public IEnumerator AddToTime(float a_TimeAdded)
    {
        AddTime.enabled = true;
        AddTime.text = " + " + a_TimeAdded.ToString();
        StartCoroutine(FadeTextToZeroAlpha(2, AddTime));
        yield return new WaitForSeconds(2);
        AddTime.enabled = false;
        TimerCount += a_TimeAdded;
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
