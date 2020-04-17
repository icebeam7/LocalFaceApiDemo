using Xamarin.Forms;

namespace LocalFaceApiDemo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Views.FaceView());
        }
    }
}
