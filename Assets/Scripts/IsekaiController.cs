using UnityEngine;

public class IsekaiController : MonoBehaviour
{
    private Animator thisAnimator;
    void Start()
    {
        thisAnimator = GetComponent<Animator>();
    }
    public void StartGame() {

    }
    public void ButtonPressed() {
        thisAnimator.SetTrigger("TriggerIsekai");
    }
}
