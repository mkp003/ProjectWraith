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
        private int id;
        private int width;
        private int length;
        private int xPosition;
        private int zPosition;
        private bool beenVisited = false;
        private LinkedList<Section> connectingSections;

        public Section(int id, int x, int z, int width, int length)
        {
            this.xPosition = x;
            this.zPosition = z;
            this.width = width;
            this.length = length;
            this.connectingSections = new LinkedList<Section>();
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetLength()
        {
            return length;
        }

        public int GetXPosition()
        {
            return xPosition;
        }

        public int GetZPosition()
        {
            return zPosition;
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

    private Section[,] levelArray;
    private LinkedList<Section> listOfSections;

    // Start is called before the first frame update
    void Start()
    {
        //this.levelArray = new Section[this.levelWidth,this.levellength];
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
        CreateSections(0, this.levelWidth - 1, 0, this.levellength - 1);
    }


    /// <summary>
    /// CreateSections() Will create all the sections that will be in the array
    /// </summary>
    /// <param name="xMin">starting position from the width of the room</param>
    /// <param name="xMax">ending position from the width of the room</param>
    /// <param name="zMin">starting position from the length of the room</param>
    /// <param name="zMax">ending position from the length of the room</param>
    private void CreateSections(int xMin, int xMax, int zMin, int zMax)
    {
        // Check for minimum size
        int currentWidth = xMax - xMin;
        int currentLength = zMax - zMin;
        if (currentWidth <= 10 && currentLength <= 10)
        {
            Section newSection = new Section(Random.Range(0, 1000), xMin, zMin, currentWidth, currentLength);
            
        }
        else
        {
            if(currentWidth > currentLength)
            {
                int randomPoint = Random.Range(xMin + 1, xMax);
                CreateSections(xMin, randomPoint, zMin, zMax);
                CreateSections(randomPoint + 1, xMax, zMin, zMax);
            }
            else
            {
                int randomPoint = Random.Range(zMin + 1, zMax);
                CreateSections(xMin, xMin, zMin, randomPoint);
                CreateSections(xMin, xMax, randomPoint + 1, zMax);
            }
        }
    }
}
