using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace memorygame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int cols = 5;
        private const int rows = 4;
        MemoryGrid grid;

        public MainWindow(string namePlayer1, string namePlayer2, string Thema, int doorgaan)
        {
            InitializeComponent();
            grid = new MemoryGrid(GameGrid, cols, rows, Thema, doorgaan);
            Closing += new CancelEventHandler(MainWindow_Closing);
            grid.SetPlayer1Name(namePlayer1);
            grid.SetPlayer2Name(namePlayer2);
            
        }

        public MainWindow(string Thema, int doorgaan)
        {
            InitializeComponent();
            grid = new MemoryGrid(GameGrid, cols, rows, Thema, doorgaan);
            Closing += new CancelEventHandler(MainWindow_Closing);
            
        }

        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            Save();
        }

        public void Save()
        {
            string score = grid.GetScore() + Environment.NewLine;
            string score1 = grid.GetScore1() + Environment.NewLine;
            string turn = grid.GetTurnCount() + Environment.NewLine;
            string namePlayer1 = grid.GetCurrentNamePlayer1() + Environment.NewLine;
            string namePlayer2 = grid.GetCurrentNamePlayer2() + Environment.NewLine;
            string themaInUse = grid.GetCurrentTheme() + Environment.NewLine;
            List<int> cardOrder = grid.GetCardOrder();
            List<int> solvedRows = grid.GetSolvedRows();
            List<int> solvedCols = grid.GetSolvedCols();
            int matches = grid.GetMatches();

            File.WriteAllText("memory.sav", score);
            File.AppendAllText("memory.sav", score1);
            File.AppendAllText("memory.sav", turn);
            File.AppendAllText("memory.sav", namePlayer1);
            File.AppendAllText("memory.sav", namePlayer2);
            File.AppendAllText("memory.sav", themaInUse);
            foreach (int s in cardOrder)
            {
                string e = s.ToString() + Environment.NewLine;
                File.AppendAllText("memory.sav", e);
            }
                foreach (int o in solvedRows)
                {
                    string c = o.ToString() + Environment.NewLine;
                    File.AppendAllText("memory.sav", c);
                    int d = solvedCols.First();
                    string g = d.ToString() + Environment.NewLine;
                    File.AppendAllText("memory.sav", g);
                    solvedCols.RemoveAt(0);
                }
        }
    }
    
}
