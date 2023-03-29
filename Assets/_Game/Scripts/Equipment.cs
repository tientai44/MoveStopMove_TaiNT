using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Equipment 
{
    private int id;
    private string name;
    private int price;
    private string wingName;
    private string headName;
    private string tailName;
    private string shieldName;
    public Equipment(int id, string name, int price)
    {
        this.id = id;
        this.name = name;
        this.price = price;
    }

    public Equipment(int id, string name, int price, string wingName, string headName, string tailName, string shieldName) : this(id, name, price)
    {
        this.wingName = wingName;
        this.headName = headName;
        this.tailName = tailName;
        this.shieldName = shieldName;
    }

    public int Id { get => id; set => id = value; }
    public string Name { get => name; set => name = value; }
    public int Price { get => price; set => price = value; }
}
