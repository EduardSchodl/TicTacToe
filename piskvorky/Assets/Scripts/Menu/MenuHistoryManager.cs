using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MenuHistoryManager : MonoBehaviour
{

    private static MenuHistoryManager instance;
    public static MenuHistoryManager INSTANCE => instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    private GameObject firstMenu;

    /// <summary>
    /// The stack that contains the history of the menus
    /// shown. The last element in the stack is the currently
    /// visible menu to the player.
    /// </summary>
    private static List<GameObject> openedMenusStack = new List<GameObject>();

    void Start()
    {
        // Main menu is already visible, but we still need
        // to push it onto the stack.
        OpenMenu(firstMenu);
    }

    public void PrintOpenedMenuStack()
    {
        string printStr = "Opened menu stack: {\n";
        foreach(GameObject openedMenu in openedMenusStack)
        {
            printStr += openedMenu + "\n";
        }

        printStr += "}";
        Debug.Log(printStr);
    }

    /// <summary>
    /// Opens a menu and pushes it onto the <see cref="openedMenusStack"/> stack.
    /// The new menu will be shown to the player.
    /// </summary>
    /// <param name="menu"></param>
    public void OpenMenu(GameObject menu)
    {
        if (openedMenusStack.Count != 0 && openedMenusStack[openedMenusStack.Count - 1] == menu)
        {
            Debug.LogWarning("Tried to open an already opened menu at the end of the stack.");
            return;
        }
        // Hide current menu if one is shown
        if (GetCurrentOpenedMenu())
        {
            HideCurrentMenu();
        }

        // Add menu to stack and show it
        openedMenusStack.Add(menu);
        menu.SetActive(true);
    }

    /// <summary>
    /// Returns the current opened menu, visible to the player.
    /// If no menu is opened, returns null.
    /// </summary>
    public GameObject GetCurrentOpenedMenu()
    {
        int lastIndex = openedMenusStack.Count - 1;

        if(lastIndex == -1)
        {
            return null;
        }

        return openedMenusStack[lastIndex];
    }

    /// <summary>
    /// Removes the last element from the <see cref="openedMenusStack"/>
    /// </summary>
    public void PopLastMenu()
    {
        openedMenusStack.RemoveAt(openedMenusStack.Count - 1);
    }

    /// <summary>
    /// Hides the current shown menu from the player without removing it from
    /// the stack.
    /// </summary>
    public void HideCurrentMenu()
    {
        GameObject currentOpenedMenu = GetCurrentOpenedMenu();
        currentOpenedMenu.SetActive(false);
    }

    /// <summary>
    /// Closes the current opened visible menu and returns to the last
    /// opened menu. If no menu is found, there is no return back to it.
    /// </summary>
    public void CloseCurrentMenu()
    {
        HideCurrentMenu();
        PopLastMenu();

        GameObject previousMenu = GetCurrentOpenedMenu();

        // No menu to return back to
        if(!previousMenu)
        {
            return;
        }

        previousMenu.SetActive(true);
    }

    public List<GameObject> GetMenuList()
    {
        return openedMenusStack;
    }

}
