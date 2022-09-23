using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ChangeWaterPlaneBackgroundByLocal : MonoBehaviour
{
    public KeyCode NextKey = KeyCode.F7;
    private Renderer _waterPlaneRenderer;
    private List<Texture2D> _imageList = new List<Texture2D>();
    private int _selectedImageIndex = 0;
    // Use this for initialization
    void Start ()
    {
        //var images = Resources.LoadAll("WaterPlane/Backgrounds", typeof(Texture2D));
        //foreach (var image in images)
        //{
        //    _imageList.Add((Texture2D) image);
        //}
        var filePaths = Directory.GetFiles(Application.dataPath + "/WaterPlane/Backgrounds").Where(
            filePath => filePath.EndsWith(".bmp") || 
                filePath.EndsWith(".jpg") || 
                filePath.EndsWith(".png") || 
                filePath.EndsWith(".gif")).ToArray();

        foreach (var filePath in filePaths)
        {
            Texture2D imgTexture = new Texture2D(1920, 1102);
            imgTexture.LoadImage(File.ReadAllBytes(filePath));
            _imageList.Add(imgTexture);
        }

        _waterPlaneRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(NextKey))
        {
            if (_imageList.Count > 0)
            {
                _selectedImageIndex++;
                if (_selectedImageIndex > _imageList.Count - 1)
                {
                    _selectedImageIndex = 0;
                }
                _waterPlaneRenderer.material.mainTexture = _imageList[_selectedImageIndex];
            }
        }

    }
}
