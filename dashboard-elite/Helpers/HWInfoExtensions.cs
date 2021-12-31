using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace dashboard_elite.Helpers
{
    public static class HWInfoExtensions
    {

        public static string NumberFormat(this HWInfo.SENSOR_TYPE sensorType, string unit, double value)
        {
            var valstr = "?";

            switch (sensorType)
            {
                case HWInfo.SENSOR_TYPE.SENSOR_TYPE_VOLT:
                    valstr = value.ToString("N3");
                    break;
                case HWInfo.SENSOR_TYPE.SENSOR_TYPE_CURRENT:
                    valstr = value.ToString("N1");
                    break;
                case HWInfo.SENSOR_TYPE.SENSOR_TYPE_POWER:
                    valstr = value.ToString("N1");
                    break;

                case HWInfo.SENSOR_TYPE.SENSOR_TYPE_CLOCK:
                    valstr = value.ToString("N1");
                    break;
                case HWInfo.SENSOR_TYPE.SENSOR_TYPE_USAGE:
                    valstr = value.ToString("N1");
                    break;
                case HWInfo.SENSOR_TYPE.SENSOR_TYPE_TEMP:
                    valstr = value.ToString("N1");
                    break;

                case HWInfo.SENSOR_TYPE.SENSOR_TYPE_FAN:
                    valstr = value.ToString("N0");
                    break;

                case HWInfo.SENSOR_TYPE.SENSOR_TYPE_OTHER:

                    if (unit == "Yes/No")
                    {
                        return value == 0 ? "No" : "Yes";
                    }
                    else if (unit.EndsWith("GT/s") || unit == "x" || unit == "%")
                    {
                        valstr = value.ToString("N1");
                    }
                    else if (unit.EndsWith("/s"))
                    {
                        valstr = value.ToString("N3");
                    }
                    else if (unit.EndsWith("MB") || unit.EndsWith("GB") || unit == "T" || unit == "FPS")
                    {
                        valstr = value.ToString("N0");
                    }
                    else
                        valstr = value.ToString();

                    break;

                case HWInfo.SENSOR_TYPE.SENSOR_TYPE_NONE:
                    valstr = value.ToString();
                    break;

            }

            return (valstr + " " + unit).Trim();
        }


    }
}
