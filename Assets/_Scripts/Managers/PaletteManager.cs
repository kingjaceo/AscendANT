using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using System.IO;

public class PaletteManager : MonoBehaviour
{
    public static PaletteManager Instance;
    public static List<Color> Colors = new List<Color>();
    
    void Awake()
    {
        string path = Directory.GetCurrentDirectory() + "\\Assets";
        string palettePath = path + "\\palette.png";
        
        Texture2D image = LoadImage(palettePath);
        
        Instance = this;
        ReadColors(image);
    }

    void ReadColors(Texture2D image)
    {
        for (int i = 0; i < 10; i++)
        {
            Color color = image.GetPixel(i * 160 + 20, 100);
            Colors.Add(color);
        }
    }

    public Texture2D LoadImage(string FilePath) {
 
     // Load a PNG or JPG file from disk to a Texture2D
     // Returns null if load fails
 
     Texture2D Tex2D;
     byte[] FileData;
 
     if (File.Exists(FilePath)){
       FileData = File.ReadAllBytes(FilePath);
       Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
       if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
         return Tex2D;                 // If data = readable -> return texture
     }  
     return null;                     // Return null if load failed
   }
}
