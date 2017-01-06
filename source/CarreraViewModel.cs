using Caliburn.Micro;
using System;
using System.IO.Ports;

namespace Carrera
{
    class CarreraViewModel : PropertyChangedBase, IDisposable
    {
        private SerialPort _serialPort;

        private DriverModel _driverOne = new DriverModel("Lane left");
        private DriverModel _driverTwo = new DriverModel("Lane right");


        //int _lapCounter1 = 0;
        //int _lapCounter2 = 0;
        DateTime _timeStamp1;
        DateTime _timeStamp2;

        //DateTime? _lastLapTime1;
        //DateTime? _lastLapTime2;

        //string _lastLapTimeString1;
        //string _lastLapTimeString2;
        public DriverModel DriverOne { get { return _driverOne; } }
        public DriverModel DriverTwo { get { return _driverTwo; } }

        public CarreraViewModel()
        {
            InitComPort(4);
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

        public void Start()
        {
            _driverOne.Update();
        }

        //public int LapCounter1
        //{   get
        //    {
        //        return _lapCounter1;
        //    }
        //    private set
        //    {
        //        _lapCounter1 = value;
        //        NotifyOfPropertyChange(() => LapCounter1);
        //    }
        //}

        //public int LapCounter2
        //{
        //    get
        //    {
        //        return _lapCounter2;
        //    }
        //    private set
        //    {
        //        _lapCounter2 = value;
        //        NotifyOfPropertyChange(() => LapCounter2);
        //    }
        //}

        //public string LastLapTimeString1
        //{
        //    get
        //    {
        //        return _lastLapTimeString1;
        //    }
        //    private set
        //    {
        //        _lastLapTimeString1 = value;
        //        NotifyOfPropertyChange(() => LastLapTimeString1);
        //    }
        //}

        //public string LastLapTimeString2
        //{
        //    get
        //    {
        //        return _lastLapTimeString2;
        //    }
        //    private set
        //    {
        //        _lastLapTimeString2 = value;
        //        NotifyOfPropertyChange(() => LastLapTimeString2);
        //    }
        //}

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
                        _driverOne.Update();
                    }
                    break;
                case SerialPinChange.DsrChanged:
                    if (_timeStamp2.AddSeconds(1) < DateTime.Now)
                    {
                        _timeStamp2 = DateTime.Now;
                        _driverTwo.Update();
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
