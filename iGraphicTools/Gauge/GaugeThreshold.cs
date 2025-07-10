using System.Collections;
using System.Drawing;

namespace ATSCADA.iGraphicTools.Gauge
{
    public class GaugeThreshold
	{
		public Color Color { get; set; } = Color.Empty;

		public double StartValue { get; set; } = 0f;

		public double EndValue { get; set; } = 1f;		
	
		public bool IsInRange(double value)
		{
			if (value > EndValue)
				return false;

			if (value < StartValue)
				return false;

			return true;
		}		
	}

	public class GaugeThresholdCollection : CollectionBase
	{		
		private bool isReadOnly = false;
	
		
		public virtual GaugeThreshold this[int index]
		{
			get { return (GaugeThreshold)InnerList[index]; }
			set { InnerList[index] = value; }
		}

		public virtual bool IsReadOnly
		{
			get { return isReadOnly; }
		}
		
		public virtual void Add(GaugeThreshold sector)
		{
			InnerList.Add(sector);
		}
		
		public virtual bool Remove(GaugeThreshold sector)
		{
			bool result = false;
			
			for (int i = 0; i < InnerList.Count; i++)
			{				
				GaugeThreshold obj = (GaugeThreshold)InnerList[i];
				
				if ((obj.StartValue == sector.StartValue) &&
					(obj.EndValue == sector.EndValue))
				{					
					InnerList.RemoveAt(i);
					result = true;
					break;
				}
			}

			return result;
		}

		public bool Contains(GaugeThreshold sector)
		{			
			foreach (GaugeThreshold obj in InnerList)
			{				
				if ((obj.StartValue == sector.StartValue) &&
					(obj.EndValue == sector.EndValue))
				{					
					return true;
				}
			}
			
			return false;
		}	
	}
}
