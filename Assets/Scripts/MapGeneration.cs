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
        public enum Type
        {
            Hallway = 0, Room = 1
        }

        private int id;
        private int width;
        private int length;
        private int xIndexPosition;
        private int zIndexPosition;
        private List<Door> doors;
        private List<GameObject> sectionComponents;
        private Type sectionType;

        /// <summary>
        /// Constructor for the Section class.
        /// </summary>
        /// <param name="id">Unique Identifier for this section</param>
        /// <param name="x">The starting Array index position for this Section (width)</param>
        /// <param name="z">The starting Array index position for this Section (length)</param>
        /// <param name="width">The physical width of this section (meters)</param>
        /// <param name="length">The physical length of this section (meters)</param>
        public Section(int id, int x, int z, int width, int length, Type _type)
        {
            this.xIndexPosition = x;
            this.zIndexPosition = z;
            this.width = width;
            this.length = length;
            sectionComponents = new List<GameObject>();
            doors = new List<Door>();
            sectionType = _type;
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetLength()
        {
            return length;
        }

        public int GetXIndexPosition()
        {
            return xIndexPosition;
        }

        public int GetZIndexPosition()
        {
            return zIndexPosition;
        }

        public List<Door> GetSectionDoors()
        {
            return doors;
        }

        public void AddSectionComponent(GameObject _component)
        {
            sectionComponents.Add(_component);
        }

        public void AddDoor(Door _door)
        {
            doors.Add(_door);
        }
    }

    /// <summary>
    /// Door represents a class of doors used as enterences to sections
    /// </summary>
    private class Door
    {
        GameObject doorGameObject;
        public int xPos;
        public int zPos;

        public Door(GameObject _door, int _xPosition, int _zPosition)
        {
            doorGameObject = _door;
            xPos = _xPosition;
            zPos = _zPosition;
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
    [SerializeField]
    private GameObject roomDoor;

    // Various corridors
    // Each corridor is 6m x 6m and will be treated at a single array element
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
    private List<Section> listOfSections = new List<Section>();

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


    /// <summary>
    /// CreateLevel will procedurally generate a new level with the predefined dimensions.
    /// </summary>
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
            // Create section and exclude 1 array element on all sides for potential hallways
            // If the it has a negative dimension because of this, discard the room
            if(currentWidth - 2 <= 0 || currentLength - 2 <= 0)
            {
                return;
            }
            Section newSection = new Section(Random.Range(0, 1000), xMin + 1, zMin + 1, currentWidth - 2, currentLength - 2, Section.Type.Room);
            CreateRoom(newSection);
            listOfSections.Add(newSection);
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
                int randomPoint = Random.Range(xMin + 1, xMax - 1);
                CreateSections(xMin, randomPoint, zMin, zMax);
                CreateSections(randomPoint + 1, xMax, zMin, zMax);
            }
            else
            {
                int randomPoint = Random.Range(zMin + 1, zMax);
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
        GameObject room = new GameObject();
        room.name = "Room-" + listOfSections.Count;

        int xEndPosition = section.GetXIndexPosition() + section.GetWidth();
        int zEndPosition = section.GetZIndexPosition() + section.GetLength();

        int xStartPosition = section.GetXIndexPosition();
        int zStartPosition = section.GetZIndexPosition();

        // Determine how many doors will be in this section (and where they will be)
        int xWalls = section.GetWidth();
        int zWalls = section.GetLength();
        if(xWalls <= 2)
        {
            xWalls = 0;
        }
        else
        {
            xWalls = 2 * (xWalls - 2);
        }
        if(zWalls <= 2)
        {
            zWalls = 0;
        }
        else
        {
            zWalls = 2 * (zWalls - 2);
        }
        int numberOfRegularWalls = xWalls + zWalls;
        int numberOfPossibleDoors = Random.Range(1, numberOfRegularWalls);

        List<System.Tuple<int, int>> doorLocations = GenerateDoorLocations(numberOfPossibleDoors, xStartPosition, xEndPosition, zStartPosition, zEndPosition);
        
        // Create all the walls, floor and ceiling
        for (int x = section.GetXIndexPosition(); x <= xEndPosition; x++)
        {
            for(int z = section.GetZIndexPosition(); z <= zEndPosition; z++)
            {
                // Check if the subsection is a corner
                int cornerRotation = CornerRotationCheck(x, z, xEndPosition, zEndPosition, xStartPosition, zStartPosition);
                if (cornerRotation >= 0)
                {
                    GameObject temp = Instantiate(this.roomCorner, new Vector3(x * 4.0f, 0, z * 4.0f), this.gameObject.transform.rotation, this.transform);
                    temp.transform.Rotate(0, cornerRotation * 90, 0);
                    section.AddSectionComponent(temp);
                    temp.transform.SetParent(room.transform);
                }
                else
                {
                    // Check if the subsection is a wall
                    int wallRotation = WallRotationCheck(x, z, xEndPosition, zEndPosition, xStartPosition, zStartPosition);
                    if (wallRotation >= 0)
                    {
                        GameObject prefabToUse;
                        // Determine if this wall is a door
                        bool isDoor = false;
                        if (CheckIsDoorLocation(doorLocations, x, z))
                        {
                            prefabToUse = this.roomDoor;
                            isDoor = true;
                        }
                        else
                        {
                            prefabToUse = this.roomWall;
                        }
                        GameObject temp = Instantiate(prefabToUse, new Vector3(x * 4.0f, 0, z * 4.0f), this.gameObject.transform.rotation, this.transform);
                        temp.transform.Rotate(0, wallRotation * 90, 0);
                        section.AddSectionComponent(temp);
                        temp.transform.SetParent(room.transform);
                        if (isDoor)
                        {
                            Door door = new Door(temp, x, z);
                            section.AddDoor(door);
                        }
                    }
                    // Assume Middle piece
                    else
                    {
                        GameObject temp = Instantiate(this.roomCenter, new Vector3(x * 4.0f, 0, z * 4.0f), this.gameObject.transform.rotation, this.transform);
                        section.AddSectionComponent(temp);
                        temp.transform.SetParent(room.transform);

                    }
                }
            }
        }
    }


    /// <summary>
    /// GenerateDoorLocations will generate locations for doors to be placed in the given room corrdinants.
    /// </summary>
    /// <param name="_numPossibleDoors"></param>
    /// <param name="_xStartPosition"></param>
    /// <param name="_xEndPosition"></param>
    /// <param name="_zStartPosition"></param>
    /// <param name="_zEndPosition"></param>
    private List<System.Tuple<int, int>> GenerateDoorLocations(int _numberOfPossibleDoors, int _xStartPosition, int _xEndPosition, int _zStartPosition, int _zEndPosition)
    {
        List<System.Tuple<int, int>> doorLocations = new List<System.Tuple<int, int>>();

        for (int i = 0; i < _numberOfPossibleDoors; i++)
        {
            // Choose a random side for this potential door
            int side = Random.Range(0, 3);
            int x = 0;
            int z = 0;
            switch (side)
            {
                case 0:
                    {
                        x = Random.Range(_xStartPosition + 1, _xEndPosition - 1);
                        z = _zStartPosition;
                        break;
                    }
                case 1:
                    {
                        x = Random.Range(_xStartPosition + 1, _xEndPosition - 1);
                        z = _zEndPosition;
                        break;
                    }
                case 2:
                    {
                        x = _xStartPosition;
                        z = Random.Range(_zStartPosition + 1, _zEndPosition - 1);
                        break;
                    }
                default:
                    {
                        x = _xEndPosition;
                        z = Random.Range(_zStartPosition + 1, _zEndPosition - 1);
                        break;
                    }
            }
            System.Tuple<int, int> doorLocation = new System.Tuple<int, int>(x, z);
            // If the position is already a door, do not add it again. Also, doors cannot be placed on corners
            if (!CheckIsDoorLocation(doorLocations, x, z) && (CornerRotationCheck(x, z, _xEndPosition, _zEndPosition, _xStartPosition, _zStartPosition) < 0))
            {
                doorLocations.Add(doorLocation);
            }
        }

        // Forceably add a door if none exists
        if(doorLocations.Count == 0)
        {
            System.Tuple<int, int> doorLocation = new System.Tuple<int, int>(_xStartPosition + 1, _zStartPosition);
            doorLocations.Add(doorLocation);
        }

        return doorLocations;
    }


    /// <summary>
    /// CornerRotationCheck() Checks what the rotation of a corner room will be based on 
    /// the dimensions given in the argument
    /// </summary>
    /// <param name="currentX">Current X position in the section</param>
    /// <param name="currentZ">Current Z position in the section</param>
    /// <param name="endX">Last X position in the section</param>
    /// <param name="endZ">Lat Z position in the section</param>
    /// <param name="startX">First X position in the section</param>
    /// <param name="startZ">First Z position in the section</param>
    /// <returns>int representing the number or 90 degree rotations:
    /// 0 = 0 Deg, 1 = 90 Deg, 2 = 180 Deg, 3 = 270 Deg, -1 = Not a corner</returns>
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
    /// WallRotationCheck() Checks what the rotation of a wall room will be based on 
    /// the dimensions given in the argument
    /// </summary>
    /// <param name="currentX">Current X position in the section</param>
    /// <param name="currentZ">Current Z position in the section</param>
    /// <param name="endX">Last X position in the section</param>
    /// <param name="endZ">Lat Z position in the section</param>
    /// <param name="startX">First X position in the section</param>
    /// <param name="startZ">First Z position in the section</param>
    /// <returns>int representing the number or 90 degree rotations:
    /// 0 = 0 Deg, 1 = 90 Deg, 2 = 180 Deg, 3 = 270 Deg, -1 = Not a wall</returns>
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
    /// CheckIsDoorLocation will check to see if the current x and z coordinates represent a door location.
    /// </summary>
    /// <param name="_locations">List of Tuples representing x and z coordinates or doors</param>
    /// <param name="_currentX">Current x coordinate</param>
    /// <param name="_currentZ">Current z coordinate</param>
    /// <returns>True if the current X and Y coordinates are a tuple in the given list, false otherwise</returns>
    private bool CheckIsDoorLocation(List<System.Tuple<int, int>> _locations, int _currentX, int _currentZ)
    {
        foreach(System.Tuple<int, int> door in _locations)
        {
            if(door.Item1 == _currentX && door.Item2 == _currentZ)
            {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// CreateCorridors
    /// </summary>
    private void CreateCorridors()
    {
        foreach(Section section in listOfSections)
        {
            foreach(Door door in section.GetSectionDoors())
            {
                GenerateHallway(0, door.xPos, door.zPos);
            }
        }
    }


    /// <summary>
    /// CreateCorridors2 will recursively create corridors until no more can be created.
    /// </summary>
    /// <param name="_direction">0 = North, 1 = East, 2 = South, 3 = West</param>
    /// <param name="_currentX"></param>
    /// <param name="_currentZ"></param>
    private void GenerateHallway(int _direction, int _currentX, int _currentZ)
    {
        if(IsHallwayComplete(_currentX, _currentZ))
        {
            Debug.LogError("***Hallway Complete***");
            return;
        }
        else
        {
            Debug.LogError("***Creating New Hallway***");
            if (CanMoveForward(_direction, _currentX, _currentZ))
            {
                System.Tuple<int,int> newPosition = CreateHallway(_direction, _currentX, _currentZ);
                GenerateHallway(_direction, newPosition.Item1, newPosition.Item2);
            }
            else
            {
                GenerateHallway(Random.Range(0, 4), _currentX, _currentZ);
            }
        }
    }


    /// <summary>
    /// IsHallwayComplete will determine if there is any more hallway that can be completed.
    /// </summary>
    /// <param name="_currentX"></param>
    /// <param name="_currentZ"></param>
    /// <returns></returns>
    private bool IsHallwayComplete(int _currentX, int _currentZ)
    {
        if(CanMoveInXDirection(_currentX, _currentZ) || CanMoveInZDirection(_currentX, _currentZ))
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    /// <summary>
    /// CanMoveInXDirection 
    /// </summary>
    /// <param name="_currentX"></param>
    /// <param name="_currentZ"></param>
    /// <returns></returns>
    private bool CanMoveInXDirection(int _currentX, int _currentZ)
    {
        bool canMoveLeft = true;
        bool canMoveRight = true;
        if (_currentX + 1 >= levelWidth || levelArray[_currentX + 1, _currentZ] != null)
        {
            canMoveRight = false;
        }

        if(_currentX - 1 < 0 || levelArray[_currentX - 1, _currentZ] != null)
        {
            canMoveLeft = false;
        }

        return (canMoveLeft || canMoveRight);
    }


    /// <summary>
    /// CanMoveInZDirection 
    /// </summary>
    /// <param name="_currentX"></param>
    /// <param name="_currentZ"></param>
    /// <returns></returns>
    private bool CanMoveInZDirection(int _currentX, int _currentZ)
    {
        bool canMoveUp = true;
        bool canMoveDown = true;
        if (_currentZ + 1 >= levellength || levelArray[_currentX, _currentZ + 1] != null) 
        {
            canMoveUp = false;
        }

        if(_currentZ - 1 < 0 || levelArray[_currentX, _currentZ - 1] != null)
        {
            canMoveDown = false;
        }

        return (canMoveUp || canMoveDown);
    }


    /// <summary>
    /// CanCreateNextHallway will determine if another hallway can be created based on the current position and direction.
    /// </summary>
    /// <param name="_direction"></param>
    /// <param name="_currentX"></param>
    /// <param name="_currentZ"></param>
    /// <returns></returns>
    private bool CanMoveForward(int _direction, int _currentX, int _currentZ)
    {
        switch (_direction)
        {
            case 0:
                {
                    if (_currentZ + 1 < levellength && levelArray[_currentX, _currentZ + 1] == null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            case 1:
                {
                    if (_currentX + 1 < levelWidth && levelArray[_currentX + 1, _currentZ] == null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            case 2:
                {
                    if (_currentZ - 1 >= 0 && levelArray[_currentX, _currentZ - 1] == null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            default:
                {
                    if (_currentX - 1 >= 0 && levelArray[_currentX - 1, _currentZ] == null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="_direction"></param>
    /// <param name="_currentX"></param>
    /// <param name="_currentZ"></param>
    /// <returns></returns>
    private System.Tuple<int,int> CreateHallway(int _direction, int _currentX, int _currentZ)
    {
        GameObject hallway = null;
        switch (_direction)
        {
            case 0:
                {
                    // Continue with a regular corridor if it can continue to go straight after this piece
                    if(_currentZ + 2 < levellength && levelArray[_currentX, _currentZ + 2] == null)
                    {
                        hallway = Instantiate(this.corridorSmall, new Vector3(_currentX * 4.0f, -2, _currentZ * 4.0f), this.transform.rotation, this.transform);
                    }
                    // Otherwise create a junction
                    else
                    {
                        hallway = Instantiate(this.corridorCorrner, new Vector3(_currentX * 4.0f, -2, _currentZ * 4.0f), this.transform.rotation, this.transform);
                    }
                    levelArray[_currentX, _currentZ] = new Section(0, _currentX, _currentZ, 1, 1, Section.Type.Hallway);
                    hallway.transform.Rotate(0, 90 * _direction, 0);
                    return new System.Tuple<int, int>(_currentX, _currentZ + 1);
                }
            case 1:
                {
                    // Continue with a regular corridor if it can continue to go straight after this piece
                    if (_currentX + 2 < levelWidth && levelArray[_currentX + 2, _currentZ] == null)
                    {
                        hallway = Instantiate(this.corridorSmall, new Vector3(_currentX * 4.0f, -2, _currentZ * 4.0f), this.transform.rotation, this.transform);
                    }
                    // Otherwise create a junction
                    else
                    {
                        hallway = Instantiate(this.corridorCorrner, new Vector3(_currentX * 4.0f, -2, _currentZ * 4.0f), this.transform.rotation, this.transform);
                    }
                    levelArray[_currentX, _currentZ] = new Section(0, _currentX, _currentZ, 1, 1, Section.Type.Hallway);
                    hallway.transform.Rotate(0, 90 * _direction, 0);
                    return new System.Tuple<int, int>(_currentX + 1, _currentZ);
                }
            case 2:
                {
                    // Continue with a regular corridor if it can continue to go straight after this piece
                    if (_currentZ - 2 >= 0 && levelArray[_currentX, _currentZ - 2] == null)
                    {
                        hallway = Instantiate(this.corridorSmall, new Vector3(_currentX * 4.0f, -2, _currentZ * 4.0f), this.transform.rotation, this.transform);
                    }
                    // Otherwise create a junction
                    else
                    {
                        hallway = Instantiate(this.corridorCorrner, new Vector3(_currentX * 4.0f, -2, _currentZ * 4.0f), this.transform.rotation, this.transform);
                    }
                    levelArray[_currentX, _currentZ] = new Section(0, _currentX, _currentZ, 1, 1, Section.Type.Hallway);
                    hallway.transform.Rotate(0, 90 * _direction, 0);
                    return new System.Tuple<int, int>(_currentX, _currentZ - 1);
                }
            default:
                {
                    // Continue with a regular corridor if it can continue to go straight after this piece
                    if (_currentX - 2 >=0 && levelArray[_currentX - 2, _currentZ] == null)
                    {
                        hallway = Instantiate(this.corridorSmall, new Vector3(_currentX * 4.0f, -2, _currentZ * 4.0f), this.transform.rotation, this.transform);
                    }
                    // Otherwise create a junction
                    else
                    {
                        hallway = Instantiate(this.corridorCorrner, new Vector3(_currentX * 4.0f, -2, _currentZ * 4.0f), this.transform.rotation, this.transform);
                    }
                    levelArray[_currentX, _currentZ] = new Section(0, _currentX, _currentZ, 1, 1, Section.Type.Hallway);
                    hallway.transform.Rotate(0, 90 * _direction, 0);
                    return new System.Tuple<int, int>(_currentX - 1, _currentZ);
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
