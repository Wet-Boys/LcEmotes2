namespace LcEmotes2AndKnucklesFeaturingDante.Utils;

internal static class ArrayUtils
{
    public static T[] Copy<T>(this T[] orig)
    {
        var newArray = new T[orig.Length];

        for (int i = 0; i < newArray.Length; i++)
            newArray[i] = orig[i];

        return newArray;
    }
}