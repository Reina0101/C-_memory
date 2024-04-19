using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.IO;

namespace memorygame
{
    public class MemoryGrid
    {
        //ATTRIBUTEN

        private Grid grid;// een grid
        private const int cols = 4;
        private const int rows = 4;
        private int doorgaan = 0;

        //lijst met kaartjes 1 t/m 8 x2
        private List<int> cards = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };
        //lijst met kaartjes 1 t/m 8 x2 voor reset functie
        private List<int> newCards = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };
        //lijst met kaartjes die nu omgedraaid zijn
        private List<double> openCards = new List<double>();
        //lijst met kaartjes die al eens gezien zijn
        private List<Image> seenCards = new List<Image>();
        //lijst met kaartjes die opgelost zijn
        private List<Image> solvedCards = new List<Image>();
        //lijst met volgorde waarin de kaarten geplaatst worden
        private List<int> cardOrder = new List<int>();
        private List<bool> flipped = new List<bool>() { false, false, false, false, false, false, false, false,
                                                        false, false, false, false, false, false, false, false, };
        private List<int> flippedRows = new List<int>();
        private List<int> flippedCols = new List<int>();
        private List<int> solvedRows = new List<int>();
        private List<int> solvedCols = new List<int>();
        private List<int> clickRow = new List<int>();
        private List<int> clickColumn = new List<int>();
        private int score = Convert.ToInt32(File.ReadLines("memory.sav").Skip(0).Take(1).First());//score speler 1
        private int score1 = Convert.ToInt32(File.ReadLines("memory.sav").Skip(1).Take(1).First());//score speler 2
        private int turnCount = Convert.ToInt32(File.ReadLines("memory.sav").Skip(2).Take(1).First());
        private int matches;
        private Label scoreboard = new Label();//scorebord speler 1
        private Label scoreboard1 = new Label();//scorebord speler 2
        private Button resetBtn = new Button();//resetknop
        private string PlayerName1 = File.ReadLines("memory.sav").Skip(3).Take(1).First();
        private string PlayerName2 = File.ReadLines("memory.sav").Skip(4).Take(1).First();
        private string Thema = "";
        private int Streak1;
        private int Streak2;
        private string Winnaar;
        private int WinScore;

        //CONSTRUCTORS
        /// <summary>
        /// MemoryGrid bestaat uit een grid met rijen en kolommen, met daarin: Images en Labels
        /// </summary>
        /// <param name="grid">de grid</param>
        /// <param name="cols">kolommen</param>
        /// <param name="rows">rijen</param>
        public MemoryGrid(Grid grid, int cols, int rows, string thema, int doorgaan)
        {
            this.grid = grid;
            InitializeGameGrid(cols, rows);
            SetThemeName(thema);
            SetDoorgaan(doorgaan);

            AddCards();
            AddScoreboard();
            AddScoreboard1();
            AddResetBtn();
        }

        //METHODEN
        //SPEELBORD
        /// <summary>
        /// maakt een speelbord aan met aantal kolommen en rijen
        /// </summary>
        /// <param name="cols">het aantal kolommen</param>
        /// <param name="rows">het aantal rijen</param>
        private void InitializeGameGrid(int cols, int rows)
        {
            for (int i = 0; i < rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < cols; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        //KAARTEN ACHTERKANT
        /// <summary>
        /// voegt images toe, klikbaar
        /// </summary>
        private void AddCards()
        {
            List<ImageSource> images = GetImagesList();
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < cols; column++)
                {
                    Image backgroundImage = new Image();
                    string Urilocation = "Resources/" + Thema + "/Back/DC_Comics_logo.png";
                    backgroundImage.Source = new BitmapImage(new Uri(Urilocation, UriKind.Relative));
                    backgroundImage.Tag = images.First();
                    images.RemoveAt(0);
                    backgroundImage.MouseDown += new MouseButtonEventHandler(CardClick);
                    backgroundImage.Loaded += new RoutedEventHandler(ClickCard);
                    Grid.SetColumn(backgroundImage, column);
                    Grid.SetRow(backgroundImage, row);
                    grid.Children.Add(backgroundImage);

                }
            }
        }
        private void ClickCard(object sender, RoutedEventArgs e)
        {
            if (doorgaan == 1)
            {


                int lineCount = File.ReadLines("memory.sav").Count();
                int lineNumber = lineCount - 22;

                for (int i = 0; i < lineNumber; i += 2)
                {



                    int aa = Convert.ToInt32(File.ReadLines("memory.sav").Skip(22 + i).Take(1).First());
                    int bb = Convert.ToInt32(File.ReadLines("memory.sav").Skip(23 + i).Take(1).First());
                    int p1 = 0;
                    int cc = aa + bb;
                    switch (aa)
                    {
                        case (0):
                            p1 = cc;
                            break;
                        case (1):
                            p1 = cc + 3;
                            break;
                        case (2):
                            p1 = cc + 6;
                            break;
                        case (3):
                            p1 = cc + 9;
                            break;


                    }


                    grid.Children[p1].Visibility = Visibility.Hidden;

                }
            }
        }

            //KAARTEN VOORKANT
            /// <summary>
            /// plaatst images in een willekeurige volgorde
            /// </summary>
            /// <returns>return een lijst met images</returns>
            private List<ImageSource> GetImagesList()
        {
            List<ImageSource> images = new List<ImageSource>();

            // rnd geeft een willekeurig getal terug
            Random rnd = new Random();
            if (doorgaan == 0)
            {
                for (int i = 0; i < 16; i++)
                {
                    //index is gelijk aan rnd{willekeurig getal} die Next{niet negatief} is 
                    //en lager dan (cards.count){hoeveel items erin de lijst staan}
                    int index = rnd.Next(cards.Count);
                    int imageNR = cards[index];//de ImageNR wordt cards[index] een random item{een getal} uit de lijst cards
                    cards.RemoveAt(index);//het item wordt verwijdert uit de lijst zodat deze niet nog een keer gepakt kan worden
                    cardOrder.Add(imageNR);
                    ImageSource source = new BitmapImage(new Uri("Resources/" + Thema + "/" + imageNR + ".png", UriKind.Relative));
                    images.Add(source);

                }
            }
            if (doorgaan == 1)
            {
                for (int i = 0; i < 16; i++)
                {
                    int imageNR = Convert.ToInt32(File.ReadLines("memory.sav").Skip(6 + i).Take(1).First());
                    cardOrder.Add(imageNR);
                    ImageSource source = new BitmapImage(new Uri("Resources/" + Thema + "/" + imageNR + ".png", UriKind.Relative));
                    images.Add(source);
                }
            }
            return images;
        }

        //MUISKLIK
        /// <summary>
        /// zorgt voor een delay
        /// </summary>
        /// <returns></returns>
        async Task PutTaskDelay()
        {
            await Task.Delay(600);
        }
        /// <summary>
        /// Wanneer erop een kaart gedrukt wordt zal deze omdraaien.
        /// En wanneer er twee zijn omgedraaid zullen deze worden gecheckt, 
        /// vervolgens zal er een passende actie voorkomen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CardClick(object sender, MouseButtonEventArgs e)
        {

            var element = (UIElement)e.Source;
            int row = Grid.GetRow(element);
            flippedRows.Add(row);
            int column = Grid.GetColumn(element);
            flippedCols.Add(column);
            bool doubleClickColumn = clickColumn.Contains(column);
            bool doubleClickRow = clickRow.Contains(row);
            bool disableSolved = solvedCards.Contains(sender);
            if (openCards.Count == 2)
            {
                return;
            }
            else if (doubleClickRow == true && doubleClickColumn == true)
            {

                if (openCards.Count == 2)
                {
                    clickRow.Clear();
                    clickColumn.Clear();
                }
                return;
            }
            else if (disableSolved == true)
            {
                return;

            }
            else
            {
                Image card = (Image)sender;
                ImageSource front = (ImageSource)card.Tag;
                card.Source = front;


                clickRow.Add(row);
                clickColumn.Add(column);

                openCards.Add(front.Height);
                seenCards.Add(card);



                if (openCards.Count == 2)
                {
                    clickRow.Clear();
                    clickColumn.Clear();
                    if (openCards[0] == openCards[1])
                    {
                        solvedRows.AddRange(flippedRows);
                        solvedCols.AddRange(flippedCols);
                        flippedRows.Clear();
                        flippedCols.Clear();
                        
                        solvedCards.AddRange(seenCards);
                        matches = solvedCards.Count / 2;


                        if (turnCount % 2 == 1)
                        {
                            Streak1++; // Streak, elke goede combinatie acthereenvolgend geeft hogere score, Tot de 2 ivm comebacks.
                            switch (Streak1)
                            {
                                case 1:
                                    score = score + 1;
                                    break;
                                case 2:
                                    score = score + 2;
                                    break;
                                case 3:
                                    score = score + 2;
                                    break;
                                case 4:
                                    score = score + 2;
                                    break;
                                case 5:
                                    score = score + 2;
                                    break;
                                case 6:
                                    score = score + 2;
                                    break;
                                case 7:
                                    score = score + 2;
                                    break;
                                case 8:
                                    score = score + 2;
                                    break;

                            }
                            scoreboard.Content = PlayerName1 + "\n\nSCORE:\n" + score + "\nSTREAK: " + Streak1;
                        }
                        else
                        {
                            Streak2++;
                            switch (Streak2)
                            {
                                case 1:
                                    score1 = score1 + 1;
                                    break;
                                case 2:
                                    score1 = score1 + 2;
                                    break;
                                case 3:
                                    score1 = score1 + 2;
                                    break;
                                case 4:
                                    score1 = score1 + 2;
                                    break;
                                case 5:
                                    score1 = score1 + 2;
                                    break;
                                case 6:
                                    score1 = score1 + 2;
                                    break;
                                case 7:
                                    score1 = score1 + 2;
                                    break;
                                case 8:
                                    score1 = score1 + 2;
                                    break;
                            }
                            scoreboard1.Content = PlayerName2 + "\n\nSCORE:\n" + score1 + "\nSTREAK: " + Streak2;
                        }
                    }

                    if (!(openCards[0] == openCards[1]))
                    {
                        turnCount++;
                        Streak1 = 0;
                        Streak2 = 0;
                        await PutTaskDelay();
                        card.Source = new BitmapImage(new Uri("Resources/" + Thema + "/Back/DC_Comics_logo.png", UriKind.Relative));
                        seenCards[0].Source = new BitmapImage(new Uri("Resources/" + Thema + "/Back/DC_Comics_logo.png", UriKind.Relative));
                        scoreboard.Content = PlayerName1 + "\n\nSCORE:\n" + score + "\nSTREAK: " + Streak1;
                        scoreboard1.Content = PlayerName2 + "\n\nSCORE:\n" + score1 + "\nSTREAK: " + Streak2;
                        flippedCols.Clear();
                        flippedRows.Clear();

                        if (turnCount % 2 == 0)
                        {
                            scoreboard1.Background = new SolidColorBrush(Color.FromRgb(2, 119, 243));
                            scoreboard.Background = null;

                        }
                        else
                        {
                            scoreboard.Background = new SolidColorBrush(Color.FromRgb(2, 119, 243));
                            scoreboard1.Background = null;

                        }
                    }
                    seenCards.RemoveRange(0, 2);
                    openCards.RemoveRange(0, 2);
                }
                if (solvedCards.Count == 16)
                {
                    solvedRows.Clear();
                    solvedCols.Clear();
                    if (score > score1)
                    {
                        Winnaar = PlayerName1;
                        WinScore = score;
                        Eindscherm Eindscherm = new Eindscherm(Winnaar, WinScore);
                        Eindscherm.Show();
                    }

                    if (score < score1)
                    {
                        Winnaar = PlayerName2;
                        WinScore = score1;
                        Eindscherm Eindscherm = new Eindscherm(Winnaar, WinScore);
                        Eindscherm.Show();
                    }

                    if (score == score1)
                    {
                        MessageBox.Show("Gelijkspel");
                    }
                }


            }
        }


        //SCOREBORD
        /// <summary>
        /// voegt een scorebord toe voor speler 1
        /// </summary>
        private void AddScoreboard()
        {
            scoreboard.Content = PlayerName1 + "\n\nSCORE:\n" + score + "\nSTREAK: " + Streak1;
            scoreboard.FontSize = 15;
            if (turnCount % 2 == 0)
            {
                scoreboard.Background = null;
            }
            else
            {
                scoreboard.Background = new SolidColorBrush(Color.FromRgb(2, 119, 243));
            }

            scoreboard.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetRow(scoreboard, 0);
            Grid.SetColumn(scoreboard, 5);
            grid.Children.Add(scoreboard);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="theme"></param>
        public void SetThemeName(string theme)
        {
            this.Thema = theme;
        }

        public void SetDoorgaan(int load)
        {
            this.doorgaan = load;
        }

        public void SetPlayer1Name(string name1)
        {
            PlayerName1 = name1;
            scoreboard.Content = PlayerName1 + "\n\nSCORE:\n" + score + "\nSTREAK: " + Streak1;
        }
        public void SetPlayer2Name(string name2)
        {
            PlayerName2 = name2;
            scoreboard1.Content = PlayerName2 + "\n\nSCORE:\n" + score1 + "\nSTREAK: " + Streak2;
        }


        //SCOREBORD1
        /// <summary>
        /// Voegt een scorebord toe voor speler 2
        /// </summary>
        private void AddScoreboard1()
        {
            scoreboard1.Content = PlayerName2 + "\n\nSCORE:\n" + score1 + "\nSTREAK: " + Streak2;
            scoreboard1.FontSize = 15;
            if (turnCount % 2 == 0)
            {
                scoreboard1.Background = new SolidColorBrush(Color.FromRgb(2, 119, 243));
            }
            else
            {
                scoreboard1.Background = null;
            }


            scoreboard1.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetRow(scoreboard1, 1);
            Grid.SetColumn(scoreboard1, 5);
            grid.Children.Add(scoreboard1);
        }

        //RESETKNOP
        /// <summary>
        /// Voegt een resetknop toe
        /// </summary>
        private void AddResetBtn()
        {
            resetBtn.Content = "RESET";
            resetBtn.FontSize = 30;
            resetBtn.Foreground = Brushes.White;
            resetBtn.Background = new SolidColorBrush(Color.FromRgb(2, 119, 243));
            resetBtn.Height = 50;
            resetBtn.Click += ResetGame;
            Grid.SetRow(resetBtn, 2);
            Grid.SetColumn(resetBtn, 5);
            grid.Children.Add(resetBtn);

        }
        /// <summary>
        /// Reset de game 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetGame(object sender, RoutedEventArgs e)
        {
            if (openCards.Count == 2)
            {
                return;
            }
            else
            {
                grid.Children.Clear();

                solvedCards.Clear();
                openCards.Clear();
                seenCards.Clear();
                clickRow.Clear();
                clickColumn.Clear();
                solvedRows.Clear();
                solvedCols.Clear();
                flippedCols.Clear();
                flippedRows.Clear();

                Streak1 = 0;
                Streak2 = 0;
                turnCount = 1;
                if (doorgaan == 0)
                {
                    cards.AddRange(newCards);
                }


                grid.Children.Add(scoreboard);
                grid.Children.Add(scoreboard1);
                scoreboard.Background = new SolidColorBrush(Color.FromRgb(2, 119, 243));
                scoreboard1.Background = null;
                score = 0;
                score1 = 0;
                scoreboard.Content = PlayerName1 + "\n\nSCORE:\n" + score + "\nSTREAK: " + Streak1;
                scoreboard1.Content = PlayerName2 + "\n\nSCORE:\n" + score1 + "\nSTREAK: " + Streak2;
                grid.Children.Add(resetBtn);

                cardOrder.RemoveRange(0, cardOrder.Count);

                doorgaan = 0;
                AddCards();
            }

        }

        //GET METHODS
        /// <summary>
        /// returned score
        /// </summary>
        /// <returns>score</returns>
        public int GetScore()
        {
            return score;

        }
        /// <summary>
        /// returned score1
        /// </summary>
        /// <returns>score1</returns>
        public int GetScore1()
        {
            return score1;
        }
        /// <summary>
        /// returned hoeveel beurten er zijn geweest
        /// </summary>
        /// <returns>turnCount</returns>
        public int GetTurnCount()
        {
            return turnCount;
        }
        public string GetCurrentNamePlayer1()
        {
            return PlayerName1;
        }
        public string GetCurrentNamePlayer2()
        {
            return PlayerName2;
        }
        public string GetCurrentTheme()
        {
            return Thema;
        }
        public List<int> GetCardOrder()
        {
            return cardOrder;
        }

        public List<int> GetSolvedRows()
        {
            return solvedRows;
        }

        public List<int> GetSolvedCols()
        {
            return solvedCols;
        }
        public int GetMatches()
        {
            return matches;
        }
    }

}
