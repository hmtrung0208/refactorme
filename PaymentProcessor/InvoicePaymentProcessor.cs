using System;
using System.Linq;

namespace PaymentProcessor
{
    public class InvoicePaymentProcessor : IInvoicePaymentProcessor
    {
        private readonly InvoiceRepository _invoiceRepository;

        public InvoicePaymentProcessor(InvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public string ProcessPayment(Payment payment)
        {
            var inv = _invoiceRepository.GetInvoice(payment.Reference);

            /// InvoiceValidation:
            /// continue to process payment when return null or empty message
            /// else return error message or throw exception message           
            string validationMessage = InvoiceValidation(inv);

            if (string.IsNullOrEmpty(validationMessage))
            {
                /// Check current invoice already existed any payment or not
                /// if yes: call method ProcessWithExistPayment
                /// else: call method ProcessWithoutExistPayment

                if (inv.Payments != null && inv.Payments.Any())
                {
                    return ProcessWithExistPayment(inv, payment);
                }
                else
                {
                    return ProcessWithoutExistPayment(inv, payment);
                }
            }
            else
            {
                return validationMessage;
            }
        }

        #region Payment Processing
        private string InvoiceValidation(Invoice invoice)
        {
            if(invoice == null)
            {
                throw new InvalidOperationException(Constants.ValidationMessage.NoInvoice);
            }
            else
            {
                if (invoice.Amount == 0)
                {
                    if (invoice.Payments == null || !invoice.Payments.Any())
                    {
                        return Constants.ValidationMessage.NoPaymentNeeded;
                    }
                    else
                    {
                        throw new InvalidOperationException(Constants.ValidationMessage.InvalidState);
                    }
                }
            }

            return string.Empty;
        }

        private string ProcessWithExistPayment(Invoice invoice, Payment payment)
        {
            string msgResult;
            var existPaymentAmount = invoice.Payments.Sum(x => x.Amount);

            if (existPaymentAmount != 0 && invoice.Amount == existPaymentAmount)
            {
                msgResult = Constants.ExistPayment.InvoiceFullyPaid;
            }
            else if (existPaymentAmount != 0 && payment.Amount > (invoice.Amount - invoice.AmountPaid))
            {
                msgResult = Constants.ExistPayment.PaymentGreaterPartialAmount;
            }
            else
            {
                if ((invoice.Amount - invoice.AmountPaid) == payment.Amount)
                {
                    msgResult = Constants.ExistPayment.FinalPartialPaymentReceived;
                }
                else
                {
                    msgResult = Constants.ExistPayment.AnotherPartialPaymentReceived;
                }

                /// Update Invoice
                _invoiceRepository.UpdateInvoice(invoice, payment);
            }

            return msgResult;
        }

        private string ProcessWithoutExistPayment(Invoice invoice, Payment payment)
        {
            string mgsResult;
            if (payment.Amount > invoice.Amount)
            {
                mgsResult = Constants.WithoutExistPayment.PaymentGreaterInvoiceAmount;
            }
            else 
            {
                if (invoice.Amount == payment.Amount) {
                    mgsResult = Constants.WithoutExistPayment.InvoiceFullyPaid;
                }
                else
                {
                    mgsResult = Constants.WithoutExistPayment.InvoicePartiallyPaid;
                }
                
                /// Update Invoice
                _invoiceRepository.UpdateInvoice(invoice, payment);
                
            }
            
            return mgsResult;
        }
        #endregion
    }
}