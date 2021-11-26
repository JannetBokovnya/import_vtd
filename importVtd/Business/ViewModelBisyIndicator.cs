using System;
using System.Collections.Generic;
using System.Windows.Threading;
using Telerik.Windows.Controls;

namespace importVtd.Business
{
    public class ViewModelBisyIndicator : ViewModelBase
    {
        private bool isBusy;
		private bool isIndeterminate;
		private double progressValue;
		private TimeSpan displayAfter;
		private DispatcherTimer progressValueTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(40) };

        public ViewModelBisyIndicator()
		{
			this.DisplayAfterChoices = new List<TimeSpan>();
			this.DisplayAfterChoices.Add(TimeSpan.FromMilliseconds(0));
			this.DisplayAfterChoices.Add(TimeSpan.FromMilliseconds(500));
			this.DisplayAfterChoices.Add(TimeSpan.FromMilliseconds(1000));

			this.IsBusy = true;
		}

		public List<TimeSpan> DisplayAfterChoices
		{
			get;
			private set;
		}

		public TimeSpan DisplayAfter
		{
			get { return this.displayAfter; }
			set
			{
				if (this.displayAfter != value)
				{
					this.displayAfter = value;
					this.OnPropertyChanged(() => this.DisplayAfter);
				}
			}
		}

		public bool IsIndeterminate
		{
			get { return this.isIndeterminate; }
			set
			{

				if (this.isIndeterminate != value)
				{
					this.isIndeterminate = value;
					this.OnPropertyChanged(() => this.IsIndeterminate);
					this.OnPropertyChanged(() => this.BusyContent);
				}
			}
		}

		public double ProgressValue
		{
			get { return this.progressValue; }
			set
			{
				if (this.progressValue != value)
				{
					this.progressValue = value;
					this.OnPropertyChanged(() => this.ProgressValue);
					this.OnPropertyChanged(() => this.BusyContent);
				}
			}
		}

		public bool IsBusy
		{
			get { return this.isBusy; }
			set
			{
				if (this.isBusy != value)
				{
					this.isBusy = value;
					this.OnPropertyChanged(() => this.IsBusy);

					if (this.isBusy)
					{
						this.progressValueTimer.Tick += this.OnProgressValueTimerTick;
						this.progressValueTimer.Start();
						this.ProgressValue = 0;
					}
					else
					{
						this.progressValueTimer.Tick -= this.OnProgressValueTimerTick;
						this.progressValueTimer.Stop();
					}
				}
			}
		}

		public string BusyContent
		{
			get { return this.IsIndeterminate ? "Loading..." : string.Concat(this.ProgressValue, "% loaded"); }
		}

		private void OnProgressValueTimerTick(object sender, EventArgs e)
		{
			if (this.IsIndeterminate)
			{
				return;
			}

			if (this.ProgressValue < 100)
			{
				this.ProgressValue++;
			}
			else
			{
				this.IsBusy = false;
			}
		}
    }
}
