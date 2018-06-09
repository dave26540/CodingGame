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

public class Heap<T> where T : IComparable<T>
{
    private List<T> elements;
    public int Count()
    {
        return this.elements.Count();
    }
    public Boolean contains(T n)
    {
        return this.elements.Contains(n);
    }
    public Heap()
    {
        elements = new List<T>();
    }
    public int GetParent(int index)
    {
        return (index - 1) / 2;
    }
    public int GetLeft(int index)
    {
        return (2 * index) + 1;
    }
    public int GetRight(int index)
    {
        return (2 * index) + 2;
    }
    public T pop()
    {
        T elt = elements[0];
        elements[0] = elements[elements.Count - 1];
        elements.RemoveAt(elements.Count - 1);
        HeapifyDown(0);
        return elt;
    }

    public void insert(T i)
    {
        elements.Add(i);
        HeapifyUp(elements.Count - 1);
    }
    public void HeapifyDown(int index)
    {
        var smallest = index;
        var left = GetLeft(index);
        var right = GetRight(index);
        if (left < elements.Count && elements[left].CompareTo(elements[index]) < 0)
        {
            smallest = left;
        }
        if (right < elements.Count && elements[right].CompareTo(elements[index]) < 0)
        {
            smallest = right;
        }
        if (smallest != index)
        {
            var tmp = elements[index];
            elements[index] = elements[smallest];
            elements[smallest] = tmp;
            HeapifyDown(smallest);
        }
    }
    public void HeapifyUp(int index)
    {
        int parent = GetParent(index);
        if (parent >= 0 && elements[index].CompareTo(elements[parent]) < 0)
        {
            var tmp = elements[index];
            elements[index] = elements[parent];
            elements[parent] = tmp;
            HeapifyUp(parent);
        }
    }
}

public class Position
{
    public int x;
    public int y;
    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

}
public class Noeud : IComparable<Noeud>
{
    public int g = int.MaxValue;
    public int f;
    public char valeur;
    public Noeud parent;
    public int x;
    public int y;
    public Boolean isVisited = false;
    public Noeud(int x, int y, char valeur)
    {
        this.valeur = valeur;
        this.x = x;
        this.y = y;
    }
    public int CompareTo(Noeud that)
    {
        return this.f.CompareTo(that.f);
    }
}
public class Maze
{
    public int nbRow;
    public int nbCol;
    public Noeud[,] data;
    public int KX;
    public int KY;
    public static int heuristic(Noeud n1, Noeud n2)
    {
        var d1 = Math.Abs(n1.x - n2.x);
        var d2 = Math.Abs(n1.y - n2.y);
         return d1 + d2;
    }

    public void reinitParent()
    {
        for (int y = 0; y < nbRow; y++)
        {
            for (int x = 0; x < nbCol; x++)
            {
                getNoeud(x, y).parent = null;
                getNoeud(x, y).g = int.MaxValue;
            }
        }
    }
    public static List<Noeud> cheminLePlusCourt(Maze m, Noeud depart, Noeud fin,Boolean esquive = false)
    {
        m.reinitParent();
        List<Noeud> closestList = new List<Noeud>();
        Heap<Noeud> openList = new Heap<Noeud>();
        depart.f = heuristic(depart, fin);
        depart.g = 0;
        openList.insert(depart);

        while (openList.Count() > 0)
        {
            Noeud currentNode = openList.pop();


            closestList.Add(currentNode);
           // Console.WriteLine(currentNode.x + " " + currentNode.y);


            //Console.WriteLine("Pop : " + currentNode.x + " " + currentNode.y);
            if (currentNode == fin)
            {
                Noeud find = fin;
                List<Noeud> retour = new List<Noeud>();
                retour.Add(fin);
                while (find.parent != null)
                {
                    retour.Add(find.parent);
                    find = find.parent;
                }
                retour.Reverse();
                return retour;
            }
          
            foreach (Noeud n in m.voisins(currentNode))
            {
                if (n.valeur == 'C' && esquive)
                {
                    continue;
                }

                if (closestList.Contains(n))
                {
                    continue;
                }
           

                var gScore = currentNode.g + 1;

                if ( gScore >= n.g)
                {
                    continue;
                }

                n.parent = currentNode;
                n.g = gScore;
                n.f = n.g + heuristic(n, fin);
                if (!openList.contains(n))
                {
                    openList.insert(n);
                }
            }
        }
        return null;
    }

    public void setPlayerPosition(int KX,int KY)
    {
        this.KX = KX;
        this.KY = KY;
    }
    public Maze(int nbRow, int nbCol)
    {
        this.nbRow = nbRow;
        this.nbCol = nbCol;
      
        data = new Noeud[nbRow, nbCol];
        init();
    }
    public Noeud getNoeudPlayer()
    {
        return getNoeud(this.KX, this.KY);
    }

