using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Week11.Entities;

namespace Week11
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();

        Dictionary<int, int> NbrOfMales = new Dictionary<int, int>();
        Dictionary<int, int> NbrOfFemales = new Dictionary<int, int>();

        Random rng = new Random(1234);
        public Form1()
        {
            InitializeComponent();
           
            BirthProbabilities = GetBirthProbabilities(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Temp\halál.csv");
        }

        private void Simulation()
        {
            for (int year = 2005; year <= numericUpDown1.Value; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);
                }

                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male
                                  && x.IsAlive

                                  select x).Count();

                NbrOfMales.Add(year,nbrOfMales);

                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female
                                    && x.IsAlive

                                    select x).Count();

                NbrOfFemales.Add(year, nbrOfFemales);
                Console.WriteLine(string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, nbrOfMales, nbrOfFemales));
            }
        }

        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NbrOfChildren = int.Parse(line[2])
                    });
                }
            }

            return population;
        }

        public List<BirthProbability> GetBirthProbabilities(string csvpath)
        {
            List<BirthProbability> birthProbablity = new List<BirthProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    birthProbablity.Add(new BirthProbability()
                    {
                        Age = int.Parse(line[0]),
                        NbrOfChildren = int.Parse(line[1]),
                        P = double.Parse(line[2])
                    });
                }
            }

            return birthProbablity;
        }

        public List<DeathProbability> GetDeathProbabilities(string csvpath)
        {
            List<DeathProbability> deathProbablity = new List<DeathProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    deathProbablity.Add(new DeathProbability()
                    {
                        Age = int.Parse(line[0]),
                        NbrOfChildren = int.Parse(line[1]),
                        P = double.Parse(line[2])
                    });
                }
            }

            return deathProbablity;
        }

        private void SimStep(int year, Person person)
        {
            if (!person.IsAlive) return;

            byte age = (byte)(year - person.BirthYear);

            double pDeath = (from x in DeathProbabilities
                             where x.Age == age
                             select x.P).FirstOrDefault();

            if (rng.NextDouble() <= pDeath)
            {
                person.IsAlive = false;
            }

            if (person.IsAlive && person.Gender == Gender.Female)
            {
                double pBirth = (from x in BirthProbabilities
                                 where x.Age == age
                                 select x.P).FirstOrDefault();

                if (rng.NextDouble() <= pBirth)
                {
                    Person newBorn = new Person();
                    newBorn.BirthYear = year;
                    newBorn.NbrOfChildren = 0;
                    newBorn.Gender = (Gender)rng.Next(1, 3);
                    Population.Add(newBorn);
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            
            Population = GetPopulation(textBox1.Text);
            richTextBox1.Clear();
            Simulation();
            DisplayResults();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (DialogResult.OK == ofd.ShowDialog())
            {
                textBox1.Text = ofd.FileName;
            }
        }

        private void DisplayResults()
        {
            for (int year = 2005; year < numericUpDown1.Value; year++)
            {
                richTextBox1.Text += $"Szimulációs év: {year} \n" +
                                       $"\tFiúk: {NbrOfMales[year]}\n" +
                                       $"\tLányok: {NbrOfFemales[year]}\n";
            }
        }
    }


}
