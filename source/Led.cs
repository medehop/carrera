using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Threading;
using System.Linq;

namespace Carrera
{

    class Led
    {

        private ObservableCollection<Color> _leds = new ObservableCollection<Color>(new [] {Colors.Gray, Colors.Gray, Colors.Gray, Colors.Gray, Colors.Gray});
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public void Start()
        {                       
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Start();
        }

        public void Stop()
        {
            dispatcherTimer.Stop();
            dispatcherTimer.Tick -= new EventHandler(dispatcherTimer_Tick);

            for (int i = 0; i < _leds.Count; i++)
            {
                _leds[i] = Colors.Gray;
            }
        }

        public ObservableCollection<Color> Signals => _leds;


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (!_leds.Any(p => p == Colors.Gray))
            {
                Stop();
            }
            else
            {
                for (int i = 0; i <= _leds.Count; i++)
                {
                    if (_leds[i] == Colors.Gray)
                    {
                        _leds[i] = Colors.Red;
                        break;
                    }
                }

            }
        }
    }
}