    public Noeud getNoeudLePlusProcheNonDiscoveredAndAccessible(Boolean esquive = false)
    {
        var n = getNoeudPlayer();
      
        List<Noeud> notDiscovered = new List<Noeud>();
        for (int y = 0; y < nbRow; y++)
        {
            for (int x = 0; x < nbCol; x++)
            {
                if (getCell(x, y) == '.' && !getNoeud(x,y).isVisited)
                {
                    notDiscovered.Add(getNoeud(x, y));
                }
            }
        }
        notDiscovered = notDiscovered.OrderBy(x => heuristic(n, x)).ToList();
      //  Console.WriteLine(notDiscovered.Count());
        //On peut les atteindre ???

        Noeud dest = null;
        foreach( Noeud n1 in notDiscovered)
        {
           Console.Error.WriteLine("Player : x:" + n.x + " y:" + n.y);
           Console.Error.WriteLine("N1 :" + n1.x + ", " + n1.y);
            List<Noeud> nfind = Maze.cheminLePlusCourt(this, n, n1,esquive);
            if ( nfind != null)
            {
          //      Console.WriteLine("Player : x:" + n.x + " y:" + n.y);
           //     Console.WriteLine("Dest : X : " + n1.x + ",Y:" + n1.y);
            //    Console.WriteLine("Find[0]: " + nfind[0].x + " " + nfind[0].y);
             //   Console.WriteLine("Find[1]: " +nfind[1].x +" " + nfind[1].y );
                dest = nfind[1];
                break;
            }
        }
        return dest;
    }

    public void init()
    {
        for (int y = 0; y < nbRow; y++)
        {
            for (int x = 0; x < nbCol; x++)
            {
                data[y, x] = new Noeud(x, y, '#');
            }
        }
    }


    public void addRow(int rowIndex, string row)
    {
        for (int i = 0; i < row.Length; i++)
        {
//            data[rowIndex, i] = new Noeud(i, rowIndex, row[i]);

            getNoeud(i, rowIndex).valeur = row[i];
        }
    }
    public Boolean CanGoRight(Noeud n)
    {
        return isCTrans(getCharRight(n.x, n.y));
    }
    public Boolean CanGoLeft(Noeud n)
    {
        return isCTrans(getCharLeft(n.x, n.y));
    }
    public Boolean CanGoBottom(Noeud n)
    {
        return isCTrans(getCharBottom(n.x, n.y));
    }
    public Boolean CanGoUp(Noeud n)
    {
        return isCTrans(getCharUp(n.x, n.y));
    }
    public string dir(Noeud n1 , Noeud n2)
    {
        if ( n1.x > n2.x)
        {
            return "LEFT";
        }else if ( n1.x < n2.x)
        {
            return "RIGHT";
        }else if ( n1.y < n2.y)
        {
            return "DOWN";
        }else if(n1.y > n2.y)
        {
            return "UP";
        }
        return "";
    }


    public List<Noeud> voisins(Noeud n)
    {
        List<Noeud> retour = new List<Noeud>();
        if (CanGoRight(n))
        {
            retour.Add(getNoeudRight(n));
        }
        if (CanGoLeft(n))
        {
            retour.Add(getNoeudLeft(n));
        }
        if (CanGoBottom(n))
        {
            retour.Add(getNoeudBottom(n));
        }
        if (CanGoUp(n))
        {
            retour.Add(getNoeudUp(n));
        }
        return retour;
    }
    public Boolean isCTrans(char c)
    {
        return c == 'T' || c == 'C' || c == '.';
    }
    public char getCharBottom(int x, int y)
    {
        int ny = (y < nbRow - 1 ? y + 1 : -1);
        return (ny > -1 ? getCell(x, ny) : '#');
    }
    public char getCharUp(int x, int y)
    {
        int ny = (y > 0 ? y - 1 : -1);
        return ny > -1 ? getCell(x, ny) : '#';
    }
    public char getCharLeft(int x, int y)
    {
        int nx = (x > 0 ? x - 1 : -1);
        return nx > -1 ? getCell(nx, y) : '#';
    }
    public char getCharRight(int x, int y)
    {
        int nx = x < nbCol - 1 ? x + 1 : -1;
        return nx > -1 ? getCell(nx, y) : '#';
    }

