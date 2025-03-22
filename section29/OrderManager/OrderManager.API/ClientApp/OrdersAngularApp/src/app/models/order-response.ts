export class OrderResponse {
  orderId: string;
  orderNumber: string;
  customerName: string;
  orderDate: Date;
  totalAmount: number | null;

  constructor(orderId: string,
    orderNumber: string,
    customerName: string,
    orderDate: Date,
    totalAmount: number | null) {
    this.orderId = orderId;
    this.orderNumber = orderNumber;
    this.customerName = customerName;
    this.orderDate = orderDate;
    this.totalAmount = totalAmount;
  }
}
