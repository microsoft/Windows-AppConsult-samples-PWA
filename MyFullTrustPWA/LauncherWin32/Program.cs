using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LauncherWin32
{


    class Program
    {
        static void Main(string[] args)
        {

            MainAsync().GetAwaiter().GetResult();

        }
        static async Task MainAsync()
        {
            var uri = new Uri("myuwp:");
            var options = new Windows.System.LauncherOptions();
            // Launch the URI without a warning prompt
            options.TreatAsUntrusted = false;
            var res = await Windows.System.Launcher.LaunchUriAsync(uri, options);

        }
    }

}