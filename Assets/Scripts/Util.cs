using System.Collections.Generic;

public class Util
{

    public static float[] cumulativeDensity(IReadOnlyList<float> weights)
    {
        var cdf = new float[weights.Count];
        var prev = 0f;
        for (var i = 0; i < weights.Count; i++)
        {
            prev += weights[i];
            cdf[i] = prev;
        }

        return cdf;
    }
    
    private static T binaryFindSelectedValue<T>(IReadOnlyList<T> values, IReadOnlyList<float> cdf, int lower, int upper, float selection)
    {
        var mid = (lower + upper) / 2;

        float lowerEdge;
        if (mid == 0)
        {
            lowerEdge = 0f;
        }
        else
        {
            lowerEdge = cdf[mid - 1];
        }
        var upperEdge = cdf[mid];

        if (selection < lowerEdge)
        {
            return binaryFindSelectedValue(values, cdf, lower, mid, selection);
        }

        if (selection >= upperEdge)
        {
            return binaryFindSelectedValue(values, cdf, mid, upper, selection);
        }
        
        return values[mid];
    }

    public static T chooseWeighted<T>(IReadOnlyList<float> weights, IReadOnlyList<T> values, float selection)
    {
        var cdf = cumulativeDensity(weights);
        var sum = cdf[cdf.Length - 1];


        return binaryFindSelectedValue(values, cdf, 0, values.Count, selection * sum);
    }
}
