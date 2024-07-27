using System.IO;
using UnityEngine;

public class Render360 : MonoBehaviour
{
    public int cubemapSize = 2048;
    public int equirectangularWidth = 4096;
    public int equirectangularHeight = 2048;
    public string outputFolder = "Assets/360/360Renders";
    public int totalFrames = 300; // User can set this to the desired number of frames
    public float frameRate = 30f; // Frames per second for playback
    private Camera cam;
    private RenderTexture cubemap;
    private Texture2D equirectangular;
    private Material cubemapToEquirectangularMaterial;
    private int currentFrame = 0;
    private float timeBetweenCaptures;
    private float lastCaptureTime;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            cam = Camera.main;
            if (cam == null)
            {
                Debug.LogError("No camera found. Please attach this script to a camera or ensure there's a main camera in the scene.");
                return;
            }
        }

        cam.fieldOfView = 90;
        cam.aspect = 1;

        cubemap = new RenderTexture(cubemapSize, cubemapSize, 24, RenderTextureFormat.ARGB32);
        cubemap.dimension = UnityEngine.Rendering.TextureDimension.Cube;

        equirectangular = new Texture2D(equirectangularWidth, equirectangularHeight, TextureFormat.RGB24, false);

        Shader shader = Resources.Load<Shader>("CubemapToEquirectangular");
        if (shader == null)
        {
            Debug.LogError("Shader 'CubemapToEquirectangular' not found. Make sure the shader file is in your project's Resources folder.");
            return;
        }
        cubemapToEquirectangularMaterial = new Material(shader);

        // Create output folder if it doesn't exist
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        timeBetweenCaptures = 1f / frameRate;
        lastCaptureTime = -timeBetweenCaptures; // Ensure we capture a frame immediately
        Time.timeScale = 1f; // Ensure normal time progression
    }

    void Update()
    {
        if (currentFrame < totalFrames)
        {
            if (Time.time >= lastCaptureTime + timeBetweenCaptures)
            {
                Capture360Frame();
                currentFrame++;
                lastCaptureTime = Time.time;
                
                // Optional: Update progress in console
                if (currentFrame % 10 == 0)
                {
                    Debug.Log($"Captured frame {currentFrame} of {totalFrames}");
                }
            }
        }
        else if (currentFrame == totalFrames)
        {
            Debug.Log($"Finished capturing {totalFrames} frames. You can find them in the {outputFolder} directory.");
            currentFrame++; // Increment to avoid repeating this message
            
            // Optional: Pause the game after capturing all frames
            Time.timeScale = 0f;
        }
    }

    void Capture360Frame()
    {
        if (!cam.RenderToCubemap(cubemap))
        {
            Debug.LogError("Failed to render to cubemap. Check if the camera is set up correctly.");
            return;
        }

        CubemapToEquirectangular();
        SaveEquirectangular();

        // Optionally, you can add your game logic here to move or animate objects in your scene
    }

    void CubemapToEquirectangular()
    {
        RenderTexture equirectangularRT = new RenderTexture(equirectangularWidth, equirectangularHeight, 0, RenderTextureFormat.ARGB32);
        equirectangularRT.Create();

        cubemapToEquirectangularMaterial.SetTexture("_Cubemap", cubemap);
        Graphics.Blit(null, equirectangularRT, cubemapToEquirectangularMaterial);

        RenderTexture.active = equirectangularRT;
        equirectangular.ReadPixels(new Rect(0, 0, equirectangularWidth, equirectangularHeight), 0, 0);
        equirectangular.Apply();
        RenderTexture.active = null;

        equirectangularRT.Release();
    }

    void SaveEquirectangular()
    {
        string filename = $"{outputFolder}/frame_{currentFrame:D5}.png";
        byte[] bytes = equirectangular.EncodeToPNG();
        File.WriteAllBytes(filename, bytes);
    }

    void OnDestroy()
    {
        if (cubemap != null)
            cubemap.Release();
    }
}
