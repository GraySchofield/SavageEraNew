using UnityEngine;
using System.Collections;

public class GeneralListButtonAudio : MonoBehaviour {



	public void PlayAudio(string AudioSourceName)
    {
        GameObject.Find(AudioSourceName).GetComponent<AudioSource>().Play();
    }
}
