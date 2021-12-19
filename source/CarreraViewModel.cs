using Caliburn.Micro;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.IO.Ports;

namespace Carrera
{
    class CarreraViewModel : PropertyChangedBase, IDisposable
    {
        private SerialPort _serialPort;
        private DateTime _timeStamp1;
        private DateTime _timeStamp2;
        private Led _led;

        public DriverModel DriverOne { get; } = new DriverModel("BMW");
        public DriverModel DriverTwo { get; } = new DriverModel("Mercedes");

        public CarreraViewModel()
        {
            InitComPort(4);
            _led = new Led();
            
        }

        public ObservableCollection<Color> Led => _led.Signals;

        public void Start()
        {
            DriverOne.Update();
            DriverTwo.Update();
            _led.Start();
        }

        public void Stop()
        {
        }

        private void InitComPort(int port)
        {
            try
            {
                _serialPort = new SerialPort(string.Format("COM{0}", port));

                _serialPort.BaudRate = 9600;
                _serialPort.Parity = Parity.None;
                _serialPort.StopBits = StopBits.One;
                _serialPort.DataBits = 8;
                _serialPort.Handshake = Handshake.None;
                _serialPort.RtsEnable = false;

                _serialPort.PinChanged += new SerialPinChangedEventHandler(PinChangedReveiveHandler);

                _serialPort.Open();
            }
            catch (Exception)
            {

                //throw;
            }

        }

        private void CloseComPort()
        {
            if (_serialPort != null)
                _serialPort.Close();
        }

        private void PinChangedReveiveHandler(
                    object sender,
                    SerialPinChangedEventArgs e)
        {
            switch (e.EventType)
            {
                case SerialPinChange.CDChanged:
                    if (_timeStamp1.AddSeconds(1) < DateTime.Now)
                    {
                        _timeStamp1 = DateTime.Now;
                        DriverOne.Update();
                    }
                    break;
                case SerialPinChange.DsrChanged:
                    if (_timeStamp2.AddSeconds(1) < DateTime.Now)
                    {
                        _timeStamp2 = DateTime.Now;
                        DriverTwo.Update();
                    }
                    break;
            }
        }

        public void Dispose()
        {
            CloseComPort();
        }
    }
}
