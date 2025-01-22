#if UNITY_EDITOR
using UnityEngine;

namespace App.Ads
{
    public class DummyBannerMonoBehaviour : MonoBehaviour
    {
        [SerializeField] private int _bannerHeight = 138;
        [SerializeField] private int _bannerOffset = 0;

        private GUIStyle _backgroundBoxStyle;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnGUI()
        {
            //if (full screen ad showed)
            //{
            //    return;
            //}

            if (_backgroundBoxStyle == null)
            {
                int fontSize = Mathf.Max(12, Mathf.CeilToInt(Mathf.Min(Screen.width, Screen.height) * 0.07f));

                _backgroundBoxStyle = new GUIStyle(GUI.skin.box);
                _backgroundBoxStyle.normal.background = new Texture2D(1, 1);

                _backgroundBoxStyle.normal.background.SetPixel(0, 0, new Color(1, 1, 1, 1));
                _backgroundBoxStyle.normal.background.Apply();

                _backgroundBoxStyle.fontSize = fontSize;
            }

            GUI.Box(new Rect(0, Screen.height - _bannerHeight - _bannerOffset, Screen.width, _bannerHeight), $"Banner", _backgroundBoxStyle);
        }

        private void OnApplicationQuit()
        {
            Destroy(this.gameObject);
        }
    }
}
#endif
