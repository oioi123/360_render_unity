# Render360: 360-Degree Video Capture for Unity

Render360 is a Unity script that allows you to capture 360-degree video frames from your Unity scene. These frames can later be combined into a full 360-degree video.

## Setup

1. Create a new folder named "Resources" in your Unity project's Assets folder if it doesn't already exist.

2. Place the "CubemapToEquirectangular" shader file in the Resources folder.

3. Attach the Render360 script to the main camera in your scene.

## Configuration

In the Unity Inspector, you can configure the following parameters for the Render360 script:

- Cubemap Size: Size of the cubemap texture (default: 2048)
- Equirectangular Width: Width of the output equirectangular image (default: 4096)
- Equirectangular Height: Height of the output equirectangular image (default: 2048)
- Output Folder: Directory where the frame images will be saved (default: "Assets/360/360Renders")
- Total Frames: Number of frames to capture (default: 300)
- Frame Rate: Frames per second for playback (default: 30)

## Usage

1. Set up your Unity scene as desired.

2. Configure the Render360 script parameters in the Inspector.

3. Press Play in the Unity Editor.

4. The script will automatically capture the specified number of frames at the set frame rate.

5. Progress updates will be logged to the console every 10 frames.

6. After capturing all frames, the game will pause automatically.

7. The captured frames will be saved as PNG files in the specified output folder, named "frame_00001.png", "frame_00002.png", etc.

## Creating the Video

After capturing the frames, you can use video editing software or a command-line tool like FFmpeg to combine the frames into a video.

Example FFmpeg command:
mpeg -framerate 30 -i frame_%05d.png -c:v libx264 -pix_fmt yuv420p output.mp4

This command assumes a frame rate of 30 fps and that your images are named "frame_00001.png", "frame_00002.png", etc.

## Notes

- The capture process may take longer than real-time, especially for high-resolution or complex scenes.
- Ensure your scene is set up for 360-degree viewing (e.g., place the camera at the center of your environment).

## Troubleshooting

If you encounter any issues:

1. Make sure the "CubemapToEquirectangular" shader is in the Resources folder.
2. Check the Unity console for any error messages.
3. Ensure your scene has a camera with the Render360 script attached.
4. Verify that the output folder is writable.