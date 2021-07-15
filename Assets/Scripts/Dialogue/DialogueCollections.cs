using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueCollections
{
    private static DialogueCollections instance;
    public static DialogueCollections Instance              //Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = new DialogueCollections();
            }
            return instance;
        }
    }

    public List<string> topic_Weather;
    public List<string> topic_MyShop;


    private DialogueCollections()                          //constructor
    {
        Initialze();
    }           

    void Initialze()
    {
        topic_Weather.Add("I: Today is weather is so hot, I wish I can become a fish and" +
                          " soap the deep sea forever.");
        topic_Weather.Add("Bob: Well you can never become a fish, but at least your brain " +
                          "is on par with a fish");


        topic_MyShop.Add("I: My new restaurant is opened. Lamb is fine-cocked, " +
                        "and free for seven days.");
        topic_MyShop.Add("Bob: I am not quite a free luch person, but i am in" +
                        " favour of fine lamb");

    }





}
