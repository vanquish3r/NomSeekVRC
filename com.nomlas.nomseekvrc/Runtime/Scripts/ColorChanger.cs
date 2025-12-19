using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nomlas.NomSeekVRC
{
    public class ColorChanger : MonoBehaviour
    {
        [SerializeField] private ColorPresetList preset;
        [SerializeField] private Image[] images;

        [SerializeField] private TextMeshProUGUI[] texts;
        [SerializeField] private Image[] icons;

        [SerializeField] private Color imagesColor;
        [SerializeField] private Color textColor;

        public void ApplyColor()
        {
            foreach (var img in images)
            {
                if (img != null) img.color = imagesColor;
            }
            foreach (var text in texts)
            {
                if (text != null) text.color = textColor;
            }
            foreach (var icon in icons)
            {
                if (icon != null) icon.color = textColor;
            }
        }
    }
}