using System;
using System.Collections.Generic;
using System.Linq;
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
using System.ComponentModel;
using SWD_projekt.AHP;
using SWD_projekt.Database;
using SWD_projekt.Core;
using SWD_projekt.UI;
using System.Threading;

namespace SWD_projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker initDatabaseWorker = new BackgroundWorker();
        private int currentPageIndex = 0;
        private List<Page> pageList = new List<Page>();
        private bool skipDistances;
        private bool skipWrongUniversities;
        
        private List<University> selectedUniversities;
        private List<University> wrongUniversities;
        private List<Cryteria> selectedCryteria;
        private AHPMatrix cryteriaEvaluationMatrix;
        private bool reverseDistance;
        private bool lowerRatingForWrong;

        public MainWindow()
        {
            InitializeComponent();
            InitializeWorkers();
            AHPHelper.Init();
            DisableAll();
            initDatabaseWorker.RunWorkerAsync();
        }

        private void Test()
        {
            AHPMatrix m0 = new AHPMatrix(new double[,] {
                {1,3,7,9},
                {0,1,3,7},
                {0,0,1,3},
                {0,0,0,1}
            });
            AHPMatrix m1 = new AHPMatrix(new double[,] {
                {1,1,7},
                {0,1,3},
                {0,0,1}
            });
            AHPMatrix m2 = new AHPMatrix(new double[,] {
                {1,0.2,1},
                {0,1,3},
                {0,0,1}
            });
            AHPMatrix m3 = new AHPMatrix(new double[,] {
                {1,1,7},
                {0,1,3},
                {0,0,1}
            });
            AHPMatrix m4 = new AHPMatrix(new double[,] {
                {1,7,9},
                {0,1,1},
                {0,0,1}
            });
            List<AHPMatrix> list = new List<AHPMatrix>();
            list.Add(m0);
            list.Add(m1);
            list.Add(m2);
            list.Add(m3);
            list.Add(m4);
            PreferenceVectorCalculator pvc = new PreferenceVectorCalculator(list);
            pvc.CalculateRankingVector();
        }

        private void InitPages()
        {
            pageList.Add(new UniverstyChoicePage());
            pageList.Add(new CryteriaChoicePage());
            pageList.Add(new NotFreeUniversitiesPage());
            pageList.Add(new GeoLocalizationPage(this));
            pageList.Add(new CryteriaEvaluationPage());

            contentFrame.Content = pageList.ElementAt(currentPageIndex);
            Title = (pageList.ElementAt(currentPageIndex) as Page).Title;
        }

        public void EnableAll()
        {
            Mouse.OverrideCursor = null;
            IsEnabled = true;
        }

        public void DisableAll()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            IsEnabled = false;
        }

        private void initDatabaseWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DBConnection.Init();
            DBConnection.OpenConnection();
            if (!DBConnection.DatabaseExists())
                DBConnection.CreateDatabase(true);
        }

        private void initDatabaseWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableAll();
            InitPages();
        }

        private void InitializeWorkers()
        {
            initDatabaseWorker.DoWork += new DoWorkEventHandler(initDatabaseWorker_DoWork);
            initDatabaseWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(initDatabaseWorker_RunWorkerCompleted);
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            bool error = false;
            currentPageIndex++;
            if (contentFrame.Content is UniverstyChoicePage)
            {
                selectedUniversities = (contentFrame.Content as UniverstyChoicePage).SelectedUniversitiesList;
                if (selectedUniversities.Count < 2)
                {
                    error = true;
                    Xceed.Wpf.Toolkit.MessageBox.Show("test błędu");
                }
            }
            else if (contentFrame.Content is CryteriaChoicePage)
            {
                selectedCryteria = (contentFrame.Content as CryteriaChoicePage).SelectedCryteriaList;
                Cryteria cryt = selectedCryteria.Find(c => c.ID == Cryteria.FREE_UNIVERSITY_CRYTERIA);
                skipWrongUniversities = cryt == null;
                if (!skipWrongUniversities)
                {
                    wrongUniversities = new List<University>();
                    foreach (University u in selectedUniversities)
                    {
                        string value = DBConnection.GetCryteriaValueForUniversity(u.ID, cryt.ID);
                        double val = Convert.ToDouble(value);
                        if (val == 1)
                            wrongUniversities.Add(u);
                    }
                    skipWrongUniversities = wrongUniversities.Count == 0;
                    if (skipWrongUniversities)
                        currentPageIndex++;
                }
                else
                    currentPageIndex++;
                if (skipWrongUniversities)
                {
                    cryt = selectedCryteria.Find(c => c.ID == Cryteria.CITY_DISTANCE_CRYTERIA);
                    skipDistances = cryt == null;
                    if (skipDistances)
                        currentPageIndex++;
                }
            }
            else if (contentFrame.Content is NotFreeUniversitiesPage)
            {
                if ((contentFrame.Content as NotFreeUniversitiesPage).GetNoWrong())
                    selectedUniversities.RemoveAll(u => wrongUniversities.Contains(u));
                else
                    lowerRatingForWrong = true;
                Cryteria cryt = selectedCryteria.Find(c => c.ID == Cryteria.CITY_DISTANCE_CRYTERIA);
                skipDistances = cryt == null;
                if (skipDistances)
                    currentPageIndex++;
            }
            else if (contentFrame.Content is GeoLocalizationPage)
            {
                (contentFrame.Content as GeoLocalizationPage).SetDistanceFromAddressInUniversities();
                reverseDistance = (contentFrame.Content as GeoLocalizationPage).GetReverseDistance();
            }
            else if (contentFrame.Content is CryteriaEvaluationPage)
            {
                cryteriaEvaluationMatrix = (contentFrame.Content as CryteriaEvaluationPage).GetCryteriaEvaluationMatrix();
                Xceed.Wpf.Toolkit.MessageBox.Show(cryteriaEvaluationMatrix.CheckConsistency().ToString());
            }
            if (currentPageIndex < pageList.Count && !error)
            {
                Page page = pageList.ElementAt(currentPageIndex);
                if (page is CryteriaEvaluationPage)
                    (page as CryteriaEvaluationPage).Init(selectedCryteria);
                else if (page is NotFreeUniversitiesPage && wrongUniversities.Count > 0)
                    (page as NotFreeUniversitiesPage).Init(selectedUniversities, wrongUniversities);
                else if (page is GeoLocalizationPage)
                    (page as GeoLocalizationPage).Init(selectedUniversities);
                contentFrame.Content = page;
                Title = (pageList.ElementAt(currentPageIndex) as Page).Title;
            }
            else if (error)
            {
                currentPageIndex--;
            }
            backButton.IsEnabled = currentPageIndex > 0;
            nextButton.IsEnabled = currentPageIndex < pageList.Count;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            currentPageIndex--;
            if (currentPageIndex >= 0)
            {
                Page page = pageList.ElementAt(currentPageIndex);
                if (skipDistances && page is GeoLocalizationPage)
                {
                    currentPageIndex--;
                    page = pageList.ElementAt(currentPageIndex);
                }
                if (skipWrongUniversities && page is NotFreeUniversitiesPage)
                {
                    currentPageIndex--;
                    page = pageList.ElementAt(currentPageIndex);
                }
                contentFrame.Content = page;
                Title = page.Title;
            }
            backButton.IsEnabled = currentPageIndex != 0;
            nextButton.IsEnabled = currentPageIndex < pageList.Count;
        }
    }
}