using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameOrCodeScript : MonoBehaviour
{
    [SerializeField]
    private MenuHistoryManager menuController;

    [SerializeField]
    private GameObject gameSetting;

    public void BackMenu()
    {
        menuController.CloseCurrentMenu();
        //networkManager.SetActive(false);
        gameSetting.SetActive(false);
    }
}
