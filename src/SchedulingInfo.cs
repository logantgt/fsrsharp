namespace FSRSharp
{
    public class SchedulingInfo
    {
        public SchedulingInfo(Card card, ReviewLog fsrsReviewLog)
        {
            Card = card;
            FsrsReviewLog = fsrsReviewLog;
        }

        public Card Card { get; set; }
        public ReviewLog FsrsReviewLog { get; set; }
    }
}
