using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

 
/*public class TakeScreenshot : MonoBehaviour
{
    public int fileCounter;
    //public KeyCode screenshotKey;

    private Camera Camera
    {
        get
        {
            if (!_camera)
            {
                _camera = Camera.main;
            }
            return _camera;
        }
    }
    private Camera _camera;

    private void LateUpdate()
    {
        OVRInput.Update();
        if (OVRInput.Get(OVRInput.Button.Start))
        {
            Capture();
        }
    }

    public void Capture()
    {
        //RenderTexture activeRenderTexture = RenderTexture.active;
        //RenderTexture.active = Camera.targetTexture;
        int width = 256;
        int height = 256;
        Camera.Render();
        Debug.Log("Here1");
        Texture2D image = new Texture2D(width, height);
        Debug.Log("Here2");
        image.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        image.Apply();
        //RenderTexture.active = activeRenderTexture;

        byte[] bytes = image.EncodeToPNG();
        Destroy(image);

        File.WriteAllBytes(Application.dataPath + "/Backgrounds/" + fileCounter + ".png", bytes);
        Debug.Log(Application.dataPath + "/DCIM/" + fileCounter + ".png");
        fileCounter++;
    }*/
    public class TakeScreenshot : MonoBehaviour
    {
        // Grab the camera's view when this variable is true.
        bool grab;
        private int fileCounter;
        // The "m_Display" is the GameObject whose Texture will be set to the captured image.
        public Renderer m_Display;

        private void Update()
        {
        //Press space to start the screen grab
            OVRInput.Update();
            if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                grab = true;
            }
        }

       /* public void OnPostRender()
        {       
            if(grab)
            { 
            
                //Create a new texture with the width and height of the screen
                Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                //Read the pixels in the Rect starting at 0,0 and ending at the screen's width and height
                texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
                texture.Apply();

                byte[] bytes = texture.EncodeToPNG();
                Destroy(texture);

                File.WriteAllBytes(Application.persistentDataPath + "/Snapshot" + fileCounter + ".png", bytes);
                Debug.Log(Application.dataPath + "/Backgrounds/Snapshot" + fileCounter + ".png");
                fileCounter++;
                //Check that the display field has been assigned in the Inspector
                if (m_Display != null)
                    //Give your GameObject with the renderer this texture
                    m_Display.material.mainTexture = texture;
                //Reset the grab state
                grab = false;
            }
            
        }*/
        public void Capture()
        {
            ScreenCapture.CaptureScreenshot("Screenshot" + fileCounter + ".png");
            fileCounter++;
        }
    }

