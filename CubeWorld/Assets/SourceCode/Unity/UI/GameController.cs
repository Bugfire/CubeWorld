using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Menu.Activator activator;
    [SerializeField]
    private MovePointer movePointer;
    [SerializeField]
    private JumpButton jumpButton;
    [SerializeField]
    private ScreenInput screenInput;
    [SerializeField]
    private ActionSelect actionSelect;
    [SerializeField]
    private InventryButton inventryButton;

    public Vector2 Move
    {
        get
        {
            return movePointer.Move;
        }
    }

    public bool Jump
    {
        get
        {
            return jumpButton.Jump;
        }
    }

    public bool Action
    {
        get
        {
            return screenInput.Action;
        }
    }

    public bool ActionIsAttack
    {
        get
        {
            return actionSelect.IsAttack;
        }
    }

    public Vector3 ActionPos
    {
        get
        {
            return screenInput.ActionPos;
        }
    }

    public Vector2 Rotation
    {
        get
        {
            return screenInput.Rotation;
        }
    }

    // TODO: Menu Layer
    public bool IsOpenInventry
    {
        get
        {
            return inventryButton.IsOpenInventry;
        }
    }

    #region Public methods

    public void OnClickBarrier()
    {
        activator.SetLastState();
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    #endregion
}
