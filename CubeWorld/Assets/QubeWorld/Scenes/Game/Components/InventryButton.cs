using System;
using UnityEngine;

public class InventryButton : MonoBehaviour
{
    [SerializeField]
    private GameManagerUnity gameManagerUnity;
    [SerializeField]
    private Game.Activator activator;

    public bool IsOpenInventry
    {
        get
        {
            var t = isOpenInventry;
            isOpenInventry = false;
            return t;
        }
        private set
        {
            isOpenInventry = value;
        }
    }

    private bool isOpenInventry;

    #region Unity Lifecycles

    void OnEnable()
    {
        isOpenInventry = false;
    }

    void Update()
    {
        updateInput();
    }

    #endregion

    #region Unity User Events

    public void OnClicked()
    {
        openInventry();
    }

    #endregion

    #region Private methods

    private void updateInput()
    {
        if (!Input.GetKeyDown(KeyCode.I))
        {
            return;
        }
        openInventry();
    }

    private void openInventry()
    {
        IsOpenInventry = true;
    }

    #endregion
}
