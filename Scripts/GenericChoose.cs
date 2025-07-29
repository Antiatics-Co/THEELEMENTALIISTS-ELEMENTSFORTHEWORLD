using System;

/*
 * Title: Generic Chooser
 * Author: Aidan Cox
 * Version: 1 - July 28, 2025
 * Description:
 *  Takes in a generic array, its choose function selects a random index of the array and returns that value
 */

public class GenericChoose<T>
{
    private T[] Options { get; set; }
    public GenericChoose(T[] opt) => Options = opt; // Constructor to initialize the options

    public T Choose()
    {
        Random random = new Random();
        int index = random.Next(Options.Length);
        return Options[index];
    }
}