    public Noeud getNoeudBottom(Noeud n)
    {
        int ny = (n.y < nbRow - 1 ? n.y + 1 : -1);
        return (ny > -1 ? getNoeud(n.x, ny) : null);
    }
    public Noeud getNoeudUp(Noeud n)
    {
        int ny = (n.y > 0 ? n.y - 1 : -1);
        return ny > -1 ? getNoeud(n.x, ny) : null;
    }
    public Noeud getNoeudLeft(Noeud n)
    {
        int nx = (n.x > 0 ? n.x - 1 : -1);
        return nx > -1 ? getNoeud(nx, n.y) : null;
    }
    public Noeud getNoeudRight(Noeud n)
    {
        int nx = n.x < nbCol - 1 ? n.x + 1 : -1;
        return nx > -1 ? getNoeud(nx, n.y) : null;
    }





    public char getCell(int x, int y)
    {
        return data[y, x].valeur;
    }
    public void setCell(int x,int y, char c)
    {
        data[y, x].valeur = c;
    }
    public Noeud getNoeud(int x, int y)
    {
        return data[y, x];
    }
    public Noeud getNoeudSalleCommande()
    {
        for (int y = 0; y < nbRow; y++)
        {
            for (int x = 0; x < nbCol; x++)
            {
                if (getCell(x, y) == 'C')
                {
                    return getNoeud(x, y);
                }
            }
        }
        return null;
    }

    public Noeud getNoeudInitial()
    {
        for (int y = 0; y < nbRow; y++)
        {
            for (int x = 0; x < nbCol; x++)
            {
                if (getCell(x, y) == 'T')
                {
                    return getNoeud(x, y);
                }
            }
        }
        return null;
    }
    public override string ToString()
    {
        string s = "KX:" + this.KX + ", KY: " + this.KY + "\n";

        Noeud p = this.getNoeudSalleCommande();
        if (p != null)
        {
            s += "Position de la salle de controle : X" + p.x + ", Y: " + p.y + "\n";
        }
        for (int y = 0; y < nbRow; y++)
        {
            for (int x = 0; x < nbCol; x++)
            {
                //Console.Error.WriteLine(x + " " + y + "=" + data[y].Length + "="+ nbCol);
                s += getCell(x, y);
            }
            s += "\n";
        }
        return s;
    }
}
class Player
{
    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int R = int.Parse(inputs[0]); // number of rows.
        int C = int.Parse(inputs[1]); // number of columns.
        int A = int.Parse(inputs[2]); // number of rounds between the time the alarm countdown is activated and the time the alarm goes off.
        var isCommandeTouch = false;




        Maze m = new Maze(R, C);

        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int KR = int.Parse(inputs[0]); // row where Kirk is located.
            int KC = int.Parse(inputs[1]); // column where Kirk is located.
             m.setPlayerPosition(KC, KR);
            for (int i = 0; i < R; i++)
            {
                string ROW = Console.ReadLine(); // C of the characters in '#.TC?' (i.e. one line of the ASCII maze).
                m.addRow(i,ROW);
            }


            m.getNoeud(KC, KR).isVisited = true;
            Console.Error.WriteLine(m.ToString());

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");
            var np = m.getNoeudPlayer();


            if (m.getNoeudSalleCommande() != null)
            {

                //Calcule de la distance actuel 
             
                if (m.getNoeudPlayer() == m.getNoeudSalleCommande())
                {
                    isCommandeTouch = true;
                }

                if (!isCommandeTouch)
                {

                    List<Noeud> n = Maze.cheminLePlusCourt(m, m.getNoeudSalleCommande(), m.getNoeudInitial());
                    if (n != null)
                    {
                        Console.Error.WriteLine("Chemin commande - initial : " + n.Count());
                        Console.Error.WriteLine("A:" + A);
                    }
                    if (n != null && A >=  n.Count()-1)
                    {
                        List<Noeud> n2 = Maze.cheminLePlusCourt(m, m.getNoeudPlayer(), m.getNoeudSalleCommande());
                        var n1 = n2[1];
                        Console.WriteLine(m.dir(m.getNoeudPlayer(), n1));
                    }
                    else
                    {
                        Noeud dest = m.getNoeudLePlusProcheNonDiscoveredAndAccessible(true);
                        Console.WriteLine(m.dir(m.getNoeudPlayer(), dest));
                    }
                }
                else
                {
                    List<Noeud> n = Maze.cheminLePlusCourt(m, m.getNoeudPlayer(), m.getNoeudInitial());
                    var n1 = n[1];
                    Console.Error.WriteLine("Dir : " + n[1].x + " " + n[1].y);
                    Console.Error.WriteLine("Pos: " + m.getNoeudPlayer().x);
                    Console.WriteLine(m.dir(m.getNoeudPlayer(), n1));
                }
            }
            else
            {
                //on cherche une case non connue la plus proche
                Noeud dest = m.getNoeudLePlusProcheNonDiscoveredAndAccessible();
              Console.Error.WriteLine(dest.x + " " + dest.y);
                Console.WriteLine(m.dir(m.getNoeudPlayer(), dest));
            }
        }
    }
}