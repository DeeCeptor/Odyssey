﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class PotentialUnit
{
    public Vector2 position;
    public string unit_name;
    public string faction_name;
}

public class LevelFileParser
{
    public int x_dimension, y_dimension;
    public List<string> hex_types;
    public List<PotentialUnit> units_to_be_spawned;


    public LevelFileParser()
    {

    }


    public void ReadInLevel(string path_to_file)
    {
        hex_types = new List<string>();
        units_to_be_spawned = new List<PotentialUnit>();

        try
        {   // Open the text file using a stream reader.
            using (StreamReader file = new StreamReader(path_to_file))
            {
                // Read the stream to a string, and write the string to the console.
                //string line = file.ReadToEnd();
                string line;

                // First line is the header, which contains the dimensions of the map
                string header_information = file.ReadLine();
                string[] dims = header_information.Split(' ');
                int.TryParse(dims[0], out x_dimension);
                int.TryParse(dims[1], out y_dimension);

                // TILES
                // Read in hex terrain information and deployment points and retreat points
                while ((line = file.ReadLine()) != null)
                {
                    // We're done reading in tiles if we encounter an open bracket
                    if (line.Contains("{"))
                        break;

                    // Process the line, which is delineated by tabs
                    string[] delineated_words = line.Split(new char[] { '\t' } , StringSplitOptions.RemoveEmptyEntries);

                    foreach (String s in delineated_words)
                    {
                        hex_types.Add(GetHexType(s));
                    }
                }


                // UNITS
                // Read in what units to spawn, what faction they belong to and where they spawn
                while ((line = file.ReadLine()) != null)
                {
                    // Closed curly brackets means we're done reading in the file
                    if (line.Contains("}"))
                        break;

                    string[] delineated_words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    PotentialUnit unit = new PotentialUnit();

                    // First word is the faction
                    unit.faction_name = delineated_words[0];

                    // Second word is the name of the unit
                    unit.unit_name = delineated_words[1];

                    // Third word is the starting coordinates
                    // Two integers separated by a comma
                    string[] coords = delineated_words[2].Split(',');
                    unit.position = new Vector2(int.Parse(coords[0]), int.Parse(coords[1]));

                    units_to_be_spawned.Add(unit);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("not be read " + e.Message);
        }
    }


    public string GetHexType(string word_input)
    {
        switch (word_input)
        {
            case ("N"):
                return "Hex";
            case ("F"):
                return "ForestHex";
            case ("R"):
                return "RuinsHex";
            case ("S"):
                return "SwampHex";
            case ("W"):
                return "WaterHex";
            case ("H"):
                return "HillHex";
        }

        Debug.Log("No hex type found. Returning Hex");
        return "Hex";
    }
}
