using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneOneScript : MonoBehaviour
{

    public ParticleSystem particles;

    public Animator bearAnimator;
    public Animator fadeScreen;
    public bool bearWalk = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bearWalk == true)
        {
            bearAnimator.gameObject.transform.position += new Vector3(0, 0, (Time.deltaTime*4));


            if (bearAnimator.gameObject.transform.localPosition.z >= 20)
            {
                fadeScreen.Play("ScreenFade");
            }
        }
    }


    public void PortalActivate()
    {
        particles.Play();
        bearAnimator.Play("walk");
        bearWalk = true;
    }
}
