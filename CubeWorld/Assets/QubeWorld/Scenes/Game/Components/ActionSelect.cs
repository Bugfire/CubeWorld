using System;
using UnityEngine;

public class ActionSelect : MonoBehaviour
{
    [SerializeField]
    private GameManagerUnity gameManagerUnity;
    [SerializeField]
    private Game.Activator activator;
    [SerializeField]
    private Animator buildTouchAnimator;
    [SerializeField]
    private Animator attackTouchAnimator;

    public bool IsAttack { get; private set; }

    #region Unity Lifecycles

    void OnEnable()
    {
        IsAttack = true;
        updateAnimator();
    }

    #endregion

    #region Unity User vents

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
