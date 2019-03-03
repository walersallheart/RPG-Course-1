using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour {

    [SerializeField] string areaToLoad;
    [SerializeField] string areaTransitionName;

    [SerializeField] AreaEntrance theEntrance;

    public float waitToLoad = 1f;
    public bool shouldLoadAfterFade;

    private void Start()
    {
        theEntrance.transitionName = areaTransitionName;
    }

    private void Update() {
        if (shouldLoadAfterFade){
            waitToLoad -= Time.deltaTime;

            if (waitToLoad <= 0f){
                SceneManager.LoadScene(areaToLoad);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController.instance.areaTransitionName = areaTransitionName;
            shouldLoadAfterFade = true;
            UIFade.instance.FadeToBlack();
        }
    }
}
