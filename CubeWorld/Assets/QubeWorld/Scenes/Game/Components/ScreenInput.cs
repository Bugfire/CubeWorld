using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private GameManagerUnity gameManagerUnity;

    private const float ROTATE_SCALE = 1 / 30f;
    private const float LONGPRESS_DELTA_TIME_THRESHOLD = 0.4f;
    private const float LONGPRESS_MOVE_THRESHOLD = 10f;
    private const float SQR_LONGPRESS_MOVE_THRESHOLD = LONGPRESS_MOVE_THRESHOLD * LONGPRESS_MOVE_THRESHOLD;
    private float lastPressedDeltaTime;
    private bool isTapped;
    private bool isBeginDrag;
    private Vector2 lastDragPosition;
    private bool isActionMode;
    private bool isRotateMode;
    private Vector2 actionPos;
    private Vector2 rotateVector;

    public bool Action {
        get
        {
            if (isTapped)
            {
                isTapped = false;
                return true;
            }
            if (isActionMode)
            {
                return true;
            }
            return false;
        }
    }

    public Vector2 ActionPos
    {
        get
        {
            return actionPos;
        }
    }

    public Vector2 Rotation
    {
        get
        {
            if (!isRotateMode)
            {
                return Vector2.zero;
            }
            var t = rotateVector;
            rotateVector = Vector2.zero;
            return t;
        }
    }

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
    }

    #endregion

    #region Unity Input Handlers

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isBeginDrag)
        {
            return;
        }
        lastPressedDeltaTime = 0f;
        lastDragPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isBeginDrag && !isActionMode)
        {
            isTapped = true;
            actionPos = eventData.position;
        }
        resetInput();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isBeginDrag = true;
        lastDragPosition = eventData.position;
        if (!isActionMode)
        {
            isRotateMode = true;
            lastPressedDeltaTime = -1f;
        }
        else
        {
            actionPos = lastDragPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isBeginDrag)
        {
            // transformStarted 前は座標がない
            return;
        }
        if (isRotateMode)
        {
            rotateVector = (eventData.position - lastDragPosition) * ROTATE_SCALE;
        }
        else if (isActionMode)
        {
            actionPos = eventData.position;
        }
        lastDragPosition = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        resetInput();
    }

    #endregion

    #region Private methods

    private void resetInput()
    {
        isBeginDrag = isActionMode = isRotateMode = false;
        rotateVector = Vector2.zero;
        lastPressedDeltaTime = -1f;
    }

    private void updateInput()
    {
        if (isBeginDrag == false && lastPressedDeltaTime >= 0f)
        {
            lastPressedDeltaTime += Time.deltaTime;
            if (lastPressedDeltaTime > LONGPRESS_DELTA_TIME_THRESHOLD)
            {
                isActionMode = true;
                lastPressedDeltaTime = -1f;
                actionPos = lastDragPosition;
            }
        }
        if (!isRotateMode)
        {
            rotateVector = Vector2.zero;
        }
    }

    #endregion
}
