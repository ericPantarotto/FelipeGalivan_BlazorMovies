using System.Timers;
using Timer = System.Timers.Timer;

namespace BlazorMovies.Client.Auth
{
    public class TokenRenewer : IDisposable
    {
        Timer? timer;
        private readonly ILoginService loginService;
        public TokenRenewer(ILoginService loginService)
        {
            this.loginService = loginService;
        }


        public void Initiate()
        {
            timer = new Timer
            {
                Interval = 1000 * 60 * 4 // 4 minutes
            };
            timer.Elapsed += Timer_Elapsed!;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Console.WriteLine("timer elapsed");
            loginService.TryRenewToken();
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
