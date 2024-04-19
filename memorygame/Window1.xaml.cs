using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace memorygame
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        public string Gamemode;
        public string Thema;
        public string Naam1;
        public string Naam2;
        public int doorgaan;

        /// <summary>
        /// Opent de window van een nieuw spel met de ingevoerde namen en thema's
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            doorgaan = 0;
            Naam1 = Naambox.Text.ToString();
            Naam2 = Naambox1.Text.ToString();
            ResetStats();
            MainWindow MainWindow = new MainWindow(Naam1, Naam2, Thema, doorgaan);
            MainWindow.Show();
            this.Close();
                        
        }
        /// <summary>
        /// Sluit de iets af
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Opent de window van een bestaand spel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int Line = File.ReadLines("memory.sav").Count();

            if (Line > 21)
            {
                doorgaan = 1;
                Thema = File.ReadLines("memory.sav").Skip(5).Take(1).First();
                MainWindow MainWindow = new MainWindow(Thema, doorgaan);
                MainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Er is geen bestaand spel gevonden!");
            }
        }

        /// <summary>
        /// Opent de highscore knop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            {
                
                Eindscherm end = new Eindscherm("",0);
                end.Show();
                this.Close();
            }
        }

        /// <summary>
        /// Zorgt ervoor dat de spelen knop actief wordt als de textbox is ingevuld
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Naambox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Naam1 = Naambox.Text.ToString();

            if (ComboBox.Text != "" && Naambox.Text != "" && Gamemode != "" && Naambox1.Text != "")
            {
                BtnSpelen.IsEnabled = true;
            }
        }
        private void Naambox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            Naam2 = Naambox1.Text.ToString();

            if (ComboBox.Text != "" && Naambox.Text != "" && Gamemode != "" && Naambox1.Text != "")
            {
                BtnSpelen.IsEnabled = true;
            }
        }


        /// <summary>
        /// Als er op de scores wordt geklikt, wordt de button rood en de variabele Gamemode scores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scores_Click(object sender, RoutedEventArgs e)
        {
            Gamemode = "Scores";
            Scores.Background = Scores.Background == Brushes.Red ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD")) : Brushes.Red;
            Timer.Background = Timer.Background == Brushes.LightGray ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD")) : Brushes.LightGray;

            if (ComboBox.Text != "" && Naambox.Text != "" && Gamemode != "" && Naambox1.Text != "")
            {
                BtnSpelen.IsEnabled = true;
            }
        }

        private void Timer_Click(object sender, RoutedEventArgs e)
        {
            Gamemode = "Timer";
            Timer.Background = Timer.Background == Brushes.Red ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD")) : Brushes.Red;
            Scores.Background = Scores.Background == Brushes.LightGray ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD")) : Brushes.LightGray;

            if (ComboBox.Text != "" && Naambox.Text != "" && Gamemode != "" && Naambox1.Text != "")
            {
                BtnSpelen.IsEnabled = true;
            }

        }


        /// <summary>
        /// Afhankelijk van het gekozen thema wordt de variabele thema gewijzigd 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Thema wordt het item, leest vanaf spatie 
            Thema = ComboBox.SelectedItem.ToString().Split(' ')[1];


            if (ComboBox.Text != "" && Naambox.Text != "" && Gamemode != "" && Naambox1.Text != "")
            {
                BtnSpelen.IsEnabled = true;
            }
            Naambox.IsEnabled = true;
            Naambox1.IsEnabled = true;
        }

      
        //easteregg + inside joke van memoryteam10
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            JHtextBlock.Text = "🅱️oey Hovinga";
        }

        /// <summary>
        /// reset de scores en beurt door de save file te herschrijven naar beginwaarden
        /// </summary>
        public void ResetStats()
        {
            string score = "0" + Environment.NewLine;
            string score1 = "0" + Environment.NewLine;
            string turn = "1" + Environment.NewLine;
            string namePlayer1 = Environment.NewLine;
            string namePlayer2 = Environment.NewLine;
            string ThemaInUse = Environment.NewLine;

            File.WriteAllText("memory.sav", score);
            File.AppendAllText("memory.sav", score1);
            File.AppendAllText("memory.sav", turn);
            File.AppendAllText("memory.sav", namePlayer1);
            File.AppendAllText("memory.sav", namePlayer2);
            File.AppendAllText("memory.sav", ThemaInUse);
        }
    }
}
