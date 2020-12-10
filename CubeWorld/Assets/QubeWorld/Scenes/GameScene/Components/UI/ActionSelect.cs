using System;
using UnityEngine;

namespace GameScene
{
    public class ActionSelect : MonoBehaviour
    {
        [SerializeField]
        private Animator buildTouchAnimator;
        [SerializeField]
        private Animator attackTouchAnimator;

        public bool IsAttack { get; private set; }

        #region Unity lifecycles

        void OnEnable()
        {
            IsAttack = true;
            updateAnimator();
        }

        #endregion

        #region Unity user events

        public void OnClickAttack()
        {
            IsAttack = true;
            updateAnimator();
        }

        public void OnClickBuild()
        {
            IsAttack = false;
            updateAnimator();
        }

        #endregion

        #region Private methods

        private void updateAnimator()
        {
            buildTouchAnimator.SetBool("Pressing", !IsAttack);
            attackTouchAnimator.SetBool("Pressing", IsAttack);
        }

        #endregion
    }
}
