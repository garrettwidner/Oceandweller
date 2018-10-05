using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    public SoilSection.Type topSoilType;
    private int minimumDiggableLayerCount = 2;

    private List<Layer> layers;
    //Records which layer in the list corresponds layer -1
    //Recorded since layer -1 will always be present in a pit
    private int belowGroundLayerIndex;

    //Layers above ground are positive, ground level is 0, and layers below ground are negative. 
    private const int MAX_HEIGHT = 3;
    private const int MAX_DEPTH = -4;
    private const int MIN_START_HEIGHT = -1;

    //Depth of the first layer of the Pit
    private int startingLayer;
    //Depth of the true final layer of the pit. This is not a diggable layer, so it will be 1 below 
    //the max diggable depth, and revealed once the last layer has been dug.
    private int finalDepth;

    private int DiggableLayerCount
    {
        get
        {
            return startingLayer - finalDepth;
        }
    }

    private void Start()
    {
        CalculatePitDepth();
    }

    private void CalculatePitDepth()
    {
        startingLayer = Random.Range(MIN_START_HEIGHT, MAX_HEIGHT + 1);

        int startingDepth = (startingLayer < 0) ? startingLayer : 0;
        //Final depth is 1 layer below diggable depth
        finalDepth = Random.Range(startingDepth, MAX_DEPTH - 1) - 1;

        ValidateFinalDepth(DiggableLayerCount);

        /*
        print("---------");
        print("Starting layer " + startingLayer);
        print("Final Layer depth " + finalDepth);
        */
    }

    private void ValidateFinalDepth(int diggableLayerCount)
    {
        if (diggableLayerCount < minimumDiggableLayerCount)
        {
            finalDepth = startingLayer - minimumDiggableLayerCount;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            CalculatePitDepth();
        }

        if(Input.GetKeyDown(KeyCode.U))
        {
            print("Diggable depth: " + DiggableLayerCount);
        }
    }

    public bool ClearSoilIfPossible(Digger.DigInfo digInfo)
    {
        //If this is the first time digging in the pit, set up all Layers.
        return true;
    }

    private void SetUpSoilLayersRandomly()
    {
        //Get random depth. Iterate through, creating layers until the depth is reached, then
        //create a layer of Caliche

        int aboveGroundLayers = Random.Range(0, MAX_HEIGHT);

    }

    public class Layer
    {
        public List<SoilSection> soilSections = new List<SoilSection>(4);
        public SoilSection.Type type;


        public void SetAllSections(SoilSection first, SoilSection second, SoilSection third, SoilSection fourth)
        {
            SetSoilSection(first, null, second, 0);
            SetSoilSection(second, first, third, 1);
            SetSoilSection(third, second, fourth, 2);
            SetSoilSection(fourth, third, null, 3);
        }

        /// <summary>
        /// Sets a SoilSection within the layer given its position
        /// </summary>
        /// <param name="newSection"></param>
        /// <param name="positionFromLeft">0 is leftmost, 3 is rightmost.</param>
        public void SetSoilSection(SoilSection newSection, SoilSection leftSection, SoilSection rightSection, int positionFromLeft)
        {
            if(positionFromLeft < 0 || positionFromLeft > 3)
            {
                Debug.LogWarning("positionFromLeft must be between 0 and 3");
                return;
            }

            newSection.SetUpSoil(leftSection, rightSection);

            soilSections[positionFromLeft] = newSection;
        }

        public void RemoveSection(int positionFromLeft)
        {
            SoilSection toRemove = soilSections[positionFromLeft];

            foreach(SoilSection s in soilSections)
            {
                if(s != toRemove)
                {
                    s.SoilOnSameLevelWasRemoved(toRemove);
                }
            }

            toRemove.RemoveSelf();
        }
    }

    
}
