namespace FSRSharp
{
    public class ReviewLog
    {
        public ReviewLog(CardRating rating, int scheduledDays, int elapsedDays, DateTime review, CardState state)
        {
            Rating = rating;
            ScheduledDays = scheduledDays;
            ElapsedDays = elapsedDays;
            Review = review;
            State = state;
        }

        public CardRating Rating { get; set; }
        public int ScheduledDays { get; set; }
        public int ElapsedDays { get; set; }
        public DateTime Review { get; set; }
        public CardState State { get; set; }
    }
}
