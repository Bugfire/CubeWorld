using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private GameManagerUnity gameManagerUnity;
    [SerializeField]
    private Animator animator;

    public bool Jump
    {
        get
        {
            return isKeyPressed | isButtonPressed;
        }
    }

    private bool isButtonPressed;
    private bool isKeyPressed;
    private bool lastJump;

    #region Unity Lifecycles

    void OnEnable()
    {
        resetInput();
    }

    void Update()
    {
        if (gameManagerUnity.GetState() != GameState.GAME)
        {
            resetInput();
        }
        else
        {
            updateInput();
        }
        if (lastJump != Jump)
        {
            animator.SetTrigger(Jump ? "Pressed" : "Normal");
            lastJump = Jump;
        }
    }

    #endregion

    #region Unity Input Handlers

    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.LogFormat("OnPointerDown[{0}]: {1}", eventData.pointerId, eventData.position);
        isButtonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Debug.LogFormat("OnPointerUp[{0}]: {1}", eventData.pointerId, eventData.position);
        isButtonPressed = false;
    }

    #endregion

    #region Private methods

    private void resetInput()
    {
        isButtonPressed = false;
        isKeyPressed = false;
        lastJump = false;
    }

    private void updateInput()
    {
        isKeyPressed = Input.GetKey(KeyCode.Space);
    }

    #endregion
}
