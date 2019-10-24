using System.Collections.Generic;

public static class ArrayExt{

    public static void Replace<T>(this IList<T> arr, T arg, T with){
        for(int i = 0; i < arr.Count; i++)
            if(arg.Equals(arr[i])) arr[i] = with;
    }

}
