// 
// Copyright (C) 2006, NinjaTrader LLC <www.ninjatrader.com>.
// NinjaTrader reserves the right to modify or overwrite this NinjaScript component with each release.
//

#region Using declarations
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Xml.Serialization;
using NinjaTrader.Data;
using NinjaTrader.Gui.Chart;
#endregion

// This namespace holds all indicators and is required. Do not change it.
namespace NinjaTrader.Indicator
{
	/// <summary>
	/// Directional Movement (DM). This is the same indicator as the ADX, with the addition of the two directional movement indicators +DI and -DI. +DI and -DI measure upward and downward momentum. A buy signal is generated when +DI crosses -DI to the upside. A sell signal is generated when -DI crosses +DI to the downside.
	/// </summary>
	[Description("This is the same indicator as the ADX, with the addition of the two directional movement indicators +DI and -DI. +DI and -DI measure upward and downward momentum. A buy signal is generated when +DI crosses -DI to the upside. A sell signal is generated when -DI crosses +DI to the downside.")]
	public class DM : Indicator
	{
		#region Variables
		private int period = 14;

		private DataSeries dmPlus;
		private DataSeries dmMinus;
		private DataSeries sumDmPlus;
		private DataSeries sumDmMinus;
		private DataSeries sumTr;
		private DataSeries tr;
		#endregion

		/// <summary>
		/// This method is used to configure the indicator and is called once before any bar data is loaded.
		/// </summary>
		protected override void Initialize()
		{
			Add(new Plot(new Pen(Color.DarkGreen, 3), "ADX"));
			Add(new Plot(Color.Blue, "+DI"));
			Add(new Plot(Color.Red, "-DI"));

			Add(new Line(Color.DarkViolet, 25, "Lower"));
			Add(new Line(Color.YellowGreen, 75, "Upper"));

			dmPlus = new DataSeries(this);
			dmMinus = new DataSeries(this);
			sumDmPlus = new DataSeries(this);
			sumDmMinus = new DataSeries(this);
			sumTr = new DataSeries(this);
			tr = new DataSeries(this);
		}

		/// <summary>
		/// Called on each bar update event (incoming tick)
		/// </summary>
		protected override void OnBarUpdate()
		{
			double trueRange = High[0] - Low[0];
			if (CurrentBar == 0)
			{
				tr.Set(trueRange);
				dmPlus.Set(0);
				dmMinus.Set(0);
				sumTr.Set(tr[0]);
				sumDmPlus.Set(dmPlus[0]);
				sumDmMinus.Set(dmMinus[0]);
				Value.Set(50);
			}
			else
			{
				tr.Set(Math.Max(Math.Abs(Low[0] - Close[1]), Math.Max(trueRange, Math.Abs(High[0] - Close[1]))));
				dmPlus.Set(High[0] - High[1] > Low[1] - Low[0] ? Math.Max(High[0] - High[1], 0) : 0);
				dmMinus.Set(Low[1] - Low[0] > High[0] - High[1] ? Math.Max(Low[1] - Low[0], 0) : 0);

				if (CurrentBar < Period)
				{
					sumTr.Set(sumTr[1] + tr[0]);
					sumDmPlus.Set(sumDmPlus[1] + dmPlus[0]);
					sumDmMinus.Set(sumDmMinus[1] + dmMinus[0]);
				}
				else
				{
					sumTr.Set(sumTr[1] - sumTr[1] / Period + tr[0]);
					sumDmPlus.Set(sumDmPlus[1] - sumDmPlus[1] / Period + dmPlus[0]);
					sumDmMinus.Set(sumDmMinus[1] - sumDmMinus[1] / Period + dmMinus[0]);
				}

				double diPlus = 100 * (sumTr[0] == 0 ? 0 : sumDmPlus[0] / sumTr[0]);
				double diMinus = 100 * (sumTr[0] == 0 ? 0 : sumDmMinus[0] / sumTr[0]);
				double diff = Math.Abs(diPlus - diMinus);
				double sum = diPlus + diMinus;

				Value.Set(sum == 0 ? 50 : ((Period - 1) * Value[1] + 100 * diff / sum) / Period);

				DiPlus.Set(diPlus);
				DiMinus.Set(diMinus);
			}
		}

		#region Properties
		/// <summary>
		/// </summary>
		[Browsable(false)]
		[XmlIgnore()]
		public DataSeries DiPlus
		{
			get { return Values[1]; }
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[XmlIgnore()]
		public DataSeries DiMinus
		{
			get { return Values[2]; }
		}
		
		/// <summary>
		/// </summary>
		[Description("Numbers of bars used for calculations")]
		[GridCategory("Parameters")]
		public int Period
		{
			get { return period; }
			set { period = Math.Max(1, value); }
		}


		#endregion
	}
}

#region NinjaScript generated code. Neither change nor remove.
// This namespace holds all indicators and is required. Do not change it.
namespace NinjaTrader.Indicator
{
    public partial class Indicator : IndicatorBase
    {
        private DM[] cacheDM = null;

