using FastFood.DB.Entities;
using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Newtonsoft.Json;
using X.PagedList;
using X.PagedList.Extensions;

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

        public async Task<IPagedList<Order>> GetOrdersPagedList(int page, int size)
        {
            var orders = await this._orderRepository.GetOrders();
            return orders.OrderBy(x => x.OrderId).ToPagedList();
        }

        public async Task<SortedDictionary<string, int>> GetOrdersStatusCard()
        {
            var orders = await this._orderRepository.GetOrders();
            SortedDictionary<string, int> orderCards = new();
            string[] orderStatuss = new string[]
            {
            "DaThanhToan",
            "ChoXacNhan",
            "DaXacNhan",
            "DangThucHien",
            "ChoGiao",
            "DangGiao",
            "DaGiao",
            "DaHuy"
            };
            byte i = 0;
            foreach (string orderStatus in orderStatuss)
            {
                orderCards[orderStatus] = orders.Count(x => x.OrderStatus == i + 1);
                i++;
            }
            return orderCards;
        }

        public async Task<IPagedList<Order>> GetOrdersByOrderStatusPagedList(int orderStatusId, int page, int size)
        {
            var orderByStatuses = await this._orderRepository.GetOrdersByOrderStatusId(orderStatusId);
            return orderByStatuses.OrderBy(x => x.OrderId).ToPagedList();
        }

        public async Task<CustomOrderDetailViewModel> GetOrderDetailViewModel(int orderId)
        {
            var order = await this._orderRepository.GetOrderByOrderId(orderId);

            CustomOrderDetailViewModel customOrderDetailViewModel = new()
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                Buyer = order.BuyerNavigation.LastName + " " + order.BuyerNavigation.FirstName,
                ShipperName = order.ShipperName ?? string.Empty,
                TotalPrice = order.TotalPrice,
                TotalPay = order.TotalPay,
                PaymentId = order.Payments.Select(x => x.PaymentId).FirstOrDefault(),
                PaymentStatusText = order.Payments.Select(x => x.PaymentStatus).FirstOrDefault() ? "Đã thanh toán" : "Chưa thanh toán",
                TotalProduct = order.OrderDetails.Count,
                ShippingFee = order.ShippingFee,
                ShippingMethod = order.ShippingMethod ?? string.Empty,
                EstimatedDeliveryTime = order.EstimatedDeliveryTime,
                ActualDeliveryTime = order.ActualDeliveryTime,
                OrderDetails = order.OrderDetails.ToList() ?? new List<OrderDetail>(),
                Customer = order.BuyerNavigation,
                OrderStatuses = order.OrderStatusNavigation,
                DiscountAmount = order.Promo?.DiscountAmount ?? 0,
                TransactionId = order.Payments.Where(x => x.PaymentStatus).Select(x => x.TransactionId).FirstOrDefault(),
                PaymentDate = order.Payments.Where(x => x.PaymentStatus).Select(x => x.CreatedAt).FirstOrDefault()
            };
            return customOrderDetailViewModel;
        }

        public async Task<(bool, string)> UpdateOrderStatus(int orderId, int orderStatus)
        {
            var order = await this._orderRepository.GetOrderByOrderId(orderId);

            if (order == null)
            {
                return (false, "Id đơn hàng không hợp lệ !");
            }

            order.OrderStatus = orderStatus;
            await this._orderRepository.UpdateOrder(order);

            return (true, $"Cập nhật trạng thái đơn hàng thành công.\nTrạng thái hiện tại là: {order.OrderStatusNavigation.StatusName}.");
        }

        public async Task<(bool, string)> AddShippingInfo(NewShippingInfo newShippingInfo)
        {
            var order = await this._orderRepository.GetOrderByOrderId(newShippingInfo.OrderId);

            if (order == null)
            {
                return (false, "Id đơn hàng không hợp lệ !");
            }

            order.ShippingFee = 20000;
            order.ShippingId = newShippingInfo.ShippingId;
            order.ShippingUnit = newShippingInfo.ShippingUnit;
            order.EstimatedDeliveryTime = DateOnly.FromDateTime(DateTime.Now).AddDays(newShippingInfo.EstimateDay);
            order.OrderStatus = 6;

            await this._orderRepository.UpdateOrder(order);

            return (true, $"Cập nhật thông tin vận chuyển cho đơn hàng {newShippingInfo.OrderId} thành công !");
        }

        public async Task<(bool, string)> IsAddShippingInfo(int orderId)
        {
            var order = await this._orderRepository.GetOrderByOrderId(orderId);

            if (order != null && order.OrderStatus >= 6)
                return (true, string.Empty);

            return (false, string.Empty);
        }
    }
}
