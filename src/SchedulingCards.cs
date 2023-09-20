namespace FSRSharp
{
    public class SchedulingCards
    {
        public Card Again { get; set; }
        public Card Hard { get; set; }
        public Card Good { get; set; }
        public Card Easy { get; set; }

        internal SchedulingCards(Card card)
        {
            Again = card.Clone();
            Hard = card.Clone();
            Good = card.Clone();
            Easy = card.Clone();
        }

        internal void UpdateState(CardState state)
        {
            if (state == CardState.New)
            {
                Again.State = CardState.Learning;
                Hard.State = CardState.Learning;
                Good.State = CardState.Learning;
                Easy.State = CardState.Review;
                Again.Lapses += 1;
            }
            else if (state == CardState.Learning || state == CardState.Relearning)
            {
                Again.State = state;
                Hard.State = state;
                Good.State = CardState.Review;
                Easy.State = CardState.Review;
            }
            else if (state == CardState.Review)
            {
                Again.State = CardState.Relearning;
                Hard.State = CardState.Review;
                Good.State = CardState.Review;
                Easy.State = CardState.Review;
            }
        }

        internal void Schedule(float hardInterval, float goodInterval, float easyInterval)
        {
            Again.ScheduledDays = 0;
            Hard.ScheduledDays = (int)hardInterval;
            Good.ScheduledDays = (int)goodInterval;
            Easy.ScheduledDays = (int)easyInterval;
            Again.Due = DateTime.Now.Add(TimeSpan.FromMinutes(5));

            Hard.Due = DateTime.Now.Add(hardInterval > 0 ? TimeSpan.FromDays(hardInterval) : TimeSpan.FromMinutes(10));

            Good.Due = DateTime.Now.Add(TimeSpan.FromDays(goodInterval));
            Easy.Due = DateTime.Now.Add(TimeSpan.FromDays(easyInterval));
        }

        internal Dictionary<CardRating, SchedulingInfo> RecordLog(Card card)
        {
            Dictionary<CardRating, SchedulingInfo> output = new Dictionary<CardRating, SchedulingInfo>
            {
                { CardRating.Again, new SchedulingInfo(Again, new ReviewLog(CardRating.Again, Again.ScheduledDays, card.ElapsedDays, DateTime.Now, card.State)) },
                { CardRating.Hard, new SchedulingInfo(Hard, new ReviewLog(CardRating.Hard, Hard.ScheduledDays, card.ElapsedDays, DateTime.Now, card.State)) },
                { CardRating.Good, new SchedulingInfo(Good, new ReviewLog(CardRating.Good, Good.ScheduledDays, card.ElapsedDays, DateTime.Now, card.State)) },
                { CardRating.Easy, new SchedulingInfo(Easy, new ReviewLog(CardRating.Easy, Easy.ScheduledDays, card.ElapsedDays, DateTime.Now, card.State)) }
            };

            return output;
        }
    }
}
