namespace DeliveryTracking.Domain.Enums
{
    public enum OrderStatus
    {
        Pending = 1,
        Assigned = 2,
        Accepted = 3,
        PickedUp = 4,
        OnTheWay = 5,
        Delivered = 6,
        Cancelled = 7,
        Failed = 8
    }
}
