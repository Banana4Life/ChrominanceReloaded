using System;
using UnityEngine;
using UnityEngine.UI;

namespace Buttons
{
    [RequireComponent(typeof(Image))]
    public class TurretSelector : MonoBehaviour
    {
        public int variant;
        public TurretPlacer placer;

        private Image image;
        private Color activeColor = new Color(0f, 0.69f, 0f);

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            var isActive = variant == placer.variant;
            image.color = isActive ? activeColor : Color.white;
        }
    
        public void HandleClick()
        {
            placer.variant = variant;
        }
    }
}
