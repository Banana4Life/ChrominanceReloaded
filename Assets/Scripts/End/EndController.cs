using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace End
{
    public class EndController : MonoBehaviour
    {
        public void OnExitClick()
        {
            if (!Application.isEditor)
            {
                Application.Quit();                
            }
        }

        public void OnAgainClick()
        {
            SceneManager.LoadScene(0);
        }
    }
}
