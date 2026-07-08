using System;
using UnityEngine;

namespace CoreModule
{
    public static class GameBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            if (UnityEngine.Object.FindObjectOfType<PlayerController>() != null)
            {
                return;
            }

            if (Camera.main == null)
            {
                var cameraGo = new GameObject("Main Camera");
                var camera = cameraGo.AddComponent<Camera>();
                camera.orthographic = true;
                camera.orthographicSize = 5f;
                camera.backgroundColor = Color.black;
                camera.clearFlags = CameraClearFlags.SolidColor;
                cameraGo.tag = "MainCamera";
            }

            CreateBackground();
            CreateBoundaryWalls();
            CreateInstructionText();

            var managerGo = new GameObject("CoreScoreManager");
            managerGo.AddComponent<CoreScoreManager>();

            var spawnerGo = new GameObject("CoreCollectibleSpawner");
            spawnerGo.AddComponent<CoreCollectibleSpawner>();

            TryInitializeExtentFeature();

            var playerGo = new GameObject("Player");
            playerGo.transform.position = Vector3.zero;
            var sprite = playerGo.AddComponent<SpriteRenderer>();
            sprite.sprite = CreateCircleSprite();
            sprite.color = Color.cyan;
            var collider = playerGo.AddComponent<CircleCollider2D>();
            collider.radius = 0.35f;
            var rb = playerGo.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            playerGo.AddComponent<PlayerController>();
        }

        private static void TryInitializeExtentFeature()
        {
            try
            {
                var extentType = Type.GetType("ExtentModule.ExtentBurstController, Extent", throwOnError: false);
                if (extentType == null)
                {
                    return;
                }

                var burstGo = new GameObject("ExtentBurstController");
                burstGo.AddComponent(extentType);
            }
            catch (Exception)
            {
                // Ignore if the Extent module is absent or incompatible.
            }
        }

        private static void CreateBackground()
        {
            var backgroundGo = new GameObject("Background");
            var renderer = backgroundGo.AddComponent<SpriteRenderer>();
            renderer.sprite = CreateBackgroundSprite();
            renderer.sortingOrder = -10;
            backgroundGo.transform.position = Vector3.zero;
        }

        private static void CreateBoundaryWalls()
        {
            CreateWall("TopWall", new Vector2(0f, 5.6f), new Vector2(12f, 0.4f));
            CreateWall("BottomWall", new Vector2(0f, -5.6f), new Vector2(12f, 0.4f));
            CreateWall("LeftWall", new Vector2(-5.9f, 0f), new Vector2(0.4f, 12f));
            CreateWall("RightWall", new Vector2(5.9f, 0f), new Vector2(0.4f, 12f));
        }

        private static void CreateWall(string name, Vector2 position, Vector2 size)
        {
            var wallGo = new GameObject(name);
            wallGo.transform.position = position;
            var renderer = wallGo.AddComponent<SpriteRenderer>();
            renderer.sprite = CreateBoxSprite();
            renderer.color = Color.gray;
            var collider = wallGo.AddComponent<BoxCollider2D>();
            collider.size = size;
        }

        private static void CreateInstructionText()
        {
            var textGo = new GameObject("InstructionText");
            textGo.transform.position = new Vector3(0f, -4.4f, 0f);
            var mesh = textGo.AddComponent<TextMesh>();
            mesh.text = "Move: WASD / Arrow Keys\nBurst: Space";
            mesh.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            mesh.fontSize = 24;
            mesh.color = Color.white;
            mesh.anchor = TextAnchor.MiddleCenter;
            mesh.characterSize = 0.12f;
        }

        private static Sprite CreateCircleSprite()
        {
            var texture = new Texture2D(64, 64);
            for (var x = 0; x < 64; x++)
            {
                for (var y = 0; y < 64; y++)
                {
                    var dx = x - 32f;
                    var dy = y - 32f;
                    var inCircle = dx * dx + dy * dy <= 32f * 32f;
                    texture.SetPixel(x, y, inCircle ? new Color(0f, 0.75f, 1f, 1f) : new Color(0f, 0f, 0f, 0f));
                }
            }

            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f), 64f);
        }

        private static Sprite CreateBackgroundSprite()
        {
            var texture = new Texture2D(64, 64);
            for (var x = 0; x < 64; x++)
            {
                for (var y = 0; y < 64; y++)
                {
                    var checker = ((x / 8) + (y / 8)) % 2 == 0;
                    texture.SetPixel(x, y, checker ? new Color(0.08f, 0.1f, 0.18f, 1f) : new Color(0.12f, 0.14f, 0.22f, 1f));
                }
            }

            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f), 64f);
        }

        private static Sprite CreateBoxSprite()
        {
            var texture = new Texture2D(16, 16);
            for (var x = 0; x < 16; x++)
            {
                for (var y = 0; y < 16; y++)
                {
                    texture.SetPixel(x, y, Color.white);
                }
            }

            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f), 16f);
        }
    }
}
