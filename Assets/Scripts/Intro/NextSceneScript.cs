using UnityEngine;
using UnityEngine.SceneManagement;

namespace Intro
{
    public class NextSceneScript : MonoBehaviour
    {
        private GameObject loadingText;
        private GameObject clickThis;

        private void Awake()
        {
            loadingText = GameObject.Find("Loading");
            clickThis = GameObject.Find("ClickThis");
            loadingText.SetActive(false);
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                clickThis.SetActive(false);
                loadingText.SetActive(true);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
