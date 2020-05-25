using HtmlAgilityPack;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace WpfApp_core
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            #region
            Grid mGrid = new Grid();
            Label lb_html = new Label();
            Image img = new Image();
            #endregion
            Canvas canvas = new Canvas();
            
            HtmlWeb webClient = new HtmlWeb(); //建立htmlweb
            //處理C# 連線 HTTPS 網站發生驗證失敗導致基礎連接已關閉
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
            SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HtmlDocument doc = webClient.Load("https://www.manhuaren.com/search?title=%E8%BC%9D%E5%A4%9Ca%E6%97%A5&language=1"); //載入網址資料
            //lb_html.Content = doc.Text;   //確認是否有抓到html代碼

            /* SelectNodes: 切換到指定的層
             * Return: 該層內的所有內容，標籤相同的會成為list
             * 註: 回傳為list之後起始位置=0, 尚未回傳要直接指向的時候起始位置=1
             */
            HtmlNodeCollection list = doc.DocumentNode.SelectNodes("/html/body/ul/li");
            //標籤img內有需要的網址，透過Attribute取出
            string Url = list[0].SelectSingleNode("div[1]/a/img").Attributes["src"].Value;
            //將網址字串轉為Uri格式
            Uri uri = new Uri(Url);
            //透過網址下載圖片轉為BitmapImage格式
            BitmapImage image = new BitmapImage(uri);
            //使用Image儲存該BitmapImage
            img.Source = image;

            string name = list[0].SelectSingleNode("div[2]/a/p[1]").InnerText;
            string introduction = list[0].SelectSingleNode("div[2]/a/p[2]").InnerText;
            lb_html.Content = "URL: " + Url +
                "\nName: " + name + "\n介紹: " + introduction;
            //設置元件位置
            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.VerticalAlignment = VerticalAlignment.Center;
            img.Margin = new Thickness(2);
            img.Width = 100;
            img.Height = 150;
            lb_html.HorizontalAlignment = HorizontalAlignment.Left;
            lb_html.VerticalAlignment = VerticalAlignment.Center;
            lb_html.HorizontalContentAlignment = HorizontalAlignment.Left;
            lb_html.VerticalContentAlignment = VerticalAlignment.Center;
            lb_html.Margin = new Thickness(img.Margin.Left + img.Width, img.Margin.Top, 0, 0);
            lb_html.Width = 300;
            lb_html.Height = 100;
            mGrid.HorizontalAlignment = HorizontalAlignment.Left;
            mGrid.VerticalAlignment = VerticalAlignment.Top;
            mGrid.Width = img.Width + lb_html.Width;
            mGrid.Height = img.Height;
            mGrid.Background = System.Windows.Media.Brushes.Red;
            mGrid.Children.Add(img);
            mGrid.Children.Add(lb_html);

            main_bg.Children.Add(mGrid);
            
        }

        public void BrowseHttp(String site)
        {
            WebBrowser webBrowser = new WebBrowser();
            webBrowser.Source = new Uri(site);
            this.Content = webBrowser;
        }

        public void SearchManHuaRen(string manhuaming)
        {
            BrowseHttp(ManHuaRenSearchFormat(manhuaming));
        }

        private string ManHuaRenSearchFormat(string target)
        {
            string res = "https://www.manhuaren.com/search?title=" + target + "&language=1";
            Console.WriteLine(res);
            return res;
        }

    }
}
