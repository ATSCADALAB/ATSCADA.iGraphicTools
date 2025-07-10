using System;

namespace ATSCADA.iGraphicTools.AnimateGauge.Utils
{
    public class EaseFunction
    {
        private double m_dLength = 1000.0;
        private EaseFunctionType m_type;
        private double m_dFrom;
        private double m_dTo;
        private Func<double, double> m_function;

        public double ToValue
        {
            get
            {
                return this.m_dTo;
            }
        }

        public EaseFunction(
          EaseFunctionType type,
          EaseMode mode,
          double lengthMs,
          double fromValue,
          double toValue)
        {
            this.m_type = type;
            this.m_dLength = lengthMs;
            this.m_dFrom = fromValue;
            this.m_dTo = toValue;
            this.m_function = EaseFunction.GetEasingFunction(this.m_type);
            if (mode == EaseMode.InOut)
                return;
            this.m_function = EaseFunction.TransformEase(this.m_function, mode == EaseMode.In);
        }

        public double GetValue(double curMs)
        {
            if (curMs > this.m_dLength)
                throw new ArgumentOutOfRangeException(nameof(curMs));
            double val1 = this.m_dFrom + (this.m_dTo - this.m_dFrom) * this.m_function(curMs / this.m_dLength);
            return this.m_dFrom > this.m_dTo ? Math.Max(val1, this.m_dTo) : Math.Min(val1, this.m_dTo);
        }

        private static Func<double, double> GetEasingFunction(EaseFunctionType type)
        {
            switch (type)
            {
                case EaseFunctionType.Linear:
                    return (Func<double, double>)(d => d);
                case EaseFunctionType.Quadratic:
                    return EaseFunction.GetPowerEase(2);
                case EaseFunctionType.Sine:
                    return (Func<double, double>)(d => (Math.Sin((d - 0.5) * Math.PI) + 1.0) / 2.0);
                case EaseFunctionType.Cubic:
                    return EaseFunction.GetPowerEase(3);
                default:
                    throw new NotImplementedException("Easing function not supported");
            }
        }

        private static Func<double, double> GetPowerEase(int power)
        {
            return (Func<double, double>)(d => d > 0.5 ? 1.0 - Math.Pow((1.0 - d) * 2.0, (double)power) / 2.0 : Math.Pow(d * 2.0, (double)power) / 2.0);
        }

        private static Func<double, double> TransformEase(
          Func<double, double> original,
          bool bToEaseIn)
        {
            return !bToEaseIn ? (Func<double, double>)(percent => (original(percent / 2.0 + 0.5) - 0.5) * 2.0) : (Func<double, double>)(percent => original(percent / 2.0) * 2.0);
        }
    }
}
