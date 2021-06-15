using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentProcessor
{
    public static class Constants
    {
        public static class ValidationMessage
        {
            public const string NoInvoice = "There is no invoice matching this payment";
            public const string NoPaymentNeeded = "No payment needed";
            public const string InvalidState = "The invoice is in an invalid state, it has an amount of 0 and it has payments.";
        }

        public static class ExistPayment
        {
            public const string InvoiceFullyPaid = "Invoice was already fully paid";
            public const string PaymentGreaterPartialAmount = "The payment is greater than the partial amount remaining";
            public const string FinalPartialPaymentReceived = "Final partial payment received, invoice is now fully paid";
            public const string AnotherPartialPaymentReceived = "Another partial payment received, still not fully paid";
        }

        public static class WithoutExistPayment
        {
            public const string PaymentGreaterInvoiceAmount = "The payment is greater than the invoice amount";
            public const string InvoiceFullyPaid = "Invoice is now fully paid";
            public const string InvoicePartiallyPaid = "Invoice is now partially paid";
        }
    }
}
