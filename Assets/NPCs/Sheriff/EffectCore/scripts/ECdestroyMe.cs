using UnityEngine;
using System.Collections;
using System.Linq;

public class ECdestroyMe : MonoBehaviour{

    float timer;
	public float deathtimer;
	ParticleSystem[] partSys; 

	// Use this for initialization
	void Start () 
	{
		partSys = GetComponentsInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;

        if (timer >= deathtimer)
        {
			foreach(ParticleSystem p in partSys)
			{
                var main = p.main;
                main.loop = false;
            }

        }
	
	}
}
