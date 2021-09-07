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
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WpfApp1
{
    /// <summary>
    /// Window2.xaml 的互動邏輯
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();            
        }


        static void WalkNode(JToken node, Action<JObject> action)
        {
            if (node.Type == JTokenType.Object)
            {
                action((JObject)node);

                foreach (JProperty child in node.Children<JProperty>())
                {
                    WalkNode(child.Value, action);
                }
            }
            else if (node.Type == JTokenType.Array)
            {
                foreach (JToken child in node.Children())
                {
                    WalkNode(child, action);
                }
            }
        }


    }


    // Respectfully adapted from https://stackoverflow.com/questions/502250/bind-to-a-method-in-wpf/844946#844946
    // stackoverflow reference https://stackoverflow.com/questions/23812357/how-to-bind-dynamic-json-into-treeview-wpf/28097883

    public sealed class MethodToValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var methodName = parameter as string;
            if (value == null || methodName == null)
                return null;
            var methodInfo = value.GetType().GetMethod(methodName, new Type[0]);
            if (methodInfo == null)
                return null;
            return methodInfo.Invoke(value, new object[0]);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException(GetType().Name + " can only be used for one way conversion.");
        }
    }






}


//walk node
//https://stackoverflow.com/questions/16181298/how-to-do-recursive-descent-of-json-using-json-net
//
//JToken node = JToken.Parse(json);

//WalkNode(node, n =>
//{
//    JToken token = n["Title"];
//    if (token != null && token.Type == JTokenType.String)
//    {
//        string title = token.Value<string>();
//        Console.WriteLine(title);
//    }
//});
//        }

//        static void WalkNode(JToken node, Action<JObject> action)
//{
//    if (node.Type == JTokenType.Object)
//    {
//        action((JObject)node);

//        foreach (JProperty child in node.Children<JProperty>())
//        {
//            WalkNode(child.Value, action);
//        }
//    }
//    else if (node.Type == JTokenType.Array)
//    {
//        foreach (JToken child in node.Children())
//        {
//            WalkNode(child, action);
//        }
//    }
//}

