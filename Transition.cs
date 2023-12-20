using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    [SerializeField]
    private float delayBeforeLoading = 3f;

    [SerializeField]
    private string sceneNameToLoad;
    private float TimeElapsed;

    private void Update()
    {
        TimeElapsed += Time.deltaTime;

        if (TimeElapsed > delayBeforeLoading)
        {
            SceneManager.LoadScene(sceneNameToLoad);

        }

    }
}
