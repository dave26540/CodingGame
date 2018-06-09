using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/

public class Cell
{
    public int x;
    public int y;
    public char c;
    public int lac = -1;
    public Boolean visited = false;
    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
public class Grille
{
    public Cell[,] data;
    public int L;
    public int H;
    public Grille(int L, int H)
    {
        data = new Cell[ L,H];
        this.L = L;
        this.H = H;
    }
    public void setVal(int x, int y, char c)
    {
        var cs = new Cell(x, y);
        this.data[x,y] = cs;
        cs.c = c;
    }
    public Cell getVal(int x, int y)
    {
        // Console.Error.WriteLine("X : " + x + ", Y: " + y);
        return this.data[x,y];
    }

    public Cell getTop(Cell c)
    {
        return c.y > 0 ? getVal(c.x, c.y - 1) : null;
    }
    public Cell getBottom(Cell c)
    {
        return c.y < H - 1 ? getVal(c.x, c.y + 1) : null;
    }
    public Cell getLeft(Cell c)
    {
        return c.x > 0 ? getVal(c.x - 1, c.y) : null;
    }
    public Cell getRight(Cell c)
    {
        return c.x < L - 1 ? getVal(c.x + 1, c.y) : null;
    }
    public int findLac(int x, int y)
    {
        Cell c = getVal(x, y);
        if (c.lac > -1)
        {
            return c.lac;
        }
        if (c.c == '#')
        {
            return 0;
        }

        Stack<Cell> p = new Stack<Cell>();
        Stack<Cell> l = new Stack<Cell>();
        p.Push(c);

        while (p.Count() > 0)
        {
            var current = p.Pop();
       //     Console.Error.WriteLine("CURRENT :" +current.x + " " + current.y);
            //     Console.WriteLine("current : x : " + current.x + ",y: " + current.y + ", val : " + current.c);
            if (current.c == 'O')
            {
                // Console.WriteLine(current.c);
                if (current.visited ==false)
                {
                    //Console.WriteLine("hoho");
                    l.Push(current);
                    current.visited = true;
                }


                var left = getLeft(current);
                var right = getRight(current);
                var bottom = getBottom(current);
                var top = getTop(current);
                if ( left !=null && left.c=='O' && left.visited == false)
                {
                    p.Push(left);
                }
                if ( right!=null && right.c=='O' && right.visited == false)
                {
                    p.Push(right);
                }
                if ( top!=null && top.c=='O' && top.visited == false)
                {
                    p.Push(top);
                }
                if ( bottom!=null && bottom.c=='O' && bottom.visited == false)
                {
                    p.Push(bottom);
                }
            }
        }
        var co = l.Count();
        foreach (Cell ce in l)
        {
            ce.lac = co;
        }
        return co;
    }



    public void display()
    {
        string s = "";
        for (int y = 0; y < this.H; y++)
        {
            for (int x = 0; x < this.L; x++)
            {
                s += getVal(x, y).c;
            }
            s += "\n";
        }
        Console.Error.WriteLine(s);
    }

}

class Solution
{
  


    static void Main(string[] args)
    {
        int L = int.Parse(Console.ReadLine());
        int H = int.Parse(Console.ReadLine());

        Grille g = new Grille(L, H);

        for (int y = 0; y < H; y++)
        {
            string row = Console.ReadLine();
            var len = row.Length;
            for(int x=0; x < len; x++)
            {
                g.setVal(x, y, row[x]);
            }
        }


      //  g.display();

        int N = int.Parse(Console.ReadLine());
        for (int i = 0; i < N; i++)
        {
            string[] inputs = Console.ReadLine().Split(' ');
            int X = int.Parse(inputs[0]);
            int Y = int.Parse(inputs[1]);
            // Console.Error.WriteLine("x : " + X + ", Y: " + Y + " " + g.getVal(X, Y).c);
          //  Console.WriteLine(i);
           Console.WriteLine(g.findLac(X, Y));
        }
        /*for (int i = 0; i < N; i++)
        {

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            Console.WriteLine("answer");
        }*/
    }
}