using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace dashboard_elite.Helpers
{
    public class ChartCircularBuffer
    {
        private readonly LinkedList<float> _buffer = new LinkedList<float>();
        private int _maxItemCount = 1000;
        private HWInfo.SENSOR_TYPE _sensorType;
        private string _unit;
        private float _minv;
        private float _maxv;

        public ChartCircularBuffer(HWInfo.SENSOR_TYPE sensorType, string unit)
        {
            _sensorType = sensorType;
            _unit = unit;
        }

        public void Put(float item)
        {
            lock (_buffer)
            {
                _buffer.AddFirst(item);
                if (_buffer.Count > _maxItemCount)
                {
                    _buffer.RemoveLast();
                }
            }
        }

        public string MinV()
        {
            return _minv == _maxv ? "" : _sensorType.NumberFormat( _unit, _minv);
        }

        public string MaxV()
        {
            return _minv == _maxv ? "" : _sensorType.NumberFormat(_unit, _maxv);
        }

        public string Svg(int chartImageDisplayWidth, int chartImageDisplayHeight)
        {
            var path = Read(chartImageDisplayWidth, chartImageDisplayHeight);

            var svg =
                $"<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 {chartImageDisplayWidth} {chartImageDisplayHeight}\" >";

            svg +=
                $"<path d=\"M0 0 L {chartImageDisplayWidth} 0 L {chartImageDisplayWidth} {chartImageDisplayHeight} L 0 {chartImageDisplayHeight} Z\" stroke=\"white\" stroke-width=\"2\" fill=\"transparent\"></path>";

            if (!string.IsNullOrEmpty(path))
            {
                svg += $"<path d=\" {path} \" stroke=\"yellow\" stroke-width=\"1\" fill=\"transparent\"></path>";

                svg += $"<text x=\"10\" y=\"10\" font-size=\"20\" stroke-width=\"0\" fill=\"white\" dominant-baseline=\"hanging\" >{MaxV()}</text>";

                svg += $"<text x=\"10\" y=\"{chartImageDisplayHeight-10}\" font-size=\"20\" stroke-width=\"0\" fill=\"white\" >{MinV()}</text>";
            }
            svg += "</svg>";

            return svg;
        }

        public string Read(int chartImageDisplayWidth, int chartImageDisplayHeight)
        {
            lock (_buffer)
            {
                var path = new StringBuilder();
                var c = _buffer.Count;
                if (c > chartImageDisplayWidth)
                {
                    c = chartImageDisplayWidth;
                }

                var minv = (float)1e10;
                var maxv = (float)-1e10;

                var node = _buffer.First;

                for (var index = 0; index < c; index++)
                {
                    var y = node.Value;

                    if (y < minv) minv = y;
                    if (y > maxv) maxv = y;

                    node = node.Next;
                }

                var range = maxv - minv;

                if (range > 0)
                {
                    minv -= (float) (range * 0.1);
                    maxv += (float) (range * 0.1);
                }
                else
                {
                    minv -= (float)(maxv * 0.1);
                    maxv += (float)(maxv * 0.1);
                }

                if (minv < 0) minv = 0;

                range = maxv - minv;

                if (range > 0)
                {
                    var yFactor = chartImageDisplayHeight / range;

                    node = _buffer.First;

                    for (var index = 0; index < c; index++)
                    {
                        var y = node.Value;

                        var x = chartImageDisplayWidth - 1 - index;

                        path.Append($"L{x.ToString("F", CultureInfo.InvariantCulture)} {(chartImageDisplayHeight - (y - minv) * yFactor).ToString("F", CultureInfo.InvariantCulture)} ");

                        node = node.Next;
                    }

                }

                if (path.Length > 0)
                {
                    path[0] = 'M';
                }

                _minv = minv;
                _maxv = maxv;

                return path.ToString();
            }
        }
    }
}
