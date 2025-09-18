using FastFood.DB;
using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
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
            }).ToList();
        }
    }
}
