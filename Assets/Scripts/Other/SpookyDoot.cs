using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shows a spooky doot every 20-30 seconds
/// </summary>
public class SpookyDoot : MonoBehaviour
{
    public GameObject doot;
    private float time;
    private bool setNewPos = false;

    // Start is called before the first frame update
    void Start()
    {
        time = Random.Range(20f, 30f);
        doot.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Show spooky doot?
        if (time <= 0)
        {
            // Doot time
            doot.SetActive(true);
            if(setNewPos == false)
            {
                doot.transform.position = new Vector3(Random.Range(Screen.width * 0.2f, Screen.width * 0.8f), Random.Range(Screen.height * 0.2f, Screen.height * 0.8f));
                setNewPos = true;
            }
            // hide spooky doot and reset
            if(!doot.GetComponent<AudioSource>().isPlaying)
            {
                doot.SetActive(false);
                time = Random.Range(20f, 30f);
                setNewPos = false;
            }
        }
        else
        {
            time -= Time.deltaTime;
        }
    }


}
