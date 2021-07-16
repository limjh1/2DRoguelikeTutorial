using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    BoardManager boardScript;

    int level = 3;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        boardScript = GetComponent<BoardManager>();

        boardScript.SetupScene(level);
    }

  
    void Update()
    {
        
    }
}
