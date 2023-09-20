namespace FSRSharp
{
    public class Card
    {
        public DateTime Due { get; set; }
        public float Stability { get; set; }
        public float Difficulty { get; set; }
        public int ElapsedDays { get; set; }
        public int ScheduledDays { get; set; }
        public int Reps { get; set; }
        public int Lapses { get; set; }
        public CardState State { get; set; } = CardState.New;
        public DateTime LastReview { get; set; }

        public Card Clone()
        {
            return new Card
            {
                Due = Due,
                Stability = Stability,
                Difficulty = Difficulty,
                ElapsedDays = ElapsedDays,
                ScheduledDays = ScheduledDays,
                Reps = Reps,
                Lapses = Lapses,
                State = State,
                LastReview = LastReview
            };
        }
    }
}
