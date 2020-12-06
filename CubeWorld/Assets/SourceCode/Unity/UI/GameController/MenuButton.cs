using System;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField]
    private GameManagerUnity gameManagerUnity;
    [SerializeField]
    private Menu.Activator activator;

    #region Unity Lifecycles

    void Update()
    {
        updateInput();
    }

    #endregion

    #region Unity User Events

    public void OnClicked()
    {
        openPauseMenu();
    }

    #endregion

    #region Private methods

    private void openPauseMenu()
    {
        gameManagerUnity.Pause();
    }

    private void updateInput()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
        {
            return;
        }
        if (gameManagerUnity.GetState() == GameState.GAME)
        {
            openPauseMenu();
        }
        else
        {
            activator.SetLastState();
        }
    }

    #endregion
}
