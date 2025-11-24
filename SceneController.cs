using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    //Script usado en el GameObject de SceneController, el cual está siempre presente en todas las escenas y nunca se destruye.
    // Me permite poder cargar cada escena con animación de fade in / fade out.
    // Y además esperar a que estén lo máximo posible cargadas antes de iniciarlas.

    [SerializeField] private Animator transitionAnimation;

    public void startScene(string scene) {
        StartCoroutine(LoadLevelScene(scene));
    }

    private IEnumerator LoadLevelScene(string scene) {
        transitionAnimation.SetTrigger("EndsScene");
        yield return new WaitForSeconds(0.5f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
        Debug.Log("escena cargada, entra animacion");
        transitionAnimation.SetTrigger("StartScene");
    }

    public void inAnimation() {
        transitionAnimation.SetTrigger("StartScene");
    }

    public void outAnimation() {
        transitionAnimation.SetTrigger("EndsScene");
    }
}
