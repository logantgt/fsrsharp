namespace FSRSharp
{
    public class FsrsAlgorithm
    {
        public Parameters P { get; set; }

        public FsrsAlgorithm()
        {
            P = new Parameters();
        }

        public Dictionary<CardRating, SchedulingInfo> Repeat(Card card)
        {
            card = card.Clone();
            card.ElapsedDays = card.State == CardState.New ? 0 : DateTime.Now.Subtract(card.LastReview).Days;
            card.LastReview = DateTime.Now;
            card.Reps += 1;

            SchedulingCards s = new SchedulingCards(card);

            s.UpdateState(card.State);

            if (card.State == CardState.New)
            {
                InitDs(s);

                s.Again.Due = DateTime.Now.Add(TimeSpan.FromMinutes(1));
                s.Hard.Due = DateTime.Now.Add(TimeSpan.FromMinutes(5));
                s.Good.Due = DateTime.Now.Add(TimeSpan.FromMinutes(10));

                float easyInterval = NextInterval(s.Easy.Stability);

                s.Easy.ScheduledDays = (int)easyInterval;
                s.Easy.Due = DateTime.Now.Add(TimeSpan.FromDays(easyInterval));
            }
            else if (card.State == CardState.Review)
            {
                int interval = card.ElapsedDays;

                float lastD = card.Difficulty;
                float lastS = card.Stability;
                float retrievability = (float)Math.Pow(1 + interval / (9 * lastS), -1);

                NextDs(s, lastD, lastS, retrievability);

                float hardInterval = NextInterval(s.Hard.Stability);
                float goodInterval = NextInterval(s.Good.Stability);

                hardInterval = Math.Min(hardInterval, goodInterval);
                goodInterval = Math.Max(goodInterval, hardInterval + 1);

                float easyInterval = Math.Max(NextInterval(s.Easy.Stability), goodInterval + 1);

                s.Schedule(hardInterval, goodInterval, easyInterval);
            }

            return s.RecordLog(card);
        }

        private void InitDs(SchedulingCards s)
        {
            s.Again.Difficulty = InitDifficulty((int)CardRating.Again);
            s.Again.Stability = InitStability((int)CardRating.Again);
            s.Hard.Difficulty = InitDifficulty((int)CardRating.Hard);
            s.Hard.Stability = InitStability((int)CardRating.Hard);
            s.Good.Difficulty = InitDifficulty((int)CardRating.Good);
            s.Good.Stability = InitStability((int)CardRating.Good);
            s.Easy.Difficulty = InitDifficulty((int)CardRating.Easy);
            s.Easy.Stability = InitStability((int)CardRating.Easy);
        }

        private void NextDs(SchedulingCards s, float lastD, float lastS, float retrievability)
        {
            s.Again.Difficulty = NextDifficulty(lastD, (int)CardRating.Again);
            s.Again.Stability = NextForgetStability(s.Again.Difficulty, lastS, retrievability);
            s.Hard.Difficulty = NextDifficulty(lastD, (int)CardRating.Hard);
            s.Hard.Stability = NextRecallStability(s.Hard.Difficulty, lastS, retrievability, CardRating.Hard);
            s.Good.Difficulty = NextDifficulty(lastD, (int)CardRating.Good);
            s.Good.Stability = NextRecallStability(s.Good.Difficulty, lastS, retrievability, CardRating.Good);
            s.Easy.Difficulty = NextDifficulty(lastD, (int)CardRating.Easy);
            s.Easy.Stability = NextRecallStability(s.Easy.Difficulty, lastS, retrievability, CardRating.Easy);
        }

        private float InitStability(int r)
        {
            return (float)Math.Max(P.W[r - 1], 0.1);
        }

        private float InitDifficulty(int r)
        {
            return Math.Min(Math.Max(P.W[4] - P.W[5] * (r - 3), 1), 10);
        }

        private float NextInterval(float s)
        {
            float newInterval = s * 9 * (1 / P.RequestRetention - 1);
            return (float)Math.Min(Math.Max(Math.Round(newInterval), 1), P.MaximumInterval);
        }

        private float NextDifficulty(float d, int r)
        {
            float nextD = d - P.W[6] * (r - 3);
            return Math.Min(Math.Max(MeanReversion(P.W[4], nextD), 1), 10);
        }

        private float MeanReversion(float init, float current)
        {
            return P.W[7] * init + (1 - P.W[7]) * current;
        }

        private float NextRecallStability(float d, float s, float r, CardRating rating)
        {
            float hardPenalty = rating == CardRating.Hard ? P.W[15] : 1;
            float easyBonus = rating == CardRating.Easy ? P.W[16] : 1;

            return (float)(s * (1 + Math.Exp(P.W[8]) * (11 - d) * Math.Pow(s, -P.W[9]) *
                (Math.Exp((1 - r) * P.W[10]) - 1) * hardPenalty * easyBonus));

        }

        private float NextForgetStability(float d, float s, float r)
        {
            return (float)(P.W[11] * Math.Pow(d, -P.W[12]) * (Math.Pow(s + 1, P.W[13]) - 1) * Math.Exp((1 - r) * P.W[14]));
        }
    }
}