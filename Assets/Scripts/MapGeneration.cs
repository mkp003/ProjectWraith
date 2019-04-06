using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for generating the layout of the level.
/// Note:: width will be determined by the x value, and length will be done with the z value
/// </summary>
public class MapGeneration : MonoBehaviour
{
    private class Section
    {
        private int width;
        private int length;

        public int GetWidth()
        {
            return width;
        }

        public int GetLength()
        {
            return length;
        }
    }
    // **The following are references to prefabs used to create the 
    // level layout**
    // Various rooms
    [SerializeField]
    private GameObject oneDoorRoom;
    [SerializeField]
    private GameObject twoDoorRoomCorner;
    [SerializeField]
    private GameObject twoDoorRoomOpposite;
    [SerializeField]
    private GameObject threeDoorRoom;
    [SerializeField]
    private GameObject corridorCorner;
    // Various corridors
    [SerializeField]
    private GameObject corridorCorrner;
    [SerializeField]
    private GameObject corridorSmall;
    [SerializeField]
    private GameObject corridorTJunction;

    // Dimensions of the map (width = x, length = z)
    [SerializeField]
    private int levelWidth = 150;
    [SerializeField]
    private int levellength = 150;


    // Start is called before the first frame update
    void Start()
    {
        AdjustDimensions();
        CreateLevel();
    }


    /// <summary>
    /// AdjustDimensions() will ensure that the dimensions of the level are adjusted if
    /// it is given invalid parameters that are not divisable by 2
    /// </summary>
    private void AdjustDimensions()
    {
        if(this.levelWidth%2 != 0)
        {
            this.levelWidth -= 1;
        }
        if (this.levellength % 2 != 0)
        {
            this.levellength -= 1;
        }
    }


    private void CreateLevel()
    {

    }


    private void CreateSections()
    {

    }
}
