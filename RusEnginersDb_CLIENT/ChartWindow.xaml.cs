using System;
using System.Collections.Generic;
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
using OxyPlot;
using OxyPlot.Series;
using RusEnginersDb_SHARED;

namespace RusEnginersDb
{
    /// <summary>
    /// Логика взаимодействия для ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Window
    {
        public ChartWindow(Project proj)
        {
            InitializeComponent();

            PlotModel timemodel=new PlotModel { Title="Соотношение времени доставки" };
            dynamic seriesP1 = new PieSeries { StrokeThickness = 2.0, InsideLabelPosition = 0.8, AngleSpan = 360, StartAngle = 0 };
            foreach (var item in proj.CurrentList)
            {
                seriesP1.Slices.Add(new PieSlice(item.Name, item.Delivery) { IsExploded = false });
            }
            timemodel.Series.Add(seriesP1);

            PlotModel pricemodel = new PlotModel { Title = "Соотношение цен" };
            dynamic seriesP2 = new PieSeries { StrokeThickness = 2.0, InsideLabelPosition = 0.8, AngleSpan = 360, StartAngle = 0 };
            foreach (var item in proj.CurrentList)
            {
                seriesP2.Slices.Add(new PieSlice(item.Name, item.Price) { IsExploded = false });
            }
            pricemodel.Series.Add(seriesP2);

            CostPlot.Model = pricemodel;
            TimePlot.Model = timemodel;
        }
    }
}
