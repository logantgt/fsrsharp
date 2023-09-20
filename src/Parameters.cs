namespace FSRSharp
{
    public class Parameters
    {
        public Parameters()
        {
            RequestRetention = 0.9f;
            MaximumInterval = 36500;
            W = new[]
            {
                0.4f,
                0.6f,
                2.4f,
                5.8f,
                4.93f,
                0.94f,
                0.86f,
                0.01f,
                1.49f,
                0.14f,
                0.94f,
                2.18f,
                0.05f,
                0.34f,
                1.26f,
                0.29f,
                2.61f,
            };
        }

        public float RequestRetention { get; set; }
        public int MaximumInterval { get; set; }
        public float[] W { get; set; }
    }
}
