using FastFood.DB.Entities;
using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VNPAY.NET;
using VNPAY.NET.Models;

namespace FastFood.Services
{
    public class PaymentService : CommonService, IPaymentService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IVnpay _vnPay;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IConfiguration _configuration;
        public PaymentService(ICustomerRepository customerRepository, IVnpay vnPay, IConfiguration configuration, IOrderRepository orderRepository, IPaymentRepository paymentRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _configuration = configuration;
            _vnPay = vnPay;
        }

        public async Task<AddPaymentViewModel> AddPaymentViewModel(PaymentSummaryViewModel? paymentSummaryViewModel, int customerId)
        {
            var customer = await this._customerRepository.GetCustomerById(customerId);

            if (customer != null && paymentSummaryViewModel != null)
            {
                CustomerInfoViewModel customerInfoViewModel = new()
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Address = customer.Address ?? string.Empty,
                    Email = customer.Email ?? string.Empty,
                    PhoneNumber = customer.Phone ?? string.Empty,
                };

                AddPaymentViewModel addPaymentViewModel = new()
                {
                    TotalProductPrice = paymentSummaryViewModel.TotalProductPrice,
                    ShippingFee = paymentSummaryViewModel.ShippingFee,
                    PromoAmount = paymentSummaryViewModel.PromoAmount,
                    PromoCode = paymentSummaryViewModel.PromoCode,
                    Customer = customerInfoViewModel,
                    TotalPay = paymentSummaryViewModel.TotalPay
                };

                return addPaymentViewModel;
            }
            return null!;

        }

        public async Task<string> AddPaymentViewPostModel(AddPaymentViewModel addPaymentViewModel, Dictionary<int, CustomerCartViewModel> customerCartViewModel, int customerId, string ipAddress, string callbackUrl)
        {
            string tmnCode = this._configuration["Vnpay:TmnCode"] ?? string.Empty;
            string hashSecret = this._configuration["Vnpay:HashSecret"] ?? string.Empty;
            string baseUrl = this._configuration["Vnpay:BaseUrl"] ?? string.Empty;

            this._vnPay.Initialize(tmnCode, hashSecret, baseUrl, callbackUrl);

            Order newOrder = new()
            {
                Buyer = customerId,
                OrderDate = DateTime.Now,
                Note = addPaymentViewModel.Customer.OrderNote,
                ShippingFee = addPaymentViewModel.ShippingFee,
                TotalPrice = addPaymentViewModel.TotalProductPrice,
                PromoId = addPaymentViewModel.PromoId,
                TotalPay = addPaymentViewModel.TotalPay,
            };

            foreach (var item in customerCartViewModel.Values)
            {
                newOrder.OrderDetails.Add(new()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            newOrder.Payments.Add(new()
            {
                PaymentStatus = false,
                OrderId = newOrder.OrderId,
                CreatedAt = DateTime.Now
            });

            await this._orderRepository.AddOrder(newOrder);

            // VNPAY

            PaymentRequest vnPayRequest = new()
            {
                CreatedDate = newOrder.OrderDate,
                Money = Convert.ToDouble(newOrder.TotalPay),
                Currency = VNPAY.NET.Enums.Currency.VND,
                BankCode = VNPAY.NET.Enums.BankCode.ANY,
                Language = VNPAY.NET.Enums.DisplayLanguage.Vietnamese,
                PaymentId = newOrder.OrderId,
                IpAddress = ipAddress
            };
            string paymentUrl = this._vnPay.GetPaymentUrl(vnPayRequest);
            return paymentUrl;


        }

        public async Task<(bool, string, PaymentResultViewModel)> GetPaymentResultViewModel(IQueryCollection queryString)
        {
            if (!queryString.Any())
                return (false, "Không tìm thấy chuỗi Query String !", null!);

            PaymentResult paymentResult = this._vnPay.GetPaymentResult(queryString);

            if (paymentResult.IsSuccess)
            {
                int paymentId = Convert.ToInt32(paymentResult.PaymentId);
                int totalPay = Convert.ToInt32(queryString["vnp_Amount"]);

                var payment = await this._paymentRepository.GetPaymentById(paymentId);

                if (payment != null)
                {
                    payment.PaymentStatus = true;
                    payment.TransactionId = paymentResult.VnpayTransactionId;

                    await this._paymentRepository.UpdatePayment(payment);

                    PaymentResultViewModel paymentResultViewModel = new()
                    {
                        OrderId = paymentId,
                        PaymentStatus = paymentResult.PaymentResponse.Description,
                        TransactionId = paymentResult.VnpayTransactionId,
                        TransactionStatus = paymentResult.TransactionStatus.Description,
                        PaymentMethod = paymentResult.PaymentMethod,
                        TotalPay = totalPay
                    };

                    return (true, string.Empty, paymentResultViewModel);
                }
            }

            return (false, "Thông tin không hợp lệ !", new PaymentResultViewModel());
        }


    }
}
