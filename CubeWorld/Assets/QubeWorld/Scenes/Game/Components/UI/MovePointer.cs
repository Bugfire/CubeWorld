using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameScene
{
    public class MovePointer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private GameManagerUnity gameManagerUnity;
        [SerializeField]
        private RectTransform moveIndicator;

        public Vector2 Move { get; private set; }

        private const float RADIUS = 150f;
        private const float SQR_RADIUS = RADIUS * RADIUS;
        private const float LERP_DELTA_TIME = .1f;
        private bool isBeginDrag;
        private Vector2 beginDragPosition;
        private Vector2 dragMove;
        private Vector2 keyMove;
        private Vector2 lastMove;
        private float lastMoveDeltaTime;
        private Vector2 displayMove;

        #region Unity lifecycles

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
            moveIndicator.anchoredPosition = 100 * displayMove;
        }

        #endregion

        #region Unity input handlers

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Debug.LogFormat("OnBeginDrag[{0}]: {1} {2} {3}", eventData.pointerId, eventData.position, eventData.dragging, eventData.delta);
            beginDragPosition = eventData.position;
            isBeginDrag = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Debug.LogFormat("OnDrag[{0}]: {1}", eventData.pointerId, eventData.position);
            if (!isBeginDrag)
            {
                return;
            }

            var transformVector = eventData.position - beginDragPosition;
            dragMove = transformVector / RADIUS;

            // 上限より遠くまで離れたら、原点も釣られて移動する
            if (transformVector.sqrMagnitude > SQR_RADIUS)
            {
                beginDragPosition = eventData.position - (dragMove.normalized * RADIUS);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // Debug.LogFormat("OnEndDrag[{0}]: {1}", eventData.pointerId, eventData.position);
            isBeginDrag = false;
            dragMove = Vector2.zero;
        }

        #endregion

        #region Private methods

        private void resetInput()
        {
            Move = keyMove = dragMove = Vector2.zero;
            lastMove = displayMove = Vector2.zero;
            lastMoveDeltaTime = 0f;
        }

        private void updateInput()
        {
            keyMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Move = keyMove + dragMove;
            if (Move.sqrMagnitude > 1.0f)
            {
                Move = Move.normalized;
            }

            if (lastMove == Move)
            {
                displayMove = lastMove = Move;
                lastMoveDeltaTime = 0f;
            }
            else
            {
                lastMoveDeltaTime += Time.deltaTime;
                var t = lastMoveDeltaTime / LERP_DELTA_TIME;
                if (t >= 1f)
                {
                    displayMove = lastMove = Move;
                    lastMoveDeltaTime = 0f;
                }
                else
                {
                    displayMove = Vector2.Lerp(lastMove, Move, t);
                }
            }
        }

        #endregion
    }
}
