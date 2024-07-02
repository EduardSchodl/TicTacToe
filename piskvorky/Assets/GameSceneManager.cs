using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{

    public static GameSceneManager INSTANCE
    {
        get;
        private set;
    }

    private void Awake()
    {
        if(INSTANCE != null)
        {
            Debug.LogError($"Tried to create a second {nameof(GameSceneManager)}");
            return;
        }

        DontDestroyOnLoad(gameObject);
        INSTANCE = this;
    }


    void Start()
    {
        RootToGUILoad();
    }

    private void OnDestroy()
    {
        INSTANCE = null;
    }

    public void RootToGUILoad()
    {
        SceneManager.LoadScene(SceneNames.GUI_SCENE, LoadSceneMode.Additive);
    }

    public static bool IsSceneLoaded(string sceneName)
    {
        for(int i = 0; i < SceneManager.sceneCount; ++i)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if(scene.name == sceneName)
            {
                return true;
            }
        }

        return false;
    }

    public void GameSceneToGUI()
    {
        AsyncOperation unloadSceneResult = SceneManager.UnloadSceneAsync(SceneNames.GAME_SCENE);
        
        // NOTE: IF SOMETHING GOES BAD, THIS IS THE CAUSE
        if(unloadSceneResult == null)
        {
            return;
        }

        unloadSceneResult.completed += EndShowcaseToMenuAfterUnload;
    }

    private void EndShowcaseToMenuAfterUnload(AsyncOperation obj)
    {
        SceneManager.LoadScene(SceneNames.GUI_SCENE, LoadSceneMode.Additive);
    }
}
