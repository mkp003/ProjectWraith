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
    // Various room components
    [SerializeField]
    private GameObject roomCorner;
    [SerializeField]
    private GameObject roomWall;
    [SerializeField]
    private GameObject roomCenter;

    // Various corridors
    // Each corridor is 4m x 4m and will be treated at a single array element
    [SerializeField]
    private GameObject corridorCorrner;
    [SerializeField]
    private GameObject corridorSmall;
    [SerializeField]
    private GameObject corridorTJunction;

    // Dimensions of the map (width = x, length = z)
    // 1 = 4m x 4m
    [SerializeField]
    private int levelWidth = 150;
    [SerializeField]
    private int levellength = 150;

    // Each array element is 4m x 4m
    private Section[,] levelArray;
    private LinkedList<Section> listOfSections;

    // Start is called before the first frame update
    void Start()
    {
        this.levelArray = new Section[this.levelWidth,this.levellength];
        AdjustDimensions();
        CreateLevel();
        PrintMap();
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
        CreateCorridors();
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
            //print("Creating slice that is " + currentWidth + "x" + currentLength);
            //print("From position " + xMin + "," + zMin + " to " + xMax + "," + zMax);
            Section newSection = new Section(Random.Range(0, 1000), xMin + 1, zMin + 1, currentWidth - 2, currentLength - 2);
            CreateRoom(newSection);
            // Assign section to level array ( +/-1 to exclude the perimeter of the section)
            for(int x = xMin + 1; x <= xMax - 1; x++)
            {
                for (int z = zMin + 1; z <= zMax - 1; z++)
                {
                    this.levelArray[x, z] = newSection;
                }
            }
        }
        else
        {
            // We want to divide the section into two, so we do so by splitting along the longest side
            if(currentWidth > currentLength)
            {
                int randomPoint = Random.Range(xMin + 1, xMax);
                //print("Random point between " + xMin + " " + xMax + " is " + randomPoint);
                CreateSections(xMin, randomPoint, zMin, zMax);
                CreateSections(randomPoint + 1, xMax, zMin, zMax);
            }
            else
            {
                int randomPoint = Random.Range(zMin + 1, zMax);
                //print("Random point between " + zMin + " " + zMax + " is " + randomPoint);
                CreateSections(xMin, xMax, zMin, randomPoint);
                CreateSections(xMin, xMax, randomPoint + 1, zMax);
            }
        }
    }

    /// <summary>
    /// CreateRoom() wil create a new room based on the dimentions given by the given section
    /// </summary>
    /// <param name="section"></param>
    private void CreateRoom(Section section)
    {
        int xEndPosition = section.GetWidth() + section.GetXPosition();
        int zEndPosition = section.GetLength() + section.GetZPosition();
        int xStartPosition = section.GetXPosition();
        int zStartPosition = section.GetZPosition();

        for (int x = section.GetXPosition(); x <= xEndPosition; x++)
        {
            for(int z = section.GetZPosition(); z <= zEndPosition; z++)
            {
                // Check if the subsection is a corner
                int cornerRotation = CornerRotationCheck(x, z, xEndPosition, zEndPosition, xStartPosition, zStartPosition);
                if (cornerRotation >= 0)
                {
                    Debug.Log("Creating a corrner of angle: " + cornerRotation);
                    GameObject temp = Instantiate(this.roomCorner, new Vector3(x * 4.0f, 0, z * 4.0f), this.gameObject.transform.rotation, this.transform);
                    temp.transform.Rotate(0, cornerRotation * 90, 0);
                    continue;
                }
                // Check if the subsection is a wall
                int wallRotation = WallRotationCheck(x, z, xEndPosition, zEndPosition, xStartPosition, zStartPosition);
                if (wallRotation >= 0)
                {
                    Debug.Log("Creating a wall of angle: " + wallRotation);
                    GameObject temp = Instantiate(this.roomWall, new Vector3(x * 4.0f, 0, z * 4.0f), this.gameObject.transform.rotation, this.transform);
                    temp.transform.Rotate(0, wallRotation * 90, 0);
                }
                // Assume Middle piece
                else
                {
                    Instantiate(this.roomCenter, new Vector3(x * 4.0f, 0, z * 4.0f), this.gameObject.transform.rotation, this.transform);
                }
            }
            
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentX"></param>
    /// <param name="currentZ"></param>
    /// <param name="endX"></param>
    /// <param name="endZ"></param>
    /// <param name="startX"></param>
    /// <param name="startZ"></param>
    /// <returns></returns>
    private int CornerRotationCheck(int currentX, int currentZ, int endX, int endZ, int startX, int startZ)
    {
        // Bottom left
        if(currentX == startX && currentZ == startZ)
        {
            return 0;
        }
        else if(currentX == startX && currentZ == endZ)
        {
            return 1;
        }
        else if(currentZ == endZ  && currentX == endX)
        {
            return 2;
        }
        else if(currentZ == startZ && currentX == endX)
        {
            return 3;
        }
        else
        {
            return -1;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentX"></param>
    /// <param name="currentZ"></param>
    /// <param name="endX"></param>
    /// <param name="endZ"></param>
    /// <param name="startX"></param>
    /// <param name="startZ"></param>
    /// <returns></returns>
    private int WallRotationCheck(int currentX, int currentZ, int endX, int endZ, int startX, int startZ)
    {
        if(currentX == startX && (currentZ > startZ && currentZ < endZ))
        {
            return 0;
        }
        else if(currentZ == endZ && (currentX > startX && currentX < endX))
        {
            return 1;
        }
        else if (currentX == endX && (currentZ > startZ && currentZ < endZ))
        {
            return 2;
        }
        else if (currentZ == startZ && (currentX > startX && currentX < endX))
        {
            return 3;
        }
        else
        {
            return -1;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    private void CreateCorridors()
    {
        for(int x = 0; x < this.levelWidth; x++)
        {
            for(int z = 0; z < this.levellength; z++)
            {
                if(this.levelArray[x,z] == null)
                {
                    Instantiate(this.corridorTJunction, new Vector3(x * 4.0f, 0, z * 4.0f), this.transform.rotation, this.transform);
                }
            }
        }
    }


    /// <summary>
    /// For testing
    /// </summary>
    public void PrintMap()
    {
        string line = "";
        for (int z = 0; z < this.levelWidth; z++)
        {
            for (int x = 0; x < this.levellength; x++)
            {
                if(this.levelArray[x,z] == null)
                {
                    line += "o";
                }
                else
                {
                    line += "X";
                }
            }
            line += "\n";
        }
        print(line);
    }
}
