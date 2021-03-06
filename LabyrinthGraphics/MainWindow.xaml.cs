﻿using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace LabyrinthGraphics {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private Labyrinth labyrinth;
        private int defaultSize = 11;
        private string labelText = "Zadejte liché číslo > 4 < 34";
        private string title = "Bludiště";
        private string dialogFilter = "Text file|*.txt";

        public MainWindow() {
            InitializeComponent();
            this.Title = title; //titulek okna
            labelInfo.Content = labelText; //label nad textboxem
            textBoxInput.Text = defaultSize.ToString(); //výchozí text

            Draw(canvas, defaultSize);
        }

        /// <summary>
        /// Zavolá metodu DrawMap ze třídy Labyrinth
        /// </summary>
        /// <param name="canvas">Plátno hlavního okna</param>
        /// <param name="size">Zvolená velikost dimenzí</param>
        private void Draw(Canvas canvas, int size) {
            labyrinth = new Labyrinth(size);
            labyrinth.DrawMap(canvas); //vykreslení
        }

        /// <summary>
        /// Pokusí se vyparsovat z textboxu zadané číslo, pokud se to podaří
        /// vyčistí Canvas a nakreslí nové bludiště s požadovanou velikostí.
        /// Pokud byl zadaný špatný vstup bude zobrazeno okno s odpovídající 
        /// chybovou hláškou.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGenerate_Click(object sender, RoutedEventArgs e) {
            int size;

            if (int.TryParse(textBoxInput.Text, out size)) {
                if (size <= 4 || (size % 2) <= 0 || size > 33) { //špatné číslo
                    MessageBox.Show("Musíte zadat liché číslo, které je větší"
                        + " než 4 a menší než 34.", "Chyba", MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                }
                else {
                    canvas.Children.Clear();
                    Draw(canvas, size);
                }
            }
            else { //nebylo zadáno číslo
                MessageBox.Show("Musíte zadat číslo.", "Chyba",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Po stisku tlačítka zobrazí dialog pro volbu cílového souboru.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = dialogFilter;

            if (dialog.ShowDialog() == true) {
                string path = dialog.FileName;
                try {
                    labyrinth.SaveToFile(path, false);

                    MessageBox.Show("Do souboru " + path + " bylo úspěšně"
                            + " zapsáno.", "Úspěch", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                }
                catch (IOException ex) {
                    MessageBox.Show("Do souboru" + path + " se nepodařilo"
                            + " zapsat.", "Chyba", MessageBoxButton.OK,
                            MessageBoxImage.Exclamation);
                }
            }
        }

        /// <summary>
        /// Po stisku tlačítka zobrazí dialog pro volbu vstupního souboru.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLoad_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = dialogFilter;

            if (dialog.ShowDialog() == true) {
                string path = dialog.FileName;

                try {
                    labyrinth.LoadFromFile(path, canvas);
                }
                catch (FileNotFoundException ex) {
                    MessageBox.Show("Soubor " + path + " nebyl nalezen.", 
                        "Chyba", MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                }
            }
        }
    }
}
