using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class isPause : MonoBehaviour
{
    public static bool paused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                ResumeButton();
            }
            else
            {
                PauseButton();
            }

        }
    }

    public void ResumeButton()
    {
        Time.timeScale = 1f;
        SaveCsv.ResumeNote();
        paused = false;
    }

    public void PauseButton()
    {
        Time.timeScale = 0f;
        SaveCsv.PauseNote();
        paused = true;
    }
}


