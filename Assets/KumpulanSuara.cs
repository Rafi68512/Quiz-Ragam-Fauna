using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KumpulanSuara : MonoBehaviour
{

    public static KumpulanSuara instance;

    public AudioClip[] Clip;

    public AudioSource source_sfx;

    public AudioSource source_bgm;
    // Start is called before the first frame update
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void Panggil_sfx(int id)
    {
        source_sfx.PlayOneShot(Clip[id]);
    }
}
