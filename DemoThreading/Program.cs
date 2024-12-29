namespace DemoThreading;

class Data
{
    public int Id { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
    public int Result { get; set; }
}

/// <summary>
/// Data Parallelism
/// Partition source of data
/// </summary>
internal class Program
{
    // Manual number of threads management
    private static readonly int NumberThreads = 4;

    static readonly List<int> Numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13];
    static void CalculateNumbers(object? state)
    {
        Data? data = state as Data;
        if (data is not null)
        {
            Console.WriteLine($"Id: {data.Id}");

            for (int i = data.Start; i < data.End; i++)
            {
                data.Result += Numbers[i];
            }
        }
    }

    static void Main(string[] args)
    {
        List<Data> datas = [];
        List<Thread> threads = [];

        var range = Numbers.Count / NumberThreads;

        if (Numbers.Count < NumberThreads)
        {
            Console.WriteLine(Numbers.Sum(n => n));
        } 
        else
        {
            for (int i = 0; i < NumberThreads; i++)
            {
                var start = i * range;
                var end = start + range;

                datas.Add(new Data
                {
                    Id = i + 1,
                    Start = start,
                    End = (i + 1 == NumberThreads) ? Numbers.Count : end
                });
            }

            foreach (Data data in datas)
                threads.Add(new Thread(() => CalculateNumbers(data)));

            foreach (var t in threads)
                t.Start();

            foreach (var t in threads)
                t.Join();

            var sum = datas.Sum(d => d.Result);
            Console.WriteLine(sum);
        }
    }
}
