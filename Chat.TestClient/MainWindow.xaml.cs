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
using Chat.Core;
using Chat.Core.Communication;
using Chat.Core.Communication.Services;
using Chat.Core.DependencyInjection;
using Chat.Interface.Contracts;
using Chat.Interface.Contracts.Test;
using Unity;

namespace Chat.TestClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IChatCallbackService _chatCallbackService;


        public MainWindow()
        {
            DI.InstallFrom(new[]
            {
                CoreAssemblyDefiningType.Assembly
            });

            InitializeComponent();

            _chatCallbackService = DI.Container.Resolve<IChatCallbackService>();
            _chatCallbackService.RegisterCallbackProcessor(new TestCallbackProcessor());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var rqSender = DI.Container.Resolve<IRequestSender>();
            var response = rqSender.SendRequest(new TestRequest("test"));
            MessageBox.Show(response.Test, "Response");
        }
    }
}
