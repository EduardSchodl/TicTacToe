using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ApplicationExit
{
    public static void Quit()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
