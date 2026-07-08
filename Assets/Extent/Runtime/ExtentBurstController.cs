using UnityEngine;

namespace ExtentModule
{
    public class ExtentBurstController : MonoBehaviour
    {
        [SerializeField] private float burstCooldown = 2.5f;
        [SerializeField] private float burstForce = 8f;

        private float cooldownTimer;
        private bool burstReady = true;

        private void Update()
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= burstCooldown)
            {
                burstReady = true;
                cooldownTimer = 0f;
            }

            if (Input.GetKeyDown(KeyCode.Space) && burstReady)
            {
                TriggerBurst();
            }
        }

        private void TriggerBurst()
        {
            burstReady = false;
            var player = FindObjectOfType<Rigidbody2D>();
            if (player != null)
            {
                player.AddForce(Vector2.up * burstForce, ForceMode2D.Impulse);
            }

            var message = new GameObject("BurstEffect");
            var renderer = message.AddComponent<SpriteRenderer>();
            renderer.sprite = CreateBurstSprite();
            renderer.color = Color.magenta;
            message.transform.position = Vector3.zero;
            Destroy(message, 0.25f);
        }

        private Sprite CreateBurstSprite()
        {
            var texture = new Texture2D(64, 64);
            for (var x = 0; x < 64; x++)
            {
                for (var y = 0; y < 64; y++)
                {
                    var dx = x - 32f;
                    var dy = y - 32f;
                    var radius = Mathf.Sqrt(dx * dx + dy * dy);
                    var alpha = Mathf.Clamp01(1f - radius / 32f);
                    texture.SetPixel(x, y, new Color(1f, 0f, 1f, alpha));
                }
            }
            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f), 64f);
        }
    }
}
