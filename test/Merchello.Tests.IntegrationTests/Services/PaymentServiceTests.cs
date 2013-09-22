﻿using System;
using System.Collections.Generic;
using System.Linq;
using Merchello.Core;
using Merchello.Core.Models;
using Merchello.Core.Services;
using Merchello.Tests.Base.DataMakers;
using NUnit.Framework;

namespace Merchello.Tests.IntegrationTests.Services
{
    [TestFixture]
    [Category("Service Integration")]
    public class PaymentServiceTests : ServiceIntegrationTestBase
    {

        private IPaymentService _paymentService;
        private ICustomer _customer;

        [SetUp]
        public void Initialize()
        {
            _paymentService = new PaymentService();
            
            _customer = MockCustomerDataMaker.CustomerForInserting();
            var customerService = new CustomerService();

            customerService.Save(_customer);

        }


        [Test]
        public void Can_Create_And_Save_An_Payments()
        {
            var payment = _paymentService.CreatePayment(_customer, Guid.NewGuid(), PaymentMethodType.Cash, "Cash", "Complete", 12.00m);

            _paymentService.Save(payment);

            Assert.IsTrue(payment.Id > 0);
        }


        [Test]
        public void Can_Create_And_Save_A_List_Of_Payments()
        {
            var providerKey = Guid.NewGuid();

            var payments = new List<IPayment>()
                {
                   { _paymentService.CreatePayment(_customer, providerKey, PaymentMethodType.Cash, "Cash", "Complete", 12.00m) },
                   { _paymentService.CreatePayment(_customer, providerKey, PaymentMethodType.CreditCard, "CC", "Complete", 156.00m) },
                   { _paymentService.CreatePayment(_customer, providerKey, PaymentMethodType.Cash, "Cash", "Complete", 1.00m) }
                };

            _paymentService.Save(payments);

            Assert.IsTrue(payments[0].Id > 0);
        }

        [Test]
        public void Can_Delete_All_Payments()
        {
            var payments = ((PaymentService) _paymentService).GetAll();

            if (payments.Any())
            {
                Console.WriteLine("Deleting {0} payments", payments.Count());
                _paymentService.Delete(payments);

                payments = ((PaymentService)_paymentService).GetAll();

                
                Assert.IsFalse(payments.Any());
            }

            Assert.Pass();

        }

        [Test]
        public void Can_Get_A_List_Of_Payments_By_Customer()
        {
            var providerKey = Guid.NewGuid();
            var payments = new List<IPayment>()
                {
                   { _paymentService.CreatePayment(_customer, providerKey, PaymentMethodType.Cash, "Cash", "Complete", 12.00m) },
                   { _paymentService.CreatePayment(_customer, providerKey, PaymentMethodType.CreditCard, "CC", "Complete", 156.00m) },
                   { _paymentService.CreatePayment(_customer, providerKey, PaymentMethodType.Cash, "Cash", "Complete", 1.00m) }
                };

            _paymentService.Save(payments);

            var customerPayments = _paymentService.GetPaymentsByCustomer(_customer.Key);

            Assert.IsTrue(customerPayments.Any());

        }

    }
}
