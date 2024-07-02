using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MenuHistoryManager))]
public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject settingMenu;
    [SerializeField]
    private GameObject modeSetting;

    private MenuHistoryManager menuController;

    void Start()
    {
        menuController = GetComponent<MenuHistoryManager>();
    }

    void Update()
    {
        
    }

    public void ExitGame()
    {
        ApplicationExit.Quit();
    }
    
    public void OpenSetting()
    {
        menuController.OpenMenu(settingMenu);
    }

    public void ModeSetting()
    {
        menuController.OpenMenu(modeSetting);
    }
    
}
