//Aluno Alison Civiero 451219
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Pilha
{
    public class HanoiTower
    {
        public int DiscsCount { get; private set; }
        public int MovesCount { get; private set; }
        public Stack<int> From { get; private set; }
        public Stack<int> To { get; private set; }
        public Stack<int> Auxiliary { get; private set; }
        public event EventHandler<EventArgs> MoveCompleted;

        public HanoiTower(int discs)
        {
            DiscsCount = discs;
            From = new Stack<int>();
            To = new Stack<int>();
            Auxiliary = new Stack<int>();

            for (int i = 1; i <= discs; i++)
            {
                int size = discs - i + 1;
                From.Push(size);
            }
        }

        public void Start()
        {
            Move(DiscsCount, From, To, Auxiliary);
        }

        private void Move(int discs, Stack<int> from, Stack<int> to, Stack<int> auxiliary)
        {
            if (discs > 0)
            {
                Move(discs - 1, from, auxiliary, to);
                to.Push(from.Pop());
                MovesCount++;
                MoveCompleted?.Invoke(this, EventArgs.Empty);
                Move(discs - 1, auxiliary, to, from);
            }
        }
    }

    private const int DISCS_COUNT = 5;
    private const int DELAY_MS = 250;
    private static int _columnSize = 30;

    static void Main(string[] args)
    {
        _columnSize = Math.Max(6, GetDiscWidth(DISCS_COUNT) + 2);
        HanoiTower algorithm = new HanoiTower(DISCS_COUNT);
        algorithm.MoveCompleted += Algorithm_Visualize;
        Algorithm_Visualize(algorithm, EventArgs.Empty);
        algorithm.Start();
    }

    private static void Algorithm_Visualize(object sender, EventArgs e)
    {
        Console.Clear();
        HanoiTower algorithm = (HanoiTower)sender;

        if (algorithm.DiscsCount <= 0)
            return;

        char[][] visualization = InitializeVisualization(algorithm);
        PrepareColumn(visualization, 1, algorithm.DiscsCount, algorithm.From);
        PrepareColumn(visualization, 2, algorithm.DiscsCount, algorithm.To);
        PrepareColumn(visualization, 3, algorithm.DiscsCount, algorithm.Auxiliary);

        Console.WriteLine(Center("FROM") + Center("TO") + Center("AUXILIARY"));
        DrawVisualization(visualization);

        Console.WriteLine();
        Console.WriteLine($"Number of moves: {algorithm.MovesCount}");
        Console.WriteLine($"Number of discs: {algorithm.DiscsCount}");

        Thread.Sleep(DELAY_MS);
    }

    private static char[][] InitializeVisualization(HanoiTower algorithm)
    {
        char[][] visualization = new char[algorithm.DiscsCount][];
        for (int y = 0; y < visualization.Length; y++)
        {
            visualization[y] = new string(' ', _columnSize * 3).ToCharArray();
        }
        return visualization;
    }

    private static void PrepareColumn(char[][] visualization, int column, int discsCount, Stack<int> stack)
    {
        int margin = _columnSize * (column - 1);
        int[] discs = stack.ToArray(); 

        for (int y = 0; y < discs.Length; y++)
        {
            int size = discs[y];
            int row = discsCount - (discs.Length - y);
            int columnStart = margin + discsCount - size;
            int columnEnd = columnStart + GetDiscWidth(size) - 1;

            for (int x = columnStart; x <= columnEnd; x++)
            {
                visualization[row][x] = '=';
            }
        }
    }

    private static void DrawVisualization(char[][] visualization)
    {
        foreach (var row in visualization)
        {
            Console.WriteLine(row);
        }
    }

    private static string Center(string text)
    {
        int margin = (_columnSize - text.Length) / 2;
        return text.PadLeft(margin + text.Length).PadRight(_columnSize);
    }

    private static int GetDiscWidth(int size)
    {
        return 2 * size - 1;
    }
}

