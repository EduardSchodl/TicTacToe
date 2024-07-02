using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public static AudioScript INSTANCE {
        private set;
        get;
    }
    
    void Awake()
    {
        // Destroy object if singleton already exists
        if (INSTANCE != null)
        {
            Destroy(gameObject);
            return;
        }

        // Initialize singleton and object
        GetComponent<AudioSource>().loop = true;
        DontDestroyOnLoad(gameObject);

        INSTANCE = this;
    }
}
