using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryItem 
{
    string GetItemName();
    void Use(); // M�todo gen�rico para usar o item
}
