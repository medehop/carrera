using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrera
{
    /// <summary>
    /// Model represents a driver.
    /// </summary>
    public class DriverModel : PropertyChangedBase
    {
        private string _name;
        private int _lap = 0;
        private TimeSpan _fastestLap = new TimeSpan(0);
        private TimeSpan _lastLap = new TimeSpan(0);
        private DateTime? _timeStamp;

        private ObservableCollection<TimeSpan> _laps = new ObservableCollection<TimeSpan>();

        public DriverModel(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Represents driver's name.
        /// </summary>
        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }
        /// <summary>
        /// Represents a lap counter.
        /// </summary>
        public virtual int Lap
        {
            get
            {
                return _lap;
            }
            set
            {
                _lap = value;
                NotifyOfPropertyChange(() => Lap);
            }
        }

        /// <summary>
        /// Represents time of fastest lap.
        /// </summary>
        public virtual TimeSpan FastestLap
        {
            get
            {
                return _fastestLap;
            }
            set
            {
                _fastestLap = value;
                NotifyOfPropertyChange(() => FastestLap);
            }
        }
        /// <summary>
        /// Represents time of last lap.
        /// </summary>
        public virtual TimeSpan LastLap
        {
            get
            {
                return _lastLap;
            }
            set
            {
                _lastLap = value;
                NotifyOfPropertyChange(() => LastLap);
            }
        }

        ///// <summary>
        ///// Represents time stamp of last contact.
        ///// </summary>
        //public virtual DateTime? TimeStamp
        //{
        //    get
        //    {
        //        return _timeStamp;
        //    }
        //    set
        //    {
        //        _timeStamp = value;
        //        NotifyOfPropertyChange(() => TimeStamp);
        //    }
        //}

        public virtual void Update()
        {
            if (!_timeStamp.HasValue)
            {
                _timeStamp = DateTime.Now;
            }
            else
            {
                Lap++;
                LastLap = DateTime.Now.Subtract(_timeStamp.Value);

                if (LastLap < FastestLap || FastestLap == new TimeSpan(0))
                    FastestLap = LastLap;

                Laps.Add(LastLap);

                _timeStamp = DateTime.Now;
            }
        }

        /// <summary>
        /// Represents a list of lap times.
        /// </summary>
        public virtual ObservableCollection<TimeSpan> Laps
        {
            get
            {
                return _laps;
            }
        }
    }
}
