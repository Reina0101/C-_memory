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

    public partial class Eindscherm : Window
    {    
        private string Winnaar = "";
        private int WinScore;

        //hier zorgt de Set functies ervoor dat de variabelen van het vorige scherm gebruikt worden in het huidige scherm
        //Eerst leest het programma de savefile en schrijft daar de score in.
        //de string infile wordt het teskt bestand
        //de outfile is de infile omgekeerd en wordt weergegeven
        //Als er vanaf het beginscherm naar highscores wordt gegaan is de winnaar knop niet actief
        
        public Eindscherm(string Winnaar, int WinScore)
        {
            
            InitializeComponent();
            SetWinaar(Winnaar);
            SetWinScore(WinScore);
            winnaarcheck();
            File.AppendAllText("Highscores.sav", WinScore + " " + Winnaar + Environment.NewLine); // sla naam en score op in Highscores.Sav
            string inFile = "Highscores.sav";
            string outFile = "SortedHighscores.sav";
            var contents = (File.ReadAllLines(inFile)); // Lees alles in Higscores.Sav sorteer het en Output in SortedHishscores.sav
            Array.Sort(contents);
            Array.Reverse(contents);
            File.WriteAllLines(outFile, contents);
            Highscores.Text = System.IO.File.ReadAllText("SortedHighscores.sav");



        }


        private void winnaarcheck()
        {
            if (Winnaar != "")
            {
                return;
            }
            else
            {
                MessageBox.Show("Er is nog geen winnaar, speel eerst een spel");
                Gefeliciteerd.IsEnabled = false;
                Textbox.IsEnabled = false;
            }
        }

        //Wanneer er op deze knop wordt gedrukt wordt het complete programma afgesloten
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void SetWinaar(string winner)
        {
            this.Winnaar = winner;
        }

        public void SetWinScore(int wscore)
        {
            this.WinScore = wscore;
        }

        //Click voor resultaat
        //wanneer er op de knop wordt gedrukt wordt de knop onzichtbaar en verschijnt de text + winaar+ score in de textbox
        //Verder wordt er ook een geluid bij afgespeeld
        private void Gefeliciteerd_Click(object sender, RoutedEventArgs e)
        {
            Gefeliciteerd.Opacity = 0;
            Textbox.Text = "Gefeliciteerd " + Winnaar + " uw score is " + WinScore + "!";
            var soundfile = Properties.Resources.yay;
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(soundfile);
            player.Play();
        }
    }
}



