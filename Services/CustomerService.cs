using FastFood.DB.Entities;
using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace FastFood.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPasswordHasher<string> _passwordHasher;
        private readonly ICustomerAccountRepository _customerAccountRepository;
        public CustomerService(ICustomerRepository customerRepository, IOrderRepository orderRepository, ICustomerAccountRepository customerAccountRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _passwordHasher = new PasswordHasher<string>();
            _customerAccountRepository = customerAccountRepository;
        }

        public async Task<double> CompareCustomersByDateTime(DateTime currentTime, DateTime previousTime)
        {
            var customers = await this._customerRepository.GetCustomers();

            int customersCurrent = customers
                .Count(c => c.CreatedAt >= currentTime && c.CreatedAt < currentTime.AddDays(1));

            int customersPrevious = customers
                .Count(c => c.CreatedAt >= previousTime && c.CreatedAt < previousTime.AddDays(1));

            if (customersPrevious == 0)
            {
                return customersCurrent > 0 ? 100 : 0;
            }

            double percentageChange = ((double)(customersCurrent - customersPrevious) / customersPrevious) * 100;

            return percentageChange;
        }

        public async Task<List<PotentialCustomersViewModel>> GetPotentialCustomers()
        {
            var customersDelivered = await this._customerRepository.GetCustomersByOrderStatus(7);

            return customersDelivered.Select(x => new PotentialCustomersViewModel
            {
                CustomerId = x.CustomerId,
                FullName = this._customerRepository.GetFullName(x.CustomerId),
                CreatedAt = x.CreatedAt,
                Bod = x.Bod,
                NumOfPurchase = x.Orders.Count,
                TotalInvoice = x.Orders.Sum(x => x.TotalPay),
                BiggestOrder = x.Orders.Max(x => x.TotalPay),
                RecentPurchase = x.Orders.OrderByDescending(x => x.OrderDate)
                                           .Select(x => x.OrderDate)
                                           .FirstOrDefault(),
                Avatar = x.ThumbnailImage ?? string.Empty,
                Address = x.Address ?? string.Empty,
                Phone = x.Phone ?? string.Empty,
                Email = x.Email ?? string.Empty


            }).ToList();
        }

        public async Task<IPagedList<PotentialCustomersViewModel>> GetPotentialCustomersPagedList(int page, int size)
        {
            var potentialCustomers = await this.GetPotentialCustomers();
            return potentialCustomers.OrderBy(x => x.CustomerId).ToPagedList(page, size);
        }

        public async Task<IPagedList<Customer>> GetCustomersPagedList(int page, int size)
        {
            var customers = await this._customerRepository.GetCustomers();
            return customers.OrderBy(x => x.CustomerId).ToPagedList(page, size);
        }

        public async Task<(CustomerDetailViewModel, IPagedList<Order>)> GetCustomerDetailWithOrdersPagedList(int customerId, int page, int size)
        {

            var potentialCustomers = await this.GetPotentialCustomers();
            var customer = potentialCustomers.FirstOrDefault(x => x.CustomerId == customerId);
            CustomerDetailViewModel customerDetailViewModel = new();

            if (customer != null) {
                customerDetailViewModel.Address = customer.Address;
                customerDetailViewModel.Phone = customer.Phone;
                customerDetailViewModel.Email = customer.Email;
                customerDetailViewModel.Avatar = customer.Avatar;
                customerDetailViewModel.TotalInvoice = customer.TotalInvoice;
                customerDetailViewModel.NumOfPurchase = customer.NumOfPurchase;
                customerDetailViewModel.CustomerId = customerId;
                customerDetailViewModel.BiggestOrder = customer.BiggestOrder;
                customerDetailViewModel.Bod = customer.Bod;
                customerDetailViewModel.FullName = customer.FullName;
                customerDetailViewModel.RecentPurchase = customer.RecentPurchase;
            }

            var orders = await this._orderRepository.GetOrdersByCustomerId(customerId);
            return (customerDetailViewModel, orders.ToPagedList(page, size));
        }

        public async Task<(bool, string, CustomerClaimInfoViewModel?)> LoginChecker(CustomerLoginViewModel customerLoginViewModel)
        {
            customerLoginViewModel.UserName = customerLoginViewModel.UserName.Normalize().Trim().ToLower();

            var customerAccount = await this._customerAccountRepository.GetCustomerAccountByUserName(customerLoginViewModel.UserName);

            if (customerAccount == null)
            {
                return (false, "Tài khoản không tồn tại", null);
            }

            var verifyResult = this._passwordHasher.VerifyHashedPassword(
                string.Empty,
                customerAccount.Password,
                customerLoginViewModel.Password
            );

            if (verifyResult != PasswordVerificationResult.Success &&
                verifyResult != PasswordVerificationResult.SuccessRehashNeeded)
            {
                return (false, "Thông tin xác thực không đúng !", null);
            }

            CustomerClaimInfoViewModel customerClaimInfoViewModel = new()
            {
                CustomerId = customerAccount.CustomerId,
                Avatar = customerAccount.Customer.ThumbnailImage ?? string.Empty,
                LastName = customerAccount.Customer.LastName,
                FirstName = customerAccount.Customer.FirstName,
                Email = customerAccount.Customer.Email ?? string.Empty
            };

            return (true, string.Empty, customerClaimInfoViewModel);
        }

        public async Task<(bool, string)> Register(CustomerRegisterViewModel customerRegisterViewModel)
        {
            customerRegisterViewModel.UserName = customerRegisterViewModel.UserName.Trim().ToLower();
            CustomerAccount? userExists = await this._customerAccountRepository.GetCustomerAccountByUserName(customerRegisterViewModel.UserName);

            if (userExists != null)
            {
                return (false, "Tên đăng nhập đã tồn tại !");
            }

            string passwordHashed = this._passwordHasher.HashPassword(string.Empty, customerRegisterViewModel.Password);

            CustomerAccount customerAccount = new()
            {
                UserName = customerRegisterViewModel.UserName,
                CreatedAt = DateTime.Now,
                Password = passwordHashed
            };

            await this._customerAccountRepository.AddCustomerAccount(customerAccount);

            return (true, $"Tạo tài khoản thành công !");
        }

    }
}
