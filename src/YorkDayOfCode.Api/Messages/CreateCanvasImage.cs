namespace YorkDayOfCode.Api.Messages
{
    public class CreateCanvasImage
    {
        public string CanvasId { get; set; }

        public byte[] Image { get; set; }
    }
}
