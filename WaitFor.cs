using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class WaitFor
{
    public static IEnumerator Frames(int frameCount)
    {
        if (frameCount <= 0)
        {
            throw new ArgumentOutOfRangeException("frameCount", "Cannot wait for less than 1 frame");
        }

        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
    }
}
