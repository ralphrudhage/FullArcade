using UnityEngine;

public class Grizzly : MonoBehaviour
{
    private Animator animator;
    private bool facingRight = false;
    private bool isWalking = false;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }

    public void Walk()
    {
        if (!isWalking)
        {
            animator.SetTrigger("walk");
            isWalking = true;
        }
    }

    public void SideIdle()
    {
        if (isWalking)
        {
            animator.SetTrigger("idleside");
            isWalking = false;
        }
    }

    public void FaceLeft()
    {
        if (facingRight)
        {
            FlipDirection();
            facingRight = false;
        }
    }

    public void FaceFront()
    {
        isWalking = false;
        animator.SetTrigger("idlefront");
    }

    public void FaceRight()
    {
        if (!facingRight)
        {
            FlipDirection();
            facingRight = true;
        }
    }

    private void FlipDirection()
    {
        var flippedScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        transform.localScale = flippedScale;
    }
}