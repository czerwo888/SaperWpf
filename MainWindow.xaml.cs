using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SapperGra;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private int rozmiar = 10;
    private int iloscBomb = 10;
    private int iloscFlag;
    
    public MainWindow()
    {
        InitializeComponent();
        iloscFlag = iloscBomb;

        for (int i = 0; i < rozmiar; i++)
        {
            plansza.RowDefinitions.Add(new RowDefinition());
            plansza.ColumnDefinitions.Add(new ColumnDefinition());
        }

        for (int i = 0; i < rozmiar; i++)
        {
            for (int j = 0; j < rozmiar; j++)
            {
                Przycisk przycisk = new Przycisk()
                {
                    wartosc = 0,
                    FontSize = 15,
                    Background = Brushes.LightGray,
                };
                przycisk.Click += (sender, e) =>
                {
                    OdkryjPrzycisk(Grid.GetRow(przycisk), Grid.GetColumn(przycisk));
                };
                przycisk.MouseRightButtonDown += PrzyciskOnPrawyClick;
                Grid.SetRow(przycisk, i);
                Grid.SetColumn(przycisk, j);
                plansza.Children.Add(przycisk);
            }
        }
        RozstawBomby(iloscBomb);
        
    }
    
    private void PrzyciskOnPrawyClick(object sender, MouseButtonEventArgs e)
    {
        Przycisk przycisk = (Przycisk) sender;
        int x = Grid.GetRow(przycisk);
        int y = Grid.GetColumn(przycisk);
        Flaga(x, y);
    }
    
    private Przycisk WyszukajPrzyciski(int x, int y)
    {
        return plansza.Children
            .Cast<Przycisk>()
            .First(p => Grid.GetRow(p) == x && Grid.GetColumn(p) == y);   
    }

    private void RozstawBomby(int ilosc)
    {
        Random random = new Random();
        while (ilosc > 0)
        {
            int x = random.Next(rozmiar);
            int y = random.Next(rozmiar);
            Przycisk przycisk = WyszukajPrzyciski(x, y);
            if (przycisk.wartosc == 0)
            {
                przycisk.wartosc = 10;
                ilosc--;
                CountBombs(x, y);
            }
        }
    }

    private void PokazPlansze()
    {
        plansza.Children
            .Cast<Przycisk>()
            .ToList()
            .ForEach(e =>
            {
                if (e.wartosc == 10)
                {
                    e.Content = "💣";
                }
                else
                {
                    e.Content = e.wartosc;
                }
            });
    }

    private void CountBombs(int x, int y)
    {
        for (int i = x-1; i <= x+1; i++)
        {
            for (int j = y-1; j <= y+1; j++)
            {
                if (i >=0 && i < rozmiar && j >= 0 && j < rozmiar)
                {
                    Przycisk przycisk = WyszukajPrzyciski(i, j);
                    if (przycisk.wartosc != 10)
                    {
                        przycisk.wartosc++;
                    }
                }
                
            }
        }
    }
    
    private void Flaga(int x, int y)
    {
        Przycisk przycisk = WyszukajPrzyciski(x, y);
        if (przycisk.Content == null)
        {
            if (iloscFlag > 0)
            {
                przycisk.Content = "🚩";
                iloscFlag--;
            }
        }
        else
        {
            if (przycisk.Content.ToString() == "🚩")
            {
                przycisk.Content = null;
                iloscFlag++;   
            }
        }
    }

    private void OdkryjPrzycisk(int x, int y)
    {
        Przycisk przycisk = WyszukajPrzyciski(x, y);
        if (przycisk.Content != null)
        {
            return;
        }
        if (przycisk.wartosc == 10)
        {
            MessageBox.Show("Przegrałeś");
            PokazPlansze();
        }
        else
        {
            przycisk.Background = Brushes.White;
            przycisk.Content = przycisk.wartosc;
            if(przycisk.wartosc == 0)
            {
                for (int i = x-1; i <= x+1; i++)
                {
                    for (int j = y-1; j <= y+1; j++)
                    {
                        if (i >=0 && i < rozmiar && j >= 0 && j < rozmiar)
                        {
                            OdkryjPrzycisk(i, j);
                        }
                    }
                }
            }
        }
    }
}