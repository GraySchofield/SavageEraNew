using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class HomeBGMController : MonoBehaviour {
    public AudioMixerSnapshot stateNormal;
    public AudioMixerSnapshot statePanic;
    private bool was_in_panic;

    void Start()
    {
        was_in_panic = Game.Current.Hero.isInPanic;
        if (was_in_panic)
        {
            GoPanic();
        }
        else
        {
            GoNormal();
        }
    }


    void Update()
    {
        if (Game.Current.Hero.isInPanic)
        {
            if (!was_in_panic)
            {
                GoPanic();
                was_in_panic = Game.Current.Hero.isInPanic;
            }
        }
        else
        {
            if (was_in_panic)
            {
                GoNormal();
                was_in_panic = Game.Current.Hero.isInPanic;
            }
        }



    }



    private void GoPanic()
    {
        statePanic.TransitionTo(0.5f);
    }

    private void GoNormal()
    {
        stateNormal.TransitionTo(0.1f);
    }

}