        private static DM checkDM = new DM();

        /// <summary>
        /// This is the same indicator as the ADX, with the addition of the two directional movement indicators +DI and -DI. +DI and -DI measure upward and downward momentum. A buy signal is generated when +DI crosses -DI to the upside. A sell signal is generated when -DI crosses +DI to the downside.
        /// </summary>
        /// <returns></returns>
        public DM DM(int period)
        {
            return DM(Input, period);
        }

        /// <summary>
        /// This is the same indicator as the ADX, with the addition of the two directional movement indicators +DI and -DI. +DI and -DI measure upward and downward momentum. A buy signal is generated when +DI crosses -DI to the upside. A sell signal is generated when -DI crosses +DI to the downside.
        /// </summary>
        /// <returns></returns>
        public DM DM(Data.IDataSeries input, int period)
        {
            if (cacheDM != null)
                for (int idx = 0; idx < cacheDM.Length; idx++)
                    if (cacheDM[idx].Period == period && cacheDM[idx].EqualsInput(input))
                        return cacheDM[idx];

            lock (checkDM)
            {
                checkDM.Period = period;
                period = checkDM.Period;

                if (cacheDM != null)
                    for (int idx = 0; idx < cacheDM.Length; idx++)
                        if (cacheDM[idx].Period == period && cacheDM[idx].EqualsInput(input))
                            return cacheDM[idx];

                DM indicator = new DM();
                indicator.BarsRequired = BarsRequired;
                indicator.CalculateOnBarClose = CalculateOnBarClose;
#if NT7
                indicator.ForceMaximumBarsLookBack256 = ForceMaximumBarsLookBack256;
                indicator.MaximumBarsLookBack = MaximumBarsLookBack;
#endif
                indicator.Input = input;
                indicator.Period = period;
                Indicators.Add(indicator);
                indicator.SetUp();

                DM[] tmp = new DM[cacheDM == null ? 1 : cacheDM.Length + 1];
                if (cacheDM != null)
                    cacheDM.CopyTo(tmp, 0);
                tmp[tmp.Length - 1] = indicator;
                cacheDM = tmp;
                return indicator;
            }
        }
    }
}

// This namespace holds all market analyzer column definitions and is required. Do not change it.
namespace NinjaTrader.MarketAnalyzer
{
    public partial class Column : ColumnBase
    {
        /// <summary>
        /// This is the same indicator as the ADX, with the addition of the two directional movement indicators +DI and -DI. +DI and -DI measure upward and downward momentum. A buy signal is generated when +DI crosses -DI to the upside. A sell signal is generated when -DI crosses +DI to the downside.
        /// </summary>
        /// <returns></returns>
        [Gui.Design.WizardCondition("Indicator")]
        public Indicator.DM DM(int period)
        {
            return _indicator.DM(Input, period);
        }

        /// <summary>
        /// This is the same indicator as the ADX, with the addition of the two directional movement indicators +DI and -DI. +DI and -DI measure upward and downward momentum. A buy signal is generated when +DI crosses -DI to the upside. A sell signal is generated when -DI crosses +DI to the downside.
        /// </summary>
        /// <returns></returns>
        public Indicator.DM DM(Data.IDataSeries input, int period)
        {
            return _indicator.DM(input, period);
        }
    }
}

// This namespace holds all strategies and is required. Do not change it.
namespace NinjaTrader.Strategy
{
    public partial class Strategy : StrategyBase
    {
        /// <summary>
        /// This is the same indicator as the ADX, with the addition of the two directional movement indicators +DI and -DI. +DI and -DI measure upward and downward momentum. A buy signal is generated when +DI crosses -DI to the upside. A sell signal is generated when -DI crosses +DI to the downside.
        /// </summary>
        /// <returns></returns>
        [Gui.Design.WizardCondition("Indicator")]
        public Indicator.DM DM(int period)
        {
            return _indicator.DM(Input, period);
        }

        /// <summary>
        /// This is the same indicator as the ADX, with the addition of the two directional movement indicators +DI and -DI. +DI and -DI measure upward and downward momentum. A buy signal is generated when +DI crosses -DI to the upside. A sell signal is generated when -DI crosses +DI to the downside.
        /// </summary>
        /// <returns></returns>
        public Indicator.DM DM(Data.IDataSeries input, int period)
        {
            if (InInitialize && input == null)
                throw new ArgumentException("You only can access an indicator with the default input/bar series from within the 'Initialize()' method");

            return _indicator.DM(input, period);
        }
    }
}
#endregion
