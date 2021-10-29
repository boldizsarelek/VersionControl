using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using Week06.MnbServiceReference;

namespace Week06
{
    public partial class Form1 : Form
    {

        BindingList<RateData> Rates = new BindingList<RateData>();
        BindingList<string> Currencies = new BindingList<string>();
        public Form1()
        {
            InitializeComponent();

            dataGridView1.DataSource = Rates;
            comboBox1.DataSource = Currencies;
            LoadCurrencies();
            RefreshData();
        }

        private void LoadCurrencies()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();

            var response = mnbService.GetCurrencies(new GetCurrenciesRequestBody());

            var result = response.GetCurrenciesResult;

            var xml = new XmlDocument();
            xml.LoadXml(result);

            string currency;
            foreach (XmlElement element in xml.DocumentElement)
            {

                for (int i = 0; i < element.ChildNodes.Count; i++)
                {
                    var childElement = (XmlElement)element.ChildNodes[i];
                    currency = childElement.InnerText;
                    Currencies.Add(currency);
                }
            }
        }

        private void RefreshData()
        {
            Rates.Clear();

            var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = (string)comboBox1.SelectedItem,
                startDate = dateTimePicker1.Value.ToString(),
                endDate = dateTimePicker2.Value.ToString()
            };

            var response = mnbService.GetExchangeRates(request);

            var result = response.GetExchangeRatesResult;

            ProcessData(result);
            ShowData();
        }

        private void ProcessData(string result)
        {
            var xml = new XmlDocument();

            xml.LoadXml(result);

            foreach (XmlElement element  in xml.DocumentElement)
            {
                var rate = new RateData();
                Rates.Add(rate);

                rate.Date = DateTime.Parse(element.GetAttribute("date"));

                var childElement = (XmlElement)element.ChildNodes[0];
                if (childElement == null)
                {
                    continue;
                }
                rate.Currency = childElement.GetAttribute("curr");

                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0)
                {
                    rate.Value = value / unit;
                }
            }
        }

        private void ShowData()
        {
            chartRateData.DataSource = Rates;

            var series = chartRateData.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";


            var legend = chartRateData.Legends[0];
            legend.Enabled = false;

            var chartArea = chartRateData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
