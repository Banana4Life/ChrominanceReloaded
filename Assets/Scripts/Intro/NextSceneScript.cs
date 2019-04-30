using UnityEngine;
using UnityEngine.SceneManagement;

namespace Intro
{
    public class NextSceneScript : MonoBehaviour
    {
        private GameObject loadingText;
        private GameObject clickThis;

        private bool mouseWasDown = false;

        private void Awake()
        {
            loadingText = GameObject.Find("Loading");
            clickThis = GameObject.Find("ClickThis");
            loadingText.SetActive(false);
        }

        void Update()
        {
            if (!mouseWasDown && Input.GetMouseButtonDown(0))
            {
                mouseWasDown = true;
            }
            if (mouseWasDown && Input.GetMouseButtonUp(0))
            {
                clickThis.SetActive(false);
                loadingText.SetActive(true);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
