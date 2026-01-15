using System.Runtime.InteropServices;
using System.Text.Json;

namespace PopLoader;

public class Tracker
{
    public Stream Stream;
    private Dictionary<string, int> IDFromString;
    private Dictionary<int, string> StringFromID;

    public Dictionary<int, List<Interval<int>>> IntervalsFromID;
    public SortedDictionary<int, int> Change;
    // Index -> Usage start, Usage end  
    public SortedDictionary<int, List<int>> UsageStarted;
    public SortedDictionary<int, List<int>> UsageRemoved;
    

    public Tracker(Stream stream)
    {
        IntervalsFromID = [];
        Change = [];
    }

    /// <summary>
    /// Mark this section is used by.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="usage"></param>
    public void Map(int start, int end, string usage)
    {

    }
    public void GetUnused()
    {
        foreach (var item in Change)
        {
            
        }
    }
}
public struct Interval<T> where T : unmanaged
{
    public T start;
    public T end;
}