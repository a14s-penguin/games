using UnityEngine;

namespace CoreModule
{
    public class CoreScoreManager : MonoBehaviour
    {
        private TextMesh scoreText;
        private int score;

        public int Score => score;

        private void Awake()
        {
            InitializeText();
        }

        private void Start()
        {
            UpdateDisplay();
        }

        public void AddScore(int amount)
        {
            score += amount;
            UpdateDisplay();
        }

        private void InitializeText()
        {
            if (scoreText != null)
            {
                return;
            }

            var textGo = new GameObject("ScoreText");
            textGo.transform.SetParent(transform, false);
            textGo.transform.position = new Vector3(-3.8f, 4.2f, 0f);
            scoreText = textGo.AddComponent<TextMesh>();
            scoreText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            scoreText.anchor = TextAnchor.MiddleLeft;
            scoreText.characterSize = 0.12f;
            scoreText.color = Color.white;
        }

        private void UpdateDisplay()
        {
            if (scoreText != null)
            {
                scoreText.text = $"Core score: {score}";
            }
        }
    }
}
