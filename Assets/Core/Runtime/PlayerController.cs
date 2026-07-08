using UnityEngine;

namespace CoreModule
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private CoreScoreManager scoreManager;

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            if (scoreManager == null)
            {
                scoreManager = FindObjectOfType<CoreScoreManager>();
            }

            if (scoreManager == null)
            {
                var managerGo = new GameObject("CoreScoreManager");
                scoreManager = managerGo.AddComponent<CoreScoreManager>();
            }
        }

        private void Update()
        {
            var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            rb.linearVelocity = input * moveSpeed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other == null)
            {
                return;
            }

            var collectible = other.GetComponent<CoreCollectible>();
            if (collectible != null)
            {
                if (scoreManager != null)
                {
                    scoreManager.AddScore(1);
                }

                Destroy(other.gameObject);
            }
        }

    }
}
