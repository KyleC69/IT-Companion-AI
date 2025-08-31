using System.Runtime.CompilerServices;

using LightweightAI.Core.Engine;



namespace LightweightAI.Core.Models
{


    public sealed class SnapshotTrendModel : ISnapshotTrendModel
    {
        private readonly TrendConfig _cfg;
        private readonly Queue<TrendPoint> _window;
        private double _ema;
        private bool _hasLast;
        private TrendPoint _last;
        private double _sum;
        private double _sumSquares;






        public SnapshotTrendModel(TrendConfig cfg)
        {
            
            _cfg = cfg;
            _window = new Queue<TrendPoint>();
            _ema = 0;
            _hasLast = false;
            _sum = 0;
            _sumSquares = 0;
        }



        public Snapshot Current { get; private set; }






        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Snapshot Update(in TrendPoint p)
        {
            if (_window.Count == 0)
            {
                _ema = p.Value;
            }
            else
            {
                _ema = _cfg.Alpha * p.Value + (1 - _cfg.Alpha) * _ema;
            }

            _window.Enqueue(p);
            _sum += p.Value;
            _sumSquares += p.Value * p.Value;

            if (_window.Count > _cfg.Window)
            {
                TrendPoint old = _window.Dequeue();
                _sum -= old.Value;
                _sumSquares -= old.Value * old.Value;
            }

            var n = _window.Count;
            var mean = _sum / Math.Max(1, n);
            var variance = Math.Max(0, _sumSquares / Math.Max(1, n) - mean * mean);
            var std = Math.Sqrt(variance);

            var trendDelta = 0d;
            if (_hasLast)
            {
                trendDelta = p.Value - _last.Value;
            }

            _last = p;
            _hasLast = true;

            this.Current = new Snapshot(
                p.At,
                n,
                _sum,
                mean,
                std,
                _ema,
                trendDelta
            );

            return this.Current;
        }
    }
}