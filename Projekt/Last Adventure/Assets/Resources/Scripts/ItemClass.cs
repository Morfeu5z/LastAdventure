using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClass {

    public string name { get; set; }
    public string use { get; set; }
    public ItemClass(string ItemName, string Use){
        name = ItemName;
        use = Use;
    }
}