using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace ScheduledTileSample
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// このページがフレームに表示されるときに呼び出されます。
        /// </summary>
        /// <param name="e">このページにどのように到達したかを説明するイベント データ。Parameter 
        /// プロパティは、通常、ページを構成するために使用します。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void tileUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // タイルのテンプレート選択
            // http://msdn.microsoft.com/ja-jp/library/windows/apps/hh761491.aspx#TileSquareText03
            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideImageAndText01);

            // キューに複数のタイル通知を設定可能にする
            //TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);

            XmlNodeList tileTextAttributes = tileXml.GetElementsByTagName("text");
            tileTextAttributes[0].InnerText = this.tileText.Text;

            XmlNodeList tileImageAttributes = tileXml.GetElementsByTagName("image");
            ((XmlElement)tileImageAttributes[0]).SetAttribute("src", "ms-appx:///Assets/wideTile.png");
            ((XmlElement)tileImageAttributes[0]).SetAttribute("alt", "wideTile.png");

            XmlDocument squareTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareText04);
            XmlNodeList squareTileTextAttributes = squareTileXml.GetElementsByTagName("text");
            squareTileTextAttributes[0].AppendChild(squareTileXml.CreateTextNode(this.tileText.Text));
            IXmlNode node = tileXml.ImportNode(squareTileXml.GetElementsByTagName("binding").Item(0), true);
            tileXml.GetElementsByTagName("visual").Item(0).AppendChild(node);

            Int16 dueTimeInSeconds = 15;
            DateTime dueTime = DateTime.Now.AddSeconds(dueTimeInSeconds);


            ScheduledTileNotification tileNotification = new ScheduledTileNotification(tileXml,dueTime);
            tileNotification.Tag = this.tileText.Text;

            //tileNotification.ExpirationTime = DateTimeOffset.UtcNow.AddSeconds(10);

            TileUpdateManager.CreateTileUpdaterForApplication().AddToSchedule(tileNotification);


        }
    }
}
