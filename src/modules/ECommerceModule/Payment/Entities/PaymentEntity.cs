namespace ECommerceModule.Payment.Entities;

internal class PaymentEntity : Entity<long>
{
    public int OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentStatus Status { get; private set; }

    private PaymentEntity() { }
}
