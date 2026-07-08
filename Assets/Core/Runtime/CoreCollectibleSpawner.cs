using UnityEngine;

namespace CoreModule
{
    public class CoreCollectibleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject collectiblePrefab;
        [SerializeField] private float spawnInterval = 1.2f;
        [SerializeField] private float spawnRadius = 3.5f;

        private float timer;

        private void Awake()
        {
            EnsureCollectibleTemplate();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                timer = 0f;
                Spawn();
            }
        }

        private void Spawn()
        {
            EnsureCollectibleTemplate();
            if (collectiblePrefab == null)
            {
                return;
            }

            var randomOffset = Random.insideUnitCircle * spawnRadius;
            var spawnPosition = new Vector3(randomOffset.x, randomOffset.y, 0f);
            var collectibleGo = Instantiate(collectiblePrefab, spawnPosition, Quaternion.identity);
            collectibleGo.name = "CoreCollectible";
        }

        private void EnsureCollectibleTemplate()
        {
            if (collectiblePrefab != null)
            {
                return;
            }

            var templateGo = new GameObject("CoreCollectiblePrefab");
            templateGo.SetActive(false);
            templateGo.AddComponent<CircleCollider2D>().isTrigger = true;
            templateGo.AddComponent<SpriteRenderer>();
            templateGo.AddComponent<CoreCollectible>();

            var spriteRenderer = templateGo.GetComponent<SpriteRenderer>();
            var texture = new Texture2D(32, 32);
            for (var x = 0; x < 32; x++)
            {
                for (var y = 0; y < 32; y++)
                {
                    var dx = x - 16f;
                    var dy = y - 16f;
                    var inCircle = dx * dx + dy * dy <= 14f * 14f;
                    texture.SetPixel(x, y, inCircle ? Color.yellow : new Color(0, 0, 0, 0));
                }
            }

            texture.Apply();
            spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32f);
            collectiblePrefab = templateGo;
        }
    }
}
