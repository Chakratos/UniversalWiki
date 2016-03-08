using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.Xml;

namespace UniversalWiki
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string path = Directory.GetCurrentDirectory();
        string gamepath;
        string category;
        string categoryPath;
        string subCategory;
        string subCategoryPath;
        string xmlPath;
        string article;
        string stretch;
        XmlDocument itemDoc = new XmlDocument();
        public MainWindow()
        {
            InitializeComponent();
            this.comboGame.SelectionChanged += new SelectionChangedEventHandler(OnGameChanged);
            this.comboCategory.SelectionChanged += new SelectionChangedEventHandler(OnCategoryChanged);            
            this.comboSubCategory.SelectionChanged += new SelectionChangedEventHandler(OnSubCategoryChanged);
            this.comboArticle.SelectionChanged += new SelectionChangedEventHandler(OnArticleChanged);
            LoadGames();
        }
        public void LoadGames()
        {
            if (!Directory.Exists(path + "\\Games"))
            {
                Directory.CreateDirectory(path + "\\Games");
            }

            string[] Games = Directory.GetDirectories(path + "\\Games");

            ObservableCollection<string> list = new ObservableCollection<string>();
            for (int i = 0; i < Games.Length; i++)
            {
                string[] dir = Games[i].Split(new Char[] { '\\' });
                list.Add(dir[dir.Length -1]);
            }
            this.comboGame.ItemsSource = list;
        }
        private void OnGameChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                imgArticle.Visibility = System.Windows.Visibility.Hidden;
                imgArticleADV.Visibility = System.Windows.Visibility.Hidden;
                imgCategory.Visibility = System.Windows.Visibility.Hidden;
                string game = e.AddedItems[0].ToString();
                Universal_Wiki.Title = "UniversalWiki - " + game;
                gamepath = path + "\\" + "\\Games\\" + game + "\\";
                try
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(gamepath + "Game.png");
                    image.EndInit();
                    imgGame.Source = image;
                }
                catch
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(gamepath + "Game.jpg");
                    image.EndInit();
                    imgGame.Source = image;
                }

                string[] Categorys = Directory.GetDirectories(gamepath);

                ObservableCollection<string> list = new ObservableCollection<string>();
                list.Add("---------------Category---------------");
                for (int i = 0; i < Categorys.Length; i++)
                {
                    string[] dir = Categorys[i].Split(new Char[] { '\\' });
                    list.Add(dir[dir.Length - 1]);
                }
                this.comboCategory.ItemsSource = list;
                comboCategory.SelectedIndex = 0;
                CheckForUpdate(gamepath, game);
            }
            catch
            {

            }
        }
        private void OnCategoryChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                category = e.AddedItems[0].ToString();

                if (category != "---------------Category---------------")
                {
                    categoryPath = gamepath + "\\" + category;
                    try
                    {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.UriSource = new Uri(categoryPath + "\\" + category + ".png");
                        image.EndInit();
                        imgCategory.Source = image;
                    }
                    catch
                    {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.UriSource = new Uri(categoryPath + "\\" + category + ".jpg");
                        image.EndInit();
                        imgCategory.Source = image;
                    }

                    imgCategory.Visibility = System.Windows.Visibility.Visible;
                    imgArticle.Visibility = System.Windows.Visibility.Hidden;
                    imgArticleADV.Visibility = System.Windows.Visibility.Hidden;

                    string[] SubCategorys = Directory.GetDirectories(categoryPath);

                    ObservableCollection<string> list = new ObservableCollection<string>();
                    list.Add("---------------Sub Category---------------");
                    for (int i = 0; i < SubCategorys.Length; i++)
                    {
                        string[] dir = SubCategorys[i].Split(new Char[] { '\\' });
                        list.Add(dir[dir.Length - 1]);
                    }
                    this.comboSubCategory.ItemsSource = list;
                    comboSubCategory.SelectedIndex = 0;
                }
            }
            catch
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                comboSubCategory.ItemsSource = list;
            }
        }
        private void OnSubCategoryChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                subCategory = e.AddedItems[0].ToString();
                if (subCategory != "---------------Sub Category---------------")
                {
                    subCategoryPath = categoryPath + "\\" + subCategory;
                    xmlPath = subCategoryPath + "\\" + subCategory + ".xml";
                    try
                    {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.UriSource = new Uri(subCategoryPath + "\\" + subCategory + ".png");
                        image.EndInit();
                        imgArticle.Source = image;
                    }
                    catch
                    {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.UriSource = new Uri(subCategoryPath + "\\" + subCategory + ".jpg");
                        image.EndInit();
                        imgArticle.Source = image;
                    }

                    imgArticle.Visibility = System.Windows.Visibility.Visible;
                    imgArticleADV.Visibility = System.Windows.Visibility.Hidden;

                    itemDoc.Load(xmlPath);
                    ObservableCollection<string> list = new ObservableCollection<string>();
                    list.Add("---------------Article---------------");
                    foreach (XmlNode itemNode in itemDoc.DocumentElement.ChildNodes)
                    {
                        XmlElement itemElement = (XmlElement)itemNode;
                        string name = itemElement.Attributes["name"].Value;
                        list.Add(name);
                    }
                    comboArticle.ItemsSource = list;
                }
            }
            catch
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                comboArticle.ItemsSource = list;
            }
        }
        private void OnArticleChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {


                article = e.AddedItems[0].ToString();
                if (article != "---------------Article---------------")
                {
                    foreach (XmlNode itemNode in itemDoc.DocumentElement.ChildNodes)
                    {
                        XmlElement itemElement = (XmlElement)itemNode;
                        string name = itemElement.Attributes["name"].Value;
                        if (article == name)
                        {
                            stretch = itemElement.Attributes["stretch"].Value;
                            if (stretch == "Fill")
                            {
                                imgArticleADV.Stretch = Stretch.Fill;
                            }
                            else if (stretch == "Uniform")
                            {
                                imgArticleADV.Stretch = Stretch.Uniform;
                            }
                            else if (stretch == "UniformToFill")
                            {
                                imgArticleADV.Stretch = Stretch.UniformToFill;
                            }
                            else if (stretch == "None")
                            {
                                imgArticleADV.Stretch = Stretch.None;
                            }

                            txtArticleDescription.Text = itemElement.Attributes["description"].Value;
                            txtArticleDescription.FontSize = Convert.ToInt32(itemElement.Attributes["descriptionFontSize"].Value);

                            txtAdittionalInfo.Text = itemElement.Attributes["advDescription"].Value;
                            txtAdittionalInfo.FontSize = Convert.ToInt32(itemElement.Attributes["advDescriptionFontSize"].Value);
                            string pic = itemElement.Attributes["picture"].Value;

                            try
                            {
                                BitmapImage img = new BitmapImage();
                                img.BeginInit();
                                img.UriSource = new Uri(subCategoryPath + "\\Images\\" + pic + ".png");
                                img.EndInit();
                                imgArticleADV.Source = img;
                            }
                            catch
                            {
                                BitmapImage img = new BitmapImage();
                                img.BeginInit();
                                img.UriSource = new Uri(subCategoryPath + "\\Images\\" + pic + ".jpg");
                                img.EndInit();
                                imgArticleADV.Source = img;
                            }
                            imgArticleADV.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                }
            }
            catch
            {
                txtArticleDescription.Text = "";
                txtAdittionalInfo.Text = "";
            }
        }

        private void CheckForUpdate(string gamePath , string game)
        {
            string wikiSite = GetUpdateSite(gamePath,game);
            string curVer = GetCurVer(gamePath,game);
            string newVer = GetCurVer(wikiSite,game);
            if (curVer != newVer)
            {
                MessageBoxResult result = MessageBox.Show("A new version of: " + game + " is available" + Environment.NewLine + "Would you like to update now?", "A new update!", MessageBoxButton.YesNo,MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    applyUpdate(wikiSite + game, gamePath);
                }
            }
        }
        private void applyUpdate(string Site,string path)
        {
            string dl = Site + ".rar";
            System.Diagnostics.Process.Start(dl);
        }
        private string GetUpdateSite(string gamePath, string game)
        {
            string site = "";
            string gameXML = gamePath + game + ".xml";
            itemDoc.Load(gameXML);
            foreach (XmlNode itemNode in itemDoc.DocumentElement.ChildNodes)
            {
                XmlElement itemElement = (XmlElement)itemNode;
                site = itemElement.Attributes["update"].Value;
            }
            return site;
        }
        private string GetCurVer(string gamePath , string game)
        {
            string ver = "";
            string gameXML = gamePath + game + ".xml";
            itemDoc.Load(gameXML);
            foreach (XmlNode itemNode in itemDoc.DocumentElement.ChildNodes)
            {
                XmlElement itemElement = (XmlElement)itemNode;
                ver = itemElement.Attributes["ver"].Value;
            }
            return ver;
        }

        private void btnCredits_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.chakratos.comlu.com/about.html");
        }

        private void btnMore_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.chakratos.comlu.com/Wikis.html");
        }
    }
}
