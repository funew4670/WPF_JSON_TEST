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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.IO;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Reflection;
using System.Runtime.Remoting;
using System.Data;



namespace WpfApp1
{




    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        //object Myobject = null;
        //List<object> changeObject = new List<object> ();
        WpfApp1.Properties.SelectJsonClass FileObject = new WpfApp1.Properties.SelectJsonClass();

        public MainWindow()
        {
            InitializeComponent();
            //List<YTSubscribeInfo> newlist = LoadCollectionData();           

            //var myClassType = Type.GetType("YTSubscribeInfo");
            //string aa = ""; 
            //dataGrid1.ItemsSource = newlist;
            //string ojson = JsonConvert.SerializeObject(newlist);
            //System.IO.File.WriteAllText(@"F:\download\output.json", ojson);



        }


        private List<WpfApp1.Properties.YTSubscribeInfo> LoadCollectionData()
        {
            List<WpfApp1.Properties.YTSubscribeInfo> SubscribeInfos = new List<WpfApp1.Properties.YTSubscribeInfo>();
            SubscribeInfos.Add(new WpfApp1.Properties.YTSubscribeInfo()
            {
                ID = 101,
                ChannelName = "CCTV纪录",
                ChannelLink = "https://www.youtube.com/channel/UCAYkj2Fz9EvAe2fGJEGMXnQ",
                DOB = new DateTime(1975, 2, 23)

            });

            SubscribeInfos.Add(new WpfApp1.Properties.YTSubscribeInfo()
            {
                ID = 201,
                ChannelName = "35線上賞屋 ",
                ChannelLink = "https://www.youtube.com/c/35%E7%B7%9A%E4%B8%8A%E8%B3%9E%E5%B1%8B",
                DOB = new DateTime(1982, 4, 12)

            });

            SubscribeInfos.Add(new WpfApp1.Properties.YTSubscribeInfo()
            {
                ID = 244,
                ChannelName = "中國浙江衛視官方頻道",
                ChannelLink = "https://www.youtube.com/channel/UC52EYUUSlzcYqHj-qsx0RHg",
                DOB = new DateTime(1985, 9, 11)

            });

            return SubscribeInfos;
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".json";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox  
            if (result == true)
            {
                // Open document 
                string filepath = dlg.FileName;
                string fileClass = System.IO.Path.GetFileNameWithoutExtension(filepath);
                TextBox1.Text = filepath;

                //read json data
                string sJson = System.IO.File.ReadAllText(filepath);

                //Check if exists, instantiate if so.  
                //var myClassType = Type.GetType(fileClass);
                //ObjectHandle handle = Activator.CreateInstance("YTSubscribeInfo, WpfApp1");
                //Object P = handle.Unwrap();
                //Type t = P.GetType();



                // try to read data by filename and cast to specific class relate to filename
                // unNecessary , using JObject instead

                //const string objectToInstantiate = "WpfApp1.Properties.YTSubscribeInfo, WpfApp1";
                //var objectType = Type.GetType(objectToInstantiate);
                //var instantiatedObject = Activator.CreateInstance(objectType);


                //ObjectCreationHandling instance = Activator.CreateInstance("class1", Type myClassType); 

                //if (instantiatedObject != null)
                //{
                //}

                //var newlist = new List<dynamic>();
                //newlist = JsonConvert.DeserializeObject<List<dynamic>>(sJson);

                


                try {
                    //Cast<JObject>().ToList();
                    JArray newlist = (JArray)JsonConvert.DeserializeObject(sJson);
                    //set global object
                    FileObject.Myobject = newlist.ToObject<List<JObject>>();
                    FileObject.FilePath = filepath;
                    //bind data
                    dataGrid1.ItemsSource = newlist;
                }
                catch {
                    //open new window show json schema
                    var token = JToken.Parse(sJson);
                    var children = new List<JToken>();
                    if (token != null)
                    {
                        children.Add(token);
                    }
                    Window2 win2 = new Window2();
                    win2.treeView1.ItemsSource = null;
                    win2.treeView1.Items.Clear();
                    win2.treeView1.ItemsSource = children;
                    win2.Show();
                }

                

                }
        }


