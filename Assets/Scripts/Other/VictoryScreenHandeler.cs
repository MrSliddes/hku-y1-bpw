using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScreenHandeler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReloadScene()
    {
        // Obselete but it still works and what works isnt broke so dont fix it. Its not like im going to upgrade this project anyway
        Application.LoadLevel(Application.loadedLevel);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
