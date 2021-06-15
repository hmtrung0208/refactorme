namespace PaymentProcessor {
	public class InvoiceRepository
	{
		private Invoice _invoice;

		public InvoiceRepository( Invoice invoice )
		{
			_invoice = invoice;
		}

		public Invoice GetInvoice( string reference )
		{
			return _invoice;
		}

		public Invoice UpdateInvoice(Invoice invoice, Payment payment)
        {
			invoice.AmountPaid = payment.Amount;
			invoice.Payments.Add(payment);

			return invoice;
		}
	}
}