        private void dataGrid1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            //Newtonsoft.Json.Linq.JObject

            DataGrid grid = (DataGrid)sender;
            //dynamic selectRow;
            JObject selectRow;
            //selectRow = (JObject)grid.CurrentItem;
            //selectRow = grid.CurrentItem;
            //int iID = selectRow.ID;
            DataGridRow oRow = (DataGridRow)e.Row;
            selectRow = (JObject)oRow.Item;

            int iID = (int)selectRow.GetValue("ID");

            //MessageBox.Show(iID.ToString() + selectRow.GetValue("ChannelName") + " editvalue:" + ((TextBox)e.EditingElement).Text);

            String sHeader = grid.CurrentCell.Column.Header.ToString();
            String sSource = selectRow.GetValue("ChannelName").ToString();
            String sEdit = ((TextBox)e.EditingElement).Text.ToString();

            if (sEdit != sSource)
            {
                Properties.changedRow obj = new Properties.changedRow(iID, sHeader, sSource, sEdit);
                FileObject.addChanged(obj);
            }



            //List<JObject> myDataset = dataGrid1.ItemsSource.Cast<JObject>().ToList();

            //var sourceRow = from oJObject in myDataset
            //                where (int)oJObject.GetValue("ID") == iID
            //                select oJObject;

            //MessageBox.Show(iID.ToString() + ((JObject)sourceRow.First()).GetValue("ChannelName"));



            //MessageBox.Show(grid.CurrentCell.Column.Header.ToString()) ;
            //MessageBox.Show(((TextBox)e.EditingElement).Text);
            //MessageBox.Show(();

        }

        private void dataGrid1_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {

        }

        private void dataGrid1_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            string aaa;
            string bbb;
            int a = 3;
            int b = 2;


        }

        private void dataGrid1_CurrentCellChanged(object sender, EventArgs e)
        {
            // MessageBox.Show(dataGrid1.CurrentCell.Column.DisplayIndex.ToString());
        }


        private void buttonSave_Click_1(object sender, RoutedEventArgs e)
        {
            bool bIsSet = false;
            string sLog = "";
            FileObject.changeObject.ForEach(delegate (Properties.changedRow row)
            {

                if (FileObject.Myobject.Exists(x => Convert.ToInt32(x.GetValue("ID").ToString()) == row.ID))
                {
                    var obj = FileObject.Myobject.FirstOrDefault((x => Convert.ToInt32(x.GetValue("ID").ToString()) == row.ID));
                    if (obj != null)
                    {
                        //obj.(row.ColumnName.ToString()) = row.NewData.ToString();
                        obj[row.ColumnName.ToString()] = row.NewData.ToString();
                        //objName.GetType().GetProperty("nameOfProperty").SetValue(objName, objValue, null)
                        bIsSet = true;

                        //save log
                        sLog += DateTime.Now.ToString("yyyy-MM-dd T", DateTimeFormatInfo.InvariantInfo) +
                                "update " + row.ColumnName + "set value " + row.OldData + "to " + row.NewData + System.Environment.NewLine;
                        saveLog(sLog);

                    }


                }
            });

            if (bIsSet)
            {
                //savefile
                //string sjson = JsonConvert.SerializeObject(FileObject.Myobject, Formatting.Indented);
                //saveFile(sjson);

                FileObject.savefile();

            }

        }





        public void saveLog(string slog)
        {
            string sLogPath = Directory.GetCurrentDirectory();

            // Set a variable to the Documents path.
            //string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(sLogPath, "log.txt"), true))
            {
                outputFile.WriteLine(slog);
            }
        }

        public void saveFile(string sJson)
        {
            //string docPath = Directory.GetCurrentDirectory();

            // Set a variable to the Documents path.
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(docPath, "log.txt")))
            {
                outputFile.WriteLine(sJson);
            }
        }


    }


}




// To DO 
//  try window2 MethodToValueConverter method that bind child object
//
//
//