using UnityEngine;
using UnityEngine.SceneManagement;

public class IsekaiController : MonoBehaviour
{
    private Animator thisAnimator;
    void Start()
    {
        thisAnimator = GetComponent<Animator>();
    }
    public void StartGame() {
        SceneManager.LoadScene("CropScene");
        SceneManager.UnloadSceneAsync(0);
    }
    public void ButtonPressed() {
        thisAnimator.SetTrigger("TriggerIsekai");
    }
}
