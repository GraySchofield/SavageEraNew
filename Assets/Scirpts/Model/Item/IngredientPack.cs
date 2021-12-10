using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class IngredientPack {
    public Food IngredientOne {
        get;
        set;
    }

    public Food IngredientTwo {
        get;
        set;
    }

    public Food IngredientThree {
        get;
        set;
    }


    public bool isEmpty() {
        if (IngredientOne == null && IngredientTwo == null && IngredientThree == null) {
            return true;
        } else {
            return false;
        }
    }

    public bool isFull()
    {
        if (IngredientOne != null && IngredientTwo != null && IngredientThree != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void clearPack() {
        IngredientOne = null;
        IngredientTwo = null;
        IngredientThree = null;
    }


    public bool ContainsFood(Food theFood)
    {
        if (IngredientOne == theFood
            || IngredientTwo == theFood
            || IngredientThree == theFood)
        {
            return true;
        }
        else
        {
            return false;
        }

    }


}
