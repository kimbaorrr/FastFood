using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Newtonsoft.Json;

namespace FastFood.Services
{
    public class OrderService : CommonService, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<int> CountOrderByDate(DateTime fromDate, DateTime toDate)
        {
            var ordersDelivered = await this._orderRepository.GetOrdersDelivered();
            return ordersDelivered.Count(x => x.OrderDate >= fromDate && x.OrderDate < toDate);
        }

        public async Task<int> GetRevenue(DateTime fromDate, DateTime toDate)
        {
            var ordersDelivered = await this._orderRepository.GetOrdersDelivered();
            return ordersDelivered
                .Where(x => x.OrderDate >= fromDate && x.OrderDate < toDate)
                .Sum(x => x.TotalPrice);
        }

        public async Task<double> CompareRevenueByPercentage(DateTime currentDate, DateTime previousDate)
        {
            var ordersDelivered = await this._orderRepository.GetOrdersDelivered();

            decimal currentRevenue = ordersDelivered
                .Where(o => o.OrderDate >= currentDate && o.OrderDate < currentDate.AddDays(1))
                .Sum(o => o.TotalPrice);

            decimal previousRevenue = ordersDelivered
                .Where(o => o.OrderDate >= previousDate && o.OrderDate < currentDate)
                .Sum(o => o.TotalPrice);

            if (previousRevenue == 0)
                return currentRevenue > 0 ? 100 : 0;

            double percentageChange = (double)(currentRevenue - previousRevenue) / (double)previousRevenue * 100;
            return percentageChange;
        }

        public async Task<double> CompareOrderCountByPercentage(DateTime currentDate, DateTime previousDate)
        {
            var ordersDelivered = await this._orderRepository.GetOrdersDelivered();

            int currentDayOrders = ordersDelivered
                .Count(o => o.OrderDate >= currentDate && o.OrderDate < currentDate.AddDays(1));

            int previousDayOrders = ordersDelivered
                .Count(o => o.OrderDate >= previousDate && o.OrderDate < currentDate);

            if (previousDayOrders == 0)
                return currentDayOrders > 0 ? 100 : 0;

            double percentageChange = (double)(currentDayOrders - previousDayOrders) / previousDayOrders * 100;
            return percentageChange;
        }

        public async Task<string> GetRevenueByTimeRange(string timeRange)
        {
            var ordersDelivered = await this._orderRepository.GetOrdersDelivered();

            DateTime today = DateTime.Today;
            SortedDictionary<DateTime, int> revenue = new SortedDictionary<DateTime, int>();

            switch (timeRange)
            {
                case "30_days":
                    for (int i = 0; i < 30; i++)
                    {
                        DateTime day = today.AddDays(-i);
                        int dailyRevenue = ordersDelivered
                            .Where(o => o.OrderDate.Date == day.Date)
                            .Sum(o => o.TotalPrice);

                        revenue[day] = dailyRevenue;
                    }
                    break;

                case "6_months":
                    for (int i = 0; i < 6; i++)
                    {
                        DateTime month = today.AddMonths(-i);
                        int monthlyRevenue = ordersDelivered
                            .Where(o => o.OrderDate.Year == month.Year && o.OrderDate.Month == month.Month)
                            .Sum(o => o.TotalPrice);

                        revenue[month] = monthlyRevenue;
                    }
                    break;

                case "1_year":
                    for (int i = 0; i < 12; i++)
                    {
                        DateTime month = today.AddMonths(-i);
                        int monthlyRevenue = ordersDelivered
                            .Where(o => o.OrderDate.Year == month.Year && o.OrderDate.Month == month.Month)
                            .Sum(o => o.TotalPrice);

                        revenue[month] = monthlyRevenue;
                    }
                    break;

                default:
                    break;
            }

            Dictionary<string, int> result = revenue.ToDictionary(
                kvp => timeRange == "30_days"
                    ? kvp.Key.ToString("dd/MM/yyyy")
                    : kvp.Key.ToString("MM/yyyy", new System.Globalization.CultureInfo("vi-VN")),
                kvp => kvp.Value
            );

            return JsonConvert.SerializeObject(result);
        }

        public async Task<string> GetOrdersByDate(DateTime fromTime, DateTime toTime)
        {
            var orders = await this._orderRepository.GetOrders();

            var ordersListByDate = orders
                .Where(x => x.OrderStatus != 0 && x.OrderDate >= fromTime && x.OrderDate < toTime)
                .GroupBy(d => d.OrderDate.Date)
                .Select(g => new
                {
                    Date = g.Key.ToString("dd-MM-yyyy"),
                    OrderCount = g.Count()
                })
                .OrderBy(g => g.Date)
                .ToList();

            return JsonConvert.SerializeObject(ordersListByDate);
        }

        public async Task<List<CustomOrderViewModel>> GetCustomOrderViewModels()
        {
            var orders = await this._orderRepository.GetOrdersWithDetails();

            var customOrderViewModels = orders.Select(order => new CustomOrderViewModel
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                Seller = order.SellerNavigation?.LastName + " " + order.SellerNavigation?.FirstName ?? string.Empty,
                TotalPay = order.TotalPay,
                OrderStatuses = order.OrderStatusNavigation,
                PaymentStatusText = order.Payments.Select(x => x.PaymentStatus).FirstOrDefault() ? "Đã thanh toán" : "Chưa thanh toán",
                TotalProduct = order.OrderDetails.Count,
                Buyer = order.BuyerNavigation?.LastName + " " + order.BuyerNavigation?.FirstName ?? string.Empty
            }).ToList();

            return customOrderViewModels;
        }
    }
}
