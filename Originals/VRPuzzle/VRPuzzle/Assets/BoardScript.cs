﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardScript : MonoBehaviour {

    public int gridwidth;
    public int gridheight;
    public int groundY;
    public GameObject Ground;
    public GameObject WestWall;
    public GameObject EastWall;
    public GameObject NorthWall;
    public GameObject SouthWall;
    // Shadows for indicating whether a piece can be placed or not
    public GameObject RSh0, RSh1, RSh2, RSh3; // Red shadows gameobjects
    public GameObject BSh0, BSh1, BSh2, BSh3; // Blue shadow game objects

    // Debug text
    public Text debug;

    // Matrix for storing pieces
    private bool[,] board;
    // Grouping shadows
    private GameObject[] RSh;
    private GameObject[] BSh;

    // Function for checking a single block on the grid for fullness
    // Input: An x,y coordinate from (-gridwidth/2,-gridheigh/2) to (gridwidh/2,gridheight/2)
    // Output: Whether this grid coordinate is empty (true) or not (false)
    public bool checkGrid(int x, int y)
    {
        // NOTE: This excludes 0, which it shouldn't. But this makes it work for some reason.
        if (x < 1 || y < 1 || x >= gridwidth || y >= gridheight)
        {
            return false;
        }
        return !board[x, y];
    }

    public void clearShadows(string flag = "all")
    {
        if (flag == "blue" || flag == "all")
        {
            foreach (GameObject brick in BSh)
            {
                brick.SetActive(false);
            }
        }
        if (flag == "red" || flag == "all")
        {
            foreach (GameObject brick in RSh)
            {
                brick.SetActive(false);
            }
        }
        //debug.text = "Clearing " + flag + " shadows.";
    }

    // Function for checking an entire piece but not placing it
    // Input: 8 coordinates of 4 blocks in 1 brick
    // Output: True if all blocks can be placed, False is not
    public bool checkBrick(int x0, int y0, int x1, int y1, int x2, int y2, int x3, int y3)
    {
        bool isValid = checkGrid(x0, y0) & checkGrid(x1, y1) & checkGrid(x2, y2) & checkGrid(x3, y3);
        //debug.text = isValid.ToString() + " " + x0.ToString() + " " + y0.ToString()
        //    + " " + x1.ToString() + " " + y1.ToString()
        //    + " " + x2.ToString() + " " + y2.ToString()
        //    + " " + x3.ToString() + " " + y3.ToString();
                   //+ checkGrid(x0,y0).ToString() + checkGrid(x1,y1).ToString() + checkGrid(x2,y2).ToString() 
                   //+ checkGrid(x3, y3).ToString();

        if (isValid)
        {
            // Clear any active shadows before casting new ones
            clearShadows("red");
            // Cast blue shadow
            foreach (GameObject brick in BSh)
            {
                brick.SetActive(true);
            }
            BSh0.transform.position = new Vector3(x0 - gridwidth/2, groundY, y0 - gridheight/2);
            BSh1.transform.position = new Vector3(x1 - gridwidth/2, groundY, y1 - gridheight/2);
            BSh2.transform.position = new Vector3(x2 - gridwidth/2, groundY, y2 - gridheight/2);
            BSh3.transform.position = new Vector3(x3 - gridwidth/2, groundY, y3 - gridheight/2);
        } else
        {
            // Clear any active shadows before casting new ones
            clearShadows("blue");
            // Cast red shadow in valid positions if other positions are invalid
            if (checkGrid(x0,y0))
            {
                RSh0.SetActive(true);
                RSh0.transform.position = new Vector3(x0 - gridwidth / 2, groundY, y0 - gridheight / 2);
            }
            else
            {
                RSh0.SetActive(false);
            }
            if (checkGrid(x1, y1))
            {
                RSh1.SetActive(true);
                RSh1.transform.position = new Vector3(x1 - gridwidth / 2, groundY, y1 - gridheight / 2);
            }
            else
            {
                RSh1.SetActive(false);
            }
            if (checkGrid(x2, y2))
            {
                RSh2.SetActive(true);
                RSh2.transform.position = new Vector3(x2 - gridwidth / 2, groundY, y2 - gridheight / 2);
            }
            else
            {
                RSh2.SetActive(false);
            }
            if (checkGrid(x3, y3))
            {
                RSh3.SetActive(true);
                RSh3.transform.position = new Vector3(x3 - gridwidth / 2, groundY, y3 - gridheight / 2);
            }
            else
            {
                RSh3.SetActive(false);
            }

        }

        return isValid;
    }

    // Sets all blocks in a brick to true in the board matrix
    public void setBrick(int x0, int y0, int x1, int y1, int x2, int y2, int x3, int y3)
    {
        if (checkBrick(x0,y0,x1,y1,x2,y2,x3,y3))
        {
            board[x0, y0] = true;
            board[x1, y1] = true;
            board[x2, y2] = true;
            board[x3, y3] = true;
        }
    }

    // Use this for initialization
    void Start () {
        board = new bool[gridwidth, gridheight];

        for (int i = 0; i < gridwidth; i++)
        {
            for (int j = 0; j < gridheight; j++)
            {
                board[i,j] = false; // Set board to empty
            }
        }
        // Set up array for accessing shadow objects
        BSh = new GameObject[4];
        BSh[0] = BSh0; BSh[1] = BSh1; BSh[2] = BSh2; BSh[3] = BSh3;
        RSh = new GameObject[4];
        RSh[0] = RSh0; RSh[1] = RSh1; RSh[2] = RSh2; RSh[3] = RSh3;
        // Resize board based on width and height
        Ground.transform.localScale = new Vector3(gridwidth / 10.0f, groundY, gridheight / 10.0f);
        WestWall.transform.localScale = new Vector3(gridwidth, groundY, 1);
        EastWall.transform.localScale = new Vector3(gridwidth, groundY, 1);
        NorthWall.transform.localScale = new Vector3(1, groundY, gridheight + 2);
        SouthWall.transform.localScale = new Vector3(1, groundY, gridheight + 2);
        // Position the walls based on width and height of board
        WestWall.transform.localPosition = new Vector3(0, groundY -1, -gridwidth/2.0f - 0.5f);
        EastWall.transform.localPosition = new Vector3(0, groundY -1, gridwidth / 2.0f + 0.5f);
        NorthWall.transform.localPosition = new Vector3(-gridwidth / 2.0f - 0.5f, groundY - 1, 0);
        SouthWall.transform.localPosition = new Vector3(gridwidth / 2.0f + 0.5f, groundY - 1, 0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
