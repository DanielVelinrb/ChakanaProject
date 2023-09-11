using UnityEngine;

namespace Assets.FantasyInventory.Scripts.Interface.Elements
{
    public class Promo : MonoBehaviour
    {
        public void OpenURL(string url)
        {
            Application.OpenURL(url);
        }
    }
}