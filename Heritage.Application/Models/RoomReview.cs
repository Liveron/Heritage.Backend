namespace Heritage.Application.Models;

public class RoomReview(string description, double rate)
{
    public string Description => description;
    public double Rate => rate;

    public RoomReview(double rate) : this(string.Empty, rate)
    {
        if (rate <= 0) throw new ArgumentOutOfRangeException(nameof(rate), 
            "Rate should be greater than 0.");
    }
}